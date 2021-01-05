using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class RobotMessageSystem : SystemBase
{
    private ROSConnection ros;
    private string topicName = "robots";
    private string referenceFrame = "global";

    private float _nextUpdate = 0f;
    private const float _updateTime = 1/60f;

    protected override void OnCreate()
    {
        ros = Object.FindObjectOfType<ROSConnection>();
    }

    protected override void OnUpdate()
    {

        if (_nextUpdate > 0) {
            _nextUpdate -= Time.DeltaTime;
            return;
        }

        _nextUpdate = _updateTime;

        EntityQuery query = GetEntityQuery(ComponentType.ReadOnly<RobotId>());

        int size = query.CalculateEntityCount();
        int arrayId = 0;

        var robotsInformation = new RosMessageTypes.Simulation.PoseRobot[size];

        Entities.WithoutBurst().ForEach((in RobotId robotId, in Rotation rotation, in Translation translation) => {
            var robotPos = new RosMessageTypes.Geometry.Point(translation.Value.x, translation.Value.y, translation.Value.z);
            var robotRot = new RosMessageTypes.Geometry.Quaternion(rotation.Value.value.x, rotation.Value.value.y, rotation.Value.value.z, rotation.Value.value.w);

            var robotInfo = new RosMessageTypes.Simulation.PoseRobot(
                robotId.Value,
                new RosMessageTypes.Geometry.Pose(robotPos, robotRot)
            );

            if (arrayId < size && arrayId >= 0){
                robotsInformation[arrayId] = robotInfo;
                arrayId++;
            }
        }).Run();

        var msgHeader = new RosMessageTypes.Std.Header();
        msgHeader.stamp = new RosMessageTypes.Std.Time((uint)System.DateTime.Now.Second, (uint)System.DateTime.Now.Millisecond);
        msgHeader.frame_id = referenceFrame;

        var message = new RosMessageTypes.Simulation.PoseRobotArray(msgHeader, robotsInformation);
  
        ros.Send(topicName, message);
    }
}
