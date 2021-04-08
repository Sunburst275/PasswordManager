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

    // TODO: DataStorage erstellen
    // TODO: Make DataStorage serializable
    // TODO: Settings erstellen (Registry)
    // TODO: Languages
    // TODO: Import/Export (XML, txt, csv)
    // TODO: Save/Load (Serializable)
    // TODO: Command line args

    public partial class MainForm : Form
    {
        #region Constants
        static string[] ALLOWED_EXTENSIONS = new string[] { ".xml", ".csv", ".txt" };
        static int DEFAULT_FILTERINDEX = 1;
        #endregion
        #region Variables
        PasswordManager.PasswordStorage data;
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
            Console.WriteLine("Data_DataSourceChanged(object sender, DataSourceEventArgs e)");
            this.dataGrid.Source = e.dataTable;
            this.dataGrid.Refresh();
        }

        private void InitializeDataGridView()
        {
            this.dataGrid = new StickyDataGridView(ref MainSplitContainer)
            {
                Source = data.GetPasswordDataTable()
            };
            this.dataGrid.StickToParentBoundaries();
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

        }

        private void MenuFileSave_Click(object sender, EventArgs e)
        {

        }

        private void MenuFileSaveAs_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region Import/Export
        private void FileMenuImport_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog fod = new OpenFileDialog())
            {
                fod.Title = "Import file ...";
                fod.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fod.CheckPathExists = true;
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
                fod.Filter = string.Join("|", filters);
                fod.FilterIndex = DEFAULT_FILTERINDEX;

                // Open and check file
                var result = fod.ShowDialog();
                string FileNameAndPath = fod.FileName;
                bool ok = FileNameAndPath.EndsWith(ALLOWED_EXTENSIONS[fod.FilterIndex - 1]);

                if ((result != DialogResult.OK) ||
                    (fod.FileNames.Length > 1) ||
                    !ok)
                {
                    _ = MessageBox.Show("Something went wrong while trying to access the file", "File access error");
                    return;
                }

                this.data.ImportFromXml(FileNameAndPath);

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

                if ((result != DialogResult.OK) ||
                    (sfd.FileNames.Length > 1) ||
                    !ok)
                {
                    _ = MessageBox.Show("Something went wrong while trying to access the file", "File access error");
                    return;
                }

                string ext = FileNameAndPath.Split('.')[FileNameAndPath.Split('.').Length - 1];
                try
                {
                    switch (ext.ToLower())
                    {
                        case "xml":
                            data.ExportToXml(FileNameAndPath);
                            break;
                        case "csv":
                            data.ExportToCsv(FileNameAndPath);
                            break;
                        case "txt":
                            data.ExportToTxt(FileNameAndPath);
                            break;
                        default:
                            break;
                    }
                    return;
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Something went wrong while trying to access the file", "File access error");
                    return;
                }
            }
        }
        #endregion

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
