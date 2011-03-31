using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.Mail;
using System.Net;


namespace T7ECommon
{
    public partial class Common
    {
        static string MailUserName = "jumplist.extender@gmail.com";
        static string MailPassword = ""; //This must be filled in
        static string MailRecipient = "digimarco35@yahoo.com";
        static string MailVersion = "v0.4";

        static public void SendExceptionLog(Exception e)
        {
            SendExceptionLog(e, "", "", "", "");
        }

        static public void SendExceptionLog(Exception e, string appName)
        {
            SendExceptionLog(e, appName, "", "", "");
        }

        static public void SendExceptionLog(Exception e, string appName, string appPath)
        {
            SendExceptionLog(e, appName, appPath, "", "");
        }

        static public void SendExceptionLog(Exception e, string appName, string appPath, string appWindowClassName, string appId)
        {
            DialogResult errSendResult = MessageBox.Show(
                    "Sorry -- Jumplist Extender reached a critical error, and has to close immediately." + Environment.NewLine
                    + "Could you send the below error report with just one click? It helps the developer GREATLY! Thanks!" + Environment.NewLine + Environment.NewLine
                    + e.Message.ToString(),
                    "Critical Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

            if (errSendResult != DialogResult.Yes) return;
            else { MessageBox.Show("Thanks! An error report will be sent.",
                "Sending Mail",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk); }

            try
            {
                MailAddress fromAddress = new MailAddress(Common.MailUserName, 
                    "JLE " 
                    + MailVersion 
                    + ": " + Environment.UserName);
                MailAddress toAddress = new MailAddress(Common.MailRecipient, "JLE Developer");
                string fromPassword = Common.MailPassword;
                string subject = "JLEx: " + e.Message;
                string body = GetExceptionLogBody(e, appName, appPath, appWindowClassName, appId);

                SmtpClient smtp = new SmtpClient
                           {
                               Host = "smtp.gmail.com",
                               Port = 587,
                               EnableSsl = true,
                               DeliveryMethod = SmtpDeliveryMethod.Network,
                               UseDefaultCredentials = false,
                               Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                           };
                using (MailMessage message = new MailMessage(fromAddress, toAddress)
                                     {
                                         Subject = subject,
                                         Body = body
                                     })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Email could not be sent: " + ee.Message + Environment.NewLine
                    + "Sorry, but thanks for your consideration! Click \"OK\" to exit.",
                    "Email Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            MessageBox.Show("Email sent! Thanks for your consideration! If this bug happens repeatedly, please file a bug report at\r\n\r\nhttp://code.google.com/p/jumplist-extender/issues/list\r\n\r\nor email me at digimarco35@yahoo.com."
                + "\r\n\r\nClick \"OK\" to exit.",
                "Email Sent",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
        }

        static public string GetExceptionLogBody(Exception e, string appName, string appPath, string appWindowClassName, string appId)
        {
            // PROGRAM FILES PATH
            // APPDATA PATH
            // ENV64
            // ENVINSTALLED
            
            // CurrentAppId/CurrentAppPath/CurrentAppProcessName/CurrentAppWindowClassName

            // CurrentJumplistItem Name/Icon/Type

            string output = "";

            try
            {
                output += "Path_ProgramFiles: " + Common.Path_ProgramFiles + Environment.NewLine;
                output += "Path_AppData: " + Common.Path_AppData + Environment.NewLine;
                output += Environment.NewLine;
                output += "Env64: " + Common.Env64.ToString() + Environment.NewLine;
                output += "Windows Version: " + Environment.OSVersion.VersionString + Environment.NewLine;
                //output += "EnvInstalled: " + Common.EnvInstalled.ToString() + Environment.NewLine;
                output += "JLE Version: " + Common.MailVersion + Environment.NewLine;

                output += appName != null && appName.Length > 0 ? "AppName: " + appName + Environment.NewLine
                    : "";
                output += appPath != null && appPath.Length > 0 ? "AppPath: " + appPath + Environment.NewLine
                    : "";
                output += appWindowClassName != null && appWindowClassName.Length > 0 ? "AppWindowClassName: " + appWindowClassName + Environment.NewLine
                    : "";
                output += appId != null && appId.Length > 0 ? "AppId: " + appId + Environment.NewLine
                    : "";
                output += Environment.NewLine;

                output += e.ToString();
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); Environment.Exit(-1); return e.ToString(); }

            return output;
        }

        static public void Fail(string messageString, int errorCode)
        {
            MessageBox.Show(messageString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Console.Error.WriteLine("FAIL ERROR::: " + messageString + " |||||");
            //Console.Error.WriteLine("Press any key to exit.");
            //Console.ReadLine();
            Environment.Exit(errorCode);
        }

        static public bool Debug_ShowMessageBox = false;

        static public void WriteDebug(string messageString)
        {
            Console.Write(messageString + " ||\r\n");
            if (Debug_ShowMessageBox) MessageBox.Show(messageString);
        }

        static public void WriteDebug(string messageString, int viewMode)
        {
            switch (viewMode)
            {
                default:
                    MessageBox.Show(messageString);
                    goto case 1;
                case 1: // No message box
                    Console.Write(messageString + " ||\r\n"); break;
            }
        }

        #region Logging

        static public string GetLogAppString(string processName, string appId)
        {
            return processName + " | " + appId;
        }

        static public int LogIndentLevel = 0;

        static public void Log(string messageString)
        {
            Log(messageString, -32767, false);
        }

        static public void Log(int logChange)
        {
            Log("", logChange, false);
        }

        static public void Log(string messageString, int logChange)
        {
            Log(messageString, logChange, false);
        }

        static public void Log(string messageString, int logChange, bool followingLine)
        {
#if (!DEBUG)
            return;
#endif
            if (messageString.Length > 0)
            {
                for (int i = 0; i < LogIndentLevel * 4; i++) Console.Write(" ");
                Console.Write(messageString + Environment.NewLine);
                if (followingLine) Console.Write(Environment.NewLine);
            }

            if (logChange != 0 && logChange != -32767)
            {
                LogIndentLevel = LogIndentLevel + logChange;
                if (LogIndentLevel < 0) LogIndentLevel = 0;
            }
            if (logChange == 0)
            {
                LogIndentLevel = 0;
                Console.Write(Environment.NewLine);
            }
        }
        #endregion
    }
}
