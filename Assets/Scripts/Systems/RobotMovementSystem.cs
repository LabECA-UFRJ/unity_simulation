using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public class RobotMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Dependency = Entities.WithBurst().ForEach((ref PhysicsVelocity velocity, in PhysicsMass mass, in Rotation rotation, in WheelSpeed wheelSpeed, in RobotDimension dimension) => {
            var linear = (wheelSpeed.Left + wheelSpeed.Right) * dimension.WheelRadius / 2f;
            var angular = dimension.WheelRadius / dimension.BodyRadius * (wheelSpeed.Left - wheelSpeed.Right);

            velocity.Linear = math.rotate(rotation.Value, new float3(0f, 0f, linear));
            velocity.SetAngularVelocity(mass, rotation, new float3(0f, angular, 0f));
        }).ScheduleParallel(Dependency);
    }
}
