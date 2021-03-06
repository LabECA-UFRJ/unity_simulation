//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Collections.Generic;
using System.Text;
using RosMessageGeneration;

namespace RosMessageTypes.Simulation
{
    public class Robot : Message
    {
        public const string RosMessageName = "simulation_msgs/Robot";

        public int robot_id;
        public Geometry.Vector3 position;
        public float rotation;

        public Robot()
        {
            this.robot_id = 0;
            this.position = new Geometry.Vector3();
            this.rotation = 0.0f;
        }

        public Robot(int robot_id, Geometry.Vector3 position, float rotation)
        {
            this.robot_id = robot_id;
            this.position = position;
            this.rotation = rotation;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.Add(BitConverter.GetBytes(this.robot_id));
            listOfSerializations.AddRange(position.SerializationStatements());
            listOfSerializations.Add(BitConverter.GetBytes(this.rotation));

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            this.robot_id = BitConverter.ToInt32(data, offset);
            offset += 4;
            offset = this.position.Deserialize(data, offset);
            this.rotation = BitConverter.ToSingle(data, offset);
            offset += 4;

            return offset;
        }

    }
}
