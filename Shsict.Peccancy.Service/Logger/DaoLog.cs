using System;

namespace Shsict.Peccancy.Service.Logger
{
    public class DaoLog : IDaoLog
    {
        public void Debug(string message)
        {
            Log.Logging(GetType().Name, LogLevel.Debug, message);
        }

        public void Debug(Exception ex)
        {
            Log.Logging(GetType().Name, LogLevel.Debug, ex);
        }

        public void Info(string message)
        {
            Log.Logging(GetType().Name, LogLevel.Info, message);
        }

        public void Error(Exception ex)
        {
            Log.Logging(GetType().Name, LogLevel.Error, ex);
        }

        public void Fatal(Exception ex)
        {
            Log.Logging(GetType().Name, LogLevel.Fatal, ex);
        }
    }
}