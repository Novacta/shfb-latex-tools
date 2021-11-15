namespace Novacta.Shfb.LatexConfiguration
{
    partial class LatexConfigDlg
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.c_redirectFileProcessors = new System.Windows.Forms.CheckBox();
            this.c_imageScalePercentage = new System.Windows.Forms.NumericUpDown();
            this.l_imageScalePercentage = new System.Windows.Forms.Label();
            this.l_latexBinFolder = new System.Windows.Forms.Label();
            this.c_latexBinFolder = new System.Windows.Forms.TextBox();
            this.latexBrowseButton = new System.Windows.Forms.Button();
            this.l_dvisvgmBinFolder = new System.Windows.Forms.Label();
            this.c_dvisvgmBinFolder = new System.Windows.Forms.TextBox();
            this.dvisvgmBrowseButton = new System.Windows.Forms.Button();
            this.l_imageFileFormat = new System.Windows.Forms.Label();
            this.c_radioFileFormatPng = new System.Windows.Forms.RadioButton();
            this.c_radioFileFormatSvg = new System.Windows.Forms.RadioButton();
            this.c_groupBoxFileFormat = new System.Windows.Forms.GroupBox();
            this.l_imageDepthCorrection = new System.Windows.Forms.Label();
            this.c_imageDepthCorrection = new System.Windows.Forms.NumericUpDown();
            this.l_defaultLaTeXMode = new System.Windows.Forms.Label();
            this.c_groupBoxDefaultLaTeXMode = new System.Windows.Forms.GroupBox();
            this.c_radioLaTeXModeDisplay = new System.Windows.Forms.RadioButton();
            this.c_radioLaTeXModeInline = new System.Windows.Forms.RadioButton();
            this.c_groupBoxPreambleAdditionalItems = new System.Windows.Forms.GroupBox();
            this.c_additionalPreambleCommands = new System.Windows.Forms.TextBox();
            this.c_groupBoxFolders = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.c_imageScalePercentage)).BeginInit();
            this.c_groupBoxFileFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c_imageDepthCorrection)).BeginInit();
            this.c_groupBoxDefaultLaTeXMode.SuspendLayout();
            this.c_groupBoxPreambleAdditionalItems.SuspendLayout();
            this.c_groupBoxFolders.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(482, 571);
            this.okButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(84, 29);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(573, 571);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(84, 29);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // c_redirectFileProcessors
            // 
            this.c_redirectFileProcessors.AutoSize = true;
            this.c_redirectFileProcessors.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.c_redirectFileProcessors.Location = new System.Drawing.Point(12, 249);
            this.c_redirectFileProcessors.Name = "c_redirectFileProcessors";
            this.c_redirectFileProcessors.Size = new System.Drawing.Size(237, 24);
            this.c_redirectFileProcessors.TabIndex = 5;
            this.c_redirectFileProcessors.Text = "Redirect file processors:        ";
            this.c_redirectFileProcessors.UseVisualStyleBackColor = true;
            // 
            // c_imageScalePercentage
            // 
            this.c_imageScalePercentage.Location = new System.Drawing.Point(228, 185);
            this.c_imageScalePercentage.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.c_imageScalePercentage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.c_imageScalePercentage.Name = "c_imageScalePercentage";
            this.c_imageScalePercentage.Size = new System.Drawing.Size(287, 26);
            this.c_imageScalePercentage.TabIndex = 6;
            this.c_imageScalePercentage.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // l_imageScalePercentage
            // 
            this.l_imageScalePercentage.AutoSize = true;
            this.l_imageScalePercentage.Location = new System.Drawing.Point(15, 187);
            this.l_imageScalePercentage.Name = "l_imageScalePercentage";
            this.l_imageScalePercentage.Size = new System.Drawing.Size(188, 20);
            this.l_imageScalePercentage.TabIndex = 7;
            this.l_imageScalePercentage.Text = "Image scale percentage: ";
            // 
            // l_latexBinFolder
            // 
            this.l_latexBinFolder.AutoSize = true;
            this.l_latexBinFolder.Location = new System.Drawing.Point(6, 41);
            this.l_latexBinFolder.Name = "l_latexBinFolder";
            this.l_latexBinFolder.Size = new System.Drawing.Size(129, 20);
            this.l_latexBinFolder.TabIndex = 8;
            this.l_latexBinFolder.Text = "LaTeX bin folder:";
            // 
            // c_latexBinFolder
            // 
            this.c_latexBinFolder.Location = new System.Drawing.Point(209, 38);
            this.c_latexBinFolder.Name = "c_latexBinFolder";
            this.c_latexBinFolder.ReadOnly = true;
            this.c_latexBinFolder.Size = new System.Drawing.Size(707, 26);
            this.c_latexBinFolder.TabIndex = 9;
            // 
            // latexBrowseButton
            // 
            this.latexBrowseButton.Location = new System.Drawing.Point(953, 34);
            this.latexBrowseButton.Name = "latexBrowseButton";
            this.latexBrowseButton.Size = new System.Drawing.Size(120, 34);
            this.latexBrowseButton.TabIndex = 10;
            this.latexBrowseButton.Text = "Browse...";
            this.latexBrowseButton.UseVisualStyleBackColor = true;
            this.latexBrowseButton.Click += new System.EventHandler(this.LatexBrowseButton_Click);
            // 
            // l_dvisvgmBinFolder
            // 
            this.l_dvisvgmBinFolder.AutoSize = true;
            this.l_dvisvgmBinFolder.Location = new System.Drawing.Point(6, 102);
            this.l_dvisvgmBinFolder.Name = "l_dvisvgmBinFolder";
            this.l_dvisvgmBinFolder.Size = new System.Drawing.Size(144, 20);
            this.l_dvisvgmBinFolder.TabIndex = 11;
            this.l_dvisvgmBinFolder.Text = "DviSvgm bin folder:";
            // 
            // c_dvisvgmBinFolder
            // 
            this.c_dvisvgmBinFolder.Location = new System.Drawing.Point(209, 102);
            this.c_dvisvgmBinFolder.Name = "c_dvisvgmBinFolder";
            this.c_dvisvgmBinFolder.ReadOnly = true;
            this.c_dvisvgmBinFolder.Size = new System.Drawing.Size(707, 26);
            this.c_dvisvgmBinFolder.TabIndex = 12;
            // 
            // dvisvgmBrowseButton
            // 
            this.dvisvgmBrowseButton.Location = new System.Drawing.Point(953, 95);
            this.dvisvgmBrowseButton.Name = "dvisvgmBrowseButton";
            this.dvisvgmBrowseButton.Size = new System.Drawing.Size(120, 34);
            this.dvisvgmBrowseButton.TabIndex = 13;
            this.dvisvgmBrowseButton.Text = "Browse...";
            this.dvisvgmBrowseButton.UseVisualStyleBackColor = true;
            this.dvisvgmBrowseButton.Click += new System.EventHandler(this.DvisvgmBrowseButton_Click);
            // 
            // l_imageFileFormat
            // 
            this.l_imageFileFormat.AutoSize = true;
            this.l_imageFileFormat.Location = new System.Drawing.Point(15, 44);
            this.l_imageFileFormat.Name = "l_imageFileFormat";
            this.l_imageFileFormat.Size = new System.Drawing.Size(132, 20);
            this.l_imageFileFormat.TabIndex = 15;
            this.l_imageFileFormat.Text = "Image file format:";
            // 
            // c_radioFileFormatPng
            // 
            this.c_radioFileFormatPng.AutoSize = true;
            this.c_radioFileFormatPng.Location = new System.Drawing.Point(36, 32);
            this.c_radioFileFormatPng.Name = "c_radioFileFormatPng";
            this.c_radioFileFormatPng.Size = new System.Drawing.Size(61, 24);
            this.c_radioFileFormatPng.TabIndex = 3;
            this.c_radioFileFormatPng.Text = "png";
            this.c_radioFileFormatPng.UseVisualStyleBackColor = true;
            // 
            // c_radioFileFormatSvg
            // 
            this.c_radioFileFormatSvg.AutoSize = true;
            this.c_radioFileFormatSvg.Checked = true;
            this.c_radioFileFormatSvg.Location = new System.Drawing.Point(175, 32);
            this.c_radioFileFormatSvg.Name = "c_radioFileFormatSvg";
            this.c_radioFileFormatSvg.Size = new System.Drawing.Size(58, 24);
            this.c_radioFileFormatSvg.TabIndex = 4;
            this.c_radioFileFormatSvg.TabStop = true;
            this.c_radioFileFormatSvg.Text = "svg";
            this.c_radioFileFormatSvg.UseVisualStyleBackColor = true;
            // 
            // c_groupBoxFileFormat
            // 
            this.c_groupBoxFileFormat.Controls.Add(this.c_radioFileFormatSvg);
            this.c_groupBoxFileFormat.Controls.Add(this.c_radioFileFormatPng);
            this.c_groupBoxFileFormat.Location = new System.Drawing.Point(228, 12);
            this.c_groupBoxFileFormat.Name = "c_groupBoxFileFormat";
            this.c_groupBoxFileFormat.Size = new System.Drawing.Size(287, 75);
            this.c_groupBoxFileFormat.TabIndex = 5;
            this.c_groupBoxFileFormat.TabStop = false;
            this.c_groupBoxFileFormat.Text = " Supported formats ";
            // 
            // l_imageDepthCorrection
            // 
            this.l_imageDepthCorrection.AutoSize = true;
            this.l_imageDepthCorrection.Location = new System.Drawing.Point(15, 123);
            this.l_imageDepthCorrection.Name = "l_imageDepthCorrection";
            this.l_imageDepthCorrection.Size = new System.Drawing.Size(181, 20);
            this.l_imageDepthCorrection.TabIndex = 19;
            this.l_imageDepthCorrection.Text = "Image depth correction: ";
            // 
            // c_imageDepthCorrection
            // 
            this.c_imageDepthCorrection.Location = new System.Drawing.Point(228, 123);
            this.c_imageDepthCorrection.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.c_imageDepthCorrection.Name = "c_imageDepthCorrection";
            this.c_imageDepthCorrection.Size = new System.Drawing.Size(287, 26);
            this.c_imageDepthCorrection.TabIndex = 18;
            // 
            // l_defaultLaTeXMode
            // 
            this.l_defaultLaTeXMode.AutoSize = true;
            this.l_defaultLaTeXMode.Location = new System.Drawing.Point(15, 321);
            this.l_defaultLaTeXMode.Name = "l_defaultLaTeXMode";
            this.l_defaultLaTeXMode.Size = new System.Drawing.Size(160, 20);
            this.l_defaultLaTeXMode.TabIndex = 20;
            this.l_defaultLaTeXMode.Text = "Default LaTeX mode:";
            // 
            // c_groupBoxDefaultLaTeXMode
            // 
            this.c_groupBoxDefaultLaTeXMode.Controls.Add(this.c_radioLaTeXModeDisplay);
            this.c_groupBoxDefaultLaTeXMode.Controls.Add(this.c_radioLaTeXModeInline);
            this.c_groupBoxDefaultLaTeXMode.Location = new System.Drawing.Point(228, 292);
            this.c_groupBoxDefaultLaTeXMode.Name = "c_groupBoxDefaultLaTeXMode";
            this.c_groupBoxDefaultLaTeXMode.Size = new System.Drawing.Size(287, 75);
            this.c_groupBoxDefaultLaTeXMode.TabIndex = 21;
            this.c_groupBoxDefaultLaTeXMode.TabStop = false;
            this.c_groupBoxDefaultLaTeXMode.Text = " Supported modes";
            // 
            // c_radioLaTeXModeDisplay
            // 
            this.c_radioLaTeXModeDisplay.AutoSize = true;
            this.c_radioLaTeXModeDisplay.Checked = true;
            this.c_radioLaTeXModeDisplay.Location = new System.Drawing.Point(175, 29);
            this.c_radioLaTeXModeDisplay.Name = "c_radioLaTeXModeDisplay";
            this.c_radioLaTeXModeDisplay.Size = new System.Drawing.Size(82, 24);
            this.c_radioLaTeXModeDisplay.TabIndex = 4;
            this.c_radioLaTeXModeDisplay.TabStop = true;
            this.c_radioLaTeXModeDisplay.Text = "display";
            this.c_radioLaTeXModeDisplay.UseVisualStyleBackColor = true;
            // 
            // c_radioLaTeXModeInline
            // 
            this.c_radioLaTeXModeInline.AutoSize = true;
            this.c_radioLaTeXModeInline.Location = new System.Drawing.Point(36, 29);
            this.c_radioLaTeXModeInline.Name = "c_radioLaTeXModeInline";
            this.c_radioLaTeXModeInline.Size = new System.Drawing.Size(70, 24);
            this.c_radioLaTeXModeInline.TabIndex = 3;
            this.c_radioLaTeXModeInline.Text = "inline";
            this.c_radioLaTeXModeInline.UseVisualStyleBackColor = true;
            // 
            // c_groupBoxPreambleAdditionalItems
            // 
            this.c_groupBoxPreambleAdditionalItems.Controls.Add(this.c_additionalPreambleCommands);
            this.c_groupBoxPreambleAdditionalItems.Location = new System.Drawing.Point(557, 12);
            this.c_groupBoxPreambleAdditionalItems.Name = "c_groupBoxPreambleAdditionalItems";
            this.c_groupBoxPreambleAdditionalItems.Size = new System.Drawing.Size(552, 355);
            this.c_groupBoxPreambleAdditionalItems.TabIndex = 22;
            this.c_groupBoxPreambleAdditionalItems.TabStop = false;
            this.c_groupBoxPreambleAdditionalItems.Text = " Additional Preamble Commands ";
            // 
            // c_additionalPreambleCommands
            // 
            this.c_additionalPreambleCommands.Location = new System.Drawing.Point(16, 34);
            this.c_additionalPreambleCommands.Multiline = true;
            this.c_additionalPreambleCommands.Name = "c_additionalPreambleCommands";
            this.c_additionalPreambleCommands.Size = new System.Drawing.Size(519, 307);
            this.c_additionalPreambleCommands.TabIndex = 0;
            // 
            // c_groupBoxFolders
            // 
            this.c_groupBoxFolders.Controls.Add(this.dvisvgmBrowseButton);
            this.c_groupBoxFolders.Controls.Add(this.l_dvisvgmBinFolder);
            this.c_groupBoxFolders.Controls.Add(this.c_dvisvgmBinFolder);
            this.c_groupBoxFolders.Controls.Add(this.latexBrowseButton);
            this.c_groupBoxFolders.Controls.Add(this.l_latexBinFolder);
            this.c_groupBoxFolders.Controls.Add(this.c_latexBinFolder);
            this.c_groupBoxFolders.Location = new System.Drawing.Point(19, 392);
            this.c_groupBoxFolders.Name = "c_groupBoxFolders";
            this.c_groupBoxFolders.Size = new System.Drawing.Size(1090, 157);
            this.c_groupBoxFolders.TabIndex = 23;
            this.c_groupBoxFolders.TabStop = false;
            this.c_groupBoxFolders.Text = " Folders ";
            // 
            // LatexConfigDlg
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1130, 622);
            this.Controls.Add(this.c_groupBoxFolders);
            this.Controls.Add(this.c_groupBoxPreambleAdditionalItems);
            this.Controls.Add(this.c_groupBoxDefaultLaTeXMode);
            this.Controls.Add(this.l_defaultLaTeXMode);
            this.Controls.Add(this.l_imageDepthCorrection);
            this.Controls.Add(this.c_imageDepthCorrection);
            this.Controls.Add(this.c_groupBoxFileFormat);
            this.Controls.Add(this.l_imageFileFormat);
            this.Controls.Add(this.l_imageScalePercentage);
            this.Controls.Add(this.c_imageScalePercentage);
            this.Controls.Add(this.c_redirectFileProcessors);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LatexConfigDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LaTeX Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.c_imageScalePercentage)).EndInit();
            this.c_groupBoxFileFormat.ResumeLayout(false);
            this.c_groupBoxFileFormat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c_imageDepthCorrection)).EndInit();
            this.c_groupBoxDefaultLaTeXMode.ResumeLayout(false);
            this.c_groupBoxDefaultLaTeXMode.PerformLayout();
            this.c_groupBoxPreambleAdditionalItems.ResumeLayout(false);
            this.c_groupBoxPreambleAdditionalItems.PerformLayout();
            this.c_groupBoxFolders.ResumeLayout(false);
            this.c_groupBoxFolders.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox c_redirectFileProcessors;
        private System.Windows.Forms.NumericUpDown c_imageScalePercentage;
        private System.Windows.Forms.Label l_imageScalePercentage;
        private System.Windows.Forms.Label l_latexBinFolder;
        private System.Windows.Forms.TextBox c_latexBinFolder;
        private System.Windows.Forms.Button latexBrowseButton;
        private System.Windows.Forms.Label l_dvisvgmBinFolder;
        private System.Windows.Forms.TextBox c_dvisvgmBinFolder;
        private System.Windows.Forms.Button dvisvgmBrowseButton;
        private System.Windows.Forms.Label l_imageFileFormat;
        private System.Windows.Forms.RadioButton c_radioFileFormatPng;
        private System.Windows.Forms.RadioButton c_radioFileFormatSvg;
        private System.Windows.Forms.GroupBox c_groupBoxFileFormat;
        private System.Windows.Forms.Label l_imageDepthCorrection;
        private System.Windows.Forms.NumericUpDown c_imageDepthCorrection;
        private System.Windows.Forms.Label l_defaultLaTeXMode;
        private System.Windows.Forms.GroupBox c_groupBoxDefaultLaTeXMode;
        private System.Windows.Forms.RadioButton c_radioLaTeXModeDisplay;
        private System.Windows.Forms.RadioButton c_radioLaTeXModeInline;
        private System.Windows.Forms.GroupBox c_groupBoxPreambleAdditionalItems;
        private System.Windows.Forms.TextBox c_additionalPreambleCommands;
        private System.Windows.Forms.GroupBox c_groupBoxFolders;
    }
}