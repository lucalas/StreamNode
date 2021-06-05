using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Threading.Tasks;

namespace StreamNodeEngine.Engine.Services.Obs
{
    public class OBSService
    {

        public IObsSettings settings {get; set;}
        private OBSWebsocket obs;
        private int port = 4444;
        public String url { get { return $"ws://{settings.ObsIp}:{settings.ObsPort}"; } }
        private EventHandler _connected;

        private Task ReconnectObs;
        public int reconnectObsSchedule = 5000;
        public bool isConnected { get { return obs.IsConnected; } }

        public EventHandler Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                obs.Connected += _connected;
            }
        }

        public OBSService()
        {
            obs = new OBSWebsocket();
            obs.Connected += onConnect;
            obs.Disconnected += onDisconnect;
        }

        public void Connect()
        {
            try
            {
                obs.Connect(url, settings.ObsPassword);
                // AuthFailureException & ErrorResponseException possible exceptions
            }
            catch (Exception ex)
            {

            }
        }

        public void setCurrentScene(string sceneName)
        {
            obs.SetCurrentScene(sceneName);
        }

        public GetSceneListInfo getScenes()
        {
            return obs.GetSceneList();
        }

        private void onConnect(object sender, EventArgs e)
        {
            LogRedirector.info($"OBS WebSocket connected to {url}");
        }
        private void onDisconnect(object sender, EventArgs e)
        {
            LogRedirector.warn($"OBS WebSocket disconnected from {url}");
            if (ReconnectObs == null || ReconnectObs.IsCompleted)
            {
                LogRedirector.debug($"Starting scheduler to reconnect OBS and run every {reconnectObsSchedule}ms");
                ReconnectObs = Task.Run(ReconnectObsTask);
            }
        }
        private async Task ReconnectObsTask()
        {
            while (!obs.IsConnected)
            {
                LogRedirector.debug("Trying to connect to OBS");
                Connect();
                await Task.Delay(reconnectObsSchedule);
            }
            LogRedirector.info("OBS reconnected, ReconnectObsTask finished");
        }
    }
}
