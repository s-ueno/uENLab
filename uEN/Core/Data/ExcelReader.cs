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

        public DataSet List()
        {
            var ds = new DataSet("Excel");
            try
            {
                using (var con = new OleDbConnection(JecConnectionString))
                {
                    Load(ds, con);
                }
            }
            catch
            {
                try
                {
                    using (var con = new OleDbConnection(AccDbConnectionString))
                    {
                        Load(ds, con);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    throw;
                }
            }
            return ds;
        }
        private void Load(DataSet ds, OleDbConnection con)
        {
            con.Open();
            var tbls = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow row in tbls.Rows)
            {
                string tableName = (string)row["TABLE_NAME"];
                if (!tableName.EndsWith("$")) continue;
                tableName = tableName.Substring(0, tableName.Length - 1);
                if (ds.Tables.Contains(tableName)) continue;

                var table = ListByQuery(string.Format("select * from [{0}$]", tableName));
                ds.Tables.Add(table);
            }
        }

        public DataTable ListByQuery(string sql)
        {
            var ds = new DataSet("Excel");
            try
            {
                using (var con = new OleDbConnection(JecConnectionString))
                {
                    Load(sql, ds, con);
                }
            }
            catch
            {
                try
                {
                    using (var con = new OleDbConnection(AccDbConnectionString))
                    {
                        Load(sql, ds, con);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    throw;
                }
            }
            var table = ds.Tables[0];
            ds.Tables.Clear();
            return table;
        }
        private static void Load(string sql, DataSet ds, OleDbConnection con)
        {
            con.Open();
            var adapter = new OleDbDataAdapter(sql, con);
            adapter.Fill(ds);
        }


        

    }
}
