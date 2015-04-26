﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace uEN.Core.Data
{
    /// <summary>
    /// データベース接続への拡張ポイント用ファクトリクラスです。
    /// </summary>
    [Export(typeof(DbConnectionRepository))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    public class DbConnectionRepository
    {
        public const string DefaultContext = "Default";
        public virtual ConnectionStringSettings CreateConnectionString(string contextName = DefaultContext)
        {
            return ConfigurationManager.ConnectionStrings[contextName];
        }
        public virtual DbProviderFactory CreateDbProviderFactory()
        {
            var con = CreateConnectionString();
            return CreateDbProviderFactory(con.ProviderName);
        }
        public virtual DbProviderFactory CreateDbProviderFactory(string providerName)
        {
            return DbProviderFactories.GetFactory(providerName);
        }
        public static DbConnectionHelper CreateDbHelper(string contextName = DefaultContext)
        {
            var repository = Repository.GetPriorityExport<DbConnectionRepository>();
            return repository.CreateDbConnectionHelper(contextName);
        }
        protected virtual DbConnectionHelper CreateDbConnectionHelper(string contextName)
        {
            var helper = Repository.GetPriorityExport<DbConnectionHelper>();
            helper.ContextName = contextName;
            return helper;
        }

    }

    /// <summary>
    /// 抽象化されたデータベース接続処理を、スレッド毎に一意に管理するヘルパークラスです。
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(DbConnectionHelper))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    public class DbConnectionHelper : IDisposable
    {
        protected internal string ContextName { get; internal set; }

        private class ConnectionStack
        {
            private Dictionary<string, Stack<DbConnection>> dic = new Dictionary<string, Stack<DbConnection>>();
            public DbConnection GetConnection(string context)
            {
                if (dic.ContainsKey(context))
                {
                    return dic[context].Peek();
                }
                return Push(context);
            }
            public DbConnection Push(string context)
            {
                var repository = Repository.GetPriorityExport<DbConnectionRepository>();
                var conInfo = repository.CreateConnectionString(context);

                var factory = repository.CreateDbProviderFactory(conInfo.ProviderName);
                var newCon = factory.CreateConnection();
                newCon.ConnectionString = conInfo.ConnectionString;

                if (!dic.ContainsKey(context))
                {
                    var stack = new Stack<DbConnection>();
                    stack.Push(newCon);
                    dic[context] = stack;
                }
                else
                {
                    dic[context].Push(newCon);
                }
                return newCon;
            }
            public DbConnection Pop(string context)
            {
                var stack = dic[context];
                return stack.Pop();
            }
        }

        private static ConnectionStack Connections
        {
            get
            {
                if (_connections == null)
                {
                    _connections = new ConnectionStack();
                }
                return _connections;
            }
        }
        [ThreadStatic]
        private static ConnectionStack _connections;

        protected DbProviderFactory Factory
        {
            get
            {
                var repository = Repository.GetPriorityExport<DbConnectionRepository>();
                var conInfo = repository.CreateConnectionString(ContextName);

                return repository.CreateDbProviderFactory(conInfo.ProviderName);
            }
        }
        public void Open()
        {
            Trace.TraceInformation("DbConnectionHelper.Open --- {0} ", ContextName);

            var con = Connections.Push(ContextName);
            con.Open();
        }

        public DbConnection DbConnection { get { return Connections.GetConnection(ContextName); } }
        protected DbTransaction Transaction { get; private set; }
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            Trace.TraceInformation("DbConnectionHelper.BeginTransaction --- {0} ", isolationLevel);

            var con = DbConnection;
            if (con.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("データベース接続が開いていません。Openが必要です。");
            Transaction = con.BeginTransaction(isolationLevel);
        }
        public void Commit()
        {
            Transaction.Commit();
            Trace.TraceInformation("DbConnectionHelper.Commit --- {0} ", ContextName);
        }
        public void Rollback()
        {
            Transaction.Rollback();
            Trace.TraceInformation("DbConnectionHelper.Rollback --- {0} ", ContextName);
        }
        public void Close()
        {
            if (closed)
                return;

            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }

            Connections.Pop(ContextName).Close();
            Trace.TraceInformation("DbConnectionHelper.Close --- {0} ", ContextName);
            closed = true;
        }
        bool closed = false;

        public DbCommand CreateCommand()
        {
            return DbConnection.CreateCommand();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Close();
            }
            disposed = true;
            Trace.TraceInformation("DbConnectionHelper.Dispose");
        }
        bool disposed = false;
    }

}
