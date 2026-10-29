namespace BeyondWireless_SFIS
{
    partial class frmOrders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOrders));
            this.cboOrders = new System.Windows.Forms.ComboBox();
            this.btnNewOrder = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOdf = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cboProducts = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpExpectedPanelArrivalDate = new System.Windows.Forms.DateTimePicker();
            this.dtpExpectedKitArrivalDate = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.txtQuantityDespatched = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtQuantityOrdered = new System.Windows.Forms.TextBox();
            this.ckbActiveOrdersOnly = new System.Windows.Forms.CheckBox();
            this.ckbAllowLabelPrint = new System.Windows.Forms.CheckBox();
            this.txtOrderStatus = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPanelOdf = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtBcAssemblyOrder = new System.Windows.Forms.TextBox();
            this.ckbIsActive = new System.Windows.Forms.CheckBox();
            this.txtCustomerOrderNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboBuildTypes = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrderDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCustomerOrderId = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.dgvOrderForProduct = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvDespatchesOfOrder = new System.Windows.Forms.DataGridView();
            this.txtStageName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderForProduct)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDespatchesOfOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // cboOrders
            // 
            this.cboOrders.FormattingEnabled = true;
            this.cboOrders.Location = new System.Drawing.Point(101, 30);
            this.cboOrders.Name = "cboOrders";
            this.cboOrders.Size = new System.Drawing.Size(486, 24);
            this.cboOrders.TabIndex = 0;
            this.cboOrders.SelectedIndexChanged += new System.EventHandler(this.cboOrders_SelectedIndexChanged);
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewOrder.Location = new System.Drawing.Point(781, 17);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Size = new System.Drawing.Size(75, 23);
            this.btnNewOrder.TabIndex = 1;
            this.btnNewOrder.Text = "New";
            this.btnNewOrder.UseVisualStyleBackColor = true;
            this.btnNewOrder.Click += new System.EventHandler(this.btnNewOrder_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(781, 46);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(781, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(781, 104);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Order:";
            // 
            // txtOdf
            // 
            this.txtOdf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOdf.Location = new System.Drawing.Point(101, 121);
            this.txtOdf.MaxLength = 20;
            this.txtOdf.Name = "txtOdf";
            this.txtOdf.ReadOnly = true;
            this.txtOdf.Size = new System.Drawing.Size(187, 22);
            this.txtOdf.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "ODF:";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(11, 64);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(56, 16);
            this.lblProduct.TabIndex = 10;
            this.lblProduct.Text = "Product:";
            // 
            // cboProducts
            // 
            this.cboProducts.Enabled = false;
            this.cboProducts.FormattingEnabled = true;
            this.cboProducts.Location = new System.Drawing.Point(101, 60);
            this.cboProducts.Name = "cboProducts";
            this.cboProducts.Size = new System.Drawing.Size(486, 24);
            this.cboProducts.TabIndex = 9;
            this.cboProducts.SelectedIndexChanged += new System.EventHandler(this.cboProducts_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Kit ETA:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Panel ETA:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpExpectedPanelArrivalDate);
            this.groupBox1.Controls.Add(this.dtpExpectedKitArrivalDate);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtQuantityDespatched);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtQuantityOrdered);
            this.groupBox1.Controls.Add(this.ckbActiveOrdersOnly);
            this.groupBox1.Controls.Add(this.ckbAllowLabelPrint);
            this.groupBox1.Controls.Add(this.txtOrderStatus);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtPanelOdf);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtBcAssemblyOrder);
            this.groupBox1.Controls.Add(this.ckbIsActive);
            this.groupBox1.Controls.Add(this.txtCustomerOrderNumber);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cboBuildTypes);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtOrderDate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCustomerOrderId);
            this.groupBox1.Controls.Add(this.cboOrders);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnNewOrder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtOdf);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblProduct);
            this.groupBox1.Controls.Add(this.cboProducts);
            this.groupBox1.Location = new System.Drawing.Point(11, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(862, 293);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Order Details";
            // 
            // dtpExpectedPanelArrivalDate
            // 
            this.dtpExpectedPanelArrivalDate.CustomFormat = "yyyy-MM-dd";
            this.dtpExpectedPanelArrivalDate.Enabled = false;
            this.dtpExpectedPanelArrivalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpectedPanelArrivalDate.Location = new System.Drawing.Point(416, 180);
            this.dtpExpectedPanelArrivalDate.Name = "dtpExpectedPanelArrivalDate";
            this.dtpExpectedPanelArrivalDate.Size = new System.Drawing.Size(171, 22);
            this.dtpExpectedPanelArrivalDate.TabIndex = 38;
            // 
            // dtpExpectedKitArrivalDate
            // 
            this.dtpExpectedKitArrivalDate.CustomFormat = "yyyy-MM-dd";
            this.dtpExpectedKitArrivalDate.Enabled = false;
            this.dtpExpectedKitArrivalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpectedKitArrivalDate.Location = new System.Drawing.Point(102, 180);
            this.dtpExpectedKitArrivalDate.Name = "dtpExpectedKitArrivalDate";
            this.dtpExpectedKitArrivalDate.Size = new System.Drawing.Size(186, 22);
            this.dtpExpectedKitArrivalDate.TabIndex = 37;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(306, 152);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 16);
            this.label13.TabIndex = 36;
            this.label13.Text = "Despatched:";
            // 
            // txtQuantityDespatched
            // 
            this.txtQuantityDespatched.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantityDespatched.Location = new System.Drawing.Point(416, 149);
            this.txtQuantityDespatched.Name = "txtQuantityDespatched";
            this.txtQuantityDespatched.ReadOnly = true;
            this.txtQuantityDespatched.Size = new System.Drawing.Size(171, 22);
            this.txtQuantityDespatched.TabIndex = 35;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 152);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 16);
            this.label14.TabIndex = 34;
            this.label14.Text = "Quantity:";
            // 
            // txtQuantityOrdered
            // 
            this.txtQuantityOrdered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantityOrdered.Location = new System.Drawing.Point(101, 149);
            this.txtQuantityOrdered.Name = "txtQuantityOrdered";
            this.txtQuantityOrdered.ReadOnly = true;
            this.txtQuantityOrdered.Size = new System.Drawing.Size(187, 22);
            this.txtQuantityOrdered.TabIndex = 33;
            // 
            // ckbActiveOrdersOnly
            // 
            this.ckbActiveOrdersOnly.AutoSize = true;
            this.ckbActiveOrdersOnly.Checked = true;
            this.ckbActiveOrdersOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbActiveOrdersOnly.Location = new System.Drawing.Point(615, 34);
            this.ckbActiveOrdersOnly.Name = "ckbActiveOrdersOnly";
            this.ckbActiveOrdersOnly.Size = new System.Drawing.Size(140, 20);
            this.ckbActiveOrdersOnly.TabIndex = 32;
            this.ckbActiveOrdersOnly.Text = "Active Orders Only";
            this.ckbActiveOrdersOnly.UseVisualStyleBackColor = true;
            this.ckbActiveOrdersOnly.CheckedChanged += new System.EventHandler(this.ckbActiveOrdersOnly_CheckedChanged);
            // 
            // ckbAllowLabelPrint
            // 
            this.ckbAllowLabelPrint.AutoSize = true;
            this.ckbAllowLabelPrint.Enabled = false;
            this.ckbAllowLabelPrint.Location = new System.Drawing.Point(416, 265);
            this.ckbAllowLabelPrint.Name = "ckbAllowLabelPrint";
            this.ckbAllowLabelPrint.Size = new System.Drawing.Size(150, 20);
            this.ckbAllowLabelPrint.TabIndex = 31;
            this.ckbAllowLabelPrint.Text = "Label Printing Active";
            this.ckbAllowLabelPrint.UseVisualStyleBackColor = true;
            // 
            // txtOrderStatus
            // 
            this.txtOrderStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderStatus.Location = new System.Drawing.Point(416, 236);
            this.txtOrderStatus.Name = "txtOrderStatus";
            this.txtOrderStatus.ReadOnly = true;
            this.txtOrderStatus.Size = new System.Drawing.Size(171, 22);
            this.txtOrderStatus.TabIndex = 29;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(306, 236);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 16);
            this.label11.TabIndex = 28;
            this.label11.Text = "Order Status:";
            // 
            // txtPanelOdf
            // 
            this.txtPanelOdf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanelOdf.Location = new System.Drawing.Point(416, 118);
            this.txtPanelOdf.MaxLength = 20;
            this.txtPanelOdf.Name = "txtPanelOdf";
            this.txtPanelOdf.ReadOnly = true;
            this.txtPanelOdf.Size = new System.Drawing.Size(171, 22);
            this.txtPanelOdf.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(306, 123);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 16);
            this.label10.TabIndex = 27;
            this.label10.Text = "Panel ODF:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(306, 209);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "BC Order No:";
            // 
            // txtBcAssemblyOrder
            // 
            this.txtBcAssemblyOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBcAssemblyOrder.Location = new System.Drawing.Point(416, 206);
            this.txtBcAssemblyOrder.Name = "txtBcAssemblyOrder";
            this.txtBcAssemblyOrder.ReadOnly = true;
            this.txtBcAssemblyOrder.Size = new System.Drawing.Size(171, 22);
            this.txtBcAssemblyOrder.TabIndex = 24;
            // 
            // ckbIsActive
            // 
            this.ckbIsActive.AutoSize = true;
            this.ckbIsActive.Enabled = false;
            this.ckbIsActive.Location = new System.Drawing.Point(101, 263);
            this.ckbIsActive.Name = "ckbIsActive";
            this.ckbIsActive.Size = new System.Drawing.Size(76, 20);
            this.ckbIsActive.TabIndex = 23;
            this.ckbIsActive.Text = "IsActive";
            this.ckbIsActive.UseVisualStyleBackColor = true;
            // 
            // txtCustomerOrderNumber
            // 
            this.txtCustomerOrderNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomerOrderNumber.Location = new System.Drawing.Point(101, 90);
            this.txtCustomerOrderNumber.MaxLength = 50;
            this.txtCustomerOrderNumber.Name = "txtCustomerOrderNumber";
            this.txtCustomerOrderNumber.ReadOnly = true;
            this.txtCustomerOrderNumber.Size = new System.Drawing.Size(187, 22);
            this.txtCustomerOrderNumber.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 16);
            this.label8.TabIndex = 22;
            this.label8.Text = "Order No:";
            // 
            // cboBuildTypes
            // 
            this.cboBuildTypes.Enabled = false;
            this.cboBuildTypes.FormattingEnabled = true;
            this.cboBuildTypes.Location = new System.Drawing.Point(101, 233);
            this.cboBuildTypes.Name = "cboBuildTypes";
            this.cboBuildTypes.Size = new System.Drawing.Size(187, 24);
            this.cboBuildTypes.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Build Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 16);
            this.label6.TabIndex = 18;
            this.label6.Text = "Order Date:";
            // 
            // txtOrderDate
            // 
            this.txtOrderDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderDate.Location = new System.Drawing.Point(101, 205);
            this.txtOrderDate.Name = "txtOrderDate";
            this.txtOrderDate.ReadOnly = true;
            this.txtOrderDate.Size = new System.Drawing.Size(187, 22);
            this.txtOrderDate.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Order ID:";
            // 
            // txtCustomerOrderId
            // 
            this.txtCustomerOrderId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomerOrderId.Location = new System.Drawing.Point(416, 90);
            this.txtCustomerOrderId.Name = "txtCustomerOrderId";
            this.txtCustomerOrderId.ReadOnly = true;
            this.txtCustomerOrderId.Size = new System.Drawing.Size(171, 22);
            this.txtCustomerOrderId.TabIndex = 15;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 368);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(865, 178);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.dgvOrderForProduct);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(857, 149);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Orders for Product";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Blue;
            this.label12.Location = new System.Drawing.Point(6, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(225, 16);
            this.label12.TabIndex = 1;
            this.label12.Text = "Double click a row to select the order";
            // 
            // dgvOrderForProduct
            // 
            this.dgvOrderForProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrderForProduct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvOrderForProduct.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrderForProduct.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvOrderForProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderForProduct.Location = new System.Drawing.Point(9, 22);
            this.dgvOrderForProduct.Name = "dgvOrderForProduct";
            this.dgvOrderForProduct.ReadOnly = true;
            this.dgvOrderForProduct.RowHeadersVisible = false;
            this.dgvOrderForProduct.RowHeadersWidth = 51;
            this.dgvOrderForProduct.RowTemplate.Height = 24;
            this.dgvOrderForProduct.Size = new System.Drawing.Size(842, 121);
            this.dgvOrderForProduct.TabIndex = 0;
            this.dgvOrderForProduct.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrderForProduct_CellDoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvDespatchesOfOrder);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(857, 149);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Despatches Of Order";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvDespatchesOfOrder
            // 
            this.dgvDespatchesOfOrder.AllowUserToAddRows = false;
            this.dgvDespatchesOfOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDespatchesOfOrder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDespatchesOfOrder.BackgroundColor = System.Drawing.Color.White;
            this.dgvDespatchesOfOrder.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvDespatchesOfOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDespatchesOfOrder.Location = new System.Drawing.Point(6, 6);
            this.dgvDespatchesOfOrder.Name = "dgvDespatchesOfOrder";
            this.dgvDespatchesOfOrder.ReadOnly = true;
            this.dgvDespatchesOfOrder.RowHeadersVisible = false;
            this.dgvDespatchesOfOrder.RowHeadersWidth = 51;
            this.dgvDespatchesOfOrder.RowTemplate.Height = 24;
            this.dgvDespatchesOfOrder.Size = new System.Drawing.Size(753, 164);
            this.dgvDespatchesOfOrder.TabIndex = 1;
            // 
            // txtStageName
            // 
            this.txtStageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStageName.BackColor = System.Drawing.SystemColors.Highlight;
            this.txtStageName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStageName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStageName.ForeColor = System.Drawing.Color.White;
            this.txtStageName.Location = new System.Drawing.Point(-1, 12);
            this.txtStageName.Name = "txtStageName";
            this.txtStageName.Size = new System.Drawing.Size(891, 29);
            this.txtStageName.TabIndex = 18;
            this.txtStageName.Text = "Customer Orders";
            this.txtStageName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 558);
            this.Controls.Add(this.txtStageName);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmOrders";
            this.Text = "Orders";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderForProduct)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDespatchesOfOrder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboOrders;
        private System.Windows.Forms.Button btnNewOrder;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOdf;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cboProducts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCustomerOrderId;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrderDate;
        private System.Windows.Forms.DataGridView dgvOrderForProduct;
        private System.Windows.Forms.DataGridView dgvDespatchesOfOrder;
        private System.Windows.Forms.ComboBox cboBuildTypes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCustomerOrderNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ckbIsActive;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtBcAssemblyOrder;
        private System.Windows.Forms.TextBox txtStageName;
        private System.Windows.Forms.TextBox txtPanelOdf;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtOrderStatus;
        private System.Windows.Forms.CheckBox ckbAllowLabelPrint;
        private System.Windows.Forms.CheckBox ckbActiveOrdersOnly;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtQuantityDespatched;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtQuantityOrdered;
        private System.Windows.Forms.DateTimePicker dtpExpectedKitArrivalDate;
        private System.Windows.Forms.DateTimePicker dtpExpectedPanelArrivalDate;
    }
}