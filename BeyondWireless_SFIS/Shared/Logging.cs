using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.IO;
using System.Windows.Forms;

namespace BeyondWireless_SFIS.Shared
{
    public class Logging
    {
        // //=======================================================================
        // //Install Nuget Packages:
        // //Install-Package Serilog
        // //Install-Package Serilog.Sinks.File
        // //Install-Package Serilog.Sinks.Email
        // //=======================================================================

        // Imports System.Net
        // Imports Serilog
        // Imports Serilog.Core
        // Imports Serilog.Events
        // Imports Serilog.Sinks.Email
        // Imports Serilog.Filters
        // Imports System.Net.Mail
        // Imports SerilogTest.Constants
        // Imports System.IO

        //using Serilog;
        //using Serilog.Core;
        //using Serilog.Events;
        //using Serilog.Sinks.Email;
        //using Serilog.Filters;
        //using System.Net.Mail;
        //using System.Windows.Forms

        private static LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);
        //private static LoggerConfiguration loggerConfiguration;

        public static ILogger Logger;

        // Public Shared Sub InitializeLogger()


        // Dim startupPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)
        // Dim logFilePath = Path.Combine(startupPath, "SeriLogTestLog.txt")

        // Logger = New LoggerConfiguration().WriteTo.File(logFilePath, rollingInterval:=RollingInterval.Day, retainedFileCountLimit:=5, outputTemplate:="{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}, [{Level}], {Message}{NewLine}{Exception}").MinimumLevel.ControlledBy(levelSwitch).WriteTo.Email(New EmailConnectionInfo With {
        // .FromEmail = errorEmailFromAddress,
        // .ToEmail = errorEmailToAddress,
        // .MailServer = "smtp.office365.com",
        // .NetworkCredentials = New Net.NetworkCredential(emailUsername, emailPassword),
        // .EnableSsl = True,
        // .Port = 587,
        // .EmailSubject = "SeriLogTest Message"
        // }, outputTemplate:="{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", batchPostingLimit:=1, restrictedToMinimumLevel:=Serilog.Events.LogEventLevel.[Error]).CreateLogger()

        // End Sub

        public static void UpdateLogLevel(LogEventLevel minLogLevel)
        {
            levelSwitch.MinimumLevel = minLogLevel;
        }

        // =======================================================================
        // ***** File Rollover based on time period, with retained file count limit *****
        // Note: To avoid bringing down apps with runaway disk usage the file sink limits file size to 1GB by default.
        // Once the limit Is reached, no further events will be written until the next roll point
        // Also specify retainedFileCountLimit: null if you want limitation only by the date.
        // With the current implementation default value of retainedFileCountLimit Is 31.
        // Therefore, if you leave the parameter out, this filter will also be applied
        // =======================================================================


        public static void InitializeLogger()
        {
            if (!Directory.Exists(Application.CommonAppDataPath))
                Directory.CreateDirectory(Application.CommonAppDataPath);
            Logger = new LoggerConfiguration().WriteTo.File($@"{Application.CommonAppDataPath}\SFIS_.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5).MinimumLevel.ControlledBy(levelSwitch).CreateLogger();
        }
    }
}
