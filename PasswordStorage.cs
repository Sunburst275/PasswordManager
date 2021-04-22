using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace PasswordManager
{
    // TODO: When "Failed to import/load/export/save" and Cancel is clicked, DataSet is eradicated and DataGridView has no cells anymore -> FIX!

    [Serializable()]
    class PasswordStorage
    {
        #region Constants
        const string DataContainerName = "PasswordManagerData";
        const string MetaDataTableName = "MetaData";
        const string PasswordDataTableName = "PasswordData";
        public const string TEMP_TEST_MASTERKEY = "[1234567890)--{";
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
        public void Clear()
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
            //DataTable tmpPwTbl = data.Tables[1];
            //tmpPwTbl.Rows.Add(new object[] { "Test1", "Test2", "Test3", "Test4", "Test5" });
            //tmpPwTbl.Rows.Add(new object[] { "Google", "Kaggne123", "google.de", "Tomboy", "This is a test" });
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

            // Columns: 
            // o Column "User" enabled/visible?
            // o Column "Service" enabled/visible?
            // o Column "Link" enabled/visible?
            // o Column "Username" enabled/visible?
            // o Column "E-Mail" enabled/visible?
            // o Column "Password" enabled/visible?
            // o Column "Comments" enabled/visible?
            // o Ist PW-Geschützt (true/false)?
            // o Password-Columns shows only dots or clear text?
            // -> 
            // -> DataSet-Name (for example "Himemis Passwörter")
            // -> "Remarks?

            // Wird in Registry gespeichert:
            // o Dark-Mode enabled?
            // o Autosave activated?
            // o Autosave activated?

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
            DataColumn user = new DataColumn
            {
                ColumnName = "User",
                Caption = "User",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn service = new DataColumn
            {
                ColumnName = "Service",
                Caption = "Service",
                AllowDBNull = false,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty,
            };
            DataColumn url = new DataColumn
            {
                ColumnName = "Link (URL)",
                Caption = "Link (URL)",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn username = new DataColumn
            {
                ColumnName = "Username",
                Caption = "Username",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn email = new DataColumn
            {
                ColumnName = "E-Mail",
                Caption = "E-Mail",
                AllowDBNull = true,
                DataType = System.Type.GetType("System.String"),
                DefaultValue = String.Empty
            };
            DataColumn password = new DataColumn
            {
                ColumnName = "Password",
                Caption = "Password",
                AllowDBNull = false,
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

            return new DataColumn[] { user, service, url, username, email, password, comments/*, othersExisting, other*/};
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
        public void ExportToCsvFile(string FilePathAndName)
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
        public void ExportToXmlFile(string FilePathAndName)
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
        public void ExportToTxtFile(string FilePathAndName)
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
        public void ImportFromXmlFile(string FilePathAndName)
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

            this.Clear();
            this.DataSource = tmpDataSet;
        }
        public void ImportFromXmlString(string xml_string)
        {
            bool retry;
            DataSet tmpDataSet = new DataSet();
            do
            {
                retry = false;
                try
                {
                    tmpDataSet.ReadXml(XmlReader.Create(new StringReader(xml_string)));
                }
                catch (Exception)
                {
                    var response = MessageBox.Show("An error occured during importing the file", "Couldn't import data", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    retry = (response == DialogResult.Retry);
                }
            } while (retry);

            this.Clear();
            this.DataSource = tmpDataSet;
        }
        public void ImportFromCsv(string FilePathAndName)
        {
            _ = MessageBox.Show("Cant import from CSV yet ¯\\_(ツ)_ /¯", "Not implemented yet", MessageBoxButtons.OK, MessageBoxIcon.Information);
            throw new NotImplementedException("Cant import from CSV yet ¯\\_(ツ)_/¯");
        }

        #endregion
        #region Serialization
        public void Serialize(string FilePathAndName)
        {
            bool retry = false;
            byte[] xml_bytes;
            do
            {
                retry = false;
                try
                {
                    using (FileStream file = new FileStream(FilePathAndName, FileMode.Create))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            this.data.WriteXml(ms);
                            xml_bytes = ms.ToArray();
                        }

                        Cryptographer c = new Cryptographer(TEMP_TEST_MASTERKEY);
                        string encrypted_data = c.Encrypt(xml_bytes);

                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            var x = Encoding.UTF8.GetString(c.MasterKeyHash);
                            sw.WriteLine(x);
                            sw.WriteLine(encrypted_data);
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
        public static PasswordStorage DeSerialize(string FilePathAndName)
        {
            bool retry = false;
            string encrypted_data, decrypted_data;
            do
            {
                retry = false;
                try
                {
                    // Read
                    using (FileStream file = new FileStream(FilePathAndName, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            encrypted_data = sr.ReadToEnd();
                        }
                    }

                    Cryptographer c = new Cryptographer(TEMP_TEST_MASTERKEY);

                    if (!encrypted_data.Contains(c.MasterKeyHashString))
                        throw new Exception("No CryptoClearance!"); // TODO: Change to good crypto-exception and explain to user that its the wrong password when this is thrown!

                    // Remove MasterKeyHash (and possible carriage returns, newlines, ...)
                    encrypted_data = RemoveSubString(encrypted_data, c.MasterKeyHashString).Trim(new char[] { '\r', '\n' });
                    decrypted_data = c.Decrypt(encrypted_data);

                    // Build tmp xml stuff
                    PasswordStorage tmp_storage = new PasswordStorage();
                    tmp_storage.ImportFromXmlString(decrypted_data);
                    return tmp_storage;
                }
                catch (Exception)
                {
                    var response = MessageBox.Show("An error occured during reading the file", "Couldn't read file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    retry = (response == DialogResult.Retry);
                }
            } while (retry);

            return null;
        }
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

        /// <summary>Removes a substring from a string.</summary>
        /// <param name="str">The string which has a <paramref name="substring"/> to be removed.</param>
        /// <param name="substring">Substring that shall be removed in <paramref name="str"/>.</param>
        /// <returns>The input string with the substring removed.</returns>
        public static string RemoveSubString(string str, string substring)
        {
            var strArr = str.ToCharArray();
            var subArr = substring.ToCharArray();
            bool stringFound = false;

            // Search for start
            int i = 0, k = 0, equalChars = 0;
            while ((i < strArr.Length) && ((k + subArr.Length) <= strArr.Length) && !stringFound)
            {
                if (strArr[i + k] == subArr[i])
                {
                    equalChars++;
                    i++;

                    if (equalChars == subArr.Length)
                        stringFound = true;
                }
                else
                {
                    equalChars = i = 0;
                    k++;
                }
            }

            return str.Remove(k, equalChars);
        }

        #endregion
        #region Crypto-Stuff

        //private void EncryptPasswords()
        //{

        //}
        //private void DecryptPasswords()
        //{

        //}

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
