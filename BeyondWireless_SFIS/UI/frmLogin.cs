using System;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Services;

namespace BeyondWireless_SFIS
{
    public partial class frmLogin : Form
    {
        public User user { get; set; }
        private DataService _dataService;
        public frmLogin(DataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
        }

        private User GetUser(string input)
        {
            User user = null;
            try
            {
                user = _dataService.GetUserByRfidSerial(input);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    txtInput.Clear();
                    txtInput.Focus();
                    MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while processing the user ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

      
        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                
                string input = txtInput.Text.Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    e.Handled = true; // Prevent the beep sound on Enter key press
                    user = GetUser(input);
                    if (user != null)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid RFID serial number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Only show warning if the user is closing the form manually (not programmatically)
            //if (e.CloseReason == CloseReason.UserClosing && this.DialogResult != DialogResult.OK)
            //{
            //    DialogResult result = MessageBox.Show(
            //        "Any unsaved changes will be lost.\n\nAre you sure you want to exit?",
            //        "Confirm Exit",
            //        MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Warning);

            //    if (result == DialogResult.No)
            //    {
            //        e.Cancel = true; // Prevent closing
            //    }
                
            //}

        }
    }
}
