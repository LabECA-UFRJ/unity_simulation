using UnityEngine;

namespace Ros
{
    public class PresencePublisher : MonoBehaviour
    {
        public ROSConnection ros;
        public string topicName = "presence";
        
        void Start()
        {
            ros.Send(topicName, new RosMessageTypes.Std.Int32(1));
        }
    }
}