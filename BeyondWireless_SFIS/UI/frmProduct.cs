using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;

namespace BeyondWireless_SFIS
{
    public partial class frmProduct : Form
    {
        private readonly DataService _dataService;
        private readonly frmMain _frmMain;
        //private List<CustomerOrder> _orders;
        private List<Product> _products;
        private int _lastSelectedProductId = 0;
        //private int _lastSelectedBuildTypeId = 0;

        private enum FormMode
        {
            View,
            Edit,
            New
        }

        public frmProduct(DataService dataService, frmMain frmMain)
        {
            InitializeComponent();
            _dataService = dataService;
            _frmMain = frmMain;
            this.MakeActivityAware();
        }

        private async  void frmProduct_Load(object sender, EventArgs e)
        { 
            txtStageName.BackColor = Constants.PrimaryAppColour;
            this.Text = $"Products {Constants.ApplicationVersion}";
            if (!DbConnectionOk())
                return;

            LoadInpectionLevels();
            GetProductsAsync();
            GetBuildTypesAsync();
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

        private void LoadInpectionLevels()
        {
            var inspectionLevels = new[]
            {
                    new { InspectionLevelId = 2, InspectionLevel = "2 - Normal" },
                    new { InspectionLevelId = 1, InspectionLevel = "1 - Loosened" },
                    new { InspectionLevelId = 3, InspectionLevel = "3 - Tightened" }
                };
            cboInspectionLevel.DisplayMember = "InspectionLevel";
            cboInspectionLevel.ValueMember = "InspectionLevelId";
            cboInspectionLevel.DataSource = inspectionLevels.ToList();
        }

        private async void GetBuildTypesAsync()
        {
            try
            {
                var buildTypes = await _dataService.GetBuildTypesAsync();
                cboBuildType.DisplayMember = "BuildTypeName";
                cboBuildType.ValueMember = "BuildTypeId";
                cboBuildType.DataSource = buildTypes;
                cboBuildType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error loading build types: {ex.Message}", true);
            }
        }

        //private void LoadBuildTypes()
        //{
        //    var buildTypes = new[]
        //    {
        //            new { BuildTypeId = 1, BuildType = "1 - CKD" },
        //            new { BuildTypeId = 2, BuildType = "2 - SKD" },
        //            new { BuildTypeId = 3, BuildType = "3 - CBU" }
        //        };
        //    cboBuildType.DisplayMember = "BuildType";
        //    cboBuildType.ValueMember = "BuildTypeId";
        //    cboBuildType.DataSource = buildTypes.ToList();
        //    cboBuildType.SelectedIndex = -1;
        //}
        private async void GetProductsAsync()
        {
            try
            {
                var products = await _dataService.GetProductsAsync(null);

                if (!products.Any())
                {
                    DisplayMessage("No active products found. You cannot continue...", true);
                    return;
                }
                _products = products;
                // Temporarily remove the event handler
                cboProducts.SelectedIndexChanged -= cboProducts_SelectedIndexChanged;
                cboProducts.DisplayMember = "ExtendedDisplayText";
                cboProducts.ValueMember = "ProductId";
                cboProducts.DataSource = _products.Where(p => p.IsActive == ckbActiveProductsOnly.Checked).ToList(); //This causes a selectedIndexChange
                cboProducts.SelectedIndex = -1;
                // Re-attach the event handler
                cboProducts.SelectedIndexChanged += cboProducts_SelectedIndexChanged;
                if (_lastSelectedProductId > 0)
                {
                    cboProducts.SelectedValue = _lastSelectedProductId;
                }

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error loading products: {ex.Message}", true);
            }
        }

        private void cboProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayProduct();
            btnEdit.Enabled = cboProducts.SelectedIndex != -1;
        }

        private void DisplayProduct()
        {
            var selectedProduct = cboProducts.SelectedItem as Product;
            txtProductId.Text = selectedProduct?.ProductId.ToString() ?? string.Empty;
            txtPartNumber.Text = selectedProduct?.PartNumber ?? string.Empty;
            txtProductName.Text = selectedProduct?.ProductName ?? string.Empty;
            txtProductShortCode.Text = selectedProduct?.ProductShortCode ?? string.Empty;
            cboInspectionLevel.SelectedValue = selectedProduct?.InspectionLevel ?? -1;
            ckbIsActive.Checked = selectedProduct?.IsActive ?? false;
            ckbHasSerialsPrinted.Checked = selectedProduct?.HasSerialsPrinted ?? false;
            cboBuildType.SelectedValue = selectedProduct?.BuildTypeId ?? -1;
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

        private void ckbActiveProductsOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbActiveProductsOnly.Checked)
            {
                cboProducts.DataSource = _products.Where(p => p.IsActive == true).ToList();
            }
            else
            {
                cboProducts.DataSource = _products;
            }

        }

        private void EnableEditInputs(bool isEnabled)
        {
            var product = cboProducts.SelectedItem as Product;
            if (product == null && isEnabled)
                return;

            txtProductName.ReadOnly = !isEnabled;
            txtPartNumber.ReadOnly = !isEnabled;
            txtProductShortCode.ReadOnly = !isEnabled;
            cboBuildType.Enabled = isEnabled;
            cboInspectionLevel.Enabled = isEnabled;
            cboProducts.Enabled = !isEnabled;
            ckbIsActive.Enabled = isEnabled;
            btnSave.Enabled = isEnabled;
            btnCancel.Enabled = isEnabled;

            if (product.HasSerialsPrinted)
            {
                txtPartNumber.ReadOnly = true;
                txtProductShortCode.ReadOnly = true;
                cboBuildType.Enabled = false;
            }
        }


