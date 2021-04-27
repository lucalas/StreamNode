using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNodeEngine.Engine.Services
{
    public class OBSService
    {
        private static string IP = "127.0.0.1";
        private static int PORT = 4444;
        private static string PASSWORD = "";

        private OBSWebsocket _obs;
        private String _url;
        private String _password;
        private EventHandler _connected;
        public bool isConnected { get { return _obs.IsConnected; } }

        public EventHandler Connected {
            get { return _connected; }
            set {
                _connected = value;
                _obs.Connected += _connected;
            }
        }

        public OBSService()
        {
            _obs = new OBSWebsocket();
            // Default configuration
            Configure(IP, PORT, PASSWORD);
        }

        public void Configure(string ip, int port, string pwd) {
            _url = "ws://" + ip + ":" + port;
            _password = pwd;
        }

        public void Connect()
        {
            try
            {
                _obs.Connect(_url, _password);
                // AuthFailureException & ErrorResponseException possible exceptions
            } catch (Exception ex) {

            }
        }

        public void setCurrentScene(string sceneName)
        {
            _obs.SetCurrentScene(sceneName);
        }

        public GetSceneListInfo getScenes()
        {
            return _obs.GetSceneList();
        }
    }
}
