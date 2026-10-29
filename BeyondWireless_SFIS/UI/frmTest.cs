using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;

namespace BeyondWireless_SFIS
{
    public partial class frmTest : Form
    {

        private readonly DataService _dataService;
        private readonly frmMain _frmMain;
        private readonly List<Stage> _stages;
        private readonly int _testType;

        private State _currentState = State.AwaitSerial;
        private Unit _currentUnit;

        private Stage _currentStage;
        private CustomerOrder _currentOrder;
        //private List<CustomerOrder> _orders;
        private List<Defect> _defectList;
        private List<UnitDefect> _currentUnitDefects;
        private TestResult _currentTestResult = new TestResult();


        private enum State
        {
            AwaitSerial,
            AwaitTestResult,
            AwaitDefect,
            AwaitPosition,
            AwaitSave
        }

        public frmTest(DataService dataService, frmMain frmMain, List<Stage> stages, int testType)
        {
            InitializeComponent();
            _dataService = dataService;
            _frmMain = frmMain;
            _stages = stages;
            _testType = testType;
            this.MakeActivityAware();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            txtStageName.BackColor = Constants.PrimaryAppColour;
            this.Text = $"Test {Constants.ApplicationVersion}";
            if (!DbConnectionOk())
                return;

            GetOrdersAsync();
            GetDefectListAsync();
            LoadDefectGroupCombo();

            txtPosition.Visible = _testType == 1; // Show position only for AOI test type
            lblPositions.Visible = _testType == 1; // Show position label only for AOI test type

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
            try
            {
                var orders = await _dataService.GetCustomerOrdersAsync(isActive: true);

                if (!orders.Any())
                {
                    DisplayMessage("No active orders found. You cannot continue...", true);
                    txtPcbaSerial.Enabled = false;
                    return;
                }
                cboOrders.SelectedIndexChanged -= cboOrders_SelectedIndexChanged;
                cboOrders.DisplayMember = "DisplayText";
                cboOrders.ValueMember = "CustomerOrderId";
                cboOrders.DataSource = orders;
                cboOrders.SelectedIndex = -1;
                cboOrders.SelectedIndexChanged += cboOrders_SelectedIndexChanged;
                cboOrders.Focus();

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error getting orders from database{Environment.NewLine}Error: {ex.Message}", true);

            }

        }

        private void LoadDefectGroupCombo()
        {

            cboDefectGroup.DataSource = null;
            if (_defectList == null || !_defectList.Any())
                return;
            var defectGroups = _defectList
                .GroupBy(d => d.DefectGroupId)
                .Select(g => g.First())
                .ToList();

            cboDefectGroup.SelectedIndexChanged -= cboDefectGroup_SelectedIndexChanged;
            cboDefectGroup.DisplayMember = "DefectGroupName";
            cboDefectGroup.ValueMember = "DefectGroupId";
            cboDefectGroup.DataSource = defectGroups;
            cboDefectGroup.SelectedIndex = -1;
            cboDefectGroup.SelectedIndexChanged += cboDefectGroup_SelectedIndexChanged;


        }

        private void DisplayHideDefectGroupSelection()
        {
            lblDefectGroup.Visible = _currentStage.TestTypeId == 3;
            cboDefectGroup.Visible = _currentStage.TestTypeId == 3;
        }

        private async void GetDefectListAsync()
        {
            try
            {
                var defects = await _dataService.GetDefectsByTestTypeIdAsync(testTypeId: _testType, isActive: true);

                if (!defects.Any())
                {
                    DisplayMessage("No defect list found. You cannot continue...", true);
                    return;
                }
                _defectList = defects;
                DisplayDefects(_defectList);
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error getting defect list from database{Environment.NewLine}Error: {ex.Message}", true);

            }
        }


        private void DisplayDefects(List<Defect> defectList)
        {
            tlpDefects.SuspendLayout();
            tlpDefects.Controls.Clear();
            tlpDefects.RowStyles.Clear();

            // Calculate required rows
            int requiredRows = (int)Math.Ceiling(defectList.Count / (double)tlpDefects.ColumnCount);
            tlpDefects.RowCount = requiredRows;

            // Set consistent row heights (50px each)
            for (int i = 0; i < requiredRows; i++)
            {
                tlpDefects.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
            }

            // Set equal column widths
            for (int i = 0; i < tlpDefects.ColumnCount; i++)
            {
                tlpDefects.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tlpDefects.ColumnCount));
            }

