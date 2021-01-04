using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
    }
}
