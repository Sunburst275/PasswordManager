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

    // TODO: DataGridView als readonly machen.
    // TODO: Das Panel1 vom SplitContainer ist dafuer da, dass man da die Daten, die man Anwaehlt, aendern und bearbeiten kann.
    // TODO: DataStorage erstellen

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
        StickyDataGridView DataGridView;
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
            InitializePanel1();
            // ...
        }

        private void InitializeDataGridView()
        {
            this.DataGridView = new StickyDataGridView(ref MainSplitContainer);
            // Bind PasswordStorage to DataGridView
            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = data.GetPasswordDataTable();
            //this.DataGridView.DataSource = bindingSource;

            this.DataGridView.Source = data.GetPasswordDataTable();

            //this.DataGridView.Columns["Other"].Visible = false;
        }

        private void InitializePanel1()
        {
            this.MainSplitContainer.BorderStyle = BorderStyle.FixedSingle;
            this.MainSplitContainer.Panel2Collapsed = true;
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

        private void MenuFileLoad_Click(object sender, EventArgs e)
        {

        }

        private void MenuFileSave_Click(object sender, EventArgs e)
        {

        }

        private void MenuFileSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void MenuFileExport_Click(object sender, EventArgs e)
        {

        }

        private void MenuFileExit_Click(object sender, EventArgs e)
        {
            // TODO: Add safety functions like "are you sure" and "unsaved data"

            Application.Exit();
        }
        #endregion

    }
}
