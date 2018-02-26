using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using System.Text;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Service.Tests
{
    [TestClass()]
    public class EmailTests
    {
        [Ignore]
        [TestMethod()]
        public void NativeSmtpTest()
        {
            var mail = new MailMessage();
            mail.To.Add("SMS@shsict.com");
            mail.CC.Add("cyrano@arsenalcn.com");

            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            mail.From = new MailAddress("SMS@shsict.com", "外集卡违章数据同步监控", System.Text.Encoding.UTF8);

            mail.Subject = "TEST MAIL";//邮件标题 
            mail.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 

            mail.Body = "以下内容为系统自动发送，请勿直接回复，谢谢。";//邮件内容 
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
                client.Send(mail);
                client.SendAsync(mail, userState);

                Assert.IsNotNull(userState);
            }
            catch (SmtpException ex)
            {
                throw ex;
            }

        }

        [Ignore]
        [TestMethod()]
        public void EmailTest()
        {
            var to = "xudanfu@shsict.com".Split('|');

            SendEmail(to, "外集卡违章数据同步停止提醒", "测试");
        }

        private void SendEmail(string[] to, string subject, string content)
        {
            var mail = new MailMessage();

            foreach (var t in to)
            {
                mail.To.Add(t);
            }

            mail.CC.Add("cyrano@arsenalcn.com");

            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            mail.From = new MailAddress("SMS@shsict.com", "外集卡违章数据同步监控", Encoding.UTF8);

            mail.Subject = subject;//邮件标题 
            mail.SubjectEncoding = Encoding.UTF8;//邮件标题编码 

            mail.Body = "以下内容为系统自动发送，请勿直接回复，谢谢。\r\n\r\n";//邮件内容 
            mail.Body += content;
            mail.BodyEncoding = Encoding.UTF8;//邮件内容编码 

            mail.IsBodyHtml = false;//是否是HTML邮件 
            mail.Priority = MailPriority.High;//邮件优先级 

            var client = new SmtpClient
            {
                Credentials = new NetworkCredential("SMS@shsict.com", "Sms@2018"),
                Host = "lotus.shsict.com"
            };

            try
            {
                client.Send(mail);
            }
            catch (SmtpException ex)
            {
            }
        }

    }
}