using System;
using System.Windows.Forms;
using BeyondWireless_SFIS.Services;

namespace BeyondWireless_SFIS.Shared
{
    public static class ActivityTrackingExtensions
    {
        public static void MakeActivityAware(this Form form)
        {
            form.HandleCreated += (sender, e) =>
            {
                LogAppVersion();
                AttachToControls(form);
            };
        }

        private static void AttachToControls(Control parentControl)
        {
            if (parentControl == null) return;
            

            foreach (Control control in parentControl.Controls)
            {
                // Skip controls that are already disposed
                if (control.IsDisposed) continue;

                // Attach to interactive controls
                if (control is Button || control is TextBox ||
                    control is DataGridView || control is ComboBox)
                {
                    control.Click += ControlActivityHandler;
                    control.KeyDown += ControlActivityHandler;
                }

                //handle datagridview 
                if(control is DataGridView dgv)
                {
                    dgv.CurrentCellChanged += ControlActivityHandler;
                }

                // Handle container controls
                if (control.HasChildren)
                {
                    AttachToControls(control);
                }
            }

            // Handle MenuStrip
            if (parentControl is MenuStrip menu)
            {
                foreach (ToolStripItem item in menu.Items)
                {
                    item.Click += ControlActivityHandler;
                }
            }
        }

        private static void LogAppVersion()
        {
            try
            {
                var dataService = new DataService();
                dataService.LogAppVersion();
            }
            catch (Exception ex)
            {
                //Logger.Error($"Failed to log application version. Error: {ex.Message}");
            }
        }

        private static void ControlActivityHandler(object sender, EventArgs e)
        {
            ActivityMonitor.RaiseUserActivity();
        }
    }
}
