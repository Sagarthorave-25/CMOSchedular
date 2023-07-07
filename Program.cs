using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CMORefMailSchedular
{
    class Program
    {
        static void Main(string[] args)
        {
            string ApplicationNo = string.Empty;
            try
            {
                //TPA Folder Structure
                string Special_Medical_Reports_TPA = ConfigurationSettings.AppSettings["Special_Medical_Reports_TPA"];
                //DMS Index Structure
                string Special_Medical_Reports_Stored = ConfigurationSettings.AppSettings["Special_Medical_Reports_Stored"];
                DirectoryInfo sr = new DirectoryInfo(Special_Medical_Reports_TPA);
                FileInfo[] smFiles = sr.GetFiles();
                foreach (FileInfo file in smFiles)
                { string str = string.Empty;
                    ApplicationNo = file.Name.ToString().Split('_')[0];
                    string result = new BussLayer().CheckApplicationNoCMO(ApplicationNo);
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (result != "Ok")
                        {
                            
                            try
                            {
                                 str= "New Business_" + ApplicationNo + "_" + "Special Medical Reports.pdf";
                                MailMessage msg = new MailMessage();
                                MailAddress madFrom = new MailAddress(ConfigurationSettings.AppSettings["FROMEMAIL"].ToString());
                                msg.From = madFrom;
                                msg.CC.Add(ConfigurationSettings.AppSettings["CCMailId"].ToString());
                                msg.Subject = ConfigurationSettings.AppSettings["SUBJECT"].ToString().Replace("ApplicationNo", ApplicationNo);
                                msg.Body = ConfigurationSettings.AppSettings["BODY"].ToString().Replace("ApplicationNo", ApplicationNo);
                                msg.IsBodyHtml = true;
                                msg.To.Add(ConfigurationSettings.AppSettings["TOEMAIL"].ToString());
                                SmtpClient smtp = new SmtpClient();
                                smtp.Port = Int32.Parse(ConfigurationSettings.AppSettings["PORT"]);
                                smtp.Host = ConfigurationSettings.AppSettings["HOSTNAME"].ToString().Trim();
                                System.Net.Mail.Attachment attachment;
                                attachment = new System.Net.Mail.Attachment(Special_Medical_Reports_TPA + file.ToString());
                                msg.Attachments.Add(attachment);
                                smtp.Send(msg);
                                msg.Dispose();
                                smtp.Dispose();
                                new BussLayer().UpdateUCNNumber(ApplicationNo);
                                new Commfunc().SaveErrorLogs(ApplicationNo, "Main", "Program", "Succcess", "Success-Mail Send");
                                //Move Files
                                File.Copy(file.FullName.ToString(), Special_Medical_Reports_Stored + str.ToString(), true);

                            }
                            catch (Exception ex)
                            {
                                new Commfunc().SaveErrorLogs(ApplicationNo, "Main", "Program", "Fail", "Fail Mail Send " + ex.Message);
                            }

                        }
                        else
                        {
                            new Commfunc().SaveErrorLogs(ApplicationNo, "Main", "Program", "Succcess", "Application Status Resolved");
                        }
                    }
                }
            }
            catch (Exception ex) {
                new Commfunc().SaveErrorLogs(ApplicationNo, "Main", "Program", "error", ex.Message);
            }
        }
    }
}
