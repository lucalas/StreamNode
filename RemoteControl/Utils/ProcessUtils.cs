using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Management;
using System.Linq;

namespace RemoteControl.Utils
{
    static class ProcessUtils
    {

        static public bool ProcessExists(uint processId)
        {
            try
            {
                var process = Process.GetProcessById((int)processId);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        static public string ProcessName(uint processId)
        {
            Process process = Process.GetProcessById((int)processId);
            return process.ProcessName;
        }

        static public string ProcessIcon(uint processId)
        {
            // TODO refactor code
            String blob = "";
            var query = "SELECT ProcessId, Name, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection results = searcher.Get())
            {
                var processes = results.Cast<ManagementObject>().Select(x => new
                {
                    ProcessId = (UInt32)x["ProcessId"],
                    Name = (string)x["Name"],
                    ExecutablePath = (string)x["ExecutablePath"]
                });
                foreach (var p in processes)
                {
                    if (File.Exists(p.ExecutablePath))
                    {
                        Icon icon = Icon.ExtractAssociatedIcon(p.ExecutablePath);
                        String key = p.ProcessId.ToString();
                        MemoryStream stream = new MemoryStream();
                        using (stream)
                        {
                            Bitmap bitmap = icon.ToBitmap();

                            if (bitmap != null)
                            {
                                bitmap.Save(stream, ImageFormat.Png);
                            }
                            //icon.Save(stream);
                        }

                        byte[] iconBytes = stream.ToArray();
                        blob = "data:image/png;base64, " + Convert.ToBase64String(iconBytes);
                    }
                }
            }
            return blob;
        }

        public static string getIcon(uint procid) {
            // TODO test old method just for fun
            string blob = "";
            Icon ico = Icon.ExtractAssociatedIcon(Process.GetProcessById((int)procid).MainModule.FileName);
            MemoryStream stream = new MemoryStream();
            using (stream)
            {
                /*if (bitmap != null)
                {
                    bitmap.Save(stream, ImageFormat.Jpeg);
                }*/
                ico.Save(stream);
            }

            byte[] iconBytes = stream.ToArray();
            blob = "data:image/jpeg;base64, " + Convert.ToBase64String(iconBytes);

            return blob;
        }

        public static Bitmap ExtractIcon(Icon icoIcon)
        {
            Bitmap bmpPngExtracted = null;
            try
            {
                byte[] srcBuf = null;
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                { icoIcon.Save(stream); srcBuf = stream.ToArray(); }
                const int SizeICONDIR = 6;
                const int SizeICONDIRENTRY = 16;
                int iCount = BitConverter.ToInt16(srcBuf, 4);
                for (int iIndex = 0; iIndex < iCount; iIndex++)
                {
                    int iWidth = srcBuf[SizeICONDIR + SizeICONDIRENTRY * iIndex];
                    int iHeight = srcBuf[SizeICONDIR + SizeICONDIRENTRY * iIndex + 1];
                    int iBitCount = BitConverter.ToInt16(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 6);
                    if (iWidth == 0 && iHeight == 0 && iBitCount == 32)
                    {
                        int iImageSize = BitConverter.ToInt32(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 8);
                        int iImageOffset = BitConverter.ToInt32(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 12);
                        System.IO.MemoryStream destStream = new System.IO.MemoryStream();
                        System.IO.BinaryWriter writer = new System.IO.BinaryWriter(destStream);
                        writer.Write(srcBuf, iImageOffset, iImageSize);
                        destStream.Seek(0, System.IO.SeekOrigin.Begin);
                        bmpPngExtracted = new Bitmap(destStream); // This is PNG! :)
                        break;
                    }
                }
            }
            catch { return null; }
            return bmpPngExtracted;
        }
    }
}
