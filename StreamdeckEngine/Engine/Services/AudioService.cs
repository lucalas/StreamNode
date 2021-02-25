using NAudio.CoreAudioApi;
using System.Collections.Generic;
using StreamdeckEngine.Utils;
using StreamdeckEngine.Objects;

namespace StreamdeckSocketManager.Engine.Services
{
    public class AudioService
    {
        MMDeviceEnumerator devicesController = new MMDeviceEnumerator();
        public AudioService()
        {
        }

        /// <summary>
        /// Return output devices.
        /// </summary>
        /// <returns></returns>
        public MMDevice GetDefaultOutputDevice()
        {
            MMDevice device = devicesController.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            return device;
        }

        /// <summary>
        /// Return default input devices.
        /// </summary>
        /// <returns></returns>
        public MMDevice GetDefaultInputDevice()
        {
            MMDevice device = devicesController.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
            return device;
        }

        /// <summary>
        /// Return output devices.
        /// </summary>
        /// <returns></returns>
        public MMDeviceCollection GetListOfOutputDevices()
        {
            MMDeviceCollection deviceList = devicesController.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            return deviceList;
        }
        /// <summary>
        /// Return input devices.
        /// </summary>
        /// <returns></returns>
        public MMDeviceCollection GetListOfInputDevices()
        {
            MMDeviceCollection deviceList = devicesController.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            return deviceList;
        }

        /// <summary>
        /// Return if device is the default device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool DeviceIsDefault(MMDevice device)
        {
            return GetDefaultOutputDevice().ID.Equals(device.ID);
        }

        public ApplicationController GetDeviceController(MMDevice device)
        {
            return new ApplicationController(device, device.AudioSessionManager.AudioSessionControl, device.FriendlyName);
        }
        /// <summary>
        /// Return Mixers List of every single application.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public List<ApplicationController> GetApplicationsMixer(MMDevice device)
        {
            List<ApplicationController> appsList = new List<ApplicationController>();
            SessionCollection sessions = device.AudioSessionManager.Sessions;

            for (int i = 0; i < sessions.Count; i++)
            {
                AudioSessionControl session = sessions[i];
                if (!session.IsSystemSoundsSession && ProcessUtils.ProcessExists(session.GetProcessID))
                //if (ProcessUtils.ProcessExists(session.GetProcessID))
                {
                    ApplicationController ac = new ApplicationController(device, session, ProcessUtils.ProcessName(session.GetProcessID));
                    appsList.Add(ac);
                }
            }
            
            return appsList;
        }

        public ApplicationController GetApplicationOutput(string application, string device)
        {
            ApplicationController foundApp = null;
            foreach (MMDevice dev in GetListOfOutputDevices())
            {
                if (dev.FriendlyName.Equals(device))
                {
                    foundApp = GetApplicationsMixer(dev).Find(app => { return app.processName.Equals(application); });
                }
            }

            return foundApp;
        }

        public MMDevice GetDeviceInput(string device)
        {
            MMDevice foundDevice = null;
            // TODO change foreach into loop with an exit condition
            foreach (MMDevice dev in GetListOfInputDevices())
            {
                if (dev.FriendlyName.Equals(device))
                {
                    foundDevice = dev;//GetApplicationsMixer(dev).Find(app => { return app.processName.Equals(application); });
                }
            }

            return foundDevice;
        }
    }
}
