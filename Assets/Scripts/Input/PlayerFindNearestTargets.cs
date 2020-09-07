using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace BurningBlueFox.Heated.Input
{
    public class PlayerFindNearestTargets : MonoBehaviour
    {
        [SerializeField] Transform reticleTransform = default;
        [SerializeField] LayerMask layerMask = default;
        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = this.transform;
        }

        private void Update()
        {
            FindNearestTargets();

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