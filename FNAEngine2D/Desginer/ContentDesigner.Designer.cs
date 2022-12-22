namespace FNAEngine2D.Desginer
{
    partial class ContentDesigner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentDesigner));
            this.cboGameContentContainer = new System.Windows.Forms.ComboBox();
            this.lblContainer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lstGameObjectTypes = new System.Windows.Forms.ListView();
            this.colFullName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboGameObjects = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboGameContentContainer
            // 
            this.cboGameContentContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGameContentContainer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGameContentContainer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGameContentContainer.DisplayMember = "AssetName";
            this.cboGameContentContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGameContentContainer.FormattingEnabled = true;
            this.cboGameContentContainer.Location = new System.Drawing.Point(1, 16);
            this.cboGameContentContainer.Name = "cboGameContentContainer";
            this.cboGameContentContainer.Size = new System.Drawing.Size(330, 21);
            this.cboGameContentContainer.TabIndex = 1;
            this.cboGameContentContainer.ValueMember = "AssetName";
            this.cboGameContentContainer.SelectedIndexChanged += new System.EventHandler(this.cboGameContentContainer_SelectedIndexChanged);
            // 
            // lblContainer
            // 
            this.lblContainer.AutoSize = true;
            this.lblContainer.Location = new System.Drawing.Point(1, 0);
            this.lblContainer.Name = "lblContainer";
            this.lblContainer.Size = new System.Drawing.Size(52, 13);
            this.lblContainer.TabIndex = 2;
            this.lblContainer.Text = "Container";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "GameObjects";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(261, 664);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(73, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Selected: ";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(1, 39);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(330, 292);
            this.propertyGrid.TabIndex = 7;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(3, 4);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lstGameObjectTypes);
            this.splitContainer.Panel1.Controls.Add(this.lblContainer);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.cboGameContentContainer);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.cboGameObjects);
            this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer.Panel2.Controls.Add(this.label2);
            this.splitContainer.Size = new System.Drawing.Size(331, 660);
            this.splitContainer.SplitterDistance = 323;
            this.splitContainer.TabIndex = 8;
            // 
            // lstGameObjectTypes
            // 
            this.lstGameObjectTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstGameObjectTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFullName});
            this.lstGameObjectTypes.FullRowSelect = true;
            this.lstGameObjectTypes.GridLines = true;
            this.lstGameObjectTypes.HideSelection = false;
            this.lstGameObjectTypes.Location = new System.Drawing.Point(0, 63);
            this.lstGameObjectTypes.Name = "lstGameObjectTypes";
            this.lstGameObjectTypes.Size = new System.Drawing.Size(331, 264);
            this.lstGameObjectTypes.TabIndex = 4;
            this.lstGameObjectTypes.UseCompatibleStateImageBehavior = false;
            this.lstGameObjectTypes.View = System.Windows.Forms.View.Details;
            this.lstGameObjectTypes.DoubleClick += new System.EventHandler(this.lstGameObjectTypes_DoubleClick);
            // 
            // colFullName
            // 
            this.colFullName.Text = "Full name";
            this.colFullName.Width = 310;
            // 
            // cboGameObjects
            // 
            this.cboGameObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGameObjects.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGameObjects.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGameObjects.DisplayMember = "DisplayName";
            this.cboGameObjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGameObjects.FormattingEnabled = true;
            this.cboGameObjects.Location = new System.Drawing.Point(0, 16);
            this.cboGameObjects.Name = "cboGameObjects";
            this.cboGameObjects.Size = new System.Drawing.Size(330, 21);
            this.cboGameObjects.TabIndex = 8;
            this.cboGameObjects.ValueMember = "DisplayName";
            this.cboGameObjects.SelectedIndexChanged += new System.EventHandler(this.cboGameObjects_SelectedIndexChanged);
            // 
            // ContentDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 691);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 100);
            this.Name = "ContentDesigner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Content designer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ContentDesigner_FormClosing);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cboGameContentContainer;
        private System.Windows.Forms.Label lblContainer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListView lstGameObjectTypes;
        private System.Windows.Forms.ColumnHeader colFullName;
        private System.Windows.Forms.ComboBox cboGameObjects;
    }
}