using OBSWebsocketDotNet;
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

        public OBSService()
        {
            _obs = new OBSWebsocket();
        }

        public void Connect(String url, String password)
        {
            _url = url;
            _password = password;
            _obs.Connect(_url, _password);
            // AuthFailureException & ErrorResponseException possible exceptions
        }
    }
}
