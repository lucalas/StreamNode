using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    class RemoteControlDataType
    {
        static public string Volumes { get; set; } = "volumes";
        static public string Obs { get; set; } = "obs";
        static public string ChangeVolume { get; set; } = "change-volume";
        static public string ChangeObs { get; set; } = "change-obs";
    }
}
