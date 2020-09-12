using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurningBlueFox.Heated.Input
{
    [BurstCompile]
    public struct MovementJob : IJob
    {
        public NativeArray<quaternion> euler;
        public NativeArray<float2> pos;

        public float camEulerY;
        public float deadzone;
        public float smoothTime;
        public float deltaTime;

        public float speed;

        public void Execute()
        {
            if(math.length(pos[0]) > 1f)
            pos[0] = math.normalize(pos[0]);

            float magnitude = math.length(pos[0]);
            if (magnitude > deadzone)
            {
                float targetAngle = math.atan2(pos[0].x, pos[0].y) + math.radians(camEulerY);
                quaternion q = math.slerp(euler[0].value, quaternion.Euler(0f, targetAngle, 0f).value, deltaTime * smoothTime);

                euler[0] = q;

                float3 moveDir = math.forward(quaternion.Euler(0f, targetAngle, 0f));

                float3 add = (moveDir * speed *  magnitude * deltaTime);
                float2 temp = pos[0];
                temp.x = add.x;
                temp.y = add.z;
                pos[0] = temp;
            }
            else
            {
                pos[0] = new float2(0f, 0f);
            }
        }
    }
}