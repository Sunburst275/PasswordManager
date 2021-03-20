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
    public partial class StickyDataGridView : DataGridView
    {
        #region Constants
        #endregion
        #region Variables
        private SplitContainer parent;
        private BindingSource innerBindingSource;
        #endregion
        #region Properties
        private DataTable _Source;
        public DataTable Source
        {
            get
            {
                return _Source;
            }
            set
            {
                _Source = value;
                innerBindingSource = new BindingSource();
                innerBindingSource.DataSource = _Source;
                DataSource = innerBindingSource;
            }

        }
        #endregion
        #region Delegates, Events, etc.
        #endregion
        #region Constructor, Initialization, etc.
        public StickyDataGridView(ref SplitContainer parent) : base()
        {
            this.parent = parent;
            this.parent.SizeChanged += Parent_SizeChanged;
            InitializeCustom();
        }
        public StickyDataGridView(ref SplitContainer parent, DataTable source) : this(ref parent)
        {
            this.Source = source;
        }
        public StickyDataGridView(ref SplitContainer parent, DataTable source, string Name) : this(ref parent, source)
        {
            this.Name = Name;
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Registered parent size change");
        }

        private void InitializeCustom()
        {
            InitializeGUI();

            this.Visible = true;
            this.Enabled = true;

        }

        private void InitializeGUI()
        {
            // Appearance
            BorderStyle = BorderStyle.FixedSingle;
            CellBorderStyle = DataGridViewCellBorderStyle.Single;
            ColumnHeadersVisible = true;
            RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            ShowCellErrors = true;
            ShowCellToolTips = true;
            // Behavior
            AllowDrop = true;
            AllowUserToOrderColumns = true;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            MultiSelect = true;
            SelectionMode = DataGridViewSelectionMode.CellSelect;
            // Design
            // ...
            // Layout
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Dock = DockStyle.Fill;
        }

        #endregion

    }
}
