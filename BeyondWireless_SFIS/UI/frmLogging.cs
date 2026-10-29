using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BeyondWireless_SFIS.Services;

namespace BeyondWireless_SFIS
{
    public partial class frmLogging : Form
    {
        private readonly DataService _dataService;
        private readonly frmMain _frmMain;
        private readonly List<Stage> _stages;

        private Stage _currentStage;
        private Unit _currentUnit;
        private CustomerOrder _currentOrder;


        public frmLogging(DataService dataService, frmMain frmMain, List<Stage> stages)
        {
            InitializeComponent();
            _frmMain = frmMain;
            _dataService = dataService;
            _stages = stages;
            this.MakeActivityAware();
        }

        private void frmLogging_Load(object sender, EventArgs e)
        {
            txtStageName.BackColor = Constants.PrimaryAppColour;
            this.Text = $"Logging {Constants.ApplicationVersion}";
            if (!DbConnectionOk())
                return;

            GetOrdersAsync();
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



        private async void GetOrdersAsync()
        {

            var orders = await _dataService.GetCustomerOrdersAsync(isActive: true);

            if (!orders.Any())
            {
                MessageBox.Show("No active orders found. You cannot continue...");
                return;
            }
            cboOrders.SelectedIndexChanged -= cboOrders_SelectedIndexChanged;
            cboOrders.DisplayMember = "DisplayText";
            cboOrders.ValueMember = "CustomerOrderId";
            cboOrders.DataSource = orders;
            cboOrders.SelectedIndex = -1;
            cboOrders.SelectedIndexChanged += cboOrders_SelectedIndexChanged;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtInput.Text))
                {
                    txtInput.ReadOnly = true;
                    string input = txtInput.Text.Trim();
                    ProcessInput(input);
                    txtInput.ReadOnly = false;
                }
            }
        }

        private void ProcessInput(string input)
        {
            _currentUnit = null;

            if (!IsValidUnit(input))
            {
                return;
            }

            if (_currentUnit != null)
            {
                var unitToSave = _currentUnit;
                unitToSave.StageId = _currentStage.StageId;
                if (SaveUnitToDatabase(unitToSave))
                    DisplayMessage($"Unit with Serial {input} has been saved. Scan next Serial Number to continue...", false);
            }


            txtInput.Clear();
        }

        private bool IsValidUnit(string input)
        {
            if (_currentStage.StageId == 10 || _currentStage.StageId == 17)//Warehouse
            {
                GetCurrentUnitBySetSerial(input);
            }
            else
            {
                GetCurrentUnitByPcbaSerial(input);
            }


            if (_currentUnit == null)
            {
                DisplayMessage($"Serial {input} not found in database.{Environment.NewLine}Please take unit to the start of the process", true);
                txtInput.Clear();
                return false;
            }

            if (_currentUnit.IsBlocked)
            {
                DisplayMessage($"Unit is BLOCKED!!!!!{Environment.NewLine}Reason:{_currentUnit.ReasonBlocked}", true);
                return false;
            }

            if (!(_currentUnit.NextStageId == _currentStage.StageId || _currentUnit.NextRequiredStageId == _currentStage.StageId))
            {
                if (_currentUnit.NextRequiredStageId == _currentUnit.NextStageId)
                {
                    DisplayMessage($"Unit is at the incorrect stage.{Environment.NewLine}Unit is expected at {_currentUnit.NextStageName}", true);
                }
                else
                {
                    DisplayMessage($"Unit is at the incorrect stage.{Environment.NewLine}Unit is expected at {_currentUnit.NextStageName} or {_currentUnit.NextRequiredStageName}", true);
                }

                txtInput.Clear();
                return false;
            }

            if (_currentUnit.IsScrapped)
            {
                DisplayMessage($"Unit with Serial {input} is scrapped.{Environment.NewLine}You cannot continue with this unit.", true);
                txtInput.Clear();
                return false;
            }

            if (_currentUnit.IsFaulty)
            {
                DisplayMessage($"Unit with Serial {input} is faulty.{Environment.NewLine}You cannot continue with this unit.", true);
                txtInput.Clear();
                return false;
            }

            if (_currentUnit.IsSold)
            {
                DisplayMessage($"Unit with Serial {input} was despatched to After Sales.{Environment.NewLine}You cannot continue with this unit.", true);
                txtInput.Clear();
                return false;
            }

            if (_currentUnit.IsStolen)
            {
                DisplayMessage($"Unit with Serial {input} was Stolen!!!.{Environment.NewLine}You cannot continue with this unit.", true);
                txtInput.Clear();
                return false;
            }

            if (_currentUnit.IsBlocked)
            {
                DisplayMessage($"Unit with Serial {input}is Blocked.{Environment.NewLine}You cannot continue with this unit.", true);
                txtInput.Clear();
                return false;
            }


            //jippo for incorrect odf loaded
            if (_currentUnit.Odf == "ELJ243687F" && _currentOrder.Odf != "ELJ243687E")
            {
                DisplayMessage($"Serial does not match the selected order (ODF){Environment.NewLine}Please scan a valid Set serial for the selected order.", true);
                return false;
            }

            //jippo for incorrect odf loaded
            if (_currentOrder.Odf != "ELJ243687E")
            {
                if (_currentUnit.Odf != _currentOrder.Odf)
                {
                    DisplayMessage($"Serial does not match the selected order (ODF){Environment.NewLine}Please scan a valid Set serial for the selected order.", true);
                    return false;
                }
            }

            //This is blocked at AOI
            //var orderBalance = _dataService.GetUnitsToBuildByCustomerOrderId(_currentOrder.CustomerOrderId);
            //if (orderBalance == 0)
            //{
            //    DisplayMessage($"This Order is already FULLY BUILT!!!!{Environment.NewLine}Quantity to build is {orderBalance}{Environment.NewLine}Stop the line and take the PCBA to your supervisor", true);
            //    return false;
            //}


            return true;
        }

        private void GetCurrentUnitByPcbaSerial(string serialNumber)
        {
            try
            {

                var unit = _dataService.GetUnitBySerialNumber(serialNumber);
                if (unit == null)
                {
                    DisplayMessage($"PCBA Serial {serialNumber} not found in database.{Environment.NewLine}Please take unit to the start of the process (AOI)", true);
                    return;
                }

               
                _currentUnit = unit;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Failed to retrieve unit information. Please try again. {Environment.NewLine}Error:{ex.Message}", true);
            }
        }

        private void GetCurrentUnitBySetSerial(string serialNumber)
        {
            try
            {
                var unit = _dataService.GetUnitBySetSerialNumber(serialNumber);
                if (unit == null)
                {
                    DisplayMessage($"SET Serial {serialNumber} not found in database.{Environment.NewLine}Please ensure PCBA is linked to a SET", true);
                    return;
                }



                if (unit == null)
                    return;

                _currentUnit = unit;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Failed to retrieve unit information. Please try again. {Environment.NewLine}Error:{ex.Message}", true);
            }
        }

        private bool SaveUnitToDatabase(Unit unit)
        {
            try
            {
                var response = _dataService.UpdateUnitStage(unit, _frmMain.CurrentUser);
                if (response == null)
                {
                    DisplayMessage($"Error updating unit stage - No response from database", IsError: true);
                    return false;
                }

                if (response.StatusCode != 0)
                {
                    DisplayMessage($"Error updating unit stage.{Environment.NewLine}Resolve any issues and try again{Environment.NewLine}Error: {response.Message}", IsError: true);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error saving unit to database.{Environment.NewLine}Resolve any issues and try again{Environment.NewLine}Error:  {ex.Message}", IsError: true);
                return false;
            }
        }

        //private bool IsValidPcba(string input)
        //{
        //    var pcbaInfo = Validation.IsValidModuleSerial(input);// This should cater for both SKD and CKD PCBA serials
        //    if (!pcbaInfo.IsValid)
        //    {
        //        DisplayMessage("Invalid PCBA serial format.", true);
        //        return false;
        //    }


        //    if (pcbaInfo.Odf != _currentOrder.Odf)
        //    {
        //        DisplayMessage($"Unit does not belong to the the selected order (ODF){Environment.NewLine}Select the correct order from the dropdown, or use PCBA's built for the selected order", true);
        //        return false;
        //    }

        //    GetCurrentUnit(input);

        //    if (_currentUnit == null)
        //    {
        //        DisplayMessage($"PCBA Serial {input} not found in database.{Environment.NewLine}Please take unit to the start of the process (AOI)", true);
        //        txtInput.Clear();
        //        return false;
        //    }

        //    if (!(_currentUnit.NextStageId == _currentStage.StageId || _currentUnit.NextRequiredStageId == _currentStage.StageId))
        //    {
        //        if (_currentUnit.NextRequiredStageId == _currentUnit.NextStageId)
        //        {
        //            DisplayMessage($"Unit is at the incorrect stage.{Environment.NewLine}Unit is expected at {_currentUnit.NextStageName}", true);
        //        }
        //        else
        //        {
        //            DisplayMessage($"Unit is at the incorrect stage.{Environment.NewLine}Unit is expected at {_currentUnit.NextStageName} or {_currentUnit.NextRequiredStageName}", true);
        //        }

        //        txtInput.Clear();
        //        return false;
        //    }

        //    if (_currentUnit.IsScrapped)
        //    {
        //        DisplayMessage($"Unit with Serial {input} is scrapped.{Environment.NewLine}You cannot continue with this unit.", true);
        //        txtInput.Clear();
        //        return false;
        //    }

        //    return true;
        //}

        private bool GetCurrentUnit(string serialNumber)
        {
            try
            {
                var unit = _dataService.GetUnitBySerialNumber(serialNumber); //Returns null if unit not found
                if (unit == null)
                {
                    DisplayMessage($"Unit with serial number {serialNumber} not found in database.{Environment.NewLine}Take the Unit to the start of the process", IsError: true);
                    return false;
                }
                _currentUnit = unit;
                return true;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving unit from database. {Environment.NewLine}Error: {ex.Message}", IsError: true);
                return false;
            }
        }

        private void DisplayMessage(string message, bool? IsError = false)
        {
            if (IsError.HasValue && IsError.Value)
            {
                frmError frm = new frmError(message);
                frm.ShowDialog(this);
            }
            else
            {
                lblMessage.Text = message;
            }
        }

        private void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOrders.SelectedIndex == -1)
            {
                txtInput.ReadOnly = true;
                lblProductName.Text = string.Empty;
                txtStageName.Text = "Logging Stage";
                _currentOrder = null;
                cboOrders.Focus();
                return;
            }

            var order = cboOrders.SelectedItem as CustomerOrder;
            if (order == null)
            {
                DisplayMessage("Cannot get order information from selected order");
                return;
            }
            _currentOrder = order;

            //Only stages relating to the menu item selected are passed to the form (E.G. FA scan in for SKD and CKD)
            var stage = _stages.FirstOrDefault(s => s.BuildTypeId == order.BuildTypeId);
            if (stage == null)
            {
                DisplayMessage($"No {order.BuildType} stage information found for the selected order", true);
                return;
            }
            _currentStage = stage;

            txtStageName.Text = $"{_currentStage.StageName} ({order.BuildType})";
            lblProductName.Text = _currentOrder.ProductName;
            DisplayMessage("Scan serial number to start...");
            txtInput.ReadOnly = false;
            txtInput.Focus();
        }
    }
}