            // Add radio buttons
            for (int i = 0; i < defectList.Count; i++)
            {
                var defect = defectList[i];
                int row = i / tlpDefects.ColumnCount;
                int col = i % tlpDefects.ColumnCount;

                var radio = new RadioButton
                {
                    Text = $"{defect.DefectId}: {defect.DefectName}",
                    Tag = defect,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Appearance = Appearance.Button,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                //radio.CheckedChanged += RadioButton_CheckedChanged;
                radio.Click += RadioButton_Clicked;

                tlpDefects.Controls.Add(radio, col, row);
            }

            // Calculate EXACT panel height needed
            int panelHeight = 0;

            // Account for each row's height and margins
            foreach (RowStyle style in tlpDefects.RowStyles)
            {
                panelHeight += (int)style.Height;
            }

            // Account for vertical margins (2px top/bottom per row)
            //panelHeight += (requiredRows * 4);

            // Account for any container padding
            //panelHeight += tlpDefects.Padding.Vertical;

            // Set the panel size
            tlpDefects.Height = panelHeight;
            tlpDefects.Width = tlpDefects.Parent.ClientSize.Width - (tlpDefects.Margin.Horizontal + tlpDefects.Padding.Horizontal);

            // Configure scrolling if needed
            tlpDefects.AutoScroll = true;
            tlpDefects.Dock = DockStyle.None;
            tlpDefects.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            tlpDefects.ResumeLayout(true);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radio = (RadioButton)sender;

            if (!radio.Checked || _currentState != State.AwaitDefect)
                return;

            var defect = GetSelectedDefect();
            if (defect == null) return;

            // Visual feedback
            radio.BackColor = Color.LightGreen;
            Task.Delay(200).ContinueWith(_ =>
            {
                radio.Invoke((Action)(() => radio.BackColor = SystemColors.Control));
            });

            ProcessDefect(defect.DefectId);
        }

        private void RadioButton_Clicked(object sender, EventArgs e)
        {
            var radio = (RadioButton)sender;

            if (!radio.Checked || _currentState != State.AwaitDefect)
                return;

            var defect = GetSelectedDefect();
            if (defect == null) return;

            // Visual feedback
            radio.BackColor = Color.LightGreen;
            Task.Delay(200).ContinueWith(_ =>
            {
                radio.Invoke((Action)(() => radio.BackColor = SystemColors.Control));
            });

            ProcessDefect(defect.DefectId);
        }



