using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BurningBlueFox.Heated.Gameplay
{
    [BurstCompile]
    public struct CalculateProjectileTrajectoryJob : IJobParallelFor
    {
        //The world rotation of where the gun barrel points
        [ReadOnly] public NativeArray<quaternion> muzzleRotation;
        //Float value representing accuracy, bigger values will result in less precise shot
        [ReadOnly] public NativeArray<float> projectileDispersion;
        public NativeArray<Unity.Mathematics.Random> random;

        //Returns the direction and returns it so it only needs to multiply by float speed to get the desired effects
        public NativeArray<float3> projectileDirection;


        public void Execute(int index)
        {
            quaternion dispersion = muzzleRotation[index];
            Unity.Mathematics.Random r = random[index];

            float angle = r.NextFloat(-180f, 180f);
            float angleCos = projectileDispersion[index] * math.cos(math.radians(angle));
            float angleSin = projectileDispersion[index] * math.sin(math.radians(angle));


            float3 vectorDispersion = new float3(angleCos, angleSin, 0f);
            projectileDirection[index] = math.normalize(math.forward(dispersion)) + vectorDispersion;
        }
    }
}