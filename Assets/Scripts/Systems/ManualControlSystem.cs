using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(RobotMovementSystem))]
public class ManualControlSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Entities.WithAll<ManualControlTag>().ForEach((ref WheelSpeed wheelSpeed) => {
            float2 linear = new float2(vertical);
            float2 angular = new float2(horizontal, -horizontal);

            float2 combined = linear + angular;
            var max = math.max(combined.x, combined.y);

            combined = math.select(combined, combined / max, max > 1);

            wheelSpeed.Left = combined.x;
            wheelSpeed.Right = combined.y;
        }).Schedule();
    }
}
