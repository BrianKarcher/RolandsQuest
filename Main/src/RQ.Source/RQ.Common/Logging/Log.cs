using System;

namespace RQ.Logging
{
    public static class Log
    {
        private static log4net.ILog _logInternal;

        private static log4net.ILog _log
        {
            get
            {
                if (_logInternal == null)
                    Init();
                return _logInternal;
            }
        }

        public static void Init()
        {
            ConfigureAllLogging.Configure();
            _logInternal = log4net.LogManager.GetLogger("Logger");
        }

        public static void Debug(string message)
        {
            _log.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            _log.Debug(message, ex);
        }

        public static void Warn(string message)
        {
            _log.Warn(message);
        }

        public static void Warn(string message, Exception ex)
        {
            _log.Warn(message, ex);
        }

        public static void Info(string message)
        {
            _log.Info(message);
        }

        public static void Info(string message, Exception ex)
        {
            _log.Info(message, ex);
        }

        public static void Error(string message)
        {
            _log.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            _log.Error(message, ex);
        }

        public static void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public static void Fatal(string message, Exception ex)
        {
            _log.Fatal(message, ex);
        }
    }
}
