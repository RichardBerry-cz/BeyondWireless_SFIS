using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using static BeyondWireless_SFIS.Shared.Logging;

namespace BeyondWireless_SFIS.Shared
{
    public class Email
    {

        public static bool SendHTMLEmail(string Subject, string MessageBody, List<string> ToList, List<string> CCList = null)
        {
            if (ToList.Count < 1)
                return false;

            string emailAddress = Constants.emailAddress;
            string emailPassword = Constants.emailPassword;

            try
            {
                using (SmtpClient Smtp_Server = new SmtpClient())
                {

                    using (MailMessage e_mail = new MailMessage())
                    {
                        Smtp_Server.UseDefaultCredentials = false;
                        Smtp_Server.Credentials = new System.Net.NetworkCredential(emailAddress, emailPassword);
                        Smtp_Server.Port = 587;
                        Smtp_Server.EnableSsl = true;
                        Smtp_Server.Host = "smtp.office365.com";

                        e_mail.From = new MailAddress(emailAddress, "CZ Electronics");

                        foreach (string address in ToList)
                        {
                            if(Validation.IsValidEmail(address))
                                e_mail.To.Add(address);
                        }
                          

                        if (CCList != null && CCList.Count > 0)
                        {
                            foreach (string address in CCList)
                            {
                                if(Validation.IsValidEmail(address))
                                    e_mail.CC.Add(address);
                            }
                        }

                        if (e_mail.To.Count < 1)
                        {
                            Logger.Debug("No valid email addresses in ToList.");
                            return false;
                        }

                        e_mail.Subject = Subject;
                        e_mail.Body = MessageBody;
                        e_mail.IsBodyHtml = true;
                        Smtp_Server.Send(e_mail);
                        e_mail.To.Clear();
                        e_mail.CC.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"Error sending email with attachment: {ex.Message}");
                return false;
            }
            return true;
        }

        public static bool SendHTMLEmail(string Subject, string MessageBody, List<string> ToList,
                      List<string> CCList, byte[] fileAttachment,
                      string attachmentFileName)
        {
            if (ToList.Count < 1)
                return false;

            string emailAddress = Constants.emailAddress;
            string emailPassword = Constants.emailPassword;

            try
            {
                using (SmtpClient Smtp_Server = new SmtpClient())
                {
                    using (MailMessage e_mail = new MailMessage())
                    {

                        Smtp_Server.UseDefaultCredentials = false;
                        Smtp_Server.Credentials = new System.Net.NetworkCredential(emailAddress, emailPassword);
                        Smtp_Server.Port = 587;
                        Smtp_Server.EnableSsl = true;
                        Smtp_Server.Host = "smtp.office365.com";

                        e_mail.From = new MailAddress(emailAddress, "CZ Electronics");

                        foreach (string address in ToList)
                        {
                            if (Validation.IsValidEmail(address))
                                e_mail.To.Add(address);
                        }


                        if (CCList != null && CCList.Count > 0)
                        {
                            foreach (string address in CCList)
                            {
                                if (Validation.IsValidEmail(address))
                                    e_mail.CC.Add(address);
                            }

                        }

                        e_mail.Subject = Subject;
                        e_mail.Body = MessageBody;
                        e_mail.IsBodyHtml = true;

                        // Add attachment if provided
                        if (fileAttachment != null && fileAttachment.Length > 0 && !string.IsNullOrEmpty(attachmentFileName))
                        {
                            // Don't use using block here - the Attachment will take ownership of the stream
                            MemoryStream ms = new MemoryStream(fileAttachment);
                            e_mail.Attachments.Add(new Attachment(ms, attachmentFileName,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                        }

                        Smtp_Server.Send(e_mail);

                        // Clean up attachments and their streams
                        foreach (var attachment in e_mail.Attachments)
                        {
                            attachment.Dispose();
                        }

                        e_mail.To.Clear();
                        e_mail.CC.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"Error sending email with attachment: {ex.Message}");
                return false;
            }
            return true;
        }

    }
}
