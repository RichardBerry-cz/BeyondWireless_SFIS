namespace BeyondWireless_SFIS
{
    partial class frmReportDefinition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportDefinition));
            this.txtReportDefinition = new System.Windows.Forms.TextBox();
            this.txtReportName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtReportDefinition
            // 
            this.txtReportDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportDefinition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtReportDefinition.Location = new System.Drawing.Point(12, 47);
            this.txtReportDefinition.Multiline = true;
            this.txtReportDefinition.Name = "txtReportDefinition";
            this.txtReportDefinition.ReadOnly = true;
            this.txtReportDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReportDefinition.Size = new System.Drawing.Size(776, 554);
            this.txtReportDefinition.TabIndex = 0;
            this.txtReportDefinition.TabStop = false;
            // 
            // txtReportName
            // 
            this.txtReportName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportName.BackColor = System.Drawing.SystemColors.Highlight;
            this.txtReportName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtReportName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReportName.ForeColor = System.Drawing.Color.White;
            this.txtReportName.Location = new System.Drawing.Point(0, 12);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new System.Drawing.Size(799, 29);
            this.txtReportName.TabIndex = 17;
            this.txtReportName.TabStop = false;
            this.txtReportName.Text = "Report";
            this.txtReportName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmReportDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 603);
            this.Controls.Add(this.txtReportName);
            this.Controls.Add(this.txtReportDefinition);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReportDefinition";
            this.Text = "Report Definition";
            this.Load += new System.EventHandler(this.frmReportDefinition_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReportDefinition;
        private System.Windows.Forms.TextBox txtReportName;
    }
}