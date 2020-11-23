using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    class OBSService
    {
        private OBSWebsocket _obs;
        private String _url;
        private String _password;
        private EventHandler _connected;
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
        }

        public void Connect(String url, String password)
        {
            try
            {
                _url = url;
                _password = password;
                _obs.Connect(_url, _password);
                // AuthFailureException & ErrorResponseException possible exceptions
            } catch (Exception ex) {

            }
        }

        public GetSceneListInfo getScenes()
        {
            return _obs.GetSceneList();
        }

        public void SetScene(string name) { 
        
        }
    }
}
