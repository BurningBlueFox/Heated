using Unity.Burst;
using Unity.Mathematics;
using UnityEngine.Jobs;

namespace BurningBlueFox.Heated.AI
{
    [BurstCompile]
    public struct FbiFsmJob : IJobParallelForTransform
    {
        public float3 playerPos;
        public float deltaTime;


        public void Execute(int index, TransformAccess transform)
        {
            quaternion q = transform.rotation;
            float3 p = transform.position;

            float3 angle = playerPos - p;

            q = math.slerp(q, quaternion.LookRotation(angle, new float3(0f, 1f, 0f)), deltaTime * 5);
            transform.rotation = q;
        }
    }
}