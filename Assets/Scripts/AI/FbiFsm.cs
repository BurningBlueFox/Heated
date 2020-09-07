using UnityEngine;

namespace BurningBlueFox.Heated.AI
{
    public class FbiFsm : MonoBehaviour, IFiniteStateMachine
    {

        private void OnEnable()
        {
            EnemyFsmManager.RegisterFiniteStateMachine(this);
        }
        private void OnDisable()
        {
            EnemyFsmManager.RemoveFiniteStateMachine(this);
        }

        public void SetRotation(Quaternion q) => transform.rotation = q;

    }
}