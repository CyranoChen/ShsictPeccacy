using System;

namespace Shsict.Peccancy.Service.Logger
{
    public interface IDaoLog
    {
        void Debug(string message);
        void Debug(Exception ex);

        void Info(string message);

        void Error(Exception ex);

        void Fatal(Exception ex);
    }

    public interface IAppLog
    {
        void Info(string message);

        void Warn(string message);

        void Error(Exception ex);
    }

    public interface IUserLog
    {
        void Info(string message);

        void Warn(string message);

        void Error(Exception ex);
    }
}