using System;
using Serilog;
using StreamNode.Services.LogProxy;
using StreamNodeEngine.Engine.Services;

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

            // Register proxy to put swan logging into serilog
            Swan.Logging.Logger.RegisterLogger(new EmbedIOLogProxy());

            // Register proxy to put StreamNodeEngine logging into serilog
            LogRedirector.OnLog += (msg, level) =>
            {
                switch (level)
                {
                    case LogRedirector.LogRedirectorLevel.INFO:
                        {
                            Log.Information($" StreamNodeEngine | {msg.ToString()}");
                            break;
                        }
                    case LogRedirector.LogRedirectorLevel.WARN:
                        {
                            Log.Warning($" StreamNodeEngine | {msg.ToString()}");
                            break;
                        }
                    case LogRedirector.LogRedirectorLevel.ERROR:
                        {
                            Log.Error($" StreamNodeEngine | {msg.ToString()}");
                            break;
                        }
                    case LogRedirector.LogRedirectorLevel.DEBUG:
                        {
                            Log.Debug($" StreamNodeEngine | {msg.ToString()}");
                            break;
                        }
                }
            };
        }
    }
}