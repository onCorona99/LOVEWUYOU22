using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedBehaviour : SharedVariable<Behaviour>
    {
        public static explicit operator SharedBehaviour(Behaviour value) { return new SharedBehaviour { mValue = value }; }
    }

    [System.Serializable]
    public class SharedMonoBehaviour : SharedVariable<MonoBehaviour>
    {
        public static implicit operator SharedMonoBehaviour(MonoBehaviour value) { return new SharedMonoBehaviour { mValue = value }; }
    }
}