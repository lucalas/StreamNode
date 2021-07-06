using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace StreamNode.Services.OBSPlugin
{
    public class OBSPluginManager
    {
        public string obsInstallPath { get; set; } = "C:\\Program Files\\OBS-Studio\\";
        public static string obsPluginPath = "obs-plugins\\64bit\\";
        public static string tmpFolder = "tmp\\";
        public static string obsWebsocketPlugin = "obs-websocket.dll";

        public event EventHandler<OBSPluginEvent> onOBSInstallEvent;


        public bool CheckObsExistence()
        {
            return Directory.Exists(obsInstallPath);
        }
        public bool CheckPluginExistence()
        {
            return CheckObsExistence() && File.Exists(obsInstallPath + obsPluginPath + obsWebsocketPlugin);
        }

        public async void InstallAsync()
        {
            await Task.Run(() =>
            {
                if (CheckObsExistence() && !CheckPluginExistence())
                {
                    // Create tmp dir
                    onOBSInstallEvent?.Invoke(this, OBSPluginEvent.GenerateEvent("Creating temporary directory"));
                    Directory.CreateDirectory(tmpFolder);
                    onOBSInstallEvent?.Invoke(this, OBSPluginEvent.GenerateEvent("Temporary directory created"));

                    // Extract obs plugin
                    onOBSInstallEvent?.Invoke(this, OBSPluginEvent.GenerateEvent("Extracting obs websocket plugin zip"));
                    ExtractZip();
                    onOBSInstallEvent?.Invoke(this, OBSPluginEvent.GenerateEvent("Obs websocket plugin extracted"));

                    // Copy file into obs folder
                    onOBSInstallEvent?.Invoke(this, OBSPluginEvent.GenerateEvent("Installing obs websocket plugin"));
                    DirectoryCopy($"{tmpFolder}obs-websocket-4.9.1-Windows\\", obsInstallPath, true, true);
                    onOBSInstallEvent?.Invoke(this, OBSPluginEvent.GenerateEvent("Obs websocket plugin installed", true));
                }
            });
        }

        private void ExtractZip()
        {
            try
            {
                //write the resource zip file to the temp directory
                using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("StreamNode.Resources.obs-websocket-4.9.1-Windows.zip"))
                {
                    using (FileStream bw = new FileStream($"{tmpFolder}obs-websocket-4.9.1-Windows.zip", FileMode.Create))
                    {
                        stream.CopyTo(bw);
                    }
                }

                // Unzip
                ZipFile.ExtractToDirectory($"{tmpFolder}obs-websocket-4.9.1-Windows.zip", $"{tmpFolder}obs-websocket-4.9.1-Windows", true);

            }
            catch (Exception e)
            {
                //handle the error
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs, overwrite);
                }
            }
        }


    }
}