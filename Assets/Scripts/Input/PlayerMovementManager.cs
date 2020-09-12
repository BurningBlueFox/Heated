using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace BurningBlueFox.Heated.Input
{
    public class PlayerMovementManager : MonoBehaviour, IProcessMovement
    {
        [SerializeField] private Animator playerAnimator;
        private readonly int ANIMATOR_MOVE_HASH = Animator.StringToHash("Velocity");
        private const float DEADZONE = 0.1f;
        private const float TURN_SMOOTH_TIME = 0.1f;
        private const float SPEED = 6f;
        private float turnSmoothVelocity;
        private Transform playerTransform;
        private Transform camTransform;
        private JobHandle jobHandle;
        NativeArray<quaternion> euler;
        NativeArray<float2> pos;


        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            playerTransform = this.GetComponent<Transform>();
            camTransform = Camera.main.GetComponent<Transform>();
        }

        public void ProcessMovement(Vector2 value)
        {

            euler = new NativeArray<quaternion>(1, Allocator.TempJob);
            pos = new NativeArray<float2>(1, Allocator.TempJob);

            pos[0] = new float2(value.x, value.y);
            euler[0] = playerTransform.rotation;

            playerAnimator.SetFloat(ANIMATOR_MOVE_HASH, math.length(pos[0])); //Temporary fix while animator specific job code doesnt exist

            MovementJob playerMovementJob = new MovementJob()
            {
                pos = pos,
                euler = euler,
                camEulerY = camTransform.eulerAngles.y,
                deadzone = DEADZONE,
                speed = SPEED,
                smoothTime = TURN_SMOOTH_TIME * 100,
                deltaTime = Time.deltaTime
            };

            jobHandle = playerMovementJob.Schedule();
        }
        private void LateUpdate()
        {
            jobHandle.Complete();

            playerTransform.rotation = euler[0];
            Vector3 temp = new Vector3(pos[0].x, 0f, pos[0].y);
            playerTransform.position += temp;

            


            euler.Dispose();
            pos.Dispose();
        }

    }
}