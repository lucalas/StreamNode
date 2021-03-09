using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using OBSWebsocketDotNet.Types;
using StreamNodeEngine.Engine;
using StreamNodeEngine.Objects;
using StreamNodeEngine.Utils;
using StreamNodeSocketManager.Engine.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StreamNodeEngine
{
    public class StreamNodeSocketManager
    {

        public OBSService obsService { get; } = new OBSService();
        public AudioService audioService { get; } = new AudioService();
        RemoteControlService webSocketEngine = new RemoteControlService();

        public StreamNodeSocketManager()
        {
            initRoutes();
            webSocketEngine.Connect();
            obsService.Connect("ws://localhost:4444", "");
        }

        private void initRoutes()
        {
            audioService.OnIOUpdate += IOUpdateHandler;

            webSocketEngine.AddRoute(RemoteControlDataType.Volumes, GetVolumes);
            webSocketEngine.AddRoute(RemoteControlDataType.ChangeVolume, ChangeVolume);
            webSocketEngine.AddRoute(RemoteControlDataType.Obs, GetObs);
            webSocketEngine.AddRoute(RemoteControlDataType.ChangeObs, ChangeObs);

        }

        private void IOUpdateHandler(object sender, AudioServiceUpdate message)
        {
            RemoteControlData WSData = new RemoteControlData();
            WSData.data = message.volumes;

            // TODO Send volumes update to client
            webSocketEngine.SendData(WSData);
        }

        private RemoteControlData GetVolumes(RemoteControlData wsData)
        {
            RemoteControlVolumes volumes = new RemoteControlVolumes();

            foreach (MMDevice dev in audioService.GetListOfOutputDevices())
            {
                foreach (ApplicationController appOut in audioService.GetApplicationsMixer(dev))
                {
                    ApplicationController appDev = audioService.GetDeviceController(dev);
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

            foreach (MMDevice dev in audioService.GetListOfInputDevices())
            {
                RemoteControlVolume audio = new RemoteControlVolume();

                audio.name = dev.FriendlyName;
                audio.mute = dev.AudioEndpointVolume.Mute;
                audio.volume = (int)(dev.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                audio.device = dev.FriendlyName;
                audio.output = false;
                volumes.Add(audio);
            }

            wsData.data = volumes;

            return wsData;
        }

        private RemoteControlData ChangeVolume(RemoteControlData wsData)
        {
            // TODO Verify input data to avoid exception when unknown data is passed
            ChangeVolumeType ChangeVolume = JsonConvert.DeserializeObject<ChangeVolumeType>(wsData.data.ToString());
            if (ChangeVolume.output)
            {
                ApplicationController app = audioService.GetApplicationOutput(ChangeVolume.name, ChangeVolume.device);
                app.updateVolume((float)ChangeVolume.volume / 100);
                app.setMute(ChangeVolume.mute);
            }
            else
            {
                MMDevice mic = audioService.GetDeviceInput(ChangeVolume.device);
                mic.AudioEndpointVolume.MasterVolumeLevelScalar = (float)ChangeVolume.volume / 100;
                mic.AudioEndpointVolume.Mute = ChangeVolume.mute;
            }

            return wsData;
        }

        private RemoteControlData GetObs(RemoteControlData wsData)
        {
            RemoteObsScenes remoteScenes = new RemoteObsScenes();
            if (obsService.isConnected)
            {
                GetSceneListInfo scenes = obsService.getScenes();
                foreach (OBSScene scene in scenes.Scenes)
                {
                    RemoteObsScene remoteScene = new RemoteObsScene();
                    remoteScene.name = scene.Name;
                    remoteScenes.Add(remoteScene);
                }
            }
            else
            {
                wsData.status = "OBS_WEBSOCKET_ERROR";
            }

            wsData.data = remoteScenes;
            return wsData;
        }

        private RemoteControlData ChangeObs(RemoteControlData wsData)
        {
            if (obsService.isConnected)
            {
                RemoteObsScene ObsScene = JsonConvert.DeserializeObject<RemoteObsScene>(wsData.data.ToString());
                obsService.setCurrentScene(ObsScene.name);
            }
            return wsData;
        }
    }
}
