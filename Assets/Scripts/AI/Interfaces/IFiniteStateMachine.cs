using UnityEngine;

namespace BurningBlueFox.Heated.AI
{
    public interface IFiniteStateMachine
    {
        Transform transform { get;}
        void SetRotation(Quaternion q);
    }
}