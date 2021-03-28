
namespace PasswordManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEditDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEditSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuEditRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.FileMenuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuEdit,
            this.MenuSettings,
            this.MenuHelp});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenu.Size = new System.Drawing.Size(1067, 28);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "MainMenu";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFileLoad,
            this.MenuFileSave,
            this.MenuFileSaveAs,
            this.MenuFileSep1,
            this.FileMenuImport,
            this.MenuFileExport,
            this.MenuFileSep2,
            this.MenuFileExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(46, 24);
            this.MenuFile.Text = "File";
            // 
            // MenuFileLoad
            // 
            this.MenuFileLoad.Name = "MenuFileLoad";
            this.MenuFileLoad.Size = new System.Drawing.Size(224, 26);
            this.MenuFileLoad.Text = "Load";
            this.MenuFileLoad.Click += new System.EventHandler(this.MenuFileLoad_Click);
            // 
            // MenuFileSave
            // 
            this.MenuFileSave.Name = "MenuFileSave";
            this.MenuFileSave.Size = new System.Drawing.Size(224, 26);
            this.MenuFileSave.Text = "Save";
            this.MenuFileSave.Click += new System.EventHandler(this.MenuFileSave_Click);
            // 
            // MenuFileSaveAs
            // 
            this.MenuFileSaveAs.Name = "MenuFileSaveAs";
            this.MenuFileSaveAs.Size = new System.Drawing.Size(224, 26);
            this.MenuFileSaveAs.Text = "Save as ...";
            this.MenuFileSaveAs.Click += new System.EventHandler(this.MenuFileSaveAs_Click);
            // 
            // MenuFileSep1
            // 
            this.MenuFileSep1.Name = "MenuFileSep1";
            this.MenuFileSep1.Size = new System.Drawing.Size(221, 6);
            // 
            // MenuFileExport
            // 
            this.MenuFileExport.Name = "MenuFileExport";
            this.MenuFileExport.Size = new System.Drawing.Size(224, 26);
            this.MenuFileExport.Text = "Export ...";
            this.MenuFileExport.Click += new System.EventHandler(this.MenuFileExport_Click);
            // 
            // MenuFileSep2
            // 
            this.MenuFileSep2.Name = "MenuFileSep2";
            this.MenuFileSep2.Size = new System.Drawing.Size(221, 6);
            // 
            // MenuFileExit
            // 
            this.MenuFileExit.Name = "MenuFileExit";
            this.MenuFileExit.Size = new System.Drawing.Size(224, 26);
            this.MenuFileExit.Text = "Exit";
            this.MenuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);
            // 
            // MenuEdit
            // 
            this.MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuEditDuplicate,
            this.MenuEditSep1,
            this.MenuEditRemove});
            this.MenuEdit.Name = "MenuEdit";
            this.MenuEdit.Size = new System.Drawing.Size(49, 24);
            this.MenuEdit.Text = "Edit";
            // 
            // MenuEditDuplicate
            // 
            this.MenuEditDuplicate.Name = "MenuEditDuplicate";
            this.MenuEditDuplicate.Size = new System.Drawing.Size(224, 26);
            this.MenuEditDuplicate.Text = "Duplicate";
            this.MenuEditDuplicate.Click += new System.EventHandler(this.MenuEditDuplicate_Click);
            // 
            // MenuEditSep1
            // 
            this.MenuEditSep1.Name = "MenuEditSep1";
            this.MenuEditSep1.Size = new System.Drawing.Size(221, 6);
            // 
            // MenuEditRemove
            // 
            this.MenuEditRemove.Name = "MenuEditRemove";
            this.MenuEditRemove.Size = new System.Drawing.Size(224, 26);
            this.MenuEditRemove.Text = "Remove";
            this.MenuEditRemove.Click += new System.EventHandler(this.MenuEditRemove_Click);
            // 
            // MenuSettings
            // 
            this.MenuSettings.Name = "MenuSettings";
            this.MenuSettings.Size = new System.Drawing.Size(76, 24);
            this.MenuSettings.Text = "Settings";
            this.MenuSettings.Click += new System.EventHandler(this.MenuSettings_Click);
            // 
            // MenuHelp
            // 
            this.MenuHelp.Name = "MenuHelp";
            this.MenuHelp.Size = new System.Drawing.Size(55, 24);
            this.MenuHelp.Text = "Help";
            this.MenuHelp.Click += new System.EventHandler(this.MenuHelp_Click);
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 28);
            this.MainSplitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.MainSplitContainer.Size = new System.Drawing.Size(1067, 526);
            this.MainSplitContainer.SplitterDistance = 328;
            this.MainSplitContainer.SplitterWidth = 5;
            this.MainSplitContainer.TabIndex = 1;
            // 
            // FileMenuImport
            // 
            this.FileMenuImport.Name = "FileMenuImport";
            this.FileMenuImport.Size = new System.Drawing.Size(224, 26);
            this.FileMenuImport.Text = "Import from ...";
            this.FileMenuImport.Click += new System.EventHandler(this.FileMenuImport_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Password Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuEdit;
        private System.Windows.Forms.ToolStripMenuItem MenuHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuFileLoad;
        private System.Windows.Forms.ToolStripMenuItem MenuFileSave;
        private System.Windows.Forms.ToolStripMenuItem MenuFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem MenuFileExit;
        private System.Windows.Forms.ToolStripSeparator MenuFileSep2;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.ToolStripSeparator MenuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem MenuFileExport;
        private System.Windows.Forms.ToolStripMenuItem MenuEditRemove;
        private System.Windows.Forms.ToolStripMenuItem MenuEditDuplicate;
        private System.Windows.Forms.ToolStripMenuItem MenuSettings;
        private System.Windows.Forms.ToolStripSeparator MenuEditSep1;
        private System.Windows.Forms.ToolStripMenuItem FileMenuImport;
    }
}

