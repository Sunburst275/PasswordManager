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
        // TODO: Make columns resizable!

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
                _Source = null;
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
            this.CellValueChanged += StickyDataGridView_CellValueChanged;
            InitializeCustom();
            InitializeDebug();
        }

        private void StickyDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            StickToParentBoundaries();
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
            StickToParentBoundaries();
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
            AllowUserToResizeColumns = false;
            AllowUserToOrderColumns = true;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            MultiSelect = true;
            SelectionMode = DataGridViewSelectionMode.CellSelect;
            // Design
            // ...
            // Layout
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Dock = DockStyle.Fill;

            this.ScrollBars = ScrollBars.Both;

            StickToParentBoundaries();
        }
        private void InitializeDebug()
        {
            //this.BackgroundColor = Color.Red;
        }
        #endregion
        #region Control

        public void StickToParentBoundaries()
        {
            if (this.Columns.Count <= 0) return;

            // Get parent size
            var pWidth = parent.Width;
            var pHeight = parent.Height;

            // -----< Columns >------------------ 
            // Calculate column widths
            //var minWidth = (pWidth / this.Columns.Count);

            // Apply columnwidths
            int accumulatedWidth = 0;
            foreach(DataGridViewColumn col in this.Columns)
            {
                if(col.Width < col.MinimumWidth)
                {
                    col.Width = col.MinimumWidth;
                }
                accumulatedWidth += col.Width;
            }
            // Set last column length to rest of the parent width
            var lastColWidth = this.Columns[this.Columns.Count - 1].Width;
            this.Columns[this.Columns.Count - 1].Width = pWidth - (accumulatedWidth - lastColWidth) - this.RowHeadersWidth; // TODO: Include width of the first header (upper left corner stuff)
            // -----< Rows >--------------------- 
            //int accumulatedHeight = 0;
            //foreach(DataGridViewRow row in this.Rows)
            //{
            //    if(row.Height < row.MinimumHeight)
            //    {
            //        row.Height = row.MinimumHeight;
            //    }
            //    accumulatedHeight += row.Height;
            //}

            //// Set last row length to rest of the parent height
            //var lastRowHeight = this.Rows[this.Rows.Count - 1].Height;
            //this.Rows[this.Rows.Count - 1].Height = pHeight - (accumulatedHeight - lastRowHeight);
        }

        #endregion
    }
}
