using System;
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
        public static DbProviderFactory CreateFactory(string contextName = DefaultContext)
        {
            var repository = Repository.GetPriorityExport<DbConnectionRepository>();
            var con = repository.CreateConnectionString(contextName);
            return repository.CreateDbProviderFactory(con.ProviderName);
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

        public static string GetProviderName(string context = DefaultContext)
        {
            var repository = Repository.GetPriorityExport<DbConnectionRepository>();
            var conInfo = repository.CreateConnectionString(context);
            return conInfo.ProviderName;
        }
        public static string GetConnectionString(string context = DefaultContext)
        {
            var repository = Repository.GetPriorityExport<DbConnectionRepository>();
            var conInfo = repository.CreateConnectionString(context);
            return conInfo.ConnectionString;
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
        protected DbConnectionHelper() { }
        protected internal string ContextName { get; internal set; }

        #region ConnectionStack - "ThreadStatic"

        private class DbPair : IDisposable
        {
            public DbConnection Connection { get; internal set; }
            public DbTransaction Transaction { get; internal set; }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected void Dispose(bool disposing)
            {
                if (disposed) return;

                if (Connection != null)
                {
                    Connection.Close();
                    Connection = null;
                }
                if (Transaction != null)
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
                disposed = true;
            }
            bool disposed = false;
        }
        private class ConnectionStack
        {
            private Dictionary<string, Stack<DbPair>> dic = new Dictionary<string, Stack<DbPair>>();

            public DbPair Push(string context)
            {
                var repository = Repository.GetPriorityExport<DbConnectionRepository>();
                var conInfo = repository.CreateConnectionString(context);

                var factory = repository.CreateDbProviderFactory(conInfo.ProviderName);

                var newCon = factory.CreateConnection();
                newCon.ConnectionString = conInfo.ConnectionString;
                var db = new DbPair() { Connection = newCon };

                if (!dic.ContainsKey(context))
                {
                    var stack = new Stack<DbPair>();
                    stack.Push(db);
                    dic[context] = stack;
                }
                else
                {
                    dic[context].Push(db);
                }
                return db;
            }
            public DbPair Pop(string context)
            {
                if (dic.ContainsKey(context) &&
                    dic[context].Count != 0)
                {
                    return dic[context].Pop();
                }
                return null;
            }
            public DbPair PeekOrNew(string context)
            {
                if (dic.ContainsKey(context) &&
                    dic[context].Count != 0)
                {
                    return dic[context].Peek();
                }
                return Push(context);
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

        #endregion

        public virtual DbProviderFactory Factory
        {
            get
            {
                var repository = Repository.GetPriorityExport<DbConnectionRepository>();
                var conInfo = repository.CreateConnectionString(ContextName);
                return repository.CreateDbProviderFactory(conInfo.ProviderName);
            }
        }
        private DbPair Db { get { return Connections.PeekOrNew(ContextName); } }
        public virtual DbConnection DbConnection { get { return Db.Connection; } }
        public virtual DbTransaction Transaction
        {
            get { return Db.Transaction; }
            set { Db.Transaction = value; }
        }
        public virtual DbDataAdapter CreateDataAdapter()
        {
            return Factory.CreateDataAdapter();
        }
        public virtual DbCommand CreateCommand()
        {
            return DbConnection.CreateCommand();
        }


        public virtual void Open()
        {
            Trace.TraceInformation("DbConnectionHelper.Open --- {0} ", ContextName);

            var db = Connections.Push(ContextName);
            db.Connection.Open();
        }
        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            Trace.TraceInformation("DbConnectionHelper.BeginTransaction --- {0} ", isolationLevel);

            var con = DbConnection;
            if (con.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("Database connection is not open. Open() is required.");
            Transaction = con.BeginTransaction(isolationLevel);
        }
        public virtual void Commit()
        {
            if (Transaction == null)
                throw new InvalidOperationException("Database connection is not begin transaction. BeginTransaction() is required.");

            Transaction.Commit();
            Trace.TraceInformation("DbConnectionHelper.Commit --- {0} ", ContextName);
        }
        public virtual void Rollback()
        {
            if (Transaction == null)
                throw new InvalidOperationException("Database connection is not begin transaction. BeginTransaction() is required.");

            Transaction.Rollback();
            Trace.TraceInformation("DbConnectionHelper.Rollback --- {0} ", ContextName);
        }
        public virtual void Close()
        {
            if (closed)
                return;

            var db = Connections.Pop(ContextName);
            db.Dispose();

            Trace.TraceInformation("DbConnectionHelper.Close --- {0} ", ContextName);
            closed = true;
        }
        bool closed = false;

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
