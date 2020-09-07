using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace BurningBlueFox.Heated.AI
{

    public class EnemyFsmManager : MonoBehaviour
    {
        private static List<IFiniteStateMachine> fsms = new List<IFiniteStateMachine>();
        public static void RegisterFiniteStateMachine(IFiniteStateMachine fsm) => fsms.Add(fsm);
        public static void RemoveFiniteStateMachine(IFiniteStateMachine fsm) => fsms.Remove(fsm);

        private Transform playerTransform;

        TransformAccessArray transformArray;
        JobHandle handle;

        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        private void Update()
        {

            Transform[] transforms = new Transform[fsms.Count];
            for (int i = 0; i < fsms.Count; i++)
            {
                transforms[i] = fsms[i].transform;
            }

            transformArray = new TransformAccessArray(transforms);

            FbiFsmJob job = new FbiFsmJob()
            {
                playerPos = playerTransform.position,
                deltaTime = Time.deltaTime
            };

            handle = job.Schedule(transformArray);
        }
        private void LateUpdate()
        {
            handle.Complete();
            transformArray.Dispose();
        }
    }
}