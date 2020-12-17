using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

namespace Ros{
    public class RobotSpawner : MonoBehaviour
    {
        public ROSConnection ros;
        public string topicName = "robot_def";
        public GameObject robotPrefab;

        void Start()
        {
            ros.Subscribe<RosMessageTypes.Simulation.Robot>(topicName, SpawnRobot);
        }

        void SpawnRobot(RosMessageTypes.Simulation.Robot robotMessage)
        {
            Vector3 position = new Vector3((float)robotMessage.position.x, (float)robotMessage.position.y, (float)robotMessage.position.z);

            GameObject robot = Instantiate(robotPrefab, position, Quaternion.AngleAxis(robotMessage.rotation, Vector3.up));
            robot.GetComponent<RobotIdAuthoring>().id = robotMessage.robot_id;
        }
    }
}
