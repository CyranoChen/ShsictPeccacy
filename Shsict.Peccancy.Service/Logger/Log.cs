using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Service.Logger
{
    [Table("SSICT_PECCANCY_LOG")]
    public class Log : Entity<int>
    {
        #region Members and Properties

        [Column("LOGGER")]
        public string Logger { get; set; }

        [Column("LOGLEVEL")]
        public LogLevel Level { get; set; }

        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }

        [Column("MESSAGE")]
        public string Message { get; set; }

        [Column("ISEXCEPTION")]
        public bool IsException { get; set; }

        [Column("STACKTRACE")]
        public string StackTrace { get; set; }

        #endregion

        public static void Logging(string logger, LogLevel level, string message)
        {
            var sql = $@"INSERT INTO {ConfigurationManager.AppSettings["Oracle.Schema.Owner"]}.{GetTableAttr<Log>().Name} 
                               (LOGGER, LOGLEVEL, CREATETIME, MESSAGE, ISEXCEPTION, STACKTRACE) 
                               VALUES (:logger, :logLevel, SYSDATE, :message, '0', :stackTrace)";

            object[] para =
            {
                new OracleParameter(":logger", logger),
                new OracleParameter(":logLevel", (int)level),
                //new OracleParameter(":createTime", DateTime.Now),
                new OracleParameter(":message", message),
                //new OracleParameter(":isException", 0),
                new OracleParameter(":stackTrace", string.Empty)
            };

            using (IRepository repo = new Repository())
            {
                repo.ExecuteSqlCommand(sql, para);
            }
        }

        public static void Logging(string logger, LogLevel level, Exception ex)
        {
            var sql = $@"INSERT INTO {ConfigurationManager.AppSettings["Oracle.Schema.Owner"]}.{GetTableAttr<Log>().Name} 
                               (LOGGER, LOGLEVEL, CREATETIME, MESSAGE, ISEXCEPTION, STACKTRACE) 
                               VALUES (:logger, :logLevel, SYSDATE, :message, '1', :stackTrace)";

            object[] para =
            {
                new OracleParameter(":logger", logger),
                new OracleParameter(":logLevel", (int)level),
                //new OracleParameter(":createTime", DateTime.Now),
                new OracleParameter(":message", ex.Message),
                //new OracleParameter(":isException", 1),
                new OracleParameter(":stackTrace", ex.StackTrace)
            };

            using (IRepository repo = new Repository())
            {
                repo.ExecuteSqlCommand(sql, para);
            }
        }

        private static TableAttribute GetTableAttr<T>() where T : class
        {
            var attr = Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute)) as TableAttribute;
            return attr ?? new TableAttribute(typeof(T).Name);
        }
    }

    //FATAL（致命错误）：记录系统中出现的能使用系统完全失去功能，服务停止，系统崩溃等使系统无法继续运行下去的错误。例如，数据库无法连接，系统出现死循环。
    //ERROR（一般错误）：记录系统中出现的导致系统不稳定，部分功能出现混乱或部分功能失效一类的错误。例如，数据字段为空，数据操作不可完成，操作出现异常等。
    //WARN（警告）：记录系统中不影响系统继续运行，但不符合系统运行正常条件，有可能引起系统错误的信息。例如，记录内容为空，数据内容不正确等。
    //INFO（一般信息）：记录系统运行中应该让用户知道的基本信息。例如，服务开始运行，功能已经开户等。
    //DEBUG （调试信息）：记录系统用于调试的一切信息，内容或者是一些关键数据内容的输出。
    /// <summary>
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = -1,
        Fatal = -2
    }
}