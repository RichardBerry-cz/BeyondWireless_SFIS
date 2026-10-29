using System.Windows.Forms;
using static BeyondWireless_SFIS.Shared.Sounds;

namespace BeyondWireless_SFIS
{
    public partial class frmError : Form
    {
        private string _errorMessage;
        public frmError(string errorMessage)
        {
            InitializeComponent();
            _errorMessage = errorMessage;
            txtErrorMessage.Text = _errorMessage;
            // Ensure no text is selected in txtHeader
            txtHeader.SelectionStart = 0;
            txtHeader.SelectionLength = 0;
            
            PlayError();
        }
    }
}
