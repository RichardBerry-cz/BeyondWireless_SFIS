using System;
using System.Windows.Forms;
using BeyondWireless_SFIS.Models;
using BeyondWireless_SFIS.Shared;

namespace BeyondWireless_SFIS
{
    public partial class frmReportDefinition : Form
    {
        private readonly ReportDefinition _reportDefinition;
        public frmReportDefinition(ReportDefinition reportDefinition)
        {
            InitializeComponent();
            _reportDefinition = reportDefinition;
           
        }

        private void frmReportDefinition_Load(object sender, EventArgs e)
        {
            txtReportName.BackColor = Constants.PrimaryAppColour;
            txtReportName.Text = _reportDefinition.ReportName;
            txtReportDefinition.Text = _reportDefinition.DefinitionText;
        }
    }
}
