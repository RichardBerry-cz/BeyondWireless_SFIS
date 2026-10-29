using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BeyondWireless_SFIS.Shared;
using static BeyondWireless_SFIS.Shared.Logging;

namespace BeyondWireless_SFIS
{
    public partial class frmMain : Form
    {

        private DataService _dataService;
        private System.Threading.Timer _inactivityTimer;
        private DateTime _lastActivityTime;
        private User _currentUser;
        public User CurrentUser => _currentUser;
        private List<Form> _openChildForms = new List<Form>();
        private List<Stage> _stages = new List<Stage>();
        public frmMain(DataService tclDataService)
        {
            InitializeComponent();
            InitializeLogger();
            UpdateLogLevel(Serilog.Events.LogEventLevel.Debug);
            _dataService = tclDataService;

            ActivityMonitor.UserActivityDetected += ResetInactivityTimer;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtWelcome.BackColor = Constants.PrimaryAppColour;
            InitializeInactivityTimer();
            LogAppVersion();
            UpdateMenuItems();
            if (!CheckDbConnection())
            {
                Application.Exit(); // Exit the application if DB connection fails
                return;
            }
            SetAppversion();
            GetStagesAsync();

#if DEBUG
            //btnUnlink.Visible = true;
            ////btnContainerArrival.Visible = true; 
            //btnSerialSwap.Visible = true;
            //btnPcbaLabelReprint.Visible = true;
#endif
        }

        public void ResetInactivityTimer()
        {
            _lastActivityTime = DateTime.Now;
            _inactivityTimer.Change(
                dueTime: (int)TimeSpan.FromMinutes(Constants.LoginTimeoutMinutes).TotalMilliseconds,
                period: Timeout.Infinite);
        }

        private void InitializeInactivityTimer()
        {
            _inactivityTimer = new System.Threading.Timer(
                callback: InactivityTimeoutCallback,
                state: null,
                dueTime: Timeout.Infinite,  // Don't start yet
                period: Timeout.Infinite);
        }

        private void InactivityTimeoutCallback(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _currentUser = null;
                CloseAllChildForms();
                UpdateMenuItems(null); // Disable restricted menus
                //CheckUserLogin(); // Prompt for re-login
            });
        }

        private void LogAppVersion()
        {
            try
            {
                _dataService.LogAppVersion();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to log application version. Error: {ex.Message}");
            }
        }

        private void SetAppversion()
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Constants.ApplicationVersion = $"(Ver: {ApplicationDeployment.CurrentDeployment.CurrentVersion})";
            }
            else
            {
                Constants.ApplicationVersion = "(Not network Deployed)";
            }
            //Logger.Debug($"*** App started. {ApplicationVersion} ");
            //MessageBox.Show($"Logging to {Application.CommonAppDataPath}");
            this.Text = $"TCL SFIS {Constants.ApplicationVersion}";
        }

        private bool CheckDbConnection()
        {
            var result = _dataService.DbConnectionOk();
            if (!result)
            {
                MessageBox.Show("Database connection failed. Please check your network connection settings. You cannot continue!!!!! App is going to close", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private async void GetStagesAsync()
        {
            try
            {
                _stages.Clear();
                var stages = await _dataService.GetStagesAsync(IsActive: true);
                if (stages.Any())
                {
                    _stages = stages;
                }
                else
                {
                    MessageBox.Show("No active stages found. Please check your database configuration. App will not work properly!!!!", "No Stages Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error fetching stages. Error: {ex.Message}");
                MessageBox.Show($"Error fetching stages. The app will not work properly!!!!! Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckUserLogin()
        {
            //if (_currentUser == null)
            //{
            using (var loginForm = new frmLogin(_dataService))
            {
#if DEBUG
                    _currentUser = new User() { AuthorityLevel = 5, FullName = "RICHARD BERRY", IsActive = true, RFIDSerial = "IDC_05000", UserId = 1532 };
                    ResetInactivityTimer();
                    UpdateMenuItems(_currentUser);
                    txtWelcome.Text = $"Welcome, {_currentUser.FullName}";
#else
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    _currentUser = loginForm.user;
                    txtWelcome.Text = $"Welcome, {_currentUser.FullName}";
                    ResetInactivityTimer();
                    UpdateMenuItems(_currentUser);
                }
                else
                {
                    CloseAllChildForms();
                    UpdateMenuItems(null); // Disable restricted menus
                    txtWelcome.Text = $"Not Logged in, restricted menu's available";
                }
#endif

            }
            // }
        }

        private void CloseAllChildForms()
        {
            foreach (var form in _openChildForms.ToList()) // ToList() avoids modification while iterating
            {
                form.Close(); // This triggers FormClosed event, removing it from the list
            }
        }

        private void UpdateMenuItems(User user = null)
        {
            // Enable theseitems regardless of login state
            //unitDetailToolStripMenuItem.Enabled = true;
            reportsToolStripMenuItem.Enabled = true;
            logInToolStripMenuItem.Enabled = true;


            //First disable all menus except ones that are never disbaled
            foreach (ToolStripItem item in menuStrip1.Items)
            {
                // Skip the two items that should always be enabled
                if (item == reportsToolStripMenuItem || item == logInToolStripMenuItem)
                    continue;

                item.Enabled = false;

                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                {
                    foreach (ToolStripItem childItem in menuItem.DropDownItems)
                    {
                        childItem.Enabled = false;
                    }
                }

            }
            //Enable basic prod and repair menus
            if (user != null)
            {
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    if (item == productionToolStripMenuItem || item == repairsToolStripMenuItem || item == qCToolStripMenuItem)
                        item.Enabled = true;

                    if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    {
                        foreach (ToolStripItem childItem in menuItem.DropDownItems)
                        {

                            if (!(childItem.Name == "editQCInspectionListToolStripMenuItem" || childItem.Name == "pCBALabelPrintToolStripMenuItem" || childItem.Name == "containerArrivalToolStripMenuItem"))
                                childItem.Enabled = true;

                        }
                    }

                }
            }

            //enable containers for Sindi
            if (user != null && user.FullName.ToUpper() == "SINDISIWE BILANKULU")
            {
                foreach (ToolStripItem item in menuStrip1.Items)
                {

                    if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    {
                        foreach (ToolStripItem childItem in menuItem.DropDownItems)
                        {
                            if (childItem.Name == "containerArrivalToolStripMenuItem")

                                childItem.Enabled = true;
                        }
                    }
                }
            }

            // Enable all menus except orders for Mfundo
            if (user != null && user.FullName.ToLower() == "mfundo mashinini")
            {
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    if (!(item == ordersToolStripMenuItem || item == scrappingToolStripMenuItem))
                        item.Enabled = true;

                    if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    {
                        foreach (ToolStripItem childItem in menuItem.DropDownItems)
                        {
                            if (!(childItem.Name == "pCBALabelPrintToolStripMenuItem" || childItem.Name == "editQCInspectionListToolStripMenuItem" || childItem.Name == "containerArrivalToolStripMenuItem"))
                                childItem.Enabled = true;
                        }
                    }
                }
            }

            //PCBA  Printing  for users with AuthorityLevel >= 3
            if (user != null && user.AuthorityLevel >= 3)
            {
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    if (item == printingToolStripMenuItem)
                        item.Enabled = true;

                    if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    {
                        foreach (ToolStripItem childItem in menuItem.DropDownItems)
                        {
                            if (childItem.Name == "pCBALabelPrintToolStripMenuItem")
                                childItem.Enabled = true;
                        }
                    }
                }
            }

            // Enable all menus  for Auth level >= 4
            if (user != null && user.AuthorityLevel >= 4)
            {
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    item.Enabled = true;

                    if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    {
                        foreach (ToolStripItem childItem in menuItem.DropDownItems)
                        {
                            childItem.Enabled = true;
                        }
                    }
                }
            }

           


        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new frmProduct(_dataService, this);
            frm.FormClosed += (s, args) => _openChildForms.Remove(frm);
            _openChildForms.Add(frm);
            frm.ShowDialog();
            ResetInactivityTimer();
        }

        private void logInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentUser != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
            }
            _currentUser = null;
            CheckUserLogin();
        }

        private void pCBALabelPrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new frmLabelPrint(_dataService, this);
            frm.FormClosed += (s, args) => _openChildForms.Remove(frm);
            _openChildForms.Add(frm);
            frm.ShowDialog();
            ResetInactivityTimer();
        }
    }
}
