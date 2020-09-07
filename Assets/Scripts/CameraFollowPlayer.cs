using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BurningBlueFox.Heated
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollowPlayer : MonoBehaviour
    {
        [SerializeField] private Vector3 positionOffset = default;
        private Transform playerTransform = default;
        private Transform cameraTransform = default;
        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            cameraTransform = this.transform;
        }
        private void Update()
        {
            Vector3 playerPos = playerTransform.position;

            playerPos.y = positionOffset.y;
            playerPos.x += positionOffset.x;
            playerPos.z += positionOffset.z;


            cameraTransform.position = playerPos;
        }
    }
}