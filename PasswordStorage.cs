using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace PasswordManager
{
    class PasswordStorage
    {
        #region Constants
        const string DataContainerName = "PasswordManagerData";
        const string MetaDataTableName = "MetaData";
        const string PasswordDataTableName = "PasswordData";
        #endregion
        #region Properties
        public DataSet DataSource 
        { 
            get 
            { 
                return data; 
            } 
            set
            {
                this.data = value;
                DataSourceChanged?.Invoke(this, new DataSourceEventArgs(this.data.Tables[PasswordDataTableName]));
            } 
        }
        #endregion
        #region Variables
        private DataSet data;
        #endregion
        #region Delegates, Events, etc.
        public delegate void DataSourceChangedEventHandler(object sender, DataSourceEventArgs e);
        public event DataSourceChangedEventHandler DataSourceChanged;
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
            tmpMetaDataTable.Rows.Add("Dummy");

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
            DataColumn Service = new DataColumn
            {
                ColumnName = "Service",
                Caption = "Service",
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

            return new DataColumn[] { Service, password, url, user, comments/*, othersExisting, other*/};
        }

        #endregion
        #region IO

        private void AddEntry(DataRow dataRow)
        {
            DataTable tmpPwTbl = data.Tables[PasswordDataTableName];
            tmpPwTbl.Rows.Add(dataRow);
        }
        private void InsertEntry(DataRow dataRow, int rowIndex)
        {
            if (!PwTblRowIndexIsValid(rowIndex)) return;
            DataTable tmpPwTbl = data.Tables[PasswordDataTableName];
            tmpPwTbl.Rows.InsertAt(dataRow, rowIndex);
        }
        public void RemoveEntry(int rowIndex)
        {
            if (!PwTblRowIndexIsValid(rowIndex)) return;
            DataTable tmpPwTbl = data.Tables[PasswordDataTableName];
            tmpPwTbl.Rows.RemoveAt(rowIndex);
        }
        public void DuplicateEntry(int rowIndex)
        {
            // Add out of bounds-warnings (or just dont prompt them to the user?)
            DataTable tmpPwTbl = data.Tables[PasswordDataTableName];
            if (!PwTblRowIndexIsValid(rowIndex)) return;

            // Get row which shall be cloned
            var toCloneRow = tmpPwTbl.Rows[rowIndex].ItemArray;
            List<string> itemArray = new List<string>();
            foreach (object item in toCloneRow)
            {
                itemArray.Add(item.ToString());
            }
            // Add copied row to DataTable
            DataRow clonedRow = tmpPwTbl.NewRow();
            clonedRow.ItemArray = itemArray.ToArray();
            tmpPwTbl.Rows.InsertAt(clonedRow, rowIndex);
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
        public void ExportToCsv(string FilePathAndName)
        {
            bool retry = false;
            do
            {
                retry = false;
                try
                {
                    using (FileStream file = new FileStream(FilePathAndName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(file))
                        {
                            writer.Write(this.ToCsvFormat());
                        }
                    }
                }
                catch (Exception)
                {
                    var response = MessageBox.Show("An error occured during writing the file", "Couldnt write file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    retry = (response == DialogResult.Retry);
                }
            } while (retry);
        }
        public void ExportToXml(string FilePathAndName)
        {
            bool retry = false;
            do
            {
                retry = false;
                try
                {
                    using (FileStream file = new FileStream(FilePathAndName, FileMode.Create))
                    {
                        this.data.WriteXml(file);
                    }
                }
                catch (Exception)
                {
                    var response = MessageBox.Show("An error occured during writing the file", "Couldn't write file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    retry = (response == DialogResult.Retry);
                }
            } while (retry);
        }
        public void ExportToTxt(string FilePathAndName)
        {
            bool retry = false;
            do
            {
                retry = false;
                try
                {
                    using (FileStream file = new FileStream(FilePathAndName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(file))
                        {
                            writer.Write(this.ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    var response = MessageBox.Show("An error occured during writing the file", "Couldn't write file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    retry = (response == DialogResult.Retry);
                }
            } while (retry);
        }

        // Import
        public void ImportFromXml(string FilePathAndName)
        {
            bool retry;
            DataSet tmpDataSet = new DataSet();
            do
            {
                retry = false;
                try
                {
                    using (FileStream file = new FileStream(FilePathAndName, FileMode.Open))
                    {
                        tmpDataSet.ReadXml(file);
                    }
                }
                catch (Exception)
                {
                    var response = MessageBox.Show("An error occured during importing the file", "Couldn't import data", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    retry = (response == DialogResult.Retry);
                }
            } while (retry);

            Console.WriteLine("Finished import:");
            Console.WriteLine(tmpDataSet.ToString());

            this.Clear();
            this.DataSource = tmpDataSet;
            Console.WriteLine("this.data = tmpDataSet:");
            Console.WriteLine(this.ToString());
        }
        
        // Save
        // ...
        
        // Load
        // ...

        #endregion
        #region Other
        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            DataTable pwDataTableHandle = this.data.Tables[PasswordDataTableName];

            // Get max cell content length
            int maxContentLength = 0;
            foreach (DataRow row in pwDataTableHandle.Rows)
            {
                foreach (DataColumn col in pwDataTableHandle.Columns)
                {
                    int tmpLength = row[col.ColumnName].ToString().Length;
                    if (tmpLength > maxContentLength)
                    {
                        maxContentLength = tmpLength;
                    }
                }
            }
            string FormatString = "{" + "0," + "-" + RoundToNextDecade(maxContentLength) + "} ";

            // Get table column header
            var tmpHeaderNames = new StringBuilder();
            foreach (DataColumn column in pwDataTableHandle.Columns)
            {
                tmpHeaderNames.Append(String.Format(FormatString, column.ColumnName));
            }
            content.AppendLine(tmpHeaderNames.ToString());
            content.AppendLine();

            // Get data from table
            foreach (DataRow row in pwDataTableHandle.Rows)
            {
                var tmpRowCells = new List<string>();
                foreach (DataColumn col in pwDataTableHandle.Columns)
                {
                    tmpRowCells.Add(Convert.ToString(row[col.ColumnName]));
                }
                var tmpRow = new StringBuilder();
                foreach (string s in tmpRowCells)
                {
                    tmpRow.Append(String.Format(FormatString, s));
                }
                content.AppendLine(tmpRow.ToString());
            }
            return content.ToString();
        }

        private string ToCsvFormat()
        {
            StringBuilder content = new StringBuilder();
            DataTable pwDataTableHandle = this.data.Tables[PasswordDataTableName];

            // Get table column header
            List<string> tmpHeaderNames = new List<string>();
            foreach (DataColumn column in pwDataTableHandle.Columns)
            {
                tmpHeaderNames.Add(column.ColumnName);
            }
            content.AppendLine(string.Join(";", tmpHeaderNames));

            // Get data from table
            foreach (DataRow row in pwDataTableHandle.Rows)
            {
                var tmpRowCells = new List<string>();
                foreach (DataColumn col in pwDataTableHandle.Columns)
                {
                    tmpRowCells.Add(Convert.ToString(row[col.ColumnName]));
                }

                content.AppendLine(string.Join(";", tmpRowCells));
            }
            return content.ToString();
        }

        /// <summary>Checks if the entered rowIndex is valid in the password data table</summary>
        /// <param name="rowIndex">The index of the row in question.</param>
        /// <returns><c>true</c> when the index is valid, <c>false</c> when its not.</returns>
        private bool PwTblRowIndexIsValid(int rowIndex)
        {
            int rowCount = data.Tables[PasswordDataTableName].Rows.Count;
            return !(rowIndex >= rowCount || rowIndex < 0);
        }
        /// <summary>Returns the to the next decade ceiled value of the input value.</summary>
        public static int RoundToNextDecade(int value)
        {
            return (int)((Math.Ceiling(value / 10.0)) * 10.0);
        }

        #endregion
    }
    public class DataSourceEventArgs : EventArgs
    {
        public DataTable dataTable;
        public DataSourceEventArgs(DataTable dataTable)
        {
            this.dataTable = dataTable;
        }
    }
}