        private void txtPcbaSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtPcbaSerial.Text))
            {

                string input = txtPcbaSerial.Text.Trim();
                txtPcbaSerial.Clear();
                ProcessInput(input);
            }
        }

        private void txtResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtResult.Text))
            {
                string input = txtResult.Text.Trim();
                ProcessInput(input);
            }
        }

        private void txtPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtPosition.Text))
            {
                string input = txtPosition.Text.Trim();
                ProcessPosition(input);
            }
        }

        private void txtDefectId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtDefectId.Text))
            {
                if (int.TryParse(txtDefectId.Text, out int defectId))
                {
                    ProcessDefect(defectId);
                }
                else
                {
                    DisplayMessage("Invalid defect ID - must be a number", true);
                }
            }
        }

        private void ProcessInput(string input)
        {
            switch (_currentState)
            {
                case State.AwaitSerial:
                    ProcessSerial(input);
                    break;
                case State.AwaitTestResult:
                    ProcessTestResult(input);
                    break;
                case State.AwaitDefect:
                    //ProcessDefect(input);
                    break;
                case State.AwaitPosition:
                    ProcessPosition(input);
                    break;
                case State.AwaitSave:
                    return;
                default:
                    return;

            }
        }

        private void ProcessSerial(string input)
        {
            if (_testType == 1 || _testType == 2 || _testType == 5) //AOI or PCBA test or Keys (PCBA Serial Number)
            {

                if (!IsValidPcbaSerial(input))
                {
                    return;
                }
            }
            if (_testType == 3) //FA Test (Set Serial Number)
            {
                if (!IsValidSetSerial(input))
                {
                    return;
                }
            }

            if (!IsValidUnit(input))
            {
                return;
            }

            DisplayMessage("Scan/Enter the test result (Pass/Fail)...");
            _currentTestResult = new TestResult() { UserId = _frmMain.CurrentUser.UserId };
            _currentState = State.AwaitTestResult;
            txtPcbaSerial.ReadOnly = true;
            txtResult.ReadOnly = false;
            txtResult.Focus();

        }

        private bool IsValidPcbaSerial(string input)
        {
            var pcbaInfo = Validation.IsValidPcbaSerial(input);
            if (!pcbaInfo.IsValid)
            {
                DisplayMessage("Invalid PCBA serial format.", true);
                return false;
            }

            if (_currentOrder.BuildTypeId == 1 && pcbaInfo.Odf != _currentOrder.Odf)
            {
                DisplayMessage($"Unit does not belong to the the selected order (ODF){Environment.NewLine}Select the correct order from the dropdown, or use PCBA's built for the selected order", true);
                return false;
            }
            else if (pcbaInfo.Odf.Substring(3, 7) != _currentOrder.Odf.Substring(3, 7))//For SKD orders the first three digits are not the same as the order but the rest are ELJ203531B vs GLJ203531B
            {
                DisplayMessage($"Unit does not belong to the the selected order (ODF){Environment.NewLine}Select the correct order from the dropdown, or use PCBA's built for the selected order", true);
                return false;
            }


            return true;
        }

        private bool IsValidSetSerial(string input)
        {
            //var setInfo = Validation.IsValidSetSerial(input);
            //if (!setInfo.IsValid)
            //{
            //    DisplayMessage($"Invalid SET serial format. ({input})", true);
            //    return false;
            //}

            //jippo for incorrect odf loaded
            //if (setInfo.Odf == "ELJ243687F" && _currentOrder.Odf != "ELJ243687E")
            //{
            //    DisplayMessage($"Set serial does not match the selected order (ODF){Environment.NewLine}Please scan a valid Set serial for the selected order.", true);
            //    return false;
            //}

            ////jippo for incorrect odf loaded
            //if (_currentOrder.Odf != "ELJ243687E")
            //{
            //    if (setInfo.Odf != _currentOrder.Odf)
            //    {
            //        DisplayMessage($"Set serial does not match the selected order (ODF){Environment.NewLine}Please scan a valid Set serial for the selected order.", true);
            //        return false;
            //    }
            //}


            return true;

        }

        private bool IsValidUnit(string input)
        {
            if (_testType == 1 || _testType == 2 || _testType == 5)//AOI or PCBA or Keys
            {
                GetCurrentUnitByPcbaSerial(input);
            }
            else //FA Test
            {
                GetCurrentUnitBySetSerial(input);
            }


            if (_currentUnit == null)
            {
                DisplayMessage($"PCBA Serial {input} not found in database.{Environment.NewLine}Please take unit to the start of the process (AOI)", true);
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

                return false;
            }

            if (!_currentUnit.CanRetest && _currentUnit.IsFaulty)
            {
                DisplayMessage($"Unit failed more than 10 minutes ago. All faults must have a repair logged by a repair technician before the unit can be retested", true);
                return false;
            }

            if (_currentUnit.IsScrapped)
            {
                DisplayMessage($"Unit with Serial {input} is scrapped.{Environment.NewLine}You cannot continue with this unit.", true);
                return false;
            }

            if (_currentUnit.IsSold)
            {
                DisplayMessage($"Unit with Serial {input} is already sold.{Environment.NewLine}You cannot continue with this unit.", true);
                return false;
            }

            if (_currentUnit.IsStolen)
            {
                DisplayMessage($"Unit with Serial {input} is Stolen!!!{Environment.NewLine}You cannot continue with this unit.", true);
                return false;
            }

            if (_currentUnit.IsBlocked)
            {
                DisplayMessage($"Unit with Serial {input} is blocked.{Environment.NewLine}You cannot continue with this unit.", true);
                return false;
            }

            return true;
        }

        private void ProcessTestResult(string input)
        {

            if (!(input.Equals("Pass", StringComparison.OrdinalIgnoreCase) || input.Equals("Fail", StringComparison.OrdinalIgnoreCase)))
            {
                DisplayMessage($"Invalid test result{Environment.NewLine}It should be Pass or Fail{Environment.NewLine}Please try again...", true);
                return;
            }

            txtResult.ReadOnly = true;

            if (input.Equals("Pass", StringComparison.OrdinalIgnoreCase))
            {
                _currentTestResult.IsPass = true;
                if (SaveTestResult())
                {
                    ResetState();
                }
                return;
            }

            if (input.Equals("Fail", StringComparison.OrdinalIgnoreCase))
            {
                _currentState = State.AwaitDefect;
                _currentTestResult.IsPass = false;
                //txtDefectId.ReadOnly = false;
                tlpDefects.Visible = true;
                DisplayMessage("Please scan or select the fail code ...");
                txtDefectId.Focus();
                return;
            }

        }

        private void ProcessDefect(int defectId)
        {
            var defect = _defectList.FirstOrDefault(d => d.DefectId == defectId) ?? GetSelectedDefect();

            if (defect == null)
            {
                DisplayMessage("Please click valid defect from the list", true);
                return;
            }

            txtDefectId.Text = defect.DefectId.ToString();
            txtDefectDescription.Text = defect.DefectName;
            txtDefectId.ReadOnly = true;
            txtPosition.Clear();
            //tlpDefects.Visible = false;
            //_currentTestResult.DefectId = defect.DefectId;
            //_currentTestResult.DefectDescription = defect.DefectName;



            if (_currentStage.TestTypeId == 1) //AOI
            {
                txtPosition.ReadOnly = false;
                _currentState = State.AwaitDefect;
                DisplayMessage("Type the faulty position and press Enter to continue...");
                txtPosition.Focus();
            }
            else
            {
                btnSave.Enabled = true;
                AddDefect();
                DisplayRecordedDefectList();
                _currentState = State.AwaitDefect;
                DisplayMessage("Click Save to continue...");
                btnSave.Focus();
            }
        }

        private void ProcessPosition(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                DisplayMessage("Position cannot be empty. Please enter a valid position.", true);
                return;
            }

            txtPosition.ReadOnly = true;
            btnSave.Enabled = true;

            if (IsValidDefect())
            {
                AddDefect();
                DisplayRecordedDefectList();
                txtDefectId.Clear();
                txtDefectDescription.Clear();
                txtPosition.Clear();
            }

            _currentState = State.AwaitDefect;
            DisplayMessage("Add additional defects or Click Save (Press Enter) to continue...");
        }

        private bool IsValidDefect()
        {
            if (_currentStage.TestTypeId == 1) //AOI Test
            {
                if (string.IsNullOrWhiteSpace(txtDefectId.Text))
                {
                    DisplayMessage("Defect Id cannot be empty for AOI tests", true);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtPosition.Text))
                {
                    DisplayMessage("Position cannot be empty for AOI tests", true);
                    return false;
                }
            }
            else //PCBA Test
            {
                if (string.IsNullOrWhiteSpace(txtDefectId.Text))
                {
                    DisplayMessage("Defect description cannot be empty for PCBA tests", true);
                    return false;
                }
            }
            return true;
        }

        private void AddDefect()
        {
            if (_currentUnitDefects == null)
                _currentUnitDefects = new List<UnitDefect>();

            if (!Int32.TryParse(txtDefectId.Text, out int defectId))
            {
                DisplayMessage("Invalid Defect Id", true);
                return;
            }
            //Prevent duplicate defect+position entries for AOI tests
            if (_currentStage.TestTypeId == 1 && _currentUnitDefects.Any(d => d.DefectId == defectId && d.Position == txtPosition.Text))
            {
                DisplayMessage("This defcet/Position has already been added", true);
                return;
            }


            //Prevent duplicate defect entries for oher test types where position is not required
            if (_currentStage.TestTypeId != 1 && _currentUnitDefects.Any(d => d.DefectId == defectId))
            {
                DisplayMessage("This defcet has already been added", true);
                return;
            }

            _currentUnitDefects.Add(new UnitDefect() { DefectId = defectId, Position = txtPosition.Text, DefectName = txtDefectDescription.Text });
        }

        private void DisplayRecordedDefectList()
        {
            if (_currentUnitDefects == null || !_currentUnitDefects.Any())
            {
                dgvDefects.DataSource = null;
                return;
            }
            dgvDefects.DataSource = _currentUnitDefects.Select(d => new
            {
                d.DefectId,
                d.DefectName,
                d.Position

            }).ToList();

        }

        private Defect GetSelectedDefect()
        {
            var selectedRadio = tlpDefects.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(rb => rb.Checked);
            return selectedRadio?.Tag as Defect;
        }

        private void GetCurrentUnitByPcbaSerial(string serialNumber)
        {
            try
            {

                var unit = _dataService.GetUnitBySerialNumber(serialNumber);
                if (unit == null && _currentStage.TestTypeId != 1) //TestTypeId != 1 (Not AOI - so unit should have been registered)
                {
                    DisplayMessage($"PCBA Serial {serialNumber} not found in database.{Environment.NewLine}Please take unit to the start of the process (AOI)", true);
                    return;
                }

                if (_currentStage.TestTypeId == 1 && unit == null) //AOI 
                {

                    var orderBalance = _dataService.GetPCBAsToBuildByCustomerOrderId(_currentOrder.CustomerOrderId); //SP allows 1.5% + count of scrapped units overbuild
                    if (orderBalance <= 0)
                    {
                        DisplayMessage($"This Order is already FULLY BUILT!!!!{Environment.NewLine}Quantity to build is {orderBalance}{Environment.NewLine}Stop the line and take the PCBA to your supervisor", true);
                        return;
                    }
                }


                if (unit == null && _currentStage.TestTypeId == 1) //TestTypeId = 1 (AOI - so unit must be registered)
                {
                    unit = RegisterUnit(serialNumber);
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

                _currentUnit = unit;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Failed to retrieve unit information. Please try again. {Environment.NewLine}Error:{ex.Message}", true);
            }
        }


        private Unit RegisterUnit(string serialNumber)
        {
            var pcbaInfo = Validation.IsValidCkdPcbaSerial(serialNumber);
            if (pcbaInfo == null)
            {
                DisplayMessage("Failed to get PCBA info from serial number. ", true);
                return null;
            }

            if (!pcbaInfo.IsValid)
            {
                DisplayMessage($"Cannot register PCBA, serial number format is not valid ({serialNumber})");
                return null;
            }

            try
            {
                Unit unit = new Unit()
                {
                    Odf = pcbaInfo.Odf,
                    SerialNumber = pcbaInfo.SerialNumber,
                    CustomerOrderId = _currentOrder.CustomerOrderId,
                    IsFaulty = false,
                    IsScrapped = false
                };
                var newUnit = _dataService.AddUnit(unit, _frmMain.CurrentUser);
                if (newUnit == null)
                    return null;

                return newUnit;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Database Error when adding unit. {Environment.NewLine}Error: {ex.Message}", true);
                return null;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveTestResult())
            {
                ResetState();
            }
        }

        private bool CanSave(TestResult testResult)
        {
            if (testResult == null)
            {
                DisplayMessage("Test result cannot be null", true);
                return false;
            }

            if (!Int32.TryParse(testResult.UserId.ToString(), out _))
            {
                DisplayMessage("Invalid UserId", true);
                return false;
            }

            if (testResult.TestTypeId <= 0)
            {
                DisplayMessage("Test Type ID must be provided", true);
                return false;
            }
            if (testResult.UnitId <= 0)
            {
                DisplayMessage("Unit ID must be provided", true);
                return false;
            }

            if (!Int32.TryParse(testResult.TestTypeId.ToString(), out _))
            {
                DisplayMessage("Test Type ID must be a valid number", true);
                return false;
            }



            // Skip the rest of the validation if the test is a pass
            if (testResult.IsPass)
                return true;


            if (testResult.Defects == null || !testResult.Defects.Any())
            {
                DisplayMessage("No defects selected for failed tests", true);
                return false;
            }

            foreach (var defect in testResult.Defects)
            {
                if (string.IsNullOrWhiteSpace(defect.DefectName))
                {
                    DisplayMessage("Defect description cannot be empty for failed tests", true);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(defect.Position) && _currentStage.TestTypeId == 1) //AOI Test requires position
                {
                    DisplayMessage("Position cannot be empty for AOI tests", true);
                    return false;
                }


                if (defect.DefectId <= 0 && !testResult.IsPass)
                {
                    DisplayMessage("Defect ID must be provided for failed tests", true);
                    return false;
                }


                if (!Int32.TryParse(defect.DefectId.ToString(), out _))
                {
                    DisplayMessage("Defect ID must be a valid number", true);
                    return false;
                }
            }

            return true;
        }

        private bool SaveTestResult()
        {
            try
            {
                TestResult testResult = new TestResult
                {
                    UnitId = _currentUnit.UnitId,
                    TestTypeId = _currentStage.TestTypeId,
                    StageId = _currentStage.StageId,
                    IsPass = _currentTestResult.IsPass,
                    UserId = _frmMain.CurrentUser.UserId,
                    Defects = _currentUnitDefects ?? new List<UnitDefect>()
                };


                if (!CanSave(testResult))
                    return false;

                var result = _dataService.AddTestResult(testResult);
                if (result == null)
                {
                    DisplayMessage($"Failed to save test result. Please try again.{Environment.NewLine}No response from database", true);
                    return false;
                }

                if (result.StatusCode != 0)
                {
                    DisplayMessage($"Failed to save test result. Please try again.{Environment.NewLine}Error: {result.Message}", true);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Failed to save test result. Please try again.{Environment.NewLine}Error: {ex.Message}", true);
                return false;
            }
        }



        private void ResetState()
        {
            txtPcbaSerial.ReadOnly = false;
            txtResult.ReadOnly = true;
            txtResult.Clear();
            txtDefectId.ReadOnly = true;
            txtDefectId.Clear();
            tlpDefects.Visible = false;
            txtDefectDescription.ReadOnly = true;
            txtDefectDescription.Clear();
            txtPosition.Clear();
            txtPosition.ReadOnly = true;
            _currentState = State.AwaitSerial;
            _currentUnit = null;
            _currentTestResult = new TestResult();
            _currentUnitDefects = null;//new list will be created when adding defects
            btnSave.Enabled = false;

            ResetRadioDefectSelection();
            switch (_currentStage.TestTypeId)
            {
                case 1:// AOI Test
                    DisplayMessage("Scan PCBA Serial Number to continue...");
                    break;
                case 2:// PCBA Test
                    DisplayMessage("Scan PCBA Serial Number to continue...");
                    break;
                case 3:// FA Test
                    DisplayMessage("Scan SET Serial Number to continue...");
                    break;
                case 5: // Keys Programming
                    DisplayMessage("Scan CZ PCBA Serial Number to continue...");
                    break;
                default:
                    DisplayMessage("Scan CZ PCBA Serial Number to continue...");
                    break;
            }
            dgvDefects.DataSource = null;
            txtPcbaSerial.Clear();
            txtPcbaSerial.Focus();
        }

        private void ResetRadioDefectSelection()
        {
            foreach (var radio in tlpDefects.Controls.OfType<RadioButton>())
            {
                radio.Checked = false;
            }
        }

        private void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOrders.SelectedIndex == -1)
            {
                txtPcbaSerial.ReadOnly = true;
                txtStageName.Text = "Testing Stage";
                DisplayMessage("Select an order to start testing.");
                _currentOrder = null;
                return;
            }



            var order = cboOrders.SelectedItem as CustomerOrder;
            if (order == null)
            {
                DisplayMessage("Cannot get order information from selected order.", true);
                return;
            }
            _currentOrder = order;

            var stage = _stages.FirstOrDefault(s => s.BuildTypeId == order.BuildTypeId);
            if (stage == null)
            {
                DisplayMessage($"Cannot find {order.BuildType} stage information. Please check the database.", true);
                return;
            }
            _currentStage = stage;

            txtStageName.Text = $"{stage.StageName} ({order.BuildType})";

            cboOrders.Enabled = false; // Disable orders combo during linking
            ResetState();
            DisplayHideDefectGroupSelection();

        }

        private void frmTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var radio in tlpDefects.Controls.OfType<RadioButton>())
            {
                radio.CheckedChanged -= RadioButton_CheckedChanged;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetState();
        }

        private void btnChangeOrder_Click(object sender, EventArgs e)
        {
            cboOrders.Enabled = true;
            cboOrders.SelectedIndex = -1;
            DisplayMessage("Select Order to start");
            ResetState();
            cboOrders.Focus();
            txtPcbaSerial.ReadOnly = true;
        }

        private void cboDefectGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDefectGroup.SelectedIndex == -1)
                return;
            var defectGroup = (int)cboDefectGroup.SelectedValue;
            var filteredDefectList = _defectList.Where(d => d.DefectGroupId == defectGroup).ToList();
            DisplayDefects(filteredDefectList);
        }
    }
}
