using System;
using Serilog;

namespace StreamNode.Services
{
    public class LoggerManager
    {
        private static String logTemplate = "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}";
        public static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: logTemplate)
                .WriteTo.File("StreamNode-{Date}.log", fileSizeLimitBytes: 20971520, outputTemplate: logTemplate)
                .CreateLogger();
        }
    }
}