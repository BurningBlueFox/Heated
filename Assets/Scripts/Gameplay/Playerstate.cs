using UnityEngine;

namespace BurningBlueFox.Heated.Gameplay
{
    public class Playerstate : MonoBehaviour
    {
        private Transform playerTransform = default;
        private void Awake()
        {
            playerTransform = transform;

            Gamestate.Instance.RegisterPlayerTransform(playerTransform);
        }
    }
}