using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

using ROSGeometry;
using System.Diagnostics;

public class RobotMessageSystem : SystemBase
{
    private ROSConnection ros;
    private string topicName = "robots";
    private string referenceFrame = "global";

    private float _nextUpdate = 0f;
    private const float _updateTime = 1 / 120f;

    private Stopwatch _stopWatch;

    protected override void OnCreate()
    {
        ros = Object.FindObjectOfType<ROSConnection>();

        _stopWatch = new Stopwatch();
        _stopWatch.Start();
    }

    protected override void OnUpdate()
    {
        _nextUpdate -= Time.DeltaTime;
        if (_nextUpdate > Time.DeltaTime / 2) {
            return;
        }

        _nextUpdate = _updateTime + _nextUpdate;

        EntityQuery query = GetEntityQuery(ComponentType.ReadOnly<RobotId>());

        int size = query.CalculateEntityCount();
        int arrayId = 0;

        var robotsInformation = new RosMessageTypes.Simulation.PoseRobot[size];

        Entities.WithoutBurst().ForEach((in RobotId robotId, in Rotation rotation, in Translation translation) => {
            var pos = ((Vector3)translation.Value).To<FLU>();
            var rot = ((Quaternion)rotation.Value).To<FLU>();

            var robotPos = new RosMessageTypes.Geometry.Point(pos.x, pos.y, pos.z);
            var robotRot = new RosMessageTypes.Geometry.Quaternion(rot.x, rot.y, rot.z, rot.w);

            var robotInfo = new RosMessageTypes.Simulation.PoseRobot(
                robotId.Value,
                new RosMessageTypes.Geometry.Pose(robotPos, robotRot)
            );

            if (arrayId < size && arrayId >= 0) {
                robotsInformation[arrayId] = robotInfo;
                arrayId++;
            }
        }).Run();

        var msgHeader = new RosMessageTypes.Std.Header();
        msgHeader.stamp = new RosMessageTypes.Std.Time(_stopWatch.Seconds(), _stopWatch.Nanoseconds());
        msgHeader.frame_id = referenceFrame;

        var message = new RosMessageTypes.Simulation.PoseRobotArray(msgHeader, robotsInformation);

        ros.Send(topicName, message);
    }
}

public static class StopwatchExtensions
{
    public static uint Seconds(this Stopwatch stopwatch)
    {
        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        return (uint)(elapsedMilliseconds / 1000);
    }

    public static uint Nanoseconds(this Stopwatch stopwatch)
    {
        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        long millisecondsAboveSecond = elapsedMilliseconds - (elapsedMilliseconds / 1000);
        return (uint)(millisecondsAboveSecond * 1_000_000);
    }
}