using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;

namespace PasswordManager
{

    #region TODOs 
    // TODO: DataStorage erstellen
    // TODO: Make DataStorage serializable
    // TODO: Settings erstellen (Registry)
    // TODO: Languages
    // TODO: Save/Load (Serializable)
    // TODO: Command line args

    // DONE: Import/Export (XML, txt, csv)
    #endregion

    public partial class MainForm : Form
    {
        #region Constants
        static string PW_FILE_EXTENSION = ".i";
        static string[] ALLOWED_EXTENSIONS = new string[] { ".xml", ".csv", ".txt" };
        static int DEFAULT_FILTERINDEX = 1;
        #endregion
        #region Variables
        PasswordManager.PasswordStorage data;
        string DefaultSaveFileNameAndPath;
        #endregion
        #region Delegates, Events, etc.
        #endregion
        #region GIO components
        StickyDataGridView dataGrid;
        #endregion
        #region Constructor, Initialization, etc.

        public MainForm()
        {
            InitializeComponent();
        }

        // CONT: Import now broken?
        // CONT: Datagrid refresh broken (when loading new file)
        // CONT: load works but somehow doesnt

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Create PasswordStorage
            data = new PasswordStorage();
            data.DataSourceChanged += Data_DataSourceChanged;
            InitializeDataGridView();
            InitializeMainSplitContainer();
            this.dataGrid.StickToParentBoundaries();
            
            // ...
        }

        private void Data_DataSourceChanged(object sender, DataSourceEventArgs e)
        {
            this.dataGrid.Source = e.dataTable;
            this.dataGrid.Refresh();
        }

        private void RefreshDataGridView()
        {
            this.dataGrid.Source = data.GetPasswordDataTable();
            this.dataGrid.StickToParentBoundaries();
            this.dataGrid.Refresh();
        }

        private void InitializeDataGridView()
        {
            this.dataGrid = new StickyDataGridView(ref MainSplitContainer)
            {
                Source = data.GetPasswordDataTable()
            };
            this.dataGrid.StickToParentBoundaries();
            this.dataGrid.Refresh();

            // Bind PasswordStorage to DataGridView
            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = data.GetPasswordDataTable();
            //this.DataGridView.DataSource = bindingSource;

            //this.DataGridView.Columns["Other"].Visible = false;
        }

        private void InitializeMainSplitContainer()
        {
            this.MainSplitContainer.BorderStyle = BorderStyle.FixedSingle;
            this.MainSplitContainer.Panel2Collapsed = true;
            this.MainSplitContainer.Panel1.Controls.Add(this.dataGrid);
        }
        #endregion

        #region Menu actions
        private void MenuHelp_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Started MenuHelp_Click(...)");
            Console.WriteLine(this.data.ToString());
            Console.WriteLine("Ended   MenuHelp_Click(...)");
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {

        }

        #region Save/Load (Serialization)
        private void MenuFileLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Save file ...";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                ofd.CheckPathExists = true;
                StringBuilder filters = new StringBuilder();
                {
                    filters.Append(this.Text);
                    filters.Append(" files (");
                    filters.Append("*" + PW_FILE_EXTENSION);
                    filters.Append(")|");
                    filters.Append("*" + PW_FILE_EXTENSION);
                }
                ofd.Filter = filters.ToString();
                ofd.FilterIndex = DEFAULT_FILTERINDEX;

                var result = ofd.ShowDialog();
                string FileNameAndPath = ofd.FileName;
                bool ok = FileNameAndPath.EndsWith(PW_FILE_EXTENSION);

