using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace uEN.Core.Data
{
    /// <summary>
    /// open office 仕様(.xlsx)ではない、旧エクセル(*.xls)の簡易リーダー
    /// </summary>
    public class ExcelReader
    {
        //クライアントのOfficeがJet
        private string _jetConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;  Data Source={0};Extended Properties=Excel 8.0;";
        //クライアントはOpenOffice以降のOffice
        private string _accDBConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=Excel 8.0;";
        public ExcelReader(string path)
        {
            Path = path;
        }
        public string Path { get; private set; }

        private string JecConnectionString
        {
            get { return string.Format(_jetConnectionString, Path); }
        }
        private string AccDbConnectionString
        {
            get { return string.Format(_accDBConnectionString, Path); }
        }

        protected T ConnectionAction<T>(Func<OleDbConnection, T> action)
        {
            var result = default(T);
            try
            {
                using (var cnn = new OleDbConnection(JecConnectionString))
                {
                    cnn.Open();
                    result = action(cnn);
                }
            }
            catch
            {
                try
                {
                    using (var cnn = new OleDbConnection(AccDbConnectionString))
                    {
                        cnn.Open();
                        result = action(cnn);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    throw;
                }
            }
            return result;
        }

        public DataSet List()
        {
            return ConnectionAction(Load);
        }
        private DataSet Load(OleDbConnection con)
        {
            var ds = new DataSet("ExcelData");
            var tbls = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow row in tbls.Rows)
            {
                string tableName = (string)row["TABLE_NAME"];
                if (!tableName.EndsWith("$")) continue;
                tableName = tableName.Substring(0, tableName.Length - 1);
                if (ds.Tables.Contains(tableName)) continue;

                var table = ListByQuery(string.Format("select * from [{0}$]", tableName));
                table.TableName = tableName;
                ds.Tables.Add(table);
            }
            return ds;
        }
        public DataTable ListByQuery(string sql)
        {
            return ConnectionAction(new Func<OleDbConnection, DataTable>(con =>
            {
                var ds = new DataSet("Excel");

                var adapter = new OleDbDataAdapter(sql, con);
                adapter.Fill(ds);

                var table = ds.Tables[0];
                ds.Tables.Clear();
                return table;
            }));
        }


        public IEnumerable<string> ListSheetNames()
        {
            return ConnectionAction(ListSheetNames);
        }
        private IEnumerable<string> ListSheetNames(OleDbConnection con)
        {
            var list = new List<string>();

            var tbls = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow row in tbls.Rows)
            {
                string tableName = (string)row["TABLE_NAME"];
                if (!tableName.EndsWith("$")) continue;
                tableName = tableName.Substring(0, tableName.Length - 1);
                if (list.Contains(tableName)) continue;

                list.Add(tableName);
            }
            return list;
        }

        public int Count(string sheetName)
        {
            return ConnectionAction(new Func<OleDbConnection, int>(con =>
            {
                var sql = string.Format("select count(*) as CNT from [{0}$]", sheetName);
                var table = ListByQuery(sql);
                var count = Convert.ToInt32(table.Rows[0][0]);
                return count;
            }));
        }
    }
}