        private void EnableInputs(bool isEnabled)
        {
            txtPartNumber.ReadOnly = !isEnabled;
            txtProductName.ReadOnly = !isEnabled;
            txtProductShortCode.ReadOnly = !isEnabled;
            cboInspectionLevel.Enabled = isEnabled;
            cboProducts.Enabled = !isEnabled;
            cboBuildType.Enabled = isEnabled;
            ckbIsActive.Enabled = isEnabled;
            btnSave.Enabled = isEnabled;
            btnCancel.Enabled = isEnabled;

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //EnableInputs(isEnabled: true);
            SetFormInputState(FormMode.New);
            cboProducts.SelectedIndex = -1;

            DisplayProduct();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //EnableInputs(isEnabled: false);
            SetFormInputState(FormMode.View);
            cboProducts.SelectedValue = _lastSelectedProductId;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _lastSelectedProductId = (int)cboProducts.SelectedValue;
            EnableEditInputs(isEnabled: true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CanSave())
                return;

            SaveProduct();
            DisplayProduct();
        }

        //private void btnSave_ClickAsync(object sender, EventArgs e)
        //{

        //}

        private void SetFormInputState(FormMode mode)
        {
            switch (mode)
            {
                case FormMode.View:
                    EnableInputs(isEnabled: false);
                    txtPartNumber.ReadOnly = true;
                    txtProductName.ReadOnly = true;
                    txtProductShortCode.ReadOnly = true;
                    cboInspectionLevel.Enabled = false;
                    cboProducts.Enabled = true;
                    cboBuildType.Enabled = false;
                    ckbIsActive.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    break;
                case FormMode.Edit:
                    EnableEditInputs(isEnabled: true);
                    break;
                case FormMode.New:
                    //EnableInputs(isEnabled: true);
                    txtPartNumber.ReadOnly = false;
                    txtProductName.ReadOnly = false;
                    txtProductShortCode.ReadOnly = false;
                    cboInspectionLevel.Enabled = true;
                    cboProducts.Enabled = true;
                    cboBuildType.Enabled = true;
                    ckbIsActive.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    break;
            }
        }

        private void SaveProduct()
        {
            if (string.IsNullOrEmpty(txtProductId.Text))
            {
                if (!AddProduct())
                    return;

                _lastSelectedProductId = 0;
                //EnableInputs(isEnabled: false);
                SetFormInputState(FormMode.View);
            }
            else
            {
                if (!UpdateProduct())
                    return;

                EnableEditInputs(isEnabled: false);
            }
            GetProductsAsync();
        }

        private bool CanSave()
        {
            if (string.IsNullOrEmpty(txtPartNumber.Text.Trim()))
            {
                DisplayMessage("Part Number cannot be empty.", true);
                return false;
            }

            if (!Regex.IsMatch(txtPartNumber.Text.Trim(), @"^Z\d{}$"))
            {

            }

            if (string.IsNullOrEmpty(txtProductName.Text.Trim()))
            {
                DisplayMessage("Description cannot be empty.", true);
                return false;
            }

            if (string.IsNullOrEmpty(txtProductShortCode.Text.Trim()))
            {
                DisplayMessage("Product Short Code cannot be empty.", true);
                return false;
            }



            if (cboInspectionLevel.SelectedIndex == -1)
            {
                DisplayMessage("Please select a valid inspection level.", true);
                return false;
            }

            if (!ckbIsActive.Checked)
            {
                DialogResult result = MessageBox.Show("You are saving an inactive product. Are you sure?", "Confirm Inactive Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (!(result == DialogResult.Yes))
                    return false;
            }
            return true;
        }

        private bool UpdateProduct()
        {
            try
            {
                var newProduct = new Product
                {
                    ProductId = int.Parse(txtProductId.Text.Trim()),
                    PartNumber = txtPartNumber.Text.Trim(),
                    ProductName = txtProductName.Text.Trim(),
                    ProductShortCode = txtProductShortCode.Text.Trim(),
                    InspectionLevel = (int)cboInspectionLevel.SelectedValue,
                    IsActive = ckbIsActive.Checked,
                    BuildTypeId = (int)cboBuildType.SelectedValue
                };

                _dataService.UpdateProduct(newProduct);
                DisplayMessage("Product Updated successfully.");
                GetProductsAsync();
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error adding product: {ex.Message}", true);
                return false;
            }
        }

        private bool AddProduct()
        {
            try
            {
                var newProduct = new Product
                {
                    PartNumber = txtPartNumber.Text.Trim(),
                    ProductName = txtProductName.Text.Trim(),
                    ProductShortCode = txtProductShortCode.Text.Trim(),
                    BuildTypeId = (int)cboBuildType.SelectedValue,
                    InspectionLevel = (int)cboInspectionLevel.SelectedValue,
                    IsActive = ckbIsActive.Checked
                };

                _dataService.AddProduct(newProduct);
                DisplayMessage("Product added successfully.");
                GetProductsAsync();
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error adding product: {ex.Message}", true);
                return false;
            }
        }

       
    }
}
