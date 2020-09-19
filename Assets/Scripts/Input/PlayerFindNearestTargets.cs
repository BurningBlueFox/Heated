using BurningBlueFox.Heated.Gameplay;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BurningBlueFox.Heated.Input
{
    public class PlayerFindNearestTargets : MonoBehaviour
    {
        [SerializeField] Transform reticleTransform = default;
        [SerializeField] Transform muzzleTransform = default;
        [SerializeField] LayerMask layerMask = default;
        [SerializeField][Range(0.001f, 1f)] float dispersion = 0.2f;
        private Transform playerTransform;

        private NativeArray<quaternion> muzzleRotation;
        private NativeArray<float> projectileDispersion;
        private NativeArray<Unity.Mathematics.Random> random;
        private NativeArray<float3> output;
        JobHandle handler;

        private void Awake()
        {
            playerTransform = this.transform;
        }

        private void Update()
        {
            FindNearestTargets();
            SetCalculateBulletTrajectoryJob();
        }
        private void LateUpdate()
        {
            handler.Complete();
            Debug.DrawRay(muzzleTransform.position, output[0], Color.red);
        }
        private void OnEnable()
        {
            muzzleRotation = new NativeArray<quaternion>(1, Allocator.Persistent);
            projectileDispersion = new NativeArray<float>(1, Allocator.Persistent);
            random = new NativeArray<Unity.Mathematics.Random>(1, Allocator.Persistent);
            
            output = new NativeArray<float3>(1, Allocator.Persistent);
        }
        private void OnDisable()
        {
            muzzleRotation.Dispose();
            projectileDispersion.Dispose();
            random.Dispose();
            output.Dispose();
        }
        private void SetCalculateBulletTrajectoryJob()
        {
            muzzleRotation[0] = muzzleTransform.rotation;
            projectileDispersion[0] = dispersion;
            random[0] = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 10000));


            CalculateProjectileTrajectoryJob job = new CalculateProjectileTrajectoryJob()
            {
                muzzleRotation = this.muzzleRotation,
                projectileDispersion = this.projectileDispersion,
                random = this.random,
                projectileDirection = this.output
            };

            handler = job.Schedule(muzzleRotation.Length, 1);
        }
        private void FindNearestTargets()
        {
            Vector3 direction = Vector3.forward;
            float radius = 3f;

            var results = Physics.SphereCastAll(playerTransform.position, radius, Vector3.forward, radius, layerMask: layerMask);

            if (results.Length == 0)
                return;

            GameObject selectedObject = null;
            float prevDistance = 0f;
            foreach (var col in results)
            {
                if (selectedObject == null)
                {
                    selectedObject = col.collider.gameObject;
                    prevDistance = Vector3.Distance(playerTransform.position, col.collider.gameObject.transform.position);
                }
                else
                {
                    float newDistance = Vector3.Distance(playerTransform.position, col.collider.gameObject.transform.position);
                    if (newDistance < prevDistance)
                    {
                        selectedObject = col.collider.gameObject;
                        prevDistance = newDistance;
                    }
                }
            }
            if (selectedObject != null)
                reticleTransform.position = selectedObject.transform.position;


        }

    }
}