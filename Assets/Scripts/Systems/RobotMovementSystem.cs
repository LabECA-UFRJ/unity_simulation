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
        Dependency = Entities.WithBurst().ForEach((ref PhysicsVelocity velocity, in PhysicsMass mass, in Rotation rotation, in WheelSpeed wheelSpeed, in RobotDimension dimension, in RobotDynamics dynamics) => {
            var leftWheelSpeed = math.clamp(wheelSpeed.Left, -1f, 1f) * dynamics.MaxSpeed;
            var rightWheelSpeed = math.clamp(wheelSpeed.Right, -1f, 1f) * dynamics.MaxSpeed;

            var linear = (leftWheelSpeed + rightWheelSpeed) * dimension.WheelRadius / 2f;
            var angular = (dimension.WheelRadius / (2 * dimension.BodyRadius)) * (leftWheelSpeed - rightWheelSpeed);
            // angular formula is supposed to be (ws_l - ws_r) * r/2l

            velocity.Linear = math.rotate(rotation.Value, new float3(0f, 0f, linear));
            velocity.SetAngularVelocity(mass, rotation, new float3(0f, angular, 0f));
        }).ScheduleParallel(Dependency);
    }
}
