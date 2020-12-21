//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Collections.Generic;
using System.Text;
using RosMessageGeneration;

namespace RosMessageTypes.Simulation
{
    public class RobotWheelSpeed : Message
    {
        public const string RosMessageName = "simulation_msgs/RobotWheelSpeed";

        public int id;
        public float left;
        public float right;

        public RobotWheelSpeed()
        {
            this.id = 0;
            this.left = 0.0f;
            this.right = 0.0f;
        }

        public RobotWheelSpeed(int id, float left, float right)
        {
            this.id = id;
            this.left = left;
            this.right = right;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.Add(BitConverter.GetBytes(this.id));
            listOfSerializations.Add(BitConverter.GetBytes(this.left));
            listOfSerializations.Add(BitConverter.GetBytes(this.right));

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            this.id = BitConverter.ToInt32(data, offset);
            offset += 4;
            this.left = BitConverter.ToSingle(data, offset);
            offset += 4;
            this.right = BitConverter.ToSingle(data, offset);
            offset += 4;

            return offset;
        }

    }
}
