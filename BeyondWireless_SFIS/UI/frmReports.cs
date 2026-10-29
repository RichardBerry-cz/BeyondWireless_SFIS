using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;
using static BeyondWireless_SFIS.Shared.Logging;

namespace BeyondWireless_SFIS
{
    public partial class frmReports : Form
    {
        private readonly frmMain _frmMain;
        private readonly DataService _DataService;
        private readonly List<Stage> _stages;
        private List<Report> _reports;

        public frmReports(DataService DataService, frmMain frmMain, List<Stage> stages)
        {
            InitializeComponent();
            _DataService = DataService;
            _frmMain = frmMain;
            _stages = stages;
            this.MakeActivityAware();
        }

        private async void frmReports_Load(object sender, EventArgs e)
        {
            txtStageName.BackColor = Constants.PrimaryAppColour;
            this.Text = $"Reports {Constants.ApplicationVersion}";

            if (!DbConnectionOk())
            {
                this.Close();
                return;
            }
            Task ordersTask = GetOrdersAsync();
            Task reportsTask = GetReportsAsync();
            Task productsTask = GetProductsAsync();

            // Await all tasks (they run in parallel)
            await Task.WhenAll(ordersTask, reportsTask, productsTask);
            LoadReportsCombo();
            dtpStartDate.Value = DateTime.Now.AddDays(-7); // Default to last 7 days
            this.Text = $"Reports {Constants.ApplicationVersion}";
            //LoadStagesCombo();
        }

        private void LoadStagesCombo()
        {
            List<Stage> filteredStages;

            cboStages.DisplayMember = "DisplayText";
            cboStages.ValueMember = "StageId";
            //cboStages.DataSource = null;

            var report = cboReports.SelectedItem as Report;
            if (report == null || !report.RequiresStageId)
                return;


            if (cboOrders.SelectedIndex > -1 && report.RequiresStageId)
            {
                var order = cboOrders.SelectedItem as CustomerOrder;
                filteredStages = _stages.Where(s => s.BuildTypeId == order.BuildTypeId).OrderBy(s => s.SequenceNumber).ToList();
                cboStages.DataSource = filteredStages;
                return;
            }

            if (cboProducts.SelectedIndex > -1 && report.RequiresStageId)
            {
                var product = cboProducts.SelectedItem as Product;
                filteredStages = _stages.Where(s => s.BuildTypeId == product.BuildTypeId).OrderBy(s => s.SequenceNumber).ToList();
                cboStages.DataSource = filteredStages;
                return;
            }

            cboStages.DataSource = _stages.OrderBy(s => s.BuildTypeId).ThenBy(s => s.SequenceNumber).ToList();

        }

        private bool DbConnectionOk()
        {
            try
            {
                return _DataService.DbConnectionOk();
            }
            catch (Exception ex)
            {
                DisplayMessage($"Database connection error.{Environment.NewLine}Check Network Connections to CZ Network.{Environment.NewLine}Error: {ex.Message}", true);
                return false;
            }
        }


