using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Extension;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Model;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Mvc.Scheduler
{
    internal class RefreshCache : ISchedule
    {
        private readonly IAppLog _log = new AppLog();

        public void Execute(object state)
        {
            try
            {
                ConfigGlobal.Refresh();
                Schedule.Cache.RefreshCache();

                CameraSource.Cache.RefreshCache();

                ExamineSchedulerException();

                _log.Info("Scheduler executed: (RefreshCache)");
            }
            catch (Exception ex)
            {
                _log.Warn("Scheduler failed: (RefreshCache)");
                _log.Error(ex);
            }
        }

        private static void ExamineSchedulerException()
        {
            // 获取当前进程的上次运行时间
            var scheduler = Schedule.Cache.ScheduleList.Find(x => x.ScheduleKey.Equals("RefreshCache"));

            if (scheduler != null)
            {
                var timeLower = DateTime.Now.AddSeconds(-scheduler.Seconds);

                using (IRepository repo = new Repository())
                {
                    // 检测warning日志
                    var warnLogs = repo.Query<Log>(x => !x.Logger.Equals(nameof(DaoLog)) && x.CreateTime >= timeLower)
                        .FindAll(x => x.Level.Equals(LogLevel.Warn))
                        .OrderByDescending(x => x.CreateTime).ToList();

                    if (warnLogs.Count > 0)
                    {
                        // 发现有warning日志, 发警告邮件
                        SendEmail(ConfigGlobal.AdminEmail, "外集卡违章数据同步告警消息", warnLogs);
                    }

                    var errorLogs = repo.Query<Log>(x => !x.Logger.Equals(nameof(DaoLog)) && x.CreateTime >= timeLower)
                        .FindAll(x => x.Level.Equals(LogLevel.Error) || x.Level.Equals(LogLevel.Fatal))
                        .OrderByDescending(x => x.CreateTime).ToList();

                    if (errorLogs.Count > 0)
                    {
                        // 发现有error日志，发异常邮件
                        SendEmail(ConfigGlobal.AdminEmail, "外集卡违章数据同步异常提醒", errorLogs);
                    }
                }
            }

        }

        private static void SendEmail(string[] to, string subject, List<Log> logs)
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

            mail.Body = "以下内容为系统自动发送，请勿直接回复，谢谢。\r\n\r\n";//邮件内容 
            mail.Body += logs.ToJson();
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