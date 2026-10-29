using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeyondWireless_SFIS
{


    public partial class frmRepair : Form
    {
        private readonly DataService _dataService;
        private readonly frmMain _frmMain;
        private List<RepairType> _repairTypes;
        private Unit _unit;

        private List<Defect> _defectTypes;

        public frmRepair(DataService dataService, frmMain frmMain)
        {
            InitializeComponent();
            _dataService = dataService;
            _frmMain = frmMain;
            this.MakeActivityAware();
        }

        private void frmRepair_Load(object sender, EventArgs e)
        {
            txtStageName.BackColor = Constants.PrimaryAppColour;
            this.Text = $"Repair  {Constants.ApplicationVersion}";
            if (!DbConnectionOk())
            {
                this.Close();
                return;
            }
            GetRepairTypes();
            GetDefectTypes();
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



        private async void GetDefectTypes()
        {
            try
            {
                var defectTypes = await _dataService.GetDefectsByTestTypeIdAsync();
                if (defectTypes == null || defectTypes.Count == 0)
                {
                    DisplayMessage("No defect types found.", true);
                    return;
                }
                _defectTypes = defectTypes;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving defect types: {ex.Message}", true);
            }
        }

        private async void GetRepairTypes()
        {
            try
            {
                var repairTypes = await _dataService.GetRepairTypesAsync();
                if (repairTypes == null || repairTypes.Count == 0)
                {
                    DisplayMessage("No repair types found.", true);
                    return;
                }
                _repairTypes = repairTypes;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving repair types: {ex.Message}", true);
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtInput.Text))
                {
                    string input = txtInput.Text.Trim();
                    ProcessInput(input);
                    txtInput.Clear();
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void ProcessInput(string input)
        {
            _unit = null;
            if (!GetUnit(input)) //_unit is set in GetUnit calls
                return;
            GetTestResults(_unit.UnitId);
            GetBom();


        }

        private void GetTestResults(int unitId)
        {
            if (_unit == null)
            {
                DisplayMessage("No unit selected. Please enter a valid serial number.", true);
                return;
            }

            try
            {
                var gridData = _dataService.GetTestResultsByUnitId(unitId)
                    .SelectMany(tr => tr.Defects
                        .Select(d => new TestResultDefectViewModel
                        {
                            TestResultId = tr.TestResultId,
                            TestResultDetailId = d.TestResultDetailId, 
                            TestTypeId = tr.TestTypeId,
                            TestDate = tr.TestDate,
                            TestTypeName = tr.TestTypeName,
                            DefectId = d.DefectId,
                            DefectName = d.DefectName,
                            Position = d.Position,
                            IsRepaired = d.IsRepaired
                        }))
                    .ToList();

                dgvTestResults.DataSource = gridData;
                dgvTestResults.Columns["TestResultDetailId"].Visible = false;
                dgvTestResults.Columns["TestResultId"].Visible = false;
                dgvTestResults.Columns["DefectId"].Visible = false;
                dgvTestResults.Columns["TestTypeId"].Visible = false;

                ckbIsRepaired.Checked = !gridData.Any(r => !r.IsRepaired);
               

                DisplayMessage("Double Click the Fault you are repairing in the Recorded Faults grid on the left", false);
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving repairs: {ex.Message}", true);
            }
        }

        private async void GetBom()
        {
            if (_unit == null)
            {
                return;
            }

            try
            {
                cboBomPosition.DataSource = null; // Clear previous data
                var bom = await _dataService.GetBomByProductIdAsync(_unit.ProductId);
                if (bom == null || bom.Count == 0)
                {
                    DisplayMessage("No BOM found for this unit.", true);
                    return;
                }

                cboBomPosition.SelectedIndexChanged -= cboBomPosition_SelectedIndexChanged;
                cboBomPosition.DisplayMember = "DisplayText";
                cboBomPosition.ValueMember = "Position";
                cboBomPosition.DataSource = bom;
                cboBomPosition.SelectedIndex = -1;
                cboBomPosition.SelectedIndexChanged += cboBomPosition_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving BOM: {ex.Message}", true);
            }
        }

        private void dgvTestResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Header row was clicked

            var selectedDefect = dgvTestResults.Rows[e.RowIndex].DataBoundItem as TestResultDefectViewModel;
            if (selectedDefect == null)
            {
                txtDefectId.Text = string.Empty;
                txtDefectDescription.Text = string.Empty;
                return;
            }

            radFaultSourceJig.Checked = true;
            txtTestResultId.Text = selectedDefect.TestResultId.ToString();
            txtTestResultDetailId.Text = selectedDefect.TestResultDetailId.ToString();
            txtDefectId.Text = selectedDefect.DefectId.ToString();
            txtDefectDescription.Text = selectedDefect.DefectName;

            LoadRepairTypeCombo(selectedDefect.TestTypeId);

            cboBomPosition.Enabled = true;
            DisplayMessage("Select the part you repaired from the Ref Des Dropdown....", false);
        }

        private void LoadRepairTypeCombo(int testTypeId)
        {

            var repairTypes = _repairTypes.Where(rt => rt.TestTypeId == testTypeId && rt.IsActive).ToList();
            if (repairTypes.Count == 0)
            {
                DisplayMessage("No active repair types found for this type of test result.", true);
                return;
            }

            cboRepairType.DisplayMember = "RepairDescription";
            cboRepairType.ValueMember = "RepairTypeId";
            cboRepairType.DataSource = repairTypes;
            cboRepairType.SelectedIndex = -1;
        }

        private bool GetUnit(string input)
        {

            if (GetUnitBySerialNo(input))
            {
                DisplayUnitInfo();
                GetRepairData();
                return true;
            }

            return false;

        }

        private async void GetRepairData()
        {
            if (_unit == null)
                return;
            try
            {
                dgvRepairsDone.DataSource = null;
                var repairs = await _dataService.GetRepairHistoryBySerialAsync(_unit.SerialNumber);
                if (repairs != null)
                {
                    dgvRepairsDone.DataSource = repairs;
                }

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving repairs: {ex.Message}", true);
            }
        }

        private bool GetUnitBySerialNo(string serialNumber)
        {
            try
            {
                var unit = _dataService.GetUnitBySerialNumber(serialNumber);
                if (unit == null)
                {
                    DisplayMessage($"Unit with serial number '{serialNumber}' not found.", true);
                    return false;
                }
                _unit = unit;
                return true;
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error retrieving unit: {ex.Message}", true);
                return false;
            }
        }

        private void DisplayUnitInfo()
        {
           
            if (_unit == null)
            {
                txtPcbaSerialNumber.Text = string.Empty;
                txtProductShortCode.Text = string.Empty;
                txtCurrentStage.Text = string.Empty;
                ckbIsFaulty.Checked = false;
                ckbIsScrap.Checked = false;
                ckbIsRepaired.Checked = false;
            }
            else
            {
                txtPcbaSerialNumber.Text = _unit.SerialNumber;
                txtProductShortCode.Text = _unit.Model;
                txtCurrentStage.Text = _unit.StageName;
                ckbIsFaulty.Checked = _unit.IsFaulty;
                ckbIsScrap.Checked = _unit.IsScrapped;
            }

        }

        private void cboBomPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBomPosition.SelectedIndex == -1)
                return;
            cboBomPosition.Enabled = false;
            if (radFaultSourceInspection.Checked)
            {
                DisplayMessage("Please select a Defect from the dropdown", false);
                cboDefectType.Enabled = true;
            }
            else
            {
                DisplayMessage("Please select the Repair done from the dropdown", false);
                cboRepairType.Enabled = true;
            }

        }

        private void cboRepairType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRepairType.SelectedIndex == -1)
                return;

            DisplayMessage("Enter a comment (if needed) or click Save", false);
            cboRepairType.Enabled = false;
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
                lblInstruction.Text = message;
            }
        }

        private void radFaultSourceInspection_CheckedChanged(object sender, EventArgs e)
        {
            if (radFaultSourceInspection.Checked)
            {
                DisplayMessage("Please select the position you are repairing");
            }
            else
            {
                DisplayMessage("Double Click the Fault you are repairing in the Recorded Faults grid on the left");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!DbConnectionOk())
                return;

            if (!CanSave())
                return;

            if (!SaveUnit())
                return;

            ResetAfterSave();
        }


        private bool CanSave()
        {
            if (_unit == null)
            {
                DisplayMessage("No unit selected. Please enter a valid serial number.", true);
                return false;
            }
            if (!Int32.TryParse(txtDefectId.Text, out int defectId) || defectId < 0)
            {
                DisplayMessage("Please select a valid defect.", true);
                return false;
            }
            if (cboRepairType.SelectedItem == null)
            {
                DisplayMessage("Please select a repair type.", true);
                return false;
            }
            if (string.IsNullOrEmpty(txtDefectDescription.Text))
            {
                DisplayMessage("Please enter a defect description.", true);
                return false;
            }
            if (cboBomPosition.SelectedItem == null)
            {
                DisplayMessage("Please select a BOM position.", true);
                return false;
            }
            if (!_unit.IsFaulty && radFaultSourceJig.Checked)
            {
                DisplayMessage($"The unit is not faulty. No repairs needed.{Environment.NewLine}Possibly choose Inspection as the Fault Source?", true);
                return false;
            }
            if (!Int32.TryParse(txtTestResultId.Text, out int testResultId) || (testResultId < 0 && radFaultSourceJig.Checked))
            {
                DisplayMessage("Please select a valid test result from the grid on the left", true);
                return false;
            }
            if (_unit.NextStageName == "Complete")
            {
                DisplayMessage("Unit is despatched already");
                return false;
            }

            return true;
        }


        private void ResetAfterSave()
        {
            txtComment.Text = string.Empty;
            txtDefectDescription.Text = string.Empty;
            txtDefectId.Text = string.Empty;
            txtTestResultId.Text = string.Empty;
            txtTestResultDetailId.Text = string.Empty;
            txtPcbaSerialNumber.Text = string.Empty;
            txtCurrentStage.Text = string.Empty;
            txtProductShortCode.Text = string.Empty;
            cboBomPosition.SelectedIndex = -1;
            cboBomPosition.DataSource = null;
            cboDefectType.SelectedIndex = -1;
            cboRepairType.SelectedIndex = -1;
            ckbIsFaulty.Checked = false;
            ckbIsScrap.Checked = false;
            ckbIsRepaired.Checked = false;
            _unit = null;
            dgvTestResults.DataSource = null;
            dgvRepairsDone.DataSource = null;
            DisplayMessage($"Repair saved successfully. Scan next unit to continue...", false);
        }

        private bool SaveUnit()
        {
            try
            {
                var repairType = cboRepairType.SelectedItem as RepairType;
                var repair = new Repair()
                {
                    UnitId = _unit.UnitId,
                    TestResultId = Convert.ToInt32(txtTestResultId.Text),
                    TestResultDetailId = Convert.ToInt32(txtTestResultDetailId.Text),
                    DefectId = Convert.ToInt32(txtDefectId.Text),
                    RepairDescription = repairType.RepairDescription,
                    Position = cboBomPosition.SelectedValue.ToString(),
                    RepairTypeId = repairType.RepairTypeId,
                    Comment = string.IsNullOrEmpty(txtComment.Text.Trim()) ? string.Empty : txtComment.Text.Trim(),
                    UserId = _frmMain.CurrentUser.UserId
                };
                var response = _dataService.AddRepair(repair);
                if (response == null)
                {
                    DisplayMessage($"Failed to save repair data. {Environment.NewLine}Error: No response from database", true);
                }
                if (response.StatusCode != 0)
                {
                    DisplayMessage($"Failed to Save Repair data.{Environment.NewLine}Error:{response.Message}", true);
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error saving repair: {ex.Message}", true);
                return false;
            }
        }
    }

}
