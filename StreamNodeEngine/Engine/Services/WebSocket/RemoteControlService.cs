using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using StreamNodeEngine.Objects;
using Config.Net;

namespace StreamNodeEngine.Engine.Services.WebSocket
{
    public class RemoteControlService
    {
        public IWebSocketSettings settings
        {
            get => _settings;
            set { _settings = value; engine.settings = value; }
        }

        private IWebSocketSettings _settings;
        private IRemoteControlEngine engine = new FleckEngine();
        private Dictionary<string, Func<RemoteControlData, RemoteControlData>> routes = new Dictionary<string, Func<RemoteControlData, RemoteControlData>>();

        public RemoteControlService()
        {
            settings = new ConfigurationBuilder<IWebSocketSettings>().Build();
            engine.OnMessage += MessageHandler;
        }

        public void Connect()
        {
            engine.Connect();
        }

        public void Disconnect()
        {
            engine.Disconnect();
        }


        public void AddRoute(string key, Func<RemoteControlData, RemoteControlData> route)
        {
            routes.Add(key, route);
            LogRedirector.info($"WebSocket Route added [{key}]");
        }

        private string MessageHandler(object sender, RemoteControlOnMessageArgs message)
        {
            RemoteControlData data = JsonConvert.DeserializeObject<RemoteControlData>(message.message);

            Func<RemoteControlData, RemoteControlData> route;

            if (routes.TryGetValue(data.type, out route))
            {
                LogRedirector.debug($"Received command to execute [{data.type}]");
                data = route(data);
            }
            else
            {
                LogRedirector.error($"Unknown command received [{data.type}]");
                data = getUnknownCommand(data);
            }


            return data != null ? JsonConvert.SerializeObject(data) : null;
        }

        public void SendData(object data2Send)
        {
            try
            {
                engine.SendMessage(JsonConvert.SerializeObject(data2Send));
            }
            catch (Exception ex)
            {
                LogRedirector.error($"Something wrong happened during execution of send data to web socket, exception: [{ex}]");
            }
        }

        private RemoteControlData getUnknownCommand(RemoteControlData wsData)
        {
            wsData.status = "COMMAND_NOT_FOUND";
            return wsData;
        }
    }
}
