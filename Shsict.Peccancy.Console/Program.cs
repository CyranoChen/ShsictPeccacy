using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.Extension;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IAppLog log = new AppLog();

            var schedulers = Schedule.Cache.ScheduleList.FindAll(x => x.IsActive && x.Seconds > 0);

            if (schedulers.Count > 0)
            {
                var warningScheduler =
                    schedulers.FindAll(x => DateTime.Now > x.LastCompletedTime.AddSeconds(x.Seconds + 300));

                if (warningScheduler.Count > 0)
                {
                    var content = new StringBuilder();
                    foreach (var s in warningScheduler)
                    {
                        content.Append(new
                        {
                            ScheduleName = s.ScheduleKey,
                            LastCompletedTime = s.LastCompletedTime.ToString("yyyyMMdd HH:mm:ss"),
                            Interval = s.Seconds
                        }.ToJson());
                        content.Append("/r/n");
                    }

                    SendEmail(ConfigGlobal.AdminEmail, "外集卡违章数据同步停止提醒", content.ToString());
                }
            }

            log.Info("Examine Scheduler executed");
        }

        private static void SendEmail(string[] to, string subject, string content)
        {
            var mail = new MailMessage();

            foreach (var t in to)
            {
                mail.To.Add(t);
            }

            // 如果cyrano在管理员名单中，则抄送给cyrano
            if (ConfigGlobal.SystemAdmin.Any(x => x.Equals("cyrano", StringComparison.OrdinalIgnoreCase)))
            {
                mail.CC.Add("cyrano@arsenalcn.com");
            }

            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            mail.From = new MailAddress("SMS@shsict.com", "外集卡违章数据同步监控", System.Text.Encoding.UTF8);

            mail.Subject = subject;//邮件标题 
            mail.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 

            mail.Body = "以下内容为系统自动发送，请勿直接回复，谢谢。/r/n/r/n";//邮件内容 
            mail.Body += content;
            mail.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 

            mail.IsBodyHtml = false;//是否是HTML邮件 
            mail.Priority = MailPriority.High;//邮件优先级 

            var client = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential("SMS@shsict.com", "Sms@2018"),
                Host = "lotus.shsict.com"
            };

            object userState = mail;

            try
            {
                client.SendAsync(mail, userState);
            }
            catch (SmtpException ex)
            {
                IAppLog log = new AppLog();
                log.Error(ex);
            }
        }

    }
}
