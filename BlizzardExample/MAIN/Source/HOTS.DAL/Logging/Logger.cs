using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HOTS.Logging
{
    public class Logger : ILogger
    {
        //private Mutex _mutex;
        private ILog _logger;

        public static readonly Logger Instance = new Logger();

        private Logger()
        {
            //Load log4net Configuration
            XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(this.GetType());
        }

        public void LogError(string message, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            // append empty line
            message += Environment.NewLine;

            _logger.Error(message);
        }

        public void LogError(Exception x)
        {
            LogError(BuildExceptionMessage(x));
        }

        public void LogError(string message, Exception x, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            message += BuildExceptionMessage(x);

            _logger.Error(message);
        }

        public void LogInfo(string message, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            // append empty line
            message += Environment.NewLine;

            _logger.Info(message);
        }

        public void LogWarn(string message, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            // append empty line
            message += Environment.NewLine;

            _logger.Warn(message);
        }

        public bool LogFile(string filePath)
        {
            return (File.Exists(filePath));
        }

        public string BuildExceptionMessage(Exception ex, bool deep = true)
        {
            // Let's get the inner most exception
            Exception exception = ex;

            // Get the top exception
            string message = GetExceptionMessage(ex);

            if (deep)
            {
                // move through the exception and log info on the inner exceptions
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    message += GetExceptionMessage(exception);
                }
            }
            return message;
        }

        private string GetExceptionMessage(Exception exception)
        {
            string message = "";

            message += string.Concat(Environment.NewLine, "Message: ", exception.Message);
            message += string.Concat(Environment.NewLine, "Source: ", exception.Source);
            message += string.Concat(Environment.NewLine, "Stack Trace: ", exception.StackTrace);
            message += string.Concat(Environment.NewLine, "TargetSite: ", exception.TargetSite);

            if (!string.IsNullOrEmpty(exception.HelpLink))
                message += string.Concat(Environment.NewLine, "Help Link: ", exception.HelpLink);

            // get additional context for exception
            message += GetAdditionalContextMessage(exception);

            // add empty line
            message += string.Concat(Environment.NewLine);

            return message;
        }

        private string GetAdditionalContextMessage(Exception exception = null)
        {
            string message = string.Empty;

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                message += Environment.NewLine + "Error in Path: " + HttpContext.Current.Request.Path;
                message += Environment.NewLine + "Raw Url: " + HttpContext.Current.Request.RawUrl;
            }

            return message;
        }
    }
}
