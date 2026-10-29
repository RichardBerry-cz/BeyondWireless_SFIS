namespace BeyondWireless_SFIS
{
    partial class frmLabelPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLabelPrint));
            this.txtStageName = new System.Windows.Forms.TextBox();
            this.cboOrders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblPrintBalance = new System.Windows.Forms.Label();
            this.lblLabelMediaLengthNeeded = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPortStatus = new System.Windows.Forms.Label();
            this.cboPrintPort = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRefreshPorts = new System.Windows.Forms.Button();
            this.btnTestPort = new System.Windows.Forms.Button();
            this.btnTestPrint = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStageName
            // 
            this.txtStageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStageName.BackColor = System.Drawing.SystemColors.Highlight;
            this.txtStageName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStageName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStageName.ForeColor = System.Drawing.Color.White;
            this.txtStageName.Location = new System.Drawing.Point(-1, 17);
            this.txtStageName.Name = "txtStageName";
            this.txtStageName.Size = new System.Drawing.Size(803, 29);
            this.txtStageName.TabIndex = 14;
            this.txtStageName.TabStop = false;
            this.txtStageName.Text = "PCBA Label Printing";
            this.txtStageName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cboOrders
            // 
            this.cboOrders.FormattingEnabled = true;
            this.cboOrders.Location = new System.Drawing.Point(101, 21);
            this.cboOrders.Name = "cboOrders";
            this.cboOrders.Size = new System.Drawing.Size(401, 24);
            this.cboOrders.TabIndex = 15;
            this.cboOrders.SelectedIndexChanged += new System.EventHandler(this.cboOrders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Message";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Location = new System.Drawing.Point(90, 148);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(11, 16);
            this.lblMessage.TabIndex = 17;
            this.lblMessage.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Order:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "Quantity:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantity.Location = new System.Drawing.Point(101, 51);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(100, 22);
            this.txtQuantity.TabIndex = 20;
            this.txtQuantity.Text = "2";
            this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(695, 17);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 28);
            this.btnPrint.TabIndex = 21;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblPrintBalance);
            this.groupBox1.Controls.Add(this.lblLabelMediaLengthNeeded);
            this.groupBox1.Controls.Add(this.cboOrders);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtQuantity);
            this.groupBox1.Controls.Add(this.lblMessage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 190);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Order Details";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 16);
            this.label10.TabIndex = 23;
            this.label10.Text = "Still To Print:";
            // 
            // lblPrintBalance
            // 
            this.lblPrintBalance.AutoSize = true;
            this.lblPrintBalance.ForeColor = System.Drawing.Color.Blue;
            this.lblPrintBalance.Location = new System.Drawing.Point(98, 84);
            this.lblPrintBalance.Name = "lblPrintBalance";
            this.lblPrintBalance.Size = new System.Drawing.Size(11, 16);
            this.lblPrintBalance.TabIndex = 24;
            this.lblPrintBalance.Text = "-";
            // 
            // lblLabelMediaLengthNeeded
            // 
            this.lblLabelMediaLengthNeeded.AutoSize = true;
            this.lblLabelMediaLengthNeeded.ForeColor = System.Drawing.Color.Blue;
            this.lblLabelMediaLengthNeeded.Location = new System.Drawing.Point(223, 57);
            this.lblLabelMediaLengthNeeded.Name = "lblLabelMediaLengthNeeded";
            this.lblLabelMediaLengthNeeded.Size = new System.Drawing.Size(14, 16);
            this.lblLabelMediaLengthNeeded.TabIndex = 22;
            this.lblLabelMediaLengthNeeded.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblPortStatus);
            this.groupBox2.Controls.Add(this.cboPrintPort);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnRefreshPorts);
            this.groupBox2.Controls.Add(this.btnTestPort);
            this.groupBox2.Controls.Add(this.btnTestPrint);
            this.groupBox2.Location = new System.Drawing.Point(12, 264);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 166);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Printer Setup";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 16);
            this.label8.TabIndex = 31;
            this.label8.Text = "Ribbon:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(90, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(178, 16);
            this.label9.TabIndex = 32;
            this.label9.Text = "--- Hybrid Ribbon (Pink Core)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "Label:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(90, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(445, 16);
            this.label7.TabIndex = 30;
            this.label7.Text = "CZ21-00008 - LABEL SEMI GLOSS PAPER 30x10mm 2 ACROSS (TT1073)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "Port Status:";
            // 
            // lblPortStatus
            // 
            this.lblPortStatus.AutoSize = true;
            this.lblPortStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblPortStatus.Location = new System.Drawing.Point(90, 69);
            this.lblPortStatus.Name = "lblPortStatus";
            this.lblPortStatus.Size = new System.Drawing.Size(11, 16);
            this.lblPortStatus.TabIndex = 28;
            this.lblPortStatus.Text = "-";
            // 
            // cboPrintPort
            // 
            this.cboPrintPort.FormattingEnabled = true;
            this.cboPrintPort.Location = new System.Drawing.Point(93, 30);
            this.cboPrintPort.Name = "cboPrintPort";
            this.cboPrintPort.Size = new System.Drawing.Size(401, 24);
            this.cboPrintPort.TabIndex = 25;
            this.cboPrintPort.SelectedIndexChanged += new System.EventHandler(this.cboPrintPort_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 16);
            this.label4.TabIndex = 26;
            this.label4.Text = "Print Port:";
            // 
            // btnRefreshPorts
            // 
            this.btnRefreshPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshPorts.Location = new System.Drawing.Point(647, 79);
            this.btnRefreshPorts.Name = "btnRefreshPorts";
            this.btnRefreshPorts.Size = new System.Drawing.Size(123, 23);
            this.btnRefreshPorts.TabIndex = 24;
            this.btnRefreshPorts.Text = "Refresh Ports";
            this.btnRefreshPorts.UseVisualStyleBackColor = true;
            this.btnRefreshPorts.Click += new System.EventHandler(this.btnRefreshPorts_Click);
            // 
            // btnTestPort
            // 
            this.btnTestPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestPort.Location = new System.Drawing.Point(647, 50);
            this.btnTestPort.Name = "btnTestPort";
            this.btnTestPort.Size = new System.Drawing.Size(123, 23);
            this.btnTestPort.TabIndex = 23;
            this.btnTestPort.Text = "Test Port";
            this.btnTestPort.UseVisualStyleBackColor = true;
            this.btnTestPort.Click += new System.EventHandler(this.btnTestPort_Click);
            // 
            // btnTestPrint
            // 
            this.btnTestPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestPrint.Location = new System.Drawing.Point(647, 21);
            this.btnTestPrint.Name = "btnTestPrint";
            this.btnTestPrint.Size = new System.Drawing.Size(123, 23);
            this.btnTestPrint.TabIndex = 22;
            this.btnTestPrint.Text = "Test Print";
            this.btnTestPrint.UseVisualStyleBackColor = true;
            this.btnTestPrint.Click += new System.EventHandler(this.btnTestPrint_Click);
            // 
            // frmLabelPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 464);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtStageName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLabelPrint";
            this.Text = "LabelPrint";
            this.Load += new System.EventHandler(this.frmLabelPrint_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStageName;
        private System.Windows.Forms.ComboBox cboOrders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRefreshPorts;
        private System.Windows.Forms.Button btnTestPort;
        private System.Windows.Forms.Button btnTestPrint;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPortStatus;
        private System.Windows.Forms.ComboBox cboPrintPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLabelMediaLengthNeeded;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblPrintBalance;
    }
}