using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct WheelSpeed : IComponentData
{
    public float Left;
    public float Right;
}
