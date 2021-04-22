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

    #region IDEAS
    // x Dateiendung: van --> Changed to *.dry
    // - Icon: Ein Knochenschlüssel

    // - Random Key gen (im Kontextmenü)
    // - Language Support
    // - Meta-Daten änderbar (See PasswordStorage.cs)
    // - User kann Spaltenbreite anpassen
    // ? User kann spalten hinzufügen
    // - Help-File als HTML oder so
    // - Dark-Mode?
    // - 2-fache sicherung: pw und Datei selbst
    // - "starter"-bat-datei erstellbar per "wizard"
    // - undo-funktion
    // - sauberes exception-handling
    // - Autosave
    // - autosave + close
    // 
    // - Alles asynchron machen?
    // 
    // - Dateiaufbau:
    //   -> Ist PW-Geschützt (true/false)
    //   -> (wenn ja -> Hash vom MasterKey)
    //   -> Daten
    //  
    // - Wenn PW-Geschützt:
    //   -> open masterkey-prompt -> verhashen -> check ob der mit der 2. Zeile in der Datei übereinstimmt

    #endregion


    #region TODOs 
    // TODO: Settings erstellen (Registry)
    // TODO: Languages
    // TODO: Command line args
    // TODO: Help erstellen
    // TODO: Increase security (real cryptography and dft)

    // DONE: Import/Export (XML, txt, csv)
    // DONE: Shortcuts
    // DONE: DataStorage erstellen
    // DONE: Make DataStorage serializable
    // DONE: Save/Load (Serializable)
    #endregion

    public partial class MainForm : Form
    {
        #region Constants
        static string PW_FILE_EXTENSION = ".dry";
        static string[] ALLOWED_EXTENSIONS = new string[] { ".xml", ".csv", ".txt", ".*" };
        static int DEFAULT_FILTERINDEX = 1;
        #endregion
        #region Variables
        PasswordManager.PasswordStorage data;

        string DefaultSaveFileNameAndPath;
        bool justSaved;

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
            InitializeGUI();

            // Create PasswordStorage
            data = new PasswordStorage();

            data.DataSourceChanged += Data_DataSourceChanged;

            InitializeDataGridView();
            InitializeMainSplitContainer();
            this.dataGrid.StickToParentBoundaries();

            this.KeyDown += MainForm_KeyDown;
            this.dataGrid.KeyDown += MainForm_KeyDown;

            // ...
        }
        private void InitializeGUI()
        {
            // Initialize everything with GUI

            // Initialize menu item tooltips
            MenuFileNew.ToolTipText = (Keys.Control.ToString() + " + " + Keys.N.ToString());
            MenuFileLoad.ToolTipText = (Keys.Control.ToString() + " + " + Keys.O.ToString());
            MenuFileSave.ToolTipText = (Keys.Control.ToString() + " + " + Keys.S.ToString());
            MenuFileSaveAs.ToolTipText = (Keys.Control.ToString() + " + " + Keys.Shift.ToString() + " + " + Keys.S.ToString());
            MenuFileExport.ToolTipText = (Keys.Control.ToString() + " + " + Keys.E.ToString());
            MenuFileImport.ToolTipText = (Keys.Control.ToString() + " + " + Keys.I.ToString());
            MenuFileExit.ToolTipText = (Keys.Alt.ToString() + " + " + Keys.F4.ToString());

            MenuEditDuplicate.ToolTipText = (Keys.Control.ToString() + " + " + Keys.D.ToString());
            MenuEditRemove.ToolTipText = (Keys.Control.ToString() + " + " + Keys.X.ToString());

            MenuHelp.AutoToolTip = true;
            MenuHelp.ToolTipText = Keys.F1.ToString();

            // ...
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
            this.dataGrid.CellValueChanged += DataGrid_CellValueChanged;
        }
        private void InitializeMainSplitContainer()
        {
            this.MainSplitContainer.BorderStyle = BorderStyle.FixedSingle;
            this.MainSplitContainer.Panel2Collapsed = true;
            this.MainSplitContainer.Panel1.Controls.Add(this.dataGrid);
        }

        #endregion
        #region Events
        private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            justSaved = false;
            MenuFileSave.Enabled = !justSaved;
        }
        private void Data_DataSourceChanged(object sender, DataSourceEventArgs e)
        {
            this.dataGrid.Source = this.data.GetPasswordDataTable();
            this.dataGrid.Refresh();
        }
        #endregion
        #region Menu actions
        private void MenuHelp_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Started MenuHelp_Click(...)");
            //Console.WriteLine(this.data.ToString());

            Cryptographer c = new Cryptographer(PasswordStorage.TEMP_TEST_MASTERKEY);
            string encr = "Test!?tseT";
            Console.WriteLine($"{encr} => {c.Encrypt(encr)} => {c.Decrypt(c.Encrypt(encr))}");

            string pp = "ppas|" + encr + "|ollse213";
            Console.WriteLine($"RemoveSubString(...) = {PasswordStorage.RemoveSubString(pp, encr)}");



            Console.WriteLine("Ended   MenuHelp_Click(...)");
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
        }

        #region Shortkeys
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                Console.WriteLine(String.Format("e.Keycode:\t", e.KeyCode.ToString()));
                switch (e.KeyCode)
                {
                    case Keys.S:
                        if (!justSaved)
                        {
                            MenuFileSave_Click(this, EventArgs.Empty);
                        }
                        else if (e.Shift)
                        {
                            MenuFileSaveAs_Click(this, EventArgs.Empty);
                        }
                        return;
                    case Keys.N:
                        MenuFileNew_Click(this, EventArgs.Empty);
                        return;
                    case Keys.O:
                        MenuFileLoad_Click(this, EventArgs.Empty);
                        return;
                    case Keys.I:
                        MenuFileImport_Click(this, EventArgs.Empty);
                        return;
                    case Keys.E:
                        MenuFileExport_Click(this, EventArgs.Empty);
                        return;
                    case Keys.D:
                        MenuEditDuplicate_Click(this, EventArgs.Empty);
                        return;
                    case Keys.X:
                        MenuEditRemove_Click(this, EventArgs.Empty);
                        return;
                }
            }
            else if (e.Alt && e.KeyCode == Keys.F4)
            {
                MenuFileExit_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.F1)
            {
                MenuHelp_Click(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Save/Load (Serialization)
        private void MenuFileLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Load file ...";
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
                // ofd.FilterIndex doesnt need to be set here (default = 1)

                var result = ofd.ShowDialog();
                string FileNameAndPath = ofd.FileName;
                bool ok = FileNameAndPath.EndsWith(PW_FILE_EXTENSION);

                if (result != DialogResult.OK)
                {
                    return;
                }
                else if (!ok)
                {
                    _ = MessageBox.Show(String.Format("Can't load files with the file extension \"{0}\".",
                                        System.IO.Path.GetExtension(FileNameAndPath)),
                                        "File loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((ofd.FileNames.Length > 1))
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
                justSaved = true;
                MenuFileSave.Enabled = !justSaved;
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

            justSaved = true;
            MenuFileSave.Enabled = !justSaved;
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
                else if (!ok)
                {
                    _ = MessageBox.Show(String.Format("Can't save files with the file extension \"{0}\".",
                                        System.IO.Path.GetExtension(FileNameAndPath)),
                                        "File saving error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((sfd.FileNames.Length > 1))
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
                justSaved = true;
                MenuFileSave.Enabled = !justSaved;
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
                }
                ofd.Filter = string.Join("|", filters);
                // ofd.FilterIndex doesnt need to be set here (default = 1)

                // Open and check file
                var result = ofd.ShowDialog();
                string FileNameAndPath = ofd.FileName;

                bool ok = (FileNameAndPath.EndsWith(ALLOWED_EXTENSIONS[0]) || FileNameAndPath.EndsWith(ALLOWED_EXTENSIONS[1]));
                if (result != DialogResult.OK)
                {
                    return;
                }
                else if (!ok)
                {
                    _ = MessageBox.Show(String.Format("Can't import files with the file extension \"{0}\".",
                                        System.IO.Path.GetExtension(FileNameAndPath)),
                                        "File importing error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((ofd.FileNames.Length > 1))
                {
                    _ = MessageBox.Show("Something went wrong while importing", "File importing error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string ext = System.IO.Path.GetExtension(FileNameAndPath);  // FileNameAndPath.Split('.')[FileNameAndPath.Split('.').Length - 1];
                try
                {
                    switch (ext.ToLower().Trim('.'))
                    {
                        case "xml":
                            this.data.ImportFromXmlFile(FileNameAndPath);
                            break;
                        case "csv":
                            this.data.ImportFromCsv(FileNameAndPath);
                            break;
                        default:
                            throw new Exception(string.Format("Unsupported file extension {0}!", ext));
                    }
                    return;
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(String.Format("Something went wrong while exporting:\n{0}", ex.Message), "File export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
                    filters.Add("TXT file (*.txt)");
                    filters.Add("*" + ALLOWED_EXTENSIONS[2]);
                }
                sfd.Filter = string.Join("|", filters);
                sfd.FilterIndex = DEFAULT_FILTERINDEX;

                // Open and check file
                var result = sfd.ShowDialog();
                string FileNameAndPath = sfd.FileName;
                bool ok = FileNameAndPath.EndsWith(ALLOWED_EXTENSIONS[sfd.FilterIndex - 1]);
                string ext = System.IO.Path.GetExtension(FileNameAndPath);  // FileNameAndPath.Split('.')[FileNameAndPath.Split('.').Length - 1];

                if (result != DialogResult.OK)
                {
                    return;
                }
                else if (!ok)
                {
                    _ = MessageBox.Show(String.Format("Unsupported file extension \"{0}\"!", ext), "File export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((sfd.FileNames.Length > 1))
                {
                    _ = MessageBox.Show("Something went wrong while exporting", "File export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    switch (ext.ToLower().Trim('.'))
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
                            throw new Exception(string.Format("Unsupported file extension \"{0}\"!", ext));
                    }
                    return;
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(String.Format("Something went wrong while exporting:\n{0}", ex.Message), "File export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        #endregion
        #region Menu Edit
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
        private void MenuFileNew_Click(object sender, EventArgs e)
        {
            // TODO: Add safety functions like "are you sure" and "unsaved data"
            this.data = new PasswordStorage();
            data.DataSourceChanged += Data_DataSourceChanged;
            RefreshDataGridView();
        }
        private void MenuFileExit_Click(object sender, EventArgs e)
        {
            // TODO: Add safety functions like "are you sure" and "unsaved data"
            Application.Exit();
        }
        #endregion

        #endregion
    }
}
