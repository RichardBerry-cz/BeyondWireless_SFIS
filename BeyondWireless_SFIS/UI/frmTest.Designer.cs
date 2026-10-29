namespace BeyondWireless_SFIS
{
    partial class frmTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTest));
            this.grpDefectSelection = new System.Windows.Forms.GroupBox();
            this.tlpDefects = new System.Windows.Forms.TableLayoutPanel();
            this.txtStageName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDefects = new System.Windows.Forms.DataGridView();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblPositions = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDefectId = new System.Windows.Forms.TextBox();
            this.txtDefectDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.txtPcbaSerial = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChangeOrder = new System.Windows.Forms.Button();
            this.cboOrders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDefectGroup = new System.Windows.Forms.ComboBox();
            this.lblDefectGroup = new System.Windows.Forms.Label();
            this.grpDefectSelection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefects)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDefectSelection
            // 
            this.grpDefectSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDefectSelection.Controls.Add(this.tlpDefects);
            this.grpDefectSelection.Location = new System.Drawing.Point(11, 374);
            this.grpDefectSelection.Name = "grpDefectSelection";
            this.grpDefectSelection.Size = new System.Drawing.Size(764, 486);
            this.grpDefectSelection.TabIndex = 18;
            this.grpDefectSelection.TabStop = false;
            this.grpDefectSelection.Text = "Defect Selection";
            // 
            // tlpDefects
            // 
            this.tlpDefects.ColumnCount = 3;
            this.tlpDefects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpDefects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpDefects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDefects.Location = new System.Drawing.Point(3, 18);
            this.tlpDefects.Name = "tlpDefects";
            this.tlpDefects.RowCount = 1;
            this.tlpDefects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDefects.Size = new System.Drawing.Size(758, 465);
            this.tlpDefects.TabIndex = 0;
            this.tlpDefects.Visible = false;
            // 
            // txtStageName
            // 
            this.txtStageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStageName.BackColor = System.Drawing.SystemColors.Highlight;
            this.txtStageName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStageName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStageName.ForeColor = System.Drawing.Color.White;
            this.txtStageName.Location = new System.Drawing.Point(-3, 12);
            this.txtStageName.Name = "txtStageName";
            this.txtStageName.Size = new System.Drawing.Size(788, 29);
            this.txtStageName.TabIndex = 16;
            this.txtStageName.Text = "Test";
            this.txtStageName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvDefects);
            this.groupBox2.Controls.Add(this.lblMessage);
            this.groupBox2.Controls.Add(this.lblPositions);
            this.groupBox2.Controls.Add(this.txtPosition);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtDefectId);
            this.groupBox2.Controls.Add(this.txtDefectDescription);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.txtResult);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnReset);
            this.groupBox2.Controls.Add(this.txtPcbaSerial);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Location = new System.Drawing.Point(11, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(765, 221);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input";
            // 
            // dgvDefects
            // 
            this.dgvDefects.AllowUserToAddRows = false;
            this.dgvDefects.AllowUserToDeleteRows = false;
            this.dgvDefects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDefects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDefects.BackgroundColor = System.Drawing.Color.White;
            this.dgvDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefects.Location = new System.Drawing.Point(3, 119);
            this.dgvDefects.Name = "dgvDefects";
            this.dgvDefects.ReadOnly = true;
            this.dgvDefects.RowHeadersVisible = false;
            this.dgvDefects.RowHeadersWidth = 51;
            this.dgvDefects.RowTemplate.Height = 24;
            this.dgvDefects.Size = new System.Drawing.Size(758, 96);
            this.dgvDefects.TabIndex = 31;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Location = new System.Drawing.Point(121, 100);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(140, 16);
            this.lblMessage.TabIndex = 28;
            this.lblMessage.Text = "Select an order to start";
            // 
            // lblPositions
            // 
            this.lblPositions.AutoSize = true;
            this.lblPositions.Location = new System.Drawing.Point(10, 78);
            this.lblPositions.Name = "lblPositions";
            this.lblPositions.Size = new System.Drawing.Size(58, 16);
            this.lblPositions.TabIndex = 27;
            this.lblPositions.Text = "Position:";
            // 
            // txtPosition
            // 
            this.txtPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPosition.Location = new System.Drawing.Point(124, 75);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.ReadOnly = true;
            this.txtPosition.Size = new System.Drawing.Size(206, 22);
            this.txtPosition.TabIndex = 26;
            this.txtPosition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPosition_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Defect ID:";
            // 
            // txtDefectId
            // 
            this.txtDefectId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDefectId.Location = new System.Drawing.Point(328, 47);
            this.txtDefectId.Name = "txtDefectId";
            this.txtDefectId.ReadOnly = true;
            this.txtDefectId.Size = new System.Drawing.Size(114, 22);
            this.txtDefectId.TabIndex = 24;
            this.txtDefectId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDefectId_KeyDown);
            // 
            // txtDefectDescription
            // 
            this.txtDefectDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDefectDescription.Location = new System.Drawing.Point(448, 47);
            this.txtDefectDescription.Name = "txtDefectDescription";
            this.txtDefectDescription.ReadOnly = true;
            this.txtDefectDescription.Size = new System.Drawing.Size(311, 22);
            this.txtDefectDescription.TabIndex = 22;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(684, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtResult
            // 
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResult.Location = new System.Drawing.Point(124, 47);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(110, 22);
            this.txtResult.TabIndex = 17;
            this.txtResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtResult_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Scan Pass/Fail:";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(586, 15);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtPcbaSerial
            // 
            this.txtPcbaSerial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPcbaSerial.Location = new System.Drawing.Point(124, 19);
            this.txtPcbaSerial.Name = "txtPcbaSerial";
            this.txtPcbaSerial.ReadOnly = true;
            this.txtPcbaSerial.Size = new System.Drawing.Size(362, 22);
            this.txtPcbaSerial.TabIndex = 12;
            this.txtPcbaSerial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPcbaSerial_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 16);
            this.label11.TabIndex = 14;
            this.label11.Text = "Message:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 16);
            this.label12.TabIndex = 13;
            this.label12.Text = "Scan Serial:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnChangeOrder);
            this.groupBox1.Controls.Add(this.cboOrders);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(765, 57);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Order Selection";
            // 
            // btnChangeOrder
            // 
            this.btnChangeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeOrder.Location = new System.Drawing.Point(641, 17);
            this.btnChangeOrder.Name = "btnChangeOrder";
            this.btnChangeOrder.Size = new System.Drawing.Size(109, 32);
            this.btnChangeOrder.TabIndex = 17;
            this.btnChangeOrder.Text = "Change Order";
            this.btnChangeOrder.UseVisualStyleBackColor = true;
            this.btnChangeOrder.Click += new System.EventHandler(this.btnChangeOrder_Click);
            // 
            // cboOrders
            // 
            this.cboOrders.FormattingEnabled = true;
            this.cboOrders.Location = new System.Drawing.Point(106, 21);
            this.cboOrders.Name = "cboOrders";
            this.cboOrders.Size = new System.Drawing.Size(437, 24);
            this.cboOrders.TabIndex = 11;
            this.cboOrders.SelectedIndexChanged += new System.EventHandler(this.cboOrders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Order:";
            // 
            // cboDefectGroup
            // 
            this.cboDefectGroup.FormattingEnabled = true;
            this.cboDefectGroup.Location = new System.Drawing.Point(119, 344);
            this.cboDefectGroup.Name = "cboDefectGroup";
            this.cboDefectGroup.Size = new System.Drawing.Size(299, 24);
            this.cboDefectGroup.TabIndex = 19;
            this.cboDefectGroup.SelectedIndexChanged += new System.EventHandler(this.cboDefectGroup_SelectedIndexChanged);
            // 
            // lblDefectGroup
            // 
            this.lblDefectGroup.AutoSize = true;
            this.lblDefectGroup.Location = new System.Drawing.Point(15, 347);
            this.lblDefectGroup.Name = "lblDefectGroup";
            this.lblDefectGroup.Size = new System.Drawing.Size(89, 16);
            this.lblDefectGroup.TabIndex = 20;
            this.lblDefectGroup.Text = "Defect Group:";
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 872);
            this.Controls.Add(this.lblDefectGroup);
            this.Controls.Add(this.cboDefectGroup);
            this.Controls.Add(this.grpDefectSelection);
            this.Controls.Add(this.txtStageName);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(804, 919);
            this.Name = "frmTest";
            this.Text = "Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTest_FormClosing);
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.grpDefectSelection.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefects)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStageName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDefectDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox txtPcbaSerial;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboOrders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpDefectSelection;
        private System.Windows.Forms.TableLayoutPanel tlpDefects;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDefectId;
        private System.Windows.Forms.Button btnChangeOrder;
        private System.Windows.Forms.Label lblPositions;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridView dgvDefects;
        private System.Windows.Forms.ComboBox cboDefectGroup;
        private System.Windows.Forms.Label lblDefectGroup;
    }
}