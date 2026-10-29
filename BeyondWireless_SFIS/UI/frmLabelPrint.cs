using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;
using BeyondWireless_SFIS.Shared;
using static BeyondWireless_SFIS.Shared.Constants;

namespace BeyondWireless_SFIS
{
    public partial class frmLabelPrint : Form
    {
        private readonly DataService _dataService;
        private readonly frmMain _frmMain;

        private SerialPort _serialPort = new SerialPort();

        public frmLabelPrint(DataService dataService, frmMain frmMain)
        {
            InitializeComponent();
            _dataService = dataService;
            _frmMain = frmMain;
            this.MakeActivityAware();
        }

        private async void frmLabelPrint_Load(object sender, EventArgs e)
        {
            try
            {
                txtStageName.BackColor = Constants.PrimaryAppColour;
                this.Text = $"Label Print {ApplicationVersion}";
                await GetOrdersForPrintAsync();
                ConfigureComPort();
                LoadComPorts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ConfigureComPort()
        {

            if (_serialPort.IsOpen)
                _serialPort.Close();

            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.RequestToSend;
            _serialPort.ReadTimeout = 5000; // Set a read timeout
        }

        private async Task GetOrdersForPrintAsync()
        {
            var orders = await _dataService.GetOrdersForPrintAsync();
            var sortedOrders = orders.OrderBy(o => o.Odf).ToList();

            if (sortedOrders.Any())
            {
                cboOrders.SelectedIndexChanged -= cboOrders_SelectedIndexChanged;
                cboOrders.DisplayMember = "DisplayText";
                cboOrders.ValueMember = "CustomerOrderId";
                cboOrders.DataSource = sortedOrders;
                cboOrders.SelectedIndex = -1;
                cboOrders.SelectedIndexChanged += cboOrders_SelectedIndexChanged;
            }
            else
            {
                MessageBox.Show("No orders available for printing.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadComPorts()
        {
            cboPrintPort.DataSource = null;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%)'");
                List<ComPortItem> portList = new List<ComPortItem>();

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string name = queryObj["Name"]?.ToString();
                    string portName = ExtractComPort(name);

                    if (!string.IsNullOrEmpty(portName))
                        portList.Add(new ComPortItem()
                        {
                            Display = $"{portName} - {name.Replace(portName, "").Trim()}",
                            Value = portName
                        });
                }

                if (portList.Count > 0)
                {
                    var lastPort = Properties.Settings.Default.LastPrinterPortUsed;
                    var foundPort = portList.FirstOrDefault(p => p.Value == lastPort);

                    cboPrintPort.SelectedIndexChanged -= cboPrintPort_SelectedIndexChanged;
                    cboPrintPort.DisplayMember = "Display";
                    cboPrintPort.ValueMember = "Value";
                    cboPrintPort.DataSource = portList;
                    cboPrintPort.SelectedIndexChanged += cboPrintPort_SelectedIndexChanged;

                    if (foundPort != null)
                        cboPrintPort.SelectedValue = foundPort.Value;
                    else
                        cboPrintPort.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading COM ports: {ex.Message}");

            }
        }

        private string ExtractComPort(string deviceName)
        {
            var match = System.Text.RegularExpressions.Regex.Match(deviceName, @"\(COM\d+\)");
            return match.Success ? match.Value.Trim('(', ')') : string.Empty;
        }

        private void cboPrintPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPrintPort.SelectedIndex == -1)
            {
                lblPortStatus.Text = $"Printer Port not selected";
                lblPortStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            var port = cboPrintPort.SelectedItem as ComPortItem;
            if (port != null)
            {
                _serialPort.PortName = port.Value;
                if (CanPrint())
                {
                    Properties.Settings.Default.LastPrinterPortUsed = cboPrintPort.SelectedValue.ToString();
                    Properties.Settings.Default.Save();
                    lblPortStatus.Text = $"Connected to: {_serialPort.PortName}";
                    lblPortStatus.ForeColor = System.Drawing.Color.Green;

                }
                else
                {
                    lblPortStatus.Text = $"Unable to connect to printer port";
                    lblPortStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;
            cboOrders.Enabled = false;

            if (!CanConnectDb())
            {
                cboOrders.Enabled = true;
                btnPrint.Enabled = true;
                return;
            }

            if (!CanPrint())
            {
                cboOrders.Enabled = true;
                btnPrint.Enabled = true;
                return;
            }
            if (!CanConnectToPrinter())
            {
                cboOrders.Enabled = true;
                btnPrint.Enabled = true;
                return;
            }


            var printRange = GetPrintRange(customerOrderId: (int)cboOrders.SelectedValue, printQuantity: Int32.Parse(txtQuantity.Text));
            if (printRange == null)
            {
                cboOrders.Enabled = true;
                btnPrint.Enabled = true;
                return;
            }


            PrintLabels(printRange);
            Thread.Sleep(1000 * 10); // Wait 10 seconds to prevent users from printing ahead
            ResetInputs();
        }

        private void ResetInputs()
        {
            cboOrders.SelectedIndex = -1;
            //txtQuantity.Text = string.Empty;
            btnPrint.Enabled = true;
            cboOrders.Enabled = true;
            btnPrint.Focus();
        }


        private SerialPrintRange GetPrintRange(int customerOrderId, int printQuantity)
        {
            var nextRange = _dataService.AddPrintRange(customerOrderId: customerOrderId, printQuantity: printQuantity, userId: _frmMain.CurrentUser.UserId);

            if (nextRange == null)
            {
                DisplayMessage("Failed to create print range.", isError: true);
                return null;
            }

            if (!nextRange.IsSuccess)
            {
                DisplayMessage($"Failed to create print range: {nextRange.Message}", isError: true);
                return null;
            }

            return nextRange.PrintRange;
        }

        private void btnTestPrint_Click(object sender, EventArgs e)
        {
            if (!CanConnectToPrinter())
                return;
            SerialPrintRange range = new SerialPrintRange()
            {
                CustomerOrderId = 0,
                Odf = "TestODF",
                Pid = "TestPID",
                Model = "Model",
                DateCode = "2501",
                FirstSerial = 1,
                LastSerial = 2,
                PcbaPartNumber = "TestPCBA",
                FirstFullSerialNumber = "TestPCBA*TestPID*00001*2501*TestODF*Model;",
                LastFullSerialNumber = "TestPCBA*TestPID*00002*2501*TestODF*Model;",
                PrintQuantity = 2,
                DatePrinted = DateTime.Now,
                UserID = _frmMain.CurrentUser.UserId

            };
            PrintLabels(range);
        }

        private void PrintLabels(SerialPrintRange serialPrintRange)
        {

            var labelText = GetLabelText(serialPrintRange, isTest: false);

            try
            {


                try
                {
                    _serialPort.Open();
                    _serialPort.WriteLine(labelText);
                    DisplayMessage($"Labels sent to printer: {serialPrintRange.FirstFullSerialNumber} to {serialPrintRange.LastFullSerialNumber}");
                }
                catch (Exception ex)
                {
                    DisplayMessage($"Printing failed: {ex.Message}", true);
                }
                finally
                {
                    _serialPort.Close();
                }

            }
            catch (Exception ex)
            {
                DisplayMessage($"Error initializing printer port: {ex.Message}", isError: true);
                return;
            }
        }



        private bool CanPrint()
        {
            if (cboOrders.SelectedIndex == -1)
            {
                DisplayMessage("Please select an order first");
                return false;
            }

            if (!Int32.TryParse(txtQuantity.Text, out int printQuantity) || printQuantity <= 0)
            {
                DisplayMessage("Please enter a valid print quantity.", isError: true);
                return false;
            }

            if (printQuantity % 2 != 0)
            {
                DisplayMessage("Print quantity must be an even number.", isError: true);
                return false;
            }

            return true;
        }
      

        private bool CanConnectToPrinter()
        {
            try
            {

                if (cboPrintPort.SelectedIndex == -1)
                {
                    DisplayMessage("Please select a printer port first.", isError: true);
                    return false;
                }
                if (_serialPort.IsOpen)
                    _serialPort.Close();

                _serialPort.BaudRate = 115200;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Parity = Parity.None;
                //_serialPort.PortName = _serialPort.PortName;
                _serialPort.Handshake = Handshake.RequestToSend;
                _serialPort.ReadTimeout = 3000; //milliseconds


                _serialPort.Open();

                _serialPort.Write(new byte[] { 0x1B, 0x73 }, 0, 2);
                Thread.Sleep(500);
                string status = _serialPort.ReadExisting();
                var statusRegex = new Regex(@"^(?<online>[YN])(?<error>.)(?<labels>\d{6})(?<job>[YN])$");

                Match match = statusRegex.Match(status);

                if (!match.Success)
                {
                    DisplayMessage("Printer status response is invalid.", isError: true);
                    return false;
                }

                char onlineStatus = match.Groups["online"].Value[0];    // 'Y'
                char errorStatus = match.Groups["error"].Value[0];      // '-'
                string labelsRemaining = match.Groups["labels"].Value;  // "000000"
                char jobStatus = match.Groups["job"].Value[0];          // 'N'

                if (onlineStatus != 'Y')
                {
                    DisplayMessage("Printer is not online. Please check the connection.", isError: true);
                    return false;
                }
                if (jobStatus != 'N')
                {
                    DisplayMessage("Printer is busy with another job. Please wait.", isError: true);
                    return false;
                }

                if (errorStatus != '-')
                {
                    string errorMessage = GetErrorMessage(errorStatus);
                    DisplayMessage($"Printer has an error: {errorMessage}", isError: true);
                    return false;
                }

                if (labelsRemaining != "000000")
                {
                    DisplayMessage("Printer is still busy with another print job.", isError: true);
                    return false;
                }
            }
            catch (TimeoutException)
            {
                DisplayMessage("Printer did not respond in time. Please check the connection.", isError: true);
                return false;
            }
            catch (Exception ex)
            {
                DisplayMessage($"An error occurred while checking printer status: {ex.Message}", isError: true);
                return false;
            }


            return true;
        }

        private string GetErrorMessage(char errorCode)
        {

            switch (errorCode)
            {
                case 'P':
                    return "Out Of Paper";
                case 'F':
                    return "Out of Ribbon";
                case 'D':
                    return "Printhead Open";
                case 'W':
                    return "Printer Head Overheat";
                case '-':
                    return "No Error";
                default:
                    return $"Unknown error code: {(int)errorCode}";
            }
        }


        private bool CanConnectDb()
        {
            try
            {
                return _dataService.DbConnectionOk();

            }
            catch (Exception ex)
            {
                DisplayMessage($"Database connection failed: {ex.Message}", isError: true);
                return false;
            }
        }

        private string GetLabelText(SerialPrintRange range, bool? isTest = null)
        {
            // S { ptype; } xo, yo, ho, dy, wd {,dx,col} {; name}

            // ptype; Gap sensor type(l0 = reflex below, l1 = transmitted light, l2 = reflex above, e = continuous material)
            //xo, Offset in X direction(from the left)
            //yo, Offset in Y direction(from above)
            //ho, Label hight
            //dy, Label start distance(height + gap)
            //wd Label width
            //dx Distance from the edge of the first label to the edge of the next label in the horizontal direction
            //col Number of labels horizontally(columns)(default value = 1)
            //name Text that is shown on the printer display
            //S l1;0,0,10,13,30,33,2

            //H speed {,h}{,t}{,s}{,Br}
            //speed, The speed as an integer in mm / s(permitted values vary depending on the printer)
            //h,     Heat value in points(from - 20 to + 20), is added to the printer setting
            //t,     Type: T = transfer, D = direct thermal(default: setup value in the printer’s menu)
            //s,     Ribbon saver on/ off(R0 = off, R1 = on)
            //Br     Retraction speed of the material in millimeters or in inches e.g. “B100” pulls the media back at 100 mm / s after printing, provided the printer is set to mm.

            //O R,P,U
            //O = Options
            //R = Rotate the label by 180°
            //P = There is no return transport20, faster printing, white stripe in the label possible
            //U = Unique label - suppresses the pause/reprint option so that no label can be accidentally printed twice.

            //string counterText = $"[SER:{range.FirstSerial.ToString().PadLeft(5, '0')}-{range.LastSerial.ToString().PadLeft(5, '0')}]";


            return $@"e IMG;*
; LABEL ->  = C:\20231130 Programming Code\_Programming\TCL\TCL SFIS\Labels\TCL_PCBA30x10mm Rev1.2.stc
; CREATED: 2026-03-16 2:58:56 PM, Driver version: 1.0.7.3, Target model: EOS5/300
m m
zO
J
j 3175935086316324
S l1;0,0,10,10,30,33,2
H 50,0,T,R0,B0
O R,P,U
; ==== Counter setup (for incrementing serial number) ====
T:Counter;1,0,0,3,3;[SER:{range.FirstSerial.ToString().PadLeft(5, '0')}][WINF][I]
; ==== Barcode using counter  ====
B:Barcode1;1.19,1.5,0,datamatrix,0.29;{range.PcbaPartNumber}*{range.Pid}*[*:1,Counter][C:0][D:5,0]*{range.DateCode}*{range.Odf}*{range.Model};
; ==== Human-readable text labels ====
T:Text_Odf;9.97,2.64,0,3,1.73,k;{range.Odf}
T:Text_Model;21.02,2.64,0,3,1.69,k;{range.Model}
T:Text_DateCode;24.13,8.57,0,3,1.77,k;{range.DateCode}
T:Text3;9.97,6.71,0,3,1.73,k;P/N:
T:Text_Serial;15.3,8.57,0,3,1.7,k;[*:1,Counter][C:0][D:5,0]
T:Text5;9.97,8.57,0,3,1.73,k;Serial:
T:Text1;9.97,4.79,0,3,1.73,k;PID:
T:Text_Pid;13.76,4.79,0,3,1.73,k;{range.Pid}
T:Text4;20.67,8.57,0,3,1.73,k;DC:
T:Text_PartNumber;13.74,6.71,0,3,1.73,k;{range.PcbaPartNumber}
A {range.PrintQuantity}";

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
                lblMessage.Text = message;
            }
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            LoadComPorts();
        }

        private void btnTestPort_Click(object sender, EventArgs e)
        {
            if (_serialPort == null || string.IsNullOrEmpty(_serialPort.PortName))
            {
                DisplayMessage("Please select a valid printer port first.", isError: true);
                return;
            }
            if (CanConnectToPrinter())
            {
                MessageBox.Show($"Printer {_serialPort.PortName} is ready for printing.", "Printer Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQuantity.Text))
            {
                if (int.TryParse(txtQuantity.Text, out int quantity) && quantity > 0)
                {
                    lblLabelMediaLengthNeeded.Text = $"Label Media Length Needed: {(quantity * 13 / 2) - 3} mm"; // Each label is 10mm + 3mm gap - 2 across
                }
                else
                {
                    lblLabelMediaLengthNeeded.Text = "Label Media Length Needed: Invalid Quantity";
                }
            }
            else
            {
                lblLabelMediaLengthNeeded.Text = "Label Media Length Needed: 0 mm";
            }
        }

        private void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cboOrders.SelectedIndex != -1)
            {
                var selectedOrder = cboOrders.SelectedItem as SerialPrintOrder;
                if (selectedOrder != null)
                {
                    btnPrint.Enabled = true;
                    GetLabelPrintBalanceForOrder(selectedOrder.CustomerOrderId);
                }
            }
            else
            {
                btnPrint.Enabled = false;
            }
        }

        private async void GetLabelPrintBalanceForOrder(int CustomerOrderId)
        {
            if (cboOrders.SelectedIndex == -1)
            {
                lblPrintBalance.Text = "N/A";
                return;
            }

            var balance = await _dataService.GetLabePrintBalanceForOrder(CustomerOrderId);
            lblPrintBalance.Text = $"{balance}";

        }
    }
}
