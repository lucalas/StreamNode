using Swan.Logging;

namespace StreamNode.Services.LogProxy
{
    public class EmbedIOLogProxy : ILogger
    {
        public LogLevel LogLevel { get; }
        public void Log(LogMessageReceivedEventArgs logEvent)
        {
            switch (logEvent.MessageType)
            {
                case LogLevel.Info:
                    {
                        Serilog.Log.Logger.Information($" EmbedIO | {logEvent.Message}");
                        break;
                    }
                case LogLevel.Warning:
                    {
                        Serilog.Log.Logger.Warning($" EmbedIO | {logEvent.Message}");
                        break;
                    }
                case LogLevel.Error:
                    {
                        Serilog.Log.Logger.Error($" EmbedIO | {logEvent.Message}");
                        break;
                    }
                case LogLevel.Fatal:
                    {
                        Serilog.Log.Logger.Fatal($" EmbedIO | {logEvent.Message}");
                        break;
                    }
                case LogLevel.Debug:
                    {
                        Serilog.Log.Logger.Debug($" EmbedIO | {logEvent.Message}");
                        break;
                    }
                case LogLevel.Trace:
                    {
                        Serilog.Log.Logger.Verbose($" EmbedIO | {logEvent.Message}");
                        break;
                    }
                case LogLevel.None:
                    {
                        Serilog.Log.Logger.Information($" EmbedIO | {logEvent.Message}");
                        break;
                    }
            }
        }

        public void Dispose()
        {

        }
    }
}