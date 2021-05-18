using NAudio.CoreAudioApi;
using System.Collections.Generic;
using StreamNodeEngine.Utils;
using StreamNodeEngine.Objects;
using System.Threading.Tasks;
using System;

namespace StreamNodeEngine.Engine.Services
{
    public class AudioService
    {
        Task RefreshApplicationList;
        int RefreshApplicationListDelay = 3000;
        AudioServiceUpdate AudioServiceUpdate;
        public event EventHandler<AudioServiceUpdate> OnIOUpdate;

        MMDeviceEnumerator devicesController = new MMDeviceEnumerator();
        public AudioService()
        {
            AudioServiceUpdate = new AudioServiceUpdate();
            RefreshApplicationList = Task.Run(RefreshApplicationListTask);
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

        private async Task RefreshApplicationListTask()
        {
            while (true)
            {
                RemoteControlVolumes volumes = GetVolumeIO();

                string hashCalculated = GetStringSha256Hash(volumes);

                if (!hashCalculated.Equals(AudioServiceUpdate.hashUpdate))
                {
                    AudioServiceUpdate.volumes = volumes;
                    AudioServiceUpdate.tsUpdate = DateTime.Now.Millisecond;
                    AudioServiceUpdate.hashUpdate = hashCalculated;
                    OnIOUpdate(this, AudioServiceUpdate);
                }

                Task.Delay(RefreshApplicationListDelay);
            }
        }

        private RemoteControlVolumes GetVolumeIO()
        {
            RemoteControlVolumes volumes = new RemoteControlVolumes();

            foreach (MMDevice dev in GetListOfOutputDevices())
            {
                foreach (ApplicationController appOut in GetApplicationsMixer(dev))
                {
                    ApplicationController appDev = GetDeviceController(dev);
                    RemoteControlVolume audio = new RemoteControlVolume();
                    audio.name = appOut.processName;
                    audio.volume = (int)(appOut.getVolume() * 100);
                    audio.mute = appOut.getMute();
                    audio.device = appDev.device.FriendlyName;
                    audio.output = true;
                    audio.icon = ProcessUtils.ProcessIcon(appOut.session.GetProcessID);
                    volumes.Add(audio);
                }
            }

            foreach (MMDevice dev in GetListOfInputDevices())
            {
                RemoteControlVolume audio = new RemoteControlVolume();

                audio.name = dev.FriendlyName;
                audio.mute = dev.AudioEndpointVolume.Mute;
                audio.volume = (int)(dev.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                audio.device = dev.FriendlyName;
                audio.output = false;
                volumes.Add(audio);
            }

            return volumes;
        }
        private string GetStringSha256Hash(RemoteControlVolumes volumes)
        {
            string toCalculateHash = "";

            foreach (RemoteControlVolume volume in volumes)
            {
                toCalculateHash += volume.name;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(toCalculateHash);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
