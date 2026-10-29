using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;
using static BeyondWireless_SFIS.Shared.Logging;

namespace BeyondWireless_SFIS
{
    public partial class frmUnit : Form
    {
        private readonly DataService _dataService;
        private readonly frmMain _frmMain;


        public frmUnit(DataService dataService, frmMain frmMain)
        {
            InitializeComponent();
            _dataService = dataService;
            _frmMain = frmMain;
            this.Text = $"Unit Details {Constants.ApplicationVersion}";
            this.MakeActivityAware();
        }



        private async void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtInput.Text))
                {
                    string input = txtInput.Text.Trim();
                    txtInput.Clear();
                    ClearAllData();
                    await PopulateAllGridsAsync(input);
                    txtInput.Focus();
                }
            }
        }

        public async Task PopulateAllGridsAsync(string serialNumber)
        {
             var tasks = new List<Task>
            {
                PopulateGridAsync(dgvUnitHistory, async () => await _dataService.GetUnitHistoryBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvAoiTestHistory, async () => await _dataService.GetAoiTestHistoryBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvPcbaTestHistory, async () => await _dataService.GetPcbaTestHistoryBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvModuleLinking, async () => await _dataService.GetLinkedModulesBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvLinkedAccessories, async () => await _dataService.GetLinkedAccessoriesBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvFaTestHistory, async () => await _dataService.GetFaTestHistoryBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvOqcInspectionHistory, async () => await _dataService.GetOqcHistoryBySerialAsync(serialNumber)),
                PopulateGridAsync(dgvRepairHistory, async () => await _dataService.GetRepairHistoryBySerialAsync(serialNumber)),
               //PopulateOrderDetailsAsync(serialNumber),
                GetUnitDetailsAsync(serialNumber)
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch
            {
                // Individual errors are handled in PopulateGridAsync
            }
        }

        private async Task GetUnitDetailsAsync(string serialNumber)
        {
            DisplayUnitDetails(null);  //Clear previous details
            try
            {
                if (string.IsNullOrEmpty(serialNumber))
                     return;

                    var unit = await _dataService.GetUnitBySerialNumberAsync(serialNumber);
                    DisplayUnitDetails(unit);
 
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to fetch unit details for {serialNumber}: {ex.Message}");
            }
        }

        private void DisplayUnitDetails(Unit unit)
        {
            lblCurrentStage.Text = unit?.StageName ?? string.Empty;
            lblModel.Text = unit?.Model ?? string.Empty; 
            lblOdf.Text = unit?.Odf ?? string.Empty; ;
            ckbIsFaulty.Checked = unit?.IsFaulty ?? false;
            ckbIsScrapped.Checked = unit?.IsScrapped ?? false;
            ckbIsSold.Checked = unit?.IsSold ?? false;
            ckbIsBlocked.Checked = unit?.IsBlocked ?? false;
            ckbIsStolen.Checked = unit?.IsStolen ?? false;
        }

        //private async Task PopulateOrderDetailsAsync(string serialNumber)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(serialNumber))
        //        {
        //            if (lblMessage.InvokeRequired)
        //            {
        //                lblMessage.Invoke((MethodInvoker)(() => lblMessage.Text = "Please enter a valid serial number."));
        //            }
        //            else
        //            {
        //                lblMessage.Text = "Please enter a valid serial number.";
        //            }
        //            return;
        //        }

        //        var info = Validation.IsValidModuleSerial(serialNumber);
        //        if (info == null || !info.IsValid)
        //        {
        //            return;
        //        }

        //        var orderDetails = await _dataService.GetCustomerOrderByOdfAsync(info.Odf);
        //        if (orderDetails != null)
        //        {
        //            // Update all labels in one Invoke call for efficiency
        //            if (lblOdf.InvokeRequired || lblModel.InvokeRequired || lblCurrentStage.InvokeRequired || lblMessage.InvokeRequired)
        //            {
        //                lblOdf.Invoke((MethodInvoker)(() =>
        //                {
        //                    lblOdf.Text = $"{orderDetails.Odf}";
        //                    lblModel.Text = $"{orderDetails.Model}";
        //                    lblCurrentStage.Text = $"{orderDetails.PartNumber}";
        //                    lblMessage.Text = serialNumber;
        //                }));
        //            }
        //            else
        //            {
        //                lblOdf.Text = $"{orderDetails.Odf}";
        //                lblModel.Text = $"{orderDetails.Model}";
        //                lblCurrentStage.Text = $"{orderDetails.PartNumber}";
        //                lblMessage.Text = serialNumber;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Failed to fetch order details for {serialNumber}: {ex.Message}");
        //    }
        //}

        private void ClearAllData()
        {
            dgvUnitHistory.DataSource = null;
            dgvAoiTestHistory.DataSource = null;
            dgvPcbaTestHistory.DataSource = null;
            dgvFaTestHistory.DataSource = null;
            dgvLinkedAccessories.DataSource = null;
            dgvModuleLinking.DataSource = null;
            dgvOqcInspectionHistory.DataSource = null;
            dgvRepairHistory.DataSource = null;
            lblCurrentStage.Text = string.Empty;
            lblModel.Text = string.Empty;
            lblOdf.Text = string.Empty;
            ckbIsFaulty.Checked = false;
            ckbIsScrapped.Checked = false;
            ckbIsSold.Checked = false;
        }

        private async Task PopulateGridAsync(DataGridView grid, Func<Task<DataTable>> dataFetchFunction)
        {
            try
            {
                var data = await dataFetchFunction();

                if (grid.InvokeRequired)
                {
                    grid.Invoke((MethodInvoker)delegate
                    {
                        grid.DataSource = data;
                        grid.Visible = true;
                    });
                }
                else
                {
                    grid.DataSource = data;
                    grid.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to populate grid {grid.Name}: {ex.Message}");

                var errorMessage = $"Error loading data: {ex.Message}";
                if (grid.InvokeRequired)
                {
                    grid.Invoke((MethodInvoker)delegate
                    {
                        ShowErrorInGrid(grid, errorMessage);
                    });
                }
                else
                {
                    ShowErrorInGrid(grid, errorMessage);
                }
            }
        }

        private void ShowErrorInGrid(DataGridView grid, string message)
        {
            grid.DataSource = null;
            grid.Rows.Clear();
            grid.Columns.Clear();

            grid.Columns.Add("Error", "Error");
            grid.Rows.Add(message);
            grid.Rows[0].DefaultCellStyle.ForeColor = Color.Red;
            grid.Visible = true;
        }

    }

}
