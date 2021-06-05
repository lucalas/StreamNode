using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Context;
using StreamNode.Services.LogProxy;
using StreamNodeEngine.Engine.Services;

namespace StreamNode.Services
{
    public class LoggerManager
    {
        private static String logTemplate = "{Timestamp:dd-MM-yyyy HH:mm:ss} | {Level,-11} | {Proxy,-16} | {Message}{NewLine}{Exception}";

        ///
        /// File Size Limit of 20MB
        ///
        private static int fileSizeLimit = 20971520;

        public static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: logTemplate)
                .WriteTo.File("log/StreamNode.log", rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLimit, outputTemplate: logTemplate)
                .MinimumLevel.Debug()
                .CreateLogger();

            // Register proxy to put swan logging into serilog
            Swan.Logging.Logger.RegisterLogger(new EmbedIOLogProxy());

            // Register proxy to put StreamNodeEngine logging into serilog
            LogRedirector.OnLog += (msg, level) =>
            {
                using (LogContext.PushProperty("Proxy", "StreamNodeEngine"))
                {
                    switch (level)
                    {
                        case LogRedirector.LogRedirectorLevel.INFO:
                            {
                                Log.Information(msg.ToString());
                                break;
                            }
                        case LogRedirector.LogRedirectorLevel.WARN:
                            {
                                Log.Warning(msg.ToString());
                                break;
                            }
                        case LogRedirector.LogRedirectorLevel.ERROR:
                            {
                                Log.Error(msg.ToString());
                                break;
                            }
                        case LogRedirector.LogRedirectorLevel.DEBUG:
                            {
                                Log.Debug(msg.ToString());
                                break;
                            }
                    }
                }
            };
        }
    }
}
