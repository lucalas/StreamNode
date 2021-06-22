using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StreamNode.Services.OBSPlugin
{
    public class OBSPluginManager
    {
        // public static string obsInstallPath = "C:\\Program Files\\OBS-Studio\\";
        public static string obsInstallPath = "D:\\GDrive\\Dati\\Programmi\\OBS-Studio-Test";
        public static string obsPluginPath = "obs-plugins\\64bit\\";
        public static string obsWebsocketPlugin = "obs-websocket.dll";

        public static string obsWebsocketUrl = "https://github.com/Palakis/obs-websocket/releases/download/4.9.1/obs-websocket-4.9.1-Windows.zip";

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
            if (true || CheckObsExistence() && !CheckPluginExistence())
            {
                using (var client = new WebClient())
                {
                    Directory.CreateDirectory("tmp");

                    client.DownloadProgressChanged += DownloadProgressChanged;
                    client.DownloadFileAsync(new System.Uri(obsWebsocketUrl), "tmp\\obs-websocket-4.9.1-Windows.zip");
                    client.DownloadFileCompleted += DownloadFileCompleted;
                }
            }
        }

        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
        }

        void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Install();
        }

        private void Install()
        {
            // Unzip
            ZipFile.ExtractToDirectory("tmp\\obs-websocket-4.9.1-Windows.zip", "tmp\\obs-websocket-4.9.1-Windows", true);
            // Copy file into obs folder
            DirectoryCopy("tmp\\obs-websocket-4.9.1-Windows\\", "D:\\GDrive\\Dati\\Programmi\\OBS-Studio-Test", true, true);
            int a = 0;
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