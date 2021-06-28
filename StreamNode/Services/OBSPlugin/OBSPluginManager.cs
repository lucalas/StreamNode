using System;
using System.IO;
using System.IO.Compression;

namespace StreamNode.Services.OBSPlugin
{
    public class OBSPluginManager
    {
        // public static string obsInstallPath = "C:\\Program Files\\OBS-Studio\\";
        public static string obsInstallPath = "D:\\GDrive\\Dati\\Programmi\\OBS-Studio-Test";
        public static string obsPluginPath = "obs-plugins\\64bit\\";
        public static string tmpFolder = "tmp\\";
        public static string obsWebsocketPlugin = "obs-websocket.dll";


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
                // Create tmp dir
                Directory.CreateDirectory(tmpFolder);
                // Extract obs plugin
                ExtractZip();
                // Copy file into obs folder
                DirectoryCopy($"{tmpFolder}obs-websocket-4.9.1-Windows\\", obsInstallPath, true, true);
            }
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