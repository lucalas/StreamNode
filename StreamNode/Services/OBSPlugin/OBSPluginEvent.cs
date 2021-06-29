namespace StreamNode.Services.OBSPlugin
{
        public class OBSPluginEvent
        {
            public static OBSPluginEvent GenerateEvent(string eventObs, bool finishedInstall = false)
            {
                return new OBSPluginEvent { obsEvent = eventObs, finished = finishedInstall };
            }
            public string obsEvent { get; set; }
            public bool finished {get;set;}
        }
}