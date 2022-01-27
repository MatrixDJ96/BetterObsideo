using BetterObsideo.Utility;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class BreakerController : AbstractController<Breaker>
    {
        private void Update()
        {
            //if (SteamIdUtility.IsMatrixDJ96Player())
            {
                if ((Input.GetKeyDown(KeyCode.PageUp) && !component.lightOn) || (Input.GetKeyDown(KeyCode.PageDown) && component.lightOn))
                {
                    if (!(PlayerController.Instance.IsTabletOpen || PlayerController.Instance.IsEscapeMenuOpen))
                    {
                        component.ToggleLightServerRpc();
                        DebuggerUtility.WriteMessage("Breaker has been " + (component.lightOn ? "activated" : "deactivated") + "!");
                    }
                }
            }
        }
    }
}
