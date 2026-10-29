using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BeyondWireless_SFIS.Models
{
    public class LinkModule : INotifyPropertyChanged
    {

        public int LinkModuleId { get; set; } = 0;
        public int ProductId { get; set; }
        public string PartNumber { get; set; }
        public string ModuleName { get; set; }
        
        public bool IsActive { get; set; }


        public int Id { get; set; }
        public string SetSerialNumber { get; set; } = string.Empty;
        public string DisplayText => $"{PartNumber} - {ModuleName}";

        private string _scannedModuleSerial;
        public string ScannedModuleSerial
        {
            get => _scannedModuleSerial;
            set
            {
                _scannedModuleSerial = value;
                OnPropertyChanged();
            }
        }

        private bool _ok;
        public bool Ok
        {
            get => _ok;
            set
            {
                _ok = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

