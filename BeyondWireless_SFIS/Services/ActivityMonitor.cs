using System;

namespace BeyondWireless_SFIS.Services
{
    public class ActivityMonitor
    {
        public static event Action UserActivityDetected;

        public static void RaiseUserActivity()
        {
            UserActivityDetected?.Invoke();
        }
    }
}
