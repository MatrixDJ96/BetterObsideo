using BetterObsideo.Utility;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterObsideo.MonoBehaviours
{
    public class SteamManagerController : MonoBehaviour
    {
        private SteamManager component = null;
        public string LastScene { get; private set; }
        public bool InLobby => LastScene == "Lobby";

        private void Awake()
        {
            component = gameObject.GetComponent<SteamManager>();

            if (component == null)
            {
                Destroy(this);
                return;
            }

            SceneManager.sceneLoaded += (scene, mode) => LastScene = scene.name;

            DebuggerUtility.WriteMessage($"{GetType().Name}.Awake ({GetInstanceID()})");
        }

        private void Update()
        {
            //if (SteamIdUtility.IsMatrixDJ96Player())
            {
                if (Input.GetKeyDown(KeyCode.T) && InLobby)
                {
                    NetworkManager.Singleton.SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
                    DebuggerUtility.WriteMessage($"Starting Tutorial...");
                }
            }
        }
    }
}
