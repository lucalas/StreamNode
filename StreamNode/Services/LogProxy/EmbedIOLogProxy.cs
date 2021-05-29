using Serilog.Context;
using Swan.Logging;

namespace StreamNode.Services.LogProxy
{
    public class EmbedIOLogProxy : ILogger
    {
        public LogLevel LogLevel { get; }
        public void Log(LogMessageReceivedEventArgs logEvent)
        {
            using (LogContext.PushProperty("Proxy", "EmbedIO"))
            {
                switch (logEvent.MessageType)
                {
                    case LogLevel.Info:
                        {
                            Serilog.Log.Logger.Information(logEvent.Message);
                            break;
                        }
                    case LogLevel.Warning:
                        {
                            Serilog.Log.Logger.Warning(logEvent.Message);
                            break;
                        }
                    case LogLevel.Error:
                        {
                            Serilog.Log.Logger.Error(logEvent.Message);
                            break;
                        }
                    case LogLevel.Fatal:
                        {
                            Serilog.Log.Logger.Fatal(logEvent.Message);
                            break;
                        }
                    case LogLevel.Debug:
                        {
                            Serilog.Log.Logger.Debug(logEvent.Message);
                            break;
                        }
                    case LogLevel.Trace:
                        {
                            Serilog.Log.Logger.Verbose(logEvent.Message);
                            break;
                        }
                    case LogLevel.None:
                        {
                            Serilog.Log.Logger.Information(logEvent.Message);
                            break;
                        }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}