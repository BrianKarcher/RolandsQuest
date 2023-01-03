using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOTS.Logging
{
    public interface ILogger
    {
        void LogError(string message, params string[] args);

        void LogError(Exception x);

        void LogError(string message, Exception x, params string[] args);

        void LogInfo(string message, params string[] args);

        void LogWarn(string message, params string[] args);

        bool LogFile(string filePath);
    }
}
