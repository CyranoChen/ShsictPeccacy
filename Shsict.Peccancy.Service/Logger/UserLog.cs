using System;

namespace Shsict.Peccancy.Service.Logger
{
    public class UserLog : IUserLog
    {
        public void Info(string message)
        {
            Log.Logging(GetType().Name, LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            Log.Logging(GetType().Name, LogLevel.Warn, message);
        }

        public void Error(Exception ex)
        {
            Log.Logging(GetType().Name, LogLevel.Error, ex);
        }
    }
}