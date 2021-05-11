using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using StreamNodeEngine.Engine;
using StreamNodeEngine.Objects;

namespace StreamNodeEngine.Engine
{
    public class RemoteControlService
    {
        private IRemoteControlEngine engine;
        private Dictionary<string, Func<RemoteControlData, RemoteControlData>> routes = new Dictionary<string, Func<RemoteControlData, RemoteControlData>>();

        public RemoteControlService() {
            engine = new FleckEngine();
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
        }

        private string MessageHandler(object sender, RemoteControlOnMessageArgs message)
        {
            RemoteControlData data = JsonConvert.DeserializeObject<RemoteControlData>(message.message);

            Func<RemoteControlData, RemoteControlData> route;

            if (routes.TryGetValue(data.type, out route))
            {
                data = route(data);
            }
            else
            {
                data = getUnknownCommand(data);
            }


            return data != null ? JsonConvert.SerializeObject(data) : null;
        }

        public void SendData(object data2Send)
        {
            engine.SendMessage(JsonConvert.SerializeObject(data2Send));
        }

        private RemoteControlData getUnknownCommand(RemoteControlData wsData)
        {
            wsData.status = "COMMAND_NOT_FOUND";
            return wsData;
        }
    }
}