        private async Task GetOrdersAsync()
        {
            try
            {
                var orders = await _DataService.GetCustomerOrdersAsync();
                if (orders != null && orders.Any())
                {
                    cboOrders.SelectedIndexChanged -= cboOrders_SelectedIndexChanged;
                    cboOrders.DisplayMember = "DisplayText";
                    cboOrders.ValueMember = "CustomerOrderID";
                    cboOrders.DataSource = orders.OrderBy(o => o.Odf).ToList();
                    cboOrders.SelectedIndex = -1;
                    cboOrders.SelectedIndexChanged += cboOrders_SelectedIndexChanged;
                }
                else
                {
                    DisplayMessage("No orders found.", true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetOrdersAsync, Error: {ex.Message}");
                DisplayMessage("An error occurred while retrieving orders. Please try again later.", true);
            }

        }
        private async Task GetProductsAsync()
        {
            try
            {
                var products = await _DataService.GetProductsAsync();
                if (products != null && products.Any())
                {
                    cboProducts.SelectedIndexChanged -= cboProducts_SelectedIndexChanged;
                    cboProducts.DisplayMember = "ProductName";
                    cboProducts.ValueMember = "ProductID";
                    cboProducts.DataSource = products;
                    cboProducts.SelectedIndex = -1;
                    cboProducts.SelectedIndexChanged += cboProducts_SelectedIndexChanged;
                }
                else
                {
                    DisplayMessage("No products found.", true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetProducts, Error: {ex.Message}");
                DisplayMessage("An error occurred while retrieving products. Please try again later.", true);

            }
        }
        private async Task GetReportsAsync()
        {
            try
            {
                _reports = await _DataService.GetReportsAsync();

            }
            catch (Exception ex)
            {
                DisplayMessage($"An error occurred while retrieving reports.{Environment.NewLine}Error: {ex.Message}{Environment.NewLine}Check network connections or resolve the error and try again.", true);
            }
        }

        private void LoadReportsCombo()
        {
            if (_reports != null && _reports.Any())
            {
                cboReports.SelectedIndexChanged -= cboReports_SelectedIndexChanged;
                cboReports.DisplayMember = "ReportName";
                cboReports.ValueMember = "SPName";
                cboReports.DataSource = ckbShowInactiveReports.Checked ? _reports : _reports.Where(r => r.IsActive).ToList();
                cboReports.SelectedIndex = -1;
                cboReports.SelectedIndexChanged += cboReports_SelectedIndexChanged;

            }
            else
            {
                DisplayMessage("No reports found.", true);
            }
        }

        private void cboReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvResults.DataSource = null;


            LoadStagesCombo();

            lblRecordCount.Text = "Record Count: 0";

            ResetComboSelections();

            ShowHideReportParameters();

        }

        private void ResetComboSelections()
        {
            cboProducts.SelectedIndex = -1;
            cboOrders.SelectedIndex = -1;
            cboStages.SelectedIndex = -1;
        }

        private void ShowHideReportParameters()
        {
            var selectedReport = cboReports.SelectedItem as Report;
            if (selectedReport != null)
            {
                grpDates.Visible = selectedReport.RequiresDateRange;
                grpStages.Visible = selectedReport.RequiresStageId;
                grpOrders.Visible = selectedReport.RequiresCustomerOrderId;
                grpProducts.Visible = selectedReport.RequiresProductId;

            }
            else
            {
                grpDates.Visible = false;
                grpProducts.Visible = false;
                grpOrders.Visible = false;
                grpStages.Visible = false;
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

        private async void btnGetReport_Click(object sender, EventArgs e)
        {
            var selectedReport = cboReports.SelectedItem as Report;
            if (selectedReport == null)
            {
                DisplayMessage("Please select a report to generate.", true);
                return;
            }

            if (!CanRunReport(selectedReport))
                return;

            RunReport(selectedReport);
            await LogReportUsage(selectedReport);
        }

        private async Task LogReportUsage(Report report)
        {
            try
            {
                await _DataService.LogReportUsageAsync(report, _frmMain.CurrentUser ?? new User());//Usrs can access reports without being logged in, so pass an empty user if CurrentUser is null
            }
            catch (Exception)
            {
                //Error Logged in dataservice
            }
        }

        private async void RunReport(Report selectedReport)
        {
            dgvResults.DataSource = null; // Clear previous results

            try
            {
                var reportParams = new ReportParams()
                {
                    SPName = selectedReport.SPName,
                    StartDate = dtpStartDate.Value != null ? dtpStartDate.Value : DateTime.Now,
                    EndDate = dtpEndDate.Value != null ? dtpEndDate.Value : DateTime.Now,
                    ProductId = cboProducts.SelectedValue != null ? (int?)Convert.ToInt32(cboProducts.SelectedValue) : null,
                    CustomerOrderId = cboOrders.SelectedValue != null ? (int?)Convert.ToInt32(cboOrders.SelectedValue) : null,
                    StageId = cboStages.SelectedValue != null ? (int?)Convert.ToInt32(cboStages.SelectedValue) : null
                };

                var result = await _DataService.GetReportAsync(reportParams);
                if (result != null)
                {
                    dgvResults.DataSource = result;
                    lblRecordCount.Text = $"Record Count: {result.Rows.Count}";
                }
                else
                {
                    DisplayMessage("No data found for the selected report parameters.", true);
                    lblRecordCount.Text = $"Record Count: 0";
                }

            }
            catch (Exception ex)
            {
                DisplayMessage($"A database error occurred while generating the report.{Environment.NewLine}Error {ex.Message} ", true);
            }
        }

        private bool CanRunReport(Report selectedReport)
        {
            if (selectedReport == null)
            {
                DisplayMessage("Please select a report to generate.", true);
                return false;
            }
            if (selectedReport.RequiresDateRange && (dtpStartDate.Value > dtpEndDate.Value))
            {
                DisplayMessage("Start date cannot be later than end date.", true);
                return false;
            }
            if (selectedReport.RequiresProductId && cboProducts.SelectedValue == null)
            {
                DisplayMessage("Please select a product.", true);
                return false;
            }
            if (selectedReport.RequiresCustomerOrderId && cboOrders.SelectedValue == null)
            {
                DisplayMessage("Please select a customer order.", true);
                return false;
            }
            if (selectedReport.RequiresStageId && cboStages.SelectedValue == null)
            {
                DisplayMessage("Please select a Stage.", true);
                return false;
            }
            return true;
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvResults.DataSource == null || dgvResults.Rows.Count == 0)
            {
                DisplayMessage("No data to export. Please run a report first.", true);
                return;
            }

            var table = dgvResults.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                DisplayMessage("No data available to export.", true);
                return;
            }

            ExportToExcel(table);
        }

        private void ExportToExcel(DataTable dataTable)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                var report = cboReports.SelectedItem as Report;
                if (report == null)
                {
                    DisplayMessage("Please select a report to export.", true);
                    return;
                }
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xls";
                saveFileDialog.Title = $"Export {report.ReportName}";
                saveFileDialog.FileName = $"{report.ReportName}_{DateTime.Now:yyyy-MM-dd HH'H'mm}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExcelHelper.WriteDataTableToExcel(dataTable, saveFileDialog.FileName);

                        DisplayMessage("Data exported successfully!");
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage($"Error exporting data.{Environment.NewLine}Error: {ex.Message}", true);
                    }
                }
            }
        }

        private void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStagesCombo();
        }

        private void cboProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStagesCombo();
        }

        private void ckbShowInactiveReports_CheckedChanged(object sender, EventArgs e)
        {
            LoadReportsCombo();
        }

        private async void btnReportDefinition_Click(object sender, EventArgs e)
        {
            var report = cboReports.SelectedItem as Report;
            if (report == null)
            {
                DisplayMessage("Please select a report first");
                return;
            }


            var definition = await GetReportDefinition(report.ReportId);
            if (definition == null)
                return;

            var frm = new frmReportDefinition(definition);
            frm.ShowDialog();
        }

        private async Task<ReportDefinition> GetReportDefinition(int reportId)
        {
            try
            {
                return await _DataService.GetReportDefinitionByIdAsync(reportId);
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
                return null;
            }
        }
    }
}
