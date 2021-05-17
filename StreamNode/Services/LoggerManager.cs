using System;
using Serilog;

namespace StreamNode.Services
{
    public class LoggerManager
    {
        private static String logTemplate = "{Timestamp:dd-MM-yyyy HH:mm:ss} | {Level} | {Message}{NewLine}{Exception}";
        
        ///
        /// File Size Limit of 20MB
        ///
        private static int fileSizeLimit = 20971520;
        
        public static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: logTemplate)
                .WriteTo.File("log/StreamNode.log", rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, outputTemplate: logTemplate)
                .CreateLogger();
        }
    }
}