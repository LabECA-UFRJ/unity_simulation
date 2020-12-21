using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using RosMessageTypes.Simulation;

public class RobotWheelSystem : SystemBase
{
    private ROSConnection ros;
    private string topicName = "robot_cmd_vel";

    protected override void OnCreate()
    {
        ros = Object.FindObjectOfType<ROSConnection>();
        ros.Subscribe<RobotWheelSpeed>(topicName, UpdateWheelSpeed);
    }

    private void UpdateWheelSpeed(RobotWheelSpeed message)
    {
        Entities.WithoutBurst().ForEach((ref WheelSpeed wheelSpeed, in RobotId robotId) => {
            if (message.id == robotId.Value){
                wheelSpeed.Left = message.left;
                wheelSpeed.Right = message.right;
            }
        }).Run();
    }

    protected override void OnUpdate()
    {
    }
}