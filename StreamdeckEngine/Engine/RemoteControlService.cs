using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using StreamdeckEngine.Engine;
using StreamdeckEngine.Objects;

namespace StreamdeckEngine.Engine
{
    class RemoteControlService
    {
        private IRemoteControlEngine engine;
        public event EventHandler<RemoteControlOnMessageArgs> OnMessage;
        private Dictionary<string, Func<RemoteControlData, RemoteControlData>> routes = new Dictionary<string, Func<RemoteControlData, RemoteControlData>>();

        public RemoteControlService() {
            engine = new FleckEngine();
        }

        public void Connect()
        {
            engine.OnMessage += MessageHandler;
            engine.Connect();
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


            return JsonConvert.SerializeObject(data);
        }

        private RemoteControlData getUnknownCommand(RemoteControlData wsData)
        {
            wsData.status = "COMMAND_NOT_FOUND";
            return wsData;
        }
    }
}
