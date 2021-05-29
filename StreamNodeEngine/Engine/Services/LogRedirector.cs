using System;
namespace StreamNodeEngine.Engine.Services
{
    public class LogRedirector
    {
        public enum LogRedirectorLevel
        {
            INFO,
            WARN,
            ERROR,
            DEBUG
        }

        public static event LogRedirectCallback OnLog;
        public delegate void LogRedirectCallback(object data, LogRedirectorLevel level);

        private static void log(LogRedirectorLevel level, String message)
        {
            OnLog(message, level);
        }

        public static void info(String message)
        {
            log(LogRedirectorLevel.INFO, message);
        }

        public static void warn(String message)
        {
            log(LogRedirectorLevel.WARN, message);
        }

        public static void error(String message)
        {
            log(LogRedirectorLevel.ERROR, message);
        }

        public static void debug(String message)
        {
            log(LogRedirectorLevel.DEBUG, message);
        }
    }
}