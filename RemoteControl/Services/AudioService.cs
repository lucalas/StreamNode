using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Text;
using RemoteControl.Utils;
using System.Windows.Input;
using RemoteControl.Objects;

namespace RemoteControl.Services
{
    class AudioService
    {
        MMDeviceEnumerator devicesController = new MMDeviceEnumerator();
        public AudioService()
        {
        }


        public MMDevice GetDefaultOutputDevice()
        {
            MMDevice device = devicesController.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return device;
        }

        public MMDeviceCollection GetListOfOutputDevices()
        {
            MMDeviceCollection deviceList = devicesController.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            return deviceList;
        }

        public List<ApplicationController> GetApplicationsMixer(MMDevice device)
        {
            List<ApplicationController> appsList = new List<ApplicationController>();
            SessionCollection sessions = device.AudioSessionManager.Sessions;

            for (int i = 0; i < sessions.Count; i++)
            {
                AudioSessionControl session = sessions[i];
                //if (!session.IsSystemSoundsSession && ProcessUtils.ProcessExists(session.GetProcessID))
                if (ProcessUtils.ProcessExists(session.GetProcessID))
                {
                    ApplicationController ac = new ApplicationController(device, session, ProcessUtils.ProcessName(session.GetProcessID));
                    appsList.Add(ac);
                }
            }

            return appsList;
        }

    }
}
