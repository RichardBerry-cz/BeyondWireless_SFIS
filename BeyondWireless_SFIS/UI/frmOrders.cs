using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeyondWireless_SFIS
{
    public partial class frmOrders : Form
    {
        private readonly DataService _dataService;
        private readonly frmMain _frmMain;
        private CustomerOrder _currentOrder;
        private List<CustomerOrder> _orders;
        private List<Product> _products;
        private int _lastSelectedOrder = 0;

        private enum FormMode
        {
            NewOrder,
            EditOrder,
            DisplayOrder
        }
        public frmOrders(DataService dataService, frmMain frmMain)
        {
            InitializeComponent();
            _dataService = dataService;
            _frmMain = frmMain;
            this.MakeActivityAware();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            txtStageName.BackColor = Constants.PrimaryAppColour;
            this.Text = $"Orders {Constants.ApplicationVersion}";
            if (!DbConnectionOk())
                return;
            try
            {
                LoadBuildTypes();
                await GetOrdersAsync();
                GetProductsAsync();
                LoadOrdersIntoComboBox();
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error initializing form: {ex.Message}", true);
            }
        }
        private bool DbConnectionOk()
        {
            try
            {
                return _dataService.DbConnectionOk();
            }
            catch (Exception ex)
            {
                DisplayMessage($"Database connection error.{Environment.NewLine}Check Network Connections to CZ Network.{Environment.NewLine}Error: {ex.Message}", true);
                return false;
            }
        }

        private void LoadBuildTypes()
        {
            var buildTypes = new[]
                {
                    new { BuildTypeId = 1, BuildType = "CKD" },
                    new { BuildTypeId = 2, BuildType = "SKD" },
                    new { BuildTypeId = 3, BuildType = "CBU" }
                };
            cboBuildTypes.DisplayMember = "BuildType";
            cboBuildTypes.ValueMember = "BuildTypeId";
            cboBuildTypes.DataSource = buildTypes.ToList();
        }

        private async Task GetOrdersAsync()
        {
            try
            {
                var orders = await _dataService.GetCustomerOrdersAsync(isActive: null);//Get all orders

                if (!orders.Any())
                {
                    MessageBox.Show("No active orders found. You cannot continue...");
                    return;
                }
                _orders = orders;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error loading orders: {ex.Message}", true);
            }
        }

        private void LoadOrdersIntoComboBox()
        {
            if (_orders == null || !_orders.Any())
                return;

            var orders = _orders.OrderBy(s => s.Model).ThenBy(s => s.Odf).ToList();

            if (ckbActiveOrdersOnly.Checked)
            {
                orders = orders.Where(o => o.IsActive).ToList();
            }

            cboOrders.SelectedIndexChanged -= cboOrders_SelectedIndexChanged;
            cboOrders.DisplayMember = "DisplayText";
            cboOrders.ValueMember = "CustomerOrderId";
            cboOrders.DataSource = orders;
            cboOrders.SelectedIndex = -1;
            cboOrders.SelectedIndexChanged += cboOrders_SelectedIndexChanged;

            if (_lastSelectedOrder != 0)
            {
                cboOrders.SelectedValue = _lastSelectedOrder;
                DisplayOrderDetails();
            }
        }

        private async void GetProductsAsync()
        {
            try
            {
                var products = await _dataService.GetProductsAsync(isActive: true);

                if (!products.Any())
                {
                    DisplayMessage("No active products found. You cannot continue...", true);
                    return;
                }
                _products = products;
                cboProducts.SelectedIndexChanged -= cboProducts_SelectedIndexChanged;
                cboProducts.DisplayMember = "DisplayText";
                cboProducts.ValueMember = "ProductId";
                cboProducts.DataSource = products;
                cboProducts.SelectedIndex = -1;
                cboProducts.SelectedIndexChanged += cboProducts_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error loading orders: {ex.Message}", true);
            }
        }

       

        private void DisplayMessage(string message, bool isError = false)
        {
            if (isError)
            {
                using (frmError frm = new frmError(message))
                {
                    frm.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOrders.SelectedIndex == -1)
            {
                _currentOrder = null;

                return;
            }
            _currentOrder = cboOrders.SelectedItem as CustomerOrder;
            DisplayOrderDetails();
            GetDespatchesOfOrderAsync(_currentOrder.CustomerOrderId);
        }

        private void DisplayOrderDetails()
        {
            if (_currentOrder == null)
            {
                cboProducts.SelectedIndex = -1;
                cboBuildTypes.SelectedIndex = -1;
                txtCustomerOrderNumber.Text = string.Empty;
                txtQuantityDespatched.Text = string.Empty;
                txtQuantityOrdered.Text = string.Empty;
                txtOdf.Text = string.Empty;
                txtPanelOdf.Text = string.Empty;
                txtOrderDate.Text = string.Empty;
                txtCustomerOrderId.Text = string.Empty;
                txtBcAssemblyOrder.Text = string.Empty;
                ckbIsActive.Checked = false;
                ckbAllowLabelPrint.Checked = false;
                txtOrderStatus.Text = string.Empty;
                return;
            }

            txtCustomerOrderNumber.Text = _currentOrder.CustomerOrderNumber;
            txtCustomerOrderId.Text = _currentOrder.CustomerOrderId.ToString();
            txtOrderDate.Text = _currentOrder.DateLoaded.ToString("yyyy-MM-dd");
            txtOdf.Text = _currentOrder.Odf;
            txtPanelOdf.Text = _currentOrder.PanelOdf;
            txtQuantityOrdered.Text = _currentOrder.QuantityOrdered.ToString();
            txtQuantityDespatched.Text = _currentOrder.QuantityDespatched.ToString();
            txtBcAssemblyOrder.Text = _currentOrder.BCAssemblyOrder;
            cboProducts.SelectedValue = _currentOrder.ProductId;
            cboBuildTypes.SelectedValue = _currentOrder.BuildTypeId;
            ckbIsActive.Checked = _currentOrder.IsActive;
            ckbAllowLabelPrint.Checked = _currentOrder.LabelPrintingActive;
            txtOrderStatus.Text = $"{_currentOrder.CustomerOrderStatusId} - {_currentOrder.StatusName}";
            dtpExpectedKitArrivalDate.Value = _currentOrder.ExpectedKitArrivalDate;
            dtpExpectedPanelArrivalDate.Value = _currentOrder.ExpectedPanelArrivalDate;
            btnEdit.Enabled = true;
        }

        private void cboProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducts.SelectedIndex == -1)
            {
                return;
            }

            var product = cboProducts.SelectedItem as Product;

            cboBuildTypes.SelectedValue = product.BuildTypeId;
            GetOrdersOfProductAsync();

        }

        private async void GetDespatchesOfOrderAsync(int customerOrderId)
        {
            try
            {
                var despatches = await _dataService.GetDespatchesByCustomerOrderIdAsync(customerOrderId: customerOrderId);
                if (despatches == null || despatches.Rows.Count == 0)
                {
                    dgvOrderForProduct.DataSource = null;
                    return;
                }

                dgvDespatchesOfOrder.DataSource = despatches;

            }
            catch (Exception ex)
            {
                DisplayMessage("Error loading orders for product: " + ex.Message, true);
            }
        }

        private async void GetOrdersOfProductAsync()
        {
            try
            {
                var orders = await _dataService.GetCustomerOrdersOfProductAsync(ProductId: (int)cboProducts.SelectedValue, IsActive: true);
                if (orders == null || !orders.Any())
                {
                    dgvOrderForProduct.DataSource = null;
                    return;
                }

                dgvOrderForProduct.DataSource = orders.Select(o => new
                {
                    o.CustomerOrderId,
                    o.CustomerOrderNumber,
                    o.PartNumber,
                    o.ProductName,
                    o.Model,
                    o.Odf,
                    o.PanelOdf,
                    o.QuantityOrdered,
                    o.QuantityDespatched,
                    o.DateLoaded,
                    o.ExpectedKitArrivalDate,
                    o.ExpectedPanelArrivalDate,
                    o.BuildType,
                    o.BCAssemblyOrder,
                    o.IsActive,

                }).ToList();
                dgvOrderForProduct.Columns["CustomerOrderId"].Visible = false;
            }
            catch (Exception ex)
            {
                DisplayMessage("Error loading orders for product: " + ex.Message, true);
            }
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            //EnableNewOrderInputs(isEnabled: true);
            SetFormInputState(FormMode.NewOrder);
            DisplayOrderDetails();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           // EnableNewOrderInputs(isEnabled: false);
            SetFormInputState(FormMode.DisplayOrder);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!CanSave())
                return;

            if (!SaveOrder())
                return;

            //EnableNewOrderInputs(isEnabled: false);
            SetFormInputState(FormMode.DisplayOrder);
            await GetOrdersAsync();
            LoadOrdersIntoComboBox();
        }

        private bool SaveOrder()
        {
            DbResponse response = null;

            if (!_dataService.DbConnectionOk())
            {
                DisplayMessage("Database connection is not available. Please check your connection settings.", true);
                return false;
            }

            try
            {
                CustomerOrder customerOrder = new CustomerOrder()
                {
                    Odf = txtOdf.Text.Trim(),
                    PanelOdf = txtPanelOdf.Text.Trim(),
                    CustomerOrderId = string.IsNullOrWhiteSpace(txtCustomerOrderId.Text) ? 0 : int.Parse(txtCustomerOrderId.Text.Trim()),
                    CustomerOrderNumber = txtCustomerOrderNumber.Text.Trim(),
                    ProductId = (int)cboProducts.SelectedValue,
                    BuildTypeId = (int)cboBuildTypes.SelectedValue,
                    QuantityOrdered = int.Parse(txtQuantityOrdered.Text.Trim()),
                    QuantityDespatched = string.IsNullOrEmpty(txtQuantityDespatched.Text) ? 0 : int.Parse(txtQuantityDespatched.Text.Trim()),
                    DateLoaded = string.IsNullOrEmpty(txtQuantityDespatched.Text) ? DateTime.Now : DateTime.Parse(txtOrderDate.Text.Trim()),
                    DateModified = DateTime.Now,
                    BCAssemblyOrder = string.IsNullOrEmpty(txtBcAssemblyOrder.Text.Trim()) ? string.Empty : txtBcAssemblyOrder.Text.Trim(), // Not currently used but will be needed for traceability
                    ModifiedBy = _frmMain.CurrentUser.UserId,
                    IsActive = ckbIsActive.Checked,
                    LabelPrintingActive = ckbAllowLabelPrint.Checked,
                    ExpectedKitArrivalDate = dtpExpectedKitArrivalDate.Value.Date,
                    ExpectedPanelArrivalDate = dtpExpectedPanelArrivalDate.Value.Date
                };

                if (string.IsNullOrWhiteSpace(txtCustomerOrderId.Text))
                {
                    // New order
                    response = _dataService.AddCustomerOrder(customerOrder);
                }
                else
                {
                    // Update Order
                    _lastSelectedOrder = customerOrder.CustomerOrderId;
                    response = _dataService.UpdateCustomerOrder(customerOrder);
                }

                if (response == null)
                {
                    DisplayMessage("Failed to update order.", true);
                }

                if (response.StatusCode != 0)
                {
                    DisplayMessage($"Failed to update order: {response.Message}", true);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error creating order: {ex.Message}", true);
                return false;
            }

        }

        private bool CanSave()
        {
            if (!string.IsNullOrWhiteSpace(txtCustomerOrderId.Text) && !Int32.TryParse(txtCustomerOrderId.Text.Trim(), out int _))
            {
                DisplayMessage("Invalid Customer Order ID", true);
                return false;
            }

            //if (!Validation.IsValidOdf(txtOdf.Text.Trim()))
            //{
            //    DisplayMessage("Invalid ODF Number.", true);
            //    return false;
            //}

            //if (!Validation.IsValidOdf(txtPanelOdf.Text.Trim()))
            //{
            //    DisplayMessage("Invalid Panel ODF Number.", true);
            //    return false;
            //}

            var productId = _products.FirstOrDefault(p => p.ProductId == (int)cboProducts.SelectedValue)?.ProductId;
            if (txtPanelOdf.Text.Trim() == txtOdf.Text.Trim() && (productId != 16 || productId != 17)) //55C655 Is Single BOM, Panel ODF can be the same as ODF for this model
            {
                DisplayMessage("Panel ODF cannot be the same as ODF.", true);
                return false;
            }

            if (cboProducts.SelectedIndex == -1)
            {
                DisplayMessage("Please select a product.", true);
                return false;
            }

            if (cboBuildTypes.SelectedIndex == -1)
            {
                DisplayMessage("Please select a build type.", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtQuantityOrdered.Text) || !int.TryParse(txtQuantityOrdered.Text.Trim(), out int quantityOrdered) || quantityOrdered <= 0)
            {
                DisplayMessage("Please enter a valid order Quantity.", true);
                return false;
            }

            if (!string.IsNullOrEmpty(txtQuantityDespatched.Text) && (!Int32.TryParse(txtQuantityDespatched.Text.Trim(), out int quantityDespatched) || quantityDespatched < 0))
            {
                DisplayMessage("Invalid Quantity Despatched", true);
                return false;
            }

            if (!string.IsNullOrEmpty(txtOrderDate.Text) && (!DateTime.TryParse(txtOrderDate.Text.Trim(), out DateTime _)))
            {
                DisplayMessage("Invalid Order Date", true);
                return false;
            }

            if (!ckbIsActive.Checked)
            {
                DialogResult result = MessageBox.Show("You are saving an inactive order. Are you sure?", "Confirm Inactive Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (!(result == DialogResult.Yes))
                    return false;
            }

            if (string.IsNullOrEmpty(txtCustomerOrderNumber.Text))
            {
                DisplayMessage("Please enter the Customer Order Number", true);
                return false;
            }
            return true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //EnableEditFields();
            SetFormInputState(FormMode.EditOrder);
        }

        //private void EnableEditFields()
        //{
        //    cboOrders.Enabled = false;
        //    txtCustomerOrderNumber.ReadOnly = _currentOrder.QuantityDespatched != 0;
        //    txtOdf.ReadOnly = _currentOrder.CustomerOrderStatusId > 1;
        //    txtPanelOdf.ReadOnly = _currentOrder.CustomerOrderStatusId > 1;
        //    txtQuantityOrdered.ReadOnly = false;
        //    txtBcAssemblyOrder.ReadOnly = false;
        //    cboProducts.Enabled = _currentOrder.CustomerOrderStatusId == 1; ;
        //    cboBuildTypes.Enabled = _currentOrder.CustomerOrderStatusId == 1;
        //    ckbIsActive.Enabled = true;
        //    ckbAllowLabelPrint.Enabled = true;
        //    btnCancel.Enabled = true;
        //    btnSave.Enabled = true;
        //    btnEdit.Enabled = false;
        //    dtpExpectedKitArrivalDate.Enabled = true;
        //    dtpExpectedPanelArrivalDate.Enabled = true;
        //}

        private void SetFormInputState(FormMode mode)
        {
            switch (mode)
            {
                case FormMode.NewOrder:
                    cboOrders.SelectedIndex = -1;
                    cboOrders.Enabled = false;
                    dtpExpectedKitArrivalDate.Enabled = true;
                    dtpExpectedKitArrivalDate.Value = new DateTime(1900, 1, 1);
                    dtpExpectedPanelArrivalDate.Enabled = true;
                    dtpExpectedPanelArrivalDate.Value = new DateTime(1900, 1, 1);
                    cboProducts.SelectedIndex = -1;
                    cboProducts.Enabled = true;
                    txtCustomerOrderNumber.ReadOnly = false;
                    txtOdf.ReadOnly = false;
                    txtPanelOdf.ReadOnly = false;
                    txtQuantityOrdered.ReadOnly = false;
                    txtBcAssemblyOrder.ReadOnly = false;
                    ckbIsActive.Enabled = true;
                    ckbAllowLabelPrint.Enabled = true;
                    ckbAllowLabelPrint.Checked = false;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    txtQuantityDespatched.Text = "0";
                    _currentOrder = null;
                    break;
                case FormMode.EditOrder:
                    cboOrders.Enabled = false;
                    txtCustomerOrderNumber.ReadOnly = _currentOrder.QuantityDespatched != 0;
                    txtOdf.ReadOnly = _currentOrder.CustomerOrderStatusId > 1;
                    txtPanelOdf.ReadOnly = _currentOrder.CustomerOrderStatusId > 1;
                    txtQuantityOrdered.ReadOnly = false;
                    txtBcAssemblyOrder.ReadOnly = false;
                    cboProducts.Enabled = _currentOrder.CustomerOrderStatusId == 1; ;
                    cboBuildTypes.Enabled = _currentOrder.CustomerOrderStatusId == 1;
                    ckbIsActive.Enabled = true;
                    ckbAllowLabelPrint.Enabled = true;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    btnEdit.Enabled = false;
                    dtpExpectedKitArrivalDate.Enabled = true;
                    dtpExpectedPanelArrivalDate.Enabled = true;
                    break;
                case FormMode.DisplayOrder:
                    cboOrders.Enabled = true;
                    txtCustomerOrderNumber.ReadOnly = true;
                    cboProducts.Enabled = false;
                    cboBuildTypes.Enabled = false;
                    dtpExpectedKitArrivalDate.Enabled = false;
                    dtpExpectedPanelArrivalDate.Enabled = false;
                    txtQuantityOrdered.ReadOnly = true;
                    txtOdf.ReadOnly = true;
                    txtPanelOdf.ReadOnly = true;
                    txtBcAssemblyOrder.ReadOnly = true;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    ckbIsActive.Enabled = false;
                    ckbAllowLabelPrint.Enabled = false;
                    break;
            }
        }

        private void dgvOrderForProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            cboOrders.SelectedValue = dgvOrderForProduct.Rows[e.RowIndex].Cells["CustomerOrderId"].Value;
        }

        private void ckbActiveOrdersOnly_CheckedChanged(object sender, EventArgs e)
        {

            if (_orders == null || !_orders.Any())
                return;
            LoadOrdersIntoComboBox();
        }

        //private void btnAllowLabelPrint_Click(object sender, EventArgs e)
        //{
        //    SetAllowLabelPrint();
        //}

        //private void SetAllowLabelPrint()
        //{
        //               if (_currentOrder == null)
        //    {
        //        DisplayMessage("No order selected.", true);
        //        return;
        //    }
        //    if (_currentOrder.QuantityDespatched > 0)
        //    {
        //        DisplayMessage("Cannot allow label printing for orders that have already been despatched.", true);
        //        return;
        //    }
        //    try
        //    {
        //        _dataService.AllowLabelPrinting(_currentOrder.CustomerOrderId);
        //        DisplayMessage("Label printing has been allowed for this order.");
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage($"Error allowing label printing: {ex.Message}", true);
        //    }
        //}
    }
}
