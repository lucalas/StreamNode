using NAudio.CoreAudioApi;
using OBSWebsocketDotNet.Types;
using RemoteControl.Objects;
using RemoteControl.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace RemoteControl.Services
{
    class RemoteControlService
    {
        private IRemoteControlEngine engine;
        public event EventHandler<RemoteControlOnMessageArgs> OnMessage;
        private AudioService ac;
        private OBSService os;

        public RemoteControlService(AudioService ac, OBSService os) {
            this.ac = ac;
            this.os = os;
            engine = new FleckEngine();
        }

        public void connect()
        {
            engine.OnMessage += MessageHandler;
            engine.Connect();
            os.Connect("ws://localhost:4444", "");
        }

        public string MessageHandler(object sender, RemoteControlOnMessageArgs message)
        {

            RemoteControlData data = JsonSerializer.Deserialize<RemoteControlData>(message.message);

            IData dataResponse = null;

            // TODO create method for RemoteControlDataType logics
            if (RemoteControlDataType.Volumes.Equals(data.type))
            {
                RemoteControlVolumes volumes = new RemoteControlVolumes();

                foreach (MMDevice dev in ac.GetListOfOutputDevices())
                {
                    foreach (ApplicationController appOut in ac.GetApplicationsMixer(dev))
                    {
                        ApplicationController appDev = ac.GetDeviceController(dev);
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

                foreach (MMDevice dev in ac.GetListOfInputDevices())
                {
                    // FIXME retrieve devices instead of application mixer because we can't control microphone of an application
                    //foreach (ApplicationController appIn in ac.GetApplicationsMixer(dev))
                    //{
                    RemoteControlVolume audio = new RemoteControlVolume();

                    audio.name = dev.FriendlyName;
                    audio.mute = dev.AudioEndpointVolume.Mute;
                    audio.volume = (int)(dev.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                    audio.device = dev.FriendlyName;
                    audio.output = false;
                    volumes.Add(audio);
                    //}
                }

                // TODO add input devices
                dataResponse = volumes;
            } else if (RemoteControlDataType.ChangeVolume.Equals(data.type)) {
                // TODO Verify input data to avoid exception when unknown data is passed
                ChangeVolumeType ChangeVolume = JsonSerializer.Deserialize<ChangeVolumeType>(data.data.ToString());
                if (ChangeVolume.output)
                {
                    ApplicationController app = ac.GetApplicationOutput(ChangeVolume.name, ChangeVolume.device);
                    app.updateVolume((float)ChangeVolume.volume / 100);
                    app.setMute(ChangeVolume.mute);
                } else
                {
                    // TODO change mic volume
                    MMDevice mic = ac.GetDeviceInput(ChangeVolume.device);
                    mic.AudioEndpointVolume.MasterVolumeLevelScalar = (float)ChangeVolume.volume / 100;
                    mic.AudioEndpointVolume.Mute = ChangeVolume.mute;
                }
                Trace.WriteLine(ChangeVolume.name + ": " + ChangeVolume.volume);
            } else if (RemoteControlDataType.Obs.Equals(data.type)) {
                RemoteObsScenes remoteScenes = new RemoteObsScenes();
                GetSceneListInfo scenes = os.getScenes();
                foreach (OBSScene scene in scenes.Scenes) {
                    RemoteObsScene remoteScene = new RemoteObsScene();
                    remoteScene.name = scene.Name;
                    remoteScenes.Add(remoteScene);
                }
                dataResponse = remoteScenes;
            } else if(RemoteControlDataType.ChangeObs.Equals(data.type))
            {
                RemoteObsScene ObsScene = JsonSerializer.Deserialize<RemoteObsScene>(data.data.ToString());
                os.setCurrentScene(ObsScene.name);
            } else
            {
                data.status = "Command not found";
            }

            data.data = dataResponse;
            return JsonSerializer.Serialize(data);
        }
    }
}