                if (result != DialogResult.OK)
                {
                    return;
                }
                else if ((ofd.FileNames.Length > 1) || !ok)
                {
                    _ = MessageBox.Show("Something went wrong while trying to access the file", "File access error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DefaultSaveFileNameAndPath = String.Empty;
                    return;
                }

                try
                {
                    data = PasswordStorage.DeSerialize(FileNameAndPath);
                    dataGrid.Source = data.GetPasswordDataTable();
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Something went wrong while trying to read the file", "File read error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DefaultSaveFileNameAndPath = String.Empty;
                    return;
                }

                // File path is valid, so fast saving should be possible
                DefaultSaveFileNameAndPath = FileNameAndPath;

            }
        }
        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            // File path is invalid, so fast saving should is not possible -> Open "Save as..."
            if (string.IsNullOrEmpty(DefaultSaveFileNameAndPath) ||
                !System.IO.File.Exists(DefaultSaveFileNameAndPath))
            {
                MenuFileSaveAs_Click(this, EventArgs.Empty);
                return;
            }

            try
            {
                data.Serialize(DefaultSaveFileNameAndPath);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Something went wrong while trying to write the file", "File write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DefaultSaveFileNameAndPath = String.Empty;
                return;
            }
        }
        private void MenuFileSaveAs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save file ...";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.CheckPathExists = true;
                StringBuilder filters = new StringBuilder();
                {
                    filters.Append(this.Text);
                    filters.Append(" files (");
                    filters.Append("*" + PW_FILE_EXTENSION);
                    filters.Append(")|");
                    filters.Append("*" + PW_FILE_EXTENSION);
                }
                sfd.Filter = filters.ToString();
                sfd.FilterIndex = DEFAULT_FILTERINDEX;

                var result = sfd.ShowDialog();
                string FileNameAndPath = sfd.FileName;
                bool ok = FileNameAndPath.EndsWith(PW_FILE_EXTENSION);

                if (result != DialogResult.OK)
                {
                    return;
                }
                else if ((sfd.FileNames.Length > 1) || !ok)
                {
                    _ = MessageBox.Show("Something went wrong while trying to write the file", "File write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DefaultSaveFileNameAndPath = String.Empty;
                    return;
                }

                try
                {
                    data.Serialize(FileNameAndPath);
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Something went wrong while trying to write the file", "File write error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DefaultSaveFileNameAndPath = String.Empty;
                    return;
                }

                // File path is valid, so fast saving should be possible
                DefaultSaveFileNameAndPath = FileNameAndPath;

            }
        }
        #endregion
        #region Import/Export
        private void MenuFileImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Import file ...";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                ofd.CheckPathExists = true;
                List<string> filters = new List<string>();
                {
                    filters.Add("XML file (*.xml)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[0]);
                    filters.Add("CSV file (*.csv)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[1]);
                    filters.Add("Text file (*.txt)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[2]);
                    filters.Add("All files (*.*)");
                    filters.Add("*.*");
                }
                ofd.Filter = string.Join("|", filters);
                ofd.FilterIndex = DEFAULT_FILTERINDEX;

                // Open and check file
                var result = ofd.ShowDialog();
                string FileNameAndPath = ofd.FileName;
                bool ok = FileNameAndPath.EndsWith(ALLOWED_EXTENSIONS[ofd.FilterIndex - 1]);

                if (result != DialogResult.OK)
                {
                    return;
                }
                else if ((ofd.FileNames.Length > 1) || !ok)
                {
                    _ = MessageBox.Show("Something went wrong while importing", "File importing error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.data.ImportFromXmlString(FileNameAndPath);

            }
        }

        private void MenuFileExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Export file ...";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.CheckPathExists = true;
                List<string> filters = new List<string>();
                {
                    filters.Add("XML file (*.xml)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[0]);
                    filters.Add("CSV file (*.csv)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[1]);
                    filters.Add("Text file (*.txt)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[2]);
                    filters.Add("All files (*.*)");
                    filters.Add("*.*");
                }
                sfd.Filter = string.Join("|", filters);
                sfd.FilterIndex = DEFAULT_FILTERINDEX;

                // Open and check file
                var result = sfd.ShowDialog();
                string FileNameAndPath = sfd.FileName;
                bool ok = FileNameAndPath.EndsWith(ALLOWED_EXTENSIONS[sfd.FilterIndex - 1]);

                if (result != DialogResult.OK)
                {
                    return;
                }
                else if ((sfd.FileNames.Length > 1) || !ok)
                {
                    _ = MessageBox.Show("Something went wrong while exporting", "File export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string ext = FileNameAndPath.Split('.')[FileNameAndPath.Split('.').Length - 1];
                try
                {
                    switch (ext.ToLower())
                    {
                        case "xml":
                            data.ExportToXmlFile(FileNameAndPath);
                            break;
                        case "csv":
                            data.ExportToCsvFile(FileNameAndPath);
                            break;
                        case "txt":
                            data.ExportToTxtFile(FileNameAndPath);
                            break;
                        default:
                            break;
                    }
                    return;
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Something went wrong while exporting", "File export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        #endregion

        private void MenuFileNew_Click(object sender, EventArgs e)
        {
            // TODO: Add safety functions like "are you sure" and "unsaved data"
            this.data = new PasswordStorage();
            RefreshDataGridView();
        }

        private void MenuFileExit_Click(object sender, EventArgs e)
        {
            // TODO: Add safety functions like "are you sure" and "unsaved data"
            Application.Exit();
        }

        private void MenuEditDuplicate_Click(object sender, EventArgs e)
        {
            data.DuplicateEntry(dataGrid.CurrentRow.Index);
        }
       
        private void MenuEditRemove_Click(object sender, EventArgs e)
        {
            // TODO: Warning, this removes this row
            data.RemoveEntry(dataGrid.CurrentRow.Index);
        }
        #endregion

        #region Other
        #endregion

    }
}
