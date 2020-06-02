using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct RobotDynamics : IComponentData
{
    public float MaxSpeed;
}
