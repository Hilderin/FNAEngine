namespace FNAEngine2D.Desginer
{
    partial class TileSetEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileSetEditor));
            this.lblTexture = new System.Windows.Forms.Label();
            this.txtTexture = new System.Windows.Forms.TextBox();
            this.txtTileSize = new System.Windows.Forms.TextBox();
            this.lblTileSize = new System.Windows.Forms.Label();
            this.txtTileScreenSize = new System.Windows.Forms.TextBox();
            this.lblTileScreenSize = new System.Windows.Forms.Label();
            this.panTileSet = new System.Windows.Forms.Panel();
            this.picTileSet = new System.Windows.Forms.PictureBox();
            this.panTileSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTileSet)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTexture
            // 
            this.lblTexture.AutoSize = true;
            this.lblTexture.Location = new System.Drawing.Point(13, 13);
            this.lblTexture.Name = "lblTexture";
            this.lblTexture.Size = new System.Drawing.Size(46, 13);
            this.lblTexture.TabIndex = 0;
            this.lblTexture.Text = "Texture:";
            // 
            // txtTexture
            // 
            this.txtTexture.Location = new System.Drawing.Point(65, 10);
            this.txtTexture.MaxLength = 255;
            this.txtTexture.Name = "txtTexture";
            this.txtTexture.Size = new System.Drawing.Size(197, 20);
            this.txtTexture.TabIndex = 0;
            this.txtTexture.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTexture_KeyDown);
            this.txtTexture.Validated += new System.EventHandler(this.txtTexture_Validated);
            // 
            // txtTileSize
            // 
            this.txtTileSize.Location = new System.Drawing.Point(333, 10);
            this.txtTileSize.MaxLength = 4;
            this.txtTileSize.Name = "txtTileSize";
            this.txtTileSize.Size = new System.Drawing.Size(40, 20);
            this.txtTileSize.TabIndex = 1;
            this.txtTileSize.Validating += new System.ComponentModel.CancelEventHandler(this.txtTileSize_Validating);
            this.txtTileSize.Validated += new System.EventHandler(this.txtTileSize_Validated);
            // 
            // lblTileSize
            // 
            this.lblTileSize.AutoSize = true;
            this.lblTileSize.Location = new System.Drawing.Point(281, 13);
            this.lblTileSize.Name = "lblTileSize";
            this.lblTileSize.Size = new System.Drawing.Size(47, 13);
            this.lblTileSize.TabIndex = 2;
            this.lblTileSize.Text = "TileSize:";
            // 
            // txtTileScreenSize
            // 
            this.txtTileScreenSize.Location = new System.Drawing.Point(469, 10);
            this.txtTileScreenSize.MaxLength = 4;
            this.txtTileScreenSize.Name = "txtTileScreenSize";
            this.txtTileScreenSize.Size = new System.Drawing.Size(40, 20);
            this.txtTileScreenSize.TabIndex = 2;
            this.txtTileScreenSize.Validating += new System.ComponentModel.CancelEventHandler(this.txtTileScreenSize_Validating);
            this.txtTileScreenSize.Validated += new System.EventHandler(this.txtTileScreenSize_Validated);
            // 
            // lblTileScreenSize
            // 
            this.lblTileScreenSize.AutoSize = true;
            this.lblTileScreenSize.Location = new System.Drawing.Point(382, 13);
            this.lblTileScreenSize.Name = "lblTileScreenSize";
            this.lblTileScreenSize.Size = new System.Drawing.Size(81, 13);
            this.lblTileScreenSize.TabIndex = 4;
            this.lblTileScreenSize.Text = "TileScreenSize:";
            // 
            // panTileSet
            // 
            this.panTileSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panTileSet.AutoScroll = true;
            this.panTileSet.Controls.Add(this.picTileSet);
            this.panTileSet.Location = new System.Drawing.Point(1, 36);
            this.panTileSet.Name = "panTileSet";
            this.panTileSet.Size = new System.Drawing.Size(785, 519);
            this.panTileSet.TabIndex = 7;
            // 
            // picTileSet
            // 
            this.picTileSet.Location = new System.Drawing.Point(0, 0);
            this.picTileSet.Name = "picTileSet";
            this.picTileSet.Size = new System.Drawing.Size(774, 515);
            this.picTileSet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTileSet.TabIndex = 6;
            this.picTileSet.TabStop = false;
            this.picTileSet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picTileSet_MouseClick);
            // 
            // TileSetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 560);
            this.Controls.Add(this.panTileSet);
            this.Controls.Add(this.txtTileScreenSize);
            this.Controls.Add(this.lblTileScreenSize);
            this.Controls.Add(this.txtTileSize);
            this.Controls.Add(this.lblTileSize);
            this.Controls.Add(this.txtTexture);
            this.Controls.Add(this.lblTexture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TileSetEditor";
            this.Text = "TileSet Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TileSetEditor_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TileSetEditorForm_FormClosed);
            this.panTileSet.ResumeLayout(false);
            this.panTileSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTileSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTexture;
        private System.Windows.Forms.TextBox txtTexture;
        private System.Windows.Forms.TextBox txtTileSize;
        private System.Windows.Forms.Label lblTileSize;
        private System.Windows.Forms.TextBox txtTileScreenSize;
        private System.Windows.Forms.Label lblTileScreenSize;
        private System.Windows.Forms.PictureBox picTileSet;
        private System.Windows.Forms.Panel panTileSet;
    }
}