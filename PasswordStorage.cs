using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PasswordManager
{
    class PasswordStorage
    {
        #region Constants
        const string DataContainerName = "PasswordManagerData";
        const string MetaDataTableName = "MetaData";
        const string PasswordDataTableName = "PasswordData";
        #endregion
        #region Variables
        private DataSet data;
        #endregion
        #region Delegates, Events, etc.
        #endregion


        //DataTable passwordData; // Data table for all password stuff
        //DataTable metaData;     // Metadata (whatever might be needed)

        #region Constructor, Initialization, etc.

        public PasswordStorage()
        {
            ReInitialize();
        }

        /// <summary>To clear all data in the storage.</summary>
        private void Clear()
        {
            if (data != null)
            {
                data.Dispose();
                data = null;
                GC.Collect();
            }
        }
        /// <summary>(Re)Initializes the PasswordStorage and all contained data</summary>
        private void ReInitialize()
        {
            Clear();
            data = new DataSet(DataContainerName);
            data.Tables.Add(CreateMetaDataTable());
            data.Tables.Add(CreatePasswordDataTable());

            // Test stuff
            DataTable tmpPwTbl = data.Tables[1];
            tmpPwTbl.Rows.Add(new object[] { "Test1", "Test2", "Test3", "Test4", "Test5" });
            tmpPwTbl.Rows.Add(new object[] { "Google", "Kaggne123", "google.de", "Tomboy", "This is a test" });

        }
        private DataTable CreateMetaDataTable()
        {
            DataTable tmpMetaDataTable = new DataTable(MetaDataTableName);

            //DataColumn masterKey = new DataColumn("Masterkey");
            //DataColumn lastEdited = new DataColumn("Last edited");

            //masterKey.AllowDBNull = true;
            //masterKey.Caption = "Masterkey";
            //masterKey.DataType = Type.GetType("String");

            DataColumn dummy = new DataColumn("Dummy", System.Type.GetType("System.String"));
            dummy.Caption = "Dummy";
            dummy.AllowDBNull = false;
            dummy.ReadOnly = true;

            tmpMetaDataTable.Columns.Add(dummy);
            tmpMetaDataTable.Rows.Add("DummyVal, kk?");

            return tmpMetaDataTable;
        }

        private DataTable CreatePasswordDataTable()
        {
            DataTable tmpPasswordDataTable = new DataTable(PasswordDataTableName);
            tmpPasswordDataTable.Columns.AddRange(CreatePasswordDataColumns());
            return tmpPasswordDataTable;
        }
        private DataColumn[] CreatePasswordDataColumns()
        {
            DataColumn websiteOrService = new DataColumn
            {
                ColumnName = "Website / Service",
                Caption = "Website / Service",
                AllowDBNull = false,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty,
            };
            DataColumn password = new DataColumn
            {
                ColumnName = "Password",
                Caption = "Password",
                AllowDBNull = false,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn url = new DataColumn
            {
                ColumnName = "Link (URL)",
                Caption = "Link (URL)",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn user = new DataColumn
            {
                ColumnName = "User",
                Caption = "User",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn comments = new DataColumn
            {
                ColumnName = "Comments",
                Caption = "Comments",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn othersExisting = new DataColumn
            {
                ColumnName = "OthersExisting",
                Caption = "Other properties exist",
                AllowDBNull = false,
                DataType = System.Type.GetType("System.Boolean"),
                DefaultValue = false
            };
            DataColumn other = new DataColumn
            {
                ColumnName = "Other",
                Caption = "Other",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty,
            };

            return new DataColumn[] { websiteOrService, password, url, user, comments/*, othersExisting, other*/};
        }

        #endregion
        #region IO

        public void AddEntry()
        {

        }

        public void RemoveEntry(int index)
        {
            DataTable tmpPwTbl = data.Tables[PasswordDataTableName];
            tmpPwTbl.Rows.RemoveAt(index);
        }

        public void EditEntry()
        {

        }

        public DataTable GetPasswordDataTable()
        {
            return data.Tables[PasswordDataTableName];
        }
        public DataTable GetMetaDataTable()
        {
            return data.Tables[MetaDataTableName];
        }

        #endregion
        #region File IO
        // Export

        public void ExportToCsv(string FilePath, string FileName)
        {

        }
        public void ExportToXml(string FilePath, string FileName)
        {

        }
        public void ExportToTxt(string FilePath, string FileName)
        {

        }

        // Import
        public void ImportFromXml(string FilePathAndName)
        {

        }

        #endregion
        #region Other

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            DataTable pwDataTableHandle = this.data.Tables[PasswordDataTableName];

            foreach (DataRow row in pwDataTableHandle.Rows)
            {
                var tmpRowCells = new List<String>();
                foreach (DataColumn col in pwDataTableHandle.Columns)
                {
                    tmpRowCells.Add(Convert.ToString(row[col.ColumnName]));
                }
                var tmpRow = new StringBuilder();
                foreach (string s in tmpRowCells)
                {
                    tmpRow.Append(String.Format("{0,-25} ", s));
                }
                content.AppendLine(tmpRow.ToString());
            }
            return content.ToString();
        }

        #endregion
    }
}
