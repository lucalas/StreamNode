using System.ComponentModel;
namespace StreamNodeEngine.Engine.Services.Obs
{
    public interface IObsSettings
    {

        [DefaultValue("127.0.0.1")]
        string ObsIp { get; set; }

        [DefaultValue(4444)]
        int ObsPort { get; set; }

        [DefaultValue("")]
        string ObsPassword { get; set; }
    }
}