using BetterObsideo.Utility;
using Unity.Netcode;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class AbstractController<T> : NetworkBehaviour where T : MonoBehaviour
    {
        protected T component = null;
        public T Component { get => component; }

        protected virtual void Awake()
        {
            component = gameObject.GetComponent<T>();

            if (component == null)
            {
                Destroy(this);
                return;
            }

            DebuggerUtility.WriteMessage($"{GetType().Name}.Awake ({GetInstanceID()})");
        }
    }
}
