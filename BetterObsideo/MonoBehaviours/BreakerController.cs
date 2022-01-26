using BetterObsideo.Utility;
using Obsideo.Player;
using Unity.Netcode;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class BreakerController : NetworkBehaviour
    {
        private Breaker component = null;

        private void Awake()
        {
            component = gameObject.GetComponent<Breaker>();

            if (component == null)
            {
                Destroy(this);
                return;
            }

            DebuggerUtility.WriteMessage($"{GetType().Name}.Awake ({GetInstanceID()})");
        }

        private void Update()
        {
            if (PlayerController.Instance?.Component is FirstPersonController player)
            {
                //if (SteamIdUtility.IsMatrixDJ96Player())
                {
                    if (Input.GetKeyDown(KeyCode.L) && !(player.tabletOpen || player.escapeMenuOpen))
                    {
                        component.ToggleLightServerRpc();
                        DebuggerUtility.WriteMessage("Breaker has been " + (component.lightOn ? "activated" : "deactivated") + "!");
                    }
                }
            }
        }
    }
}
