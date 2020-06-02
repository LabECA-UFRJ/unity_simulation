using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct RobotDimension : IComponentData
{
    public float BodyRadius;
    public float WheelRadius;
    public float Height;
}
