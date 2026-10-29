namespace BeyondWireless_SFIS
{
    partial class frmRepair
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRepair));
            this.txtStageName = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ckbIsRepaired = new System.Windows.Forms.CheckBox();
            this.ckbIsScrap = new System.Windows.Forms.CheckBox();
            this.ckbIsFaulty = new System.Windows.Forms.CheckBox();
            this.txtCurrentStage = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtProductShortCode = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPcbaSerialNumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvTestResults = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvRepairsDone = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cboDefectType = new System.Windows.Forms.ComboBox();
            this.cboBomPosition = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboRepairType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTestResultDetailId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDefectId = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTestResultId = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.radFaultSourceJig = new System.Windows.Forms.RadioButton();
            this.radFaultSourceInspection = new System.Windows.Forms.RadioButton();
            this.txtDefectDescription = new System.Windows.Forms.TextBox();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestResults)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepairsDone)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStageName
            // 
            this.txtStageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStageName.BackColor = System.Drawing.SystemColors.Highlight;
            this.txtStageName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStageName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStageName.ForeColor = System.Drawing.Color.White;
            this.txtStageName.Location = new System.Drawing.Point(0, 17);
            this.txtStageName.Name = "txtStageName";
            this.txtStageName.Size = new System.Drawing.Size(1260, 31);
            this.txtStageName.TabIndex = 6;
            this.txtStageName.TabStop = false;
            this.txtStageName.Text = "Repair";
            this.txtStageName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.ckbIsRepaired);
            this.groupBox6.Controls.Add(this.ckbIsScrap);
            this.groupBox6.Controls.Add(this.ckbIsFaulty);
            this.groupBox6.Controls.Add(this.txtCurrentStage);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.txtProductShortCode);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.txtPcbaSerialNumber);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.txtInput);
            this.groupBox6.Controls.Add(this.Label1);
            this.groupBox6.Controls.Add(this.lblMessage);
            this.groupBox6.Location = new System.Drawing.Point(12, 61);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1236, 95);
            this.groupBox6.TabIndex = 47;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Unit Info";
            // 
            // ckbIsRepaired
            // 
            this.ckbIsRepaired.AutoCheck = false;
            this.ckbIsRepaired.AutoSize = true;
            this.ckbIsRepaired.Location = new System.Drawing.Point(1046, 72);
            this.ckbIsRepaired.Name = "ckbIsRepaired";
            this.ckbIsRepaired.Size = new System.Drawing.Size(153, 20);
            this.ckbIsRepaired.TabIndex = 65;
            this.ckbIsRepaired.Text = "All Defects Repaired";
            this.ckbIsRepaired.UseVisualStyleBackColor = true;
            // 
            // ckbIsScrap
            // 
            this.ckbIsScrap.AutoCheck = false;
            this.ckbIsScrap.AutoSize = true;
            this.ckbIsScrap.Location = new System.Drawing.Point(1046, 43);
            this.ckbIsScrap.Name = "ckbIsScrap";
            this.ckbIsScrap.Size = new System.Drawing.Size(89, 20);
            this.ckbIsScrap.TabIndex = 64;
            this.ckbIsScrap.Text = "Scrapped";
            this.ckbIsScrap.UseVisualStyleBackColor = true;
            // 
            // ckbIsFaulty
            // 
            this.ckbIsFaulty.AutoCheck = false;
            this.ckbIsFaulty.AutoSize = true;
            this.ckbIsFaulty.Location = new System.Drawing.Point(1046, 17);
            this.ckbIsFaulty.Name = "ckbIsFaulty";
            this.ckbIsFaulty.Size = new System.Drawing.Size(65, 20);
            this.ckbIsFaulty.TabIndex = 63;
            this.ckbIsFaulty.Text = "Faulty";
            this.ckbIsFaulty.UseVisualStyleBackColor = true;
            // 
            // txtCurrentStage
            // 
            this.txtCurrentStage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrentStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentStage.Location = new System.Drawing.Point(517, 51);
            this.txtCurrentStage.Name = "txtCurrentStage";
            this.txtCurrentStage.ReadOnly = true;
            this.txtCurrentStage.Size = new System.Drawing.Size(139, 26);
            this.txtCurrentStage.TabIndex = 62;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(421, 57);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 16);
            this.label14.TabIndex = 61;
            this.label14.Text = "Stage:";
            // 
            // txtProductShortCode
            // 
            this.txtProductShortCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProductShortCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductShortCode.Location = new System.Drawing.Point(732, 52);
            this.txtProductShortCode.Name = "txtProductShortCode";
            this.txtProductShortCode.ReadOnly = true;
            this.txtProductShortCode.Size = new System.Drawing.Size(293, 26);
            this.txtProductShortCode.TabIndex = 60;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(668, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 16);
            this.label12.TabIndex = 59;
            this.label12.Text = "Product:";
            // 
            // txtPcbaSerialNumber
            // 
            this.txtPcbaSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPcbaSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcbaSerialNumber.Location = new System.Drawing.Point(517, 20);
            this.txtPcbaSerialNumber.Name = "txtPcbaSerialNumber";
            this.txtPcbaSerialNumber.ReadOnly = true;
            this.txtPcbaSerialNumber.Size = new System.Drawing.Size(508, 26);
            this.txtPcbaSerialNumber.TabIndex = 41;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(421, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 16);
            this.label9.TabIndex = 40;
            this.label9.Text = "PCBA Serial:";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(65, 18);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(335, 26);
            this.txtInput.TabIndex = 36;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(6, 20);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(44, 17);
            this.Label1.TabIndex = 37;
            this.Label1.Text = "Scan:";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblMessage.Location = new System.Drawing.Point(62, 47);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(232, 17);
            this.lblMessage.TabIndex = 39;
            this.lblMessage.Text = "(Scan PCBA Serial Number to start)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 169);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1236, 491);
            this.tableLayoutPanel1.TabIndex = 48;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvTestResults);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(488, 485);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recorded Faults";
            // 
            // dgvTestResults
            // 
            this.dgvTestResults.AllowUserToAddRows = false;
            this.dgvTestResults.AllowUserToDeleteRows = false;
            this.dgvTestResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTestResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTestResults.BackgroundColor = System.Drawing.Color.White;
            this.dgvTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTestResults.Location = new System.Drawing.Point(6, 21);
            this.dgvTestResults.Name = "dgvTestResults";
            this.dgvTestResults.ReadOnly = true;
            this.dgvTestResults.RowHeadersVisible = false;
            this.dgvTestResults.RowHeadersWidth = 51;
            this.dgvTestResults.RowTemplate.Height = 24;
            this.dgvTestResults.Size = new System.Drawing.Size(476, 458);
            this.dgvTestResults.TabIndex = 47;
            this.dgvTestResults.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTestResults_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Location = new System.Drawing.Point(497, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 485);
            this.panel1.TabIndex = 43;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.dgvRepairsDone);
            this.groupBox5.Location = new System.Drawing.Point(3, 274);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(724, 208);
            this.groupBox5.TabIndex = 58;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Repair History";
            // 
            // dgvRepairsDone
            // 
            this.dgvRepairsDone.AllowUserToAddRows = false;
            this.dgvRepairsDone.AllowUserToDeleteRows = false;
            this.dgvRepairsDone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRepairsDone.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRepairsDone.BackgroundColor = System.Drawing.Color.White;
            this.dgvRepairsDone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRepairsDone.Location = new System.Drawing.Point(7, 22);
            this.dgvRepairsDone.Name = "dgvRepairsDone";
            this.dgvRepairsDone.ReadOnly = true;
            this.dgvRepairsDone.RowHeadersVisible = false;
            this.dgvRepairsDone.RowHeadersWidth = 51;
            this.dgvRepairsDone.RowTemplate.Height = 24;
            this.dgvRepairsDone.Size = new System.Drawing.Size(709, 183);
            this.dgvRepairsDone.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.txtComment);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.cboDefectType);
            this.groupBox4.Controls.Add(this.cboBomPosition);
            this.groupBox4.Controls.Add(this.btnSave);
            this.groupBox4.Controls.Add(this.cboRepairType);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(3, 121);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(724, 147);
            this.groupBox4.TabIndex = 57;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Repair Detail";
            // 
            // txtComment
            // 
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComment.Location = new System.Drawing.Point(82, 110);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(634, 22);
            this.txtComment.TabIndex = 56;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 113);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 16);
            this.label15.TabIndex = 55;
            this.label15.Text = "Comment:";
            // 
            // cboDefectType
            // 
            this.cboDefectType.Enabled = false;
            this.cboDefectType.FormattingEnabled = true;
            this.cboDefectType.Location = new System.Drawing.Point(82, 50);
            this.cboDefectType.Name = "cboDefectType";
            this.cboDefectType.Size = new System.Drawing.Size(419, 24);
            this.cboDefectType.TabIndex = 52;
            // 
            // cboBomPosition
            // 
            this.cboBomPosition.FormattingEnabled = true;
            this.cboBomPosition.Location = new System.Drawing.Point(82, 20);
            this.cboBomPosition.Name = "cboBomPosition";
            this.cboBomPosition.Size = new System.Drawing.Size(419, 24);
            this.cboBomPosition.TabIndex = 48;
            this.cboBomPosition.SelectedIndexChanged += new System.EventHandler(this.cboBomPosition_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(596, 21);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(122, 23);
            this.btnSave.TabIndex = 54;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboRepairType
            // 
            this.cboRepairType.FormattingEnabled = true;
            this.cboRepairType.Location = new System.Drawing.Point(82, 80);
            this.cboRepairType.Name = "cboRepairType";
            this.cboRepairType.Size = new System.Drawing.Size(419, 24);
            this.cboRepairType.TabIndex = 49;
            this.cboRepairType.SelectedIndexChanged += new System.EventHandler(this.cboRepairType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 53;
            this.label4.Text = "Defect:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 16);
            this.label2.TabIndex = 50;
            this.label2.Text = "Ref Des:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 51;
            this.label3.Text = "Repair:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtTestResultDetailId);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtDefectId);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtTestResultId);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.lblInstruction);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.radFaultSourceJig);
            this.groupBox3.Controls.Add(this.radFaultSourceInspection);
            this.groupBox3.Controls.Add(this.txtDefectDescription);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(724, 112);
            this.groupBox3.TabIndex = 56;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fault Data";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 16);
            this.label8.TabIndex = 64;
            this.label8.Text = "Msg:";
            // 
            // txtTestResultDetailId
            // 
            this.txtTestResultDetailId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTestResultDetailId.Location = new System.Drawing.Point(503, 14);
            this.txtTestResultDetailId.Name = "txtTestResultDetailId";
            this.txtTestResultDetailId.ReadOnly = true;
            this.txtTestResultDetailId.Size = new System.Drawing.Size(61, 22);
            this.txtTestResultDetailId.TabIndex = 63;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(376, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 16);
            this.label5.TabIndex = 62;
            this.label5.Text = "TestResultDetailId:";
            // 
            // txtDefectId
            // 
            this.txtDefectId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDefectId.Location = new System.Drawing.Point(656, 14);
            this.txtDefectId.Name = "txtDefectId";
            this.txtDefectId.ReadOnly = true;
            this.txtDefectId.Size = new System.Drawing.Size(62, 22);
            this.txtDefectId.TabIndex = 61;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(575, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 16);
            this.label11.TabIndex = 60;
            this.label11.Text = "DefectId:";
            // 
            // txtTestResultId
            // 
            this.txtTestResultId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTestResultId.Location = new System.Drawing.Point(309, 14);
            this.txtTestResultId.Name = "txtTestResultId";
            this.txtTestResultId.ReadOnly = true;
            this.txtTestResultId.Size = new System.Drawing.Size(61, 22);
            this.txtTestResultId.TabIndex = 59;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(218, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 16);
            this.label13.TabIndex = 58;
            this.label13.Text = "TestResultId:";
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.ForeColor = System.Drawing.Color.Blue;
            this.lblInstruction.Location = new System.Drawing.Point(79, 80);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(11, 16);
            this.lblInstruction.TabIndex = 57;
            this.lblInstruction.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 16);
            this.label7.TabIndex = 56;
            this.label7.Text = "Defect:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 16);
            this.label6.TabIndex = 55;
            this.label6.Text = "Source:";
            // 
            // radFaultSourceJig
            // 
            this.radFaultSourceJig.AutoSize = true;
            this.radFaultSourceJig.Checked = true;
            this.radFaultSourceJig.Location = new System.Drawing.Point(68, 16);
            this.radFaultSourceJig.Name = "radFaultSourceJig";
            this.radFaultSourceJig.Size = new System.Drawing.Size(46, 20);
            this.radFaultSourceJig.TabIndex = 45;
            this.radFaultSourceJig.TabStop = true;
            this.radFaultSourceJig.Text = "Jig";
            this.radFaultSourceJig.UseVisualStyleBackColor = true;
            // 
            // radFaultSourceInspection
            // 
            this.radFaultSourceInspection.AutoSize = true;
            this.radFaultSourceInspection.Enabled = false;
            this.radFaultSourceInspection.Location = new System.Drawing.Point(120, 16);
            this.radFaultSourceInspection.Name = "radFaultSourceInspection";
            this.radFaultSourceInspection.Size = new System.Drawing.Size(89, 20);
            this.radFaultSourceInspection.TabIndex = 46;
            this.radFaultSourceInspection.Text = "Inspection";
            this.radFaultSourceInspection.UseVisualStyleBackColor = true;
            this.radFaultSourceInspection.CheckedChanged += new System.EventHandler(this.radFaultSourceInspection_CheckedChanged);
            // 
            // txtDefectDescription
            // 
            this.txtDefectDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDefectDescription.Location = new System.Drawing.Point(82, 52);
            this.txtDefectDescription.Name = "txtDefectDescription";
            this.txtDefectDescription.ReadOnly = true;
            this.txtDefectDescription.Size = new System.Drawing.Size(636, 22);
            this.txtDefectDescription.TabIndex = 47;
            // 
            // frmRepair
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 672);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.txtStageName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRepair";
            this.Text = "frmRepair";
            this.Load += new System.EventHandler(this.frmRepair_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestResults)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepairsDone)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStageName;
        private System.Windows.Forms.GroupBox groupBox6;
        internal System.Windows.Forms.TextBox txtCurrentStage;
        private System.Windows.Forms.Label label14;
        internal System.Windows.Forms.TextBox txtProductShortCode;
        private System.Windows.Forms.Label label12;
        internal System.Windows.Forms.TextBox txtPcbaSerialNumber;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox txtInput;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvTestResults;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvRepairsDone;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cboDefectType;
        private System.Windows.Forms.ComboBox cboBomPosition;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboRepairType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtDefectId;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTestResultId;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radFaultSourceJig;
        private System.Windows.Forms.RadioButton radFaultSourceInspection;
        private System.Windows.Forms.TextBox txtDefectDescription;
        private System.Windows.Forms.CheckBox ckbIsScrap;
        private System.Windows.Forms.CheckBox ckbIsFaulty;
        private System.Windows.Forms.TextBox txtTestResultDetailId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ckbIsRepaired;
    }
}