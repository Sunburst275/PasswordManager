using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            InitializeDataGridView();
            InitializeMainSplitContainer();
            this.dataGrid.StickToParentBoundaries();

            // ...
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

        }

        private void MenuFileExport_Click(object sender, EventArgs e)
        {

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

        
    }
}
