using BurningBlueFox.Heated.Core;
using Unity.Mathematics;
using UnityEngine;

namespace BurningBlueFox.Heated.Gameplay
{

    public class Gamestate : Singleton<Gamestate>
    {
        private Data currentState = new Data();
        private Transform playerTransform;

        public Data GetCurrentState() => currentState;
        public void RegisterPlayerTransform(Transform player) => player = this.playerTransform;



        public struct Data
        {
            public enum AlertState
            {
                Stealth,
                Combat
            }

            public AlertState currentAlertState;
            public int currentLawLevel;
            public int currentNumberOfHostages;
            public float3 playerPosition;

        }
    }
}