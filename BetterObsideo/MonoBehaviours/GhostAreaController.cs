using BetterObsideo.Utility;
using Obsideo.Ghosts;
using Obsideo.Player;
using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class GhostAreaController : NetworkBehaviour
    {
        private GhostArea component = null;
        private BibleStart bible = null;

        public float HuntTime { get; set; } = 0;
        public float HuntChance { get => component.chanceOfHunt / 101 * 100; }

        public bool CanHunt { get => component.canHunt; set => component.canHunt = value; }
        public bool IsInHunt { get => component.isInHunt; set => component.isInHunt = value; }

        public FirstPersonController Player { get => component.myPlayer.GetComponent<FirstPersonController>(); }
        public GameObject[] MainDoors { get => GameObject.FindGameObjectsWithTag("MainDoor"); }

        public string ChosenGhost { get => component.chosenGhost; }
        public string[] GhostTypes { get => component.ghostTypes; }

        public bool ShowStats { get; set; } = false;

        private void Awake()
        {
            component = gameObject.GetComponent<GhostArea>();

            if (component == null)
            {
                Destroy(this);
                return;
            }

            StartCoroutine(SetBible());

            DebuggerUtility.WriteMessage($"{GetType().Name}.Awake ({GetInstanceID()})");
        }

        private void Update()
        {
            if (PlayerController.Instance?.Component is FirstPersonController player)
            {
                // Fix wrong heart rate for non-host player after successful exorcism
                if (player.exorcised != component.exorcismSuccess)
                {
                    player.exorcised = component.exorcismSuccess;
                }

                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    ShowStats = !ShowStats;
                    DebuggerUtility.ClearMessages();
                }

                if (ShowStats)
                {
                    if (CanHunt)
                    {
                        DebuggerUtility.ShowMessage($"{HuntChance:0.00} %", $"Hunt Chance", false);
                    }

                    if (HuntTime > 0 && (NetworkManager.IsServer || NetworkManager.IsHost))
                    {
                        DebuggerUtility.ShowMessage($"{HuntTime}", $"Hunt Time", false);
                    }
                }

                //if (SteamIdUtility.IsMatrixDJ96Player())
                {
                    if (Input.GetKeyDown(KeyCode.Y) && !(player.tabletOpen || player.escapeMenuOpen) && !IsInHunt)
                    {
                        CanHunt = true;
                        IsInHunt = true;
                        DebuggerUtility.WriteMessage($"Ghost is in hunt!");
                    }

                    if (Input.GetKeyDown(KeyCode.O) && !(player.tabletOpen || player.escapeMenuOpen) && bible != null)
                    {
                        bible.ExercismOutcomeClientRpc(true);
                        DebuggerUtility.WriteMessage($"Ghost has been exorcised!");
                    }
                }
            }
        }

        private IEnumerator SetBible()
        {
            if (GameObject.FindGameObjectWithTag("Bible") == null)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(SetBible());
            }

            bible = GameObject.FindGameObjectWithTag("Bible").GetComponent<BibleStart>();
            DebuggerUtility.WriteMessage($"Bible found!");
        }

        public void FixedGracePeriodClientRpc()
        {
            if (NetworkManager == null || !NetworkManager.IsListening)
            {
                return;
            }

            if (component.__rpc_exec_stage != __RpcExecStage.Client && (NetworkManager.IsServer || NetworkManager.IsHost))
            {
                using var writer = new FastBufferWriter(1285, Allocator.Temp, 63985);
                component.__sendClientRpc(writer, 1226921989u, default, RpcDelivery.Reliable);
            }

            DebuggerUtility.WriteMessage($"Server: {NetworkManager.IsServer} | Host: {NetworkManager.IsHost} | Client: {NetworkManager.IsClient}", $"{GetType().Name}.FixedGracePeriodClientRpc ({component.__rpc_exec_stage})");

            if (component.__rpc_exec_stage == __RpcExecStage.Client && (NetworkManager.IsClient || NetworkManager.IsHost))
            {
                Player.displayHuntWarning = true;

                foreach (var flashlight in component.playerFlashlight)
                {
                    flashlight.FlickeringLightServerRpc(doFlicker: true);
                }

                foreach (var mainDoor in MainDoors)
                {
                    if (mainDoor.GetComponent<SimpleOpenClose>() is SimpleOpenClose simple)
                    {
                        if (simple.canOpen)
                        {
                            simple.Interact();
                        }

                        simple.canOpen = false;
                    }
                }
            }
        }

        public IEnumerator FixedHunt()
        {
            yield return new WaitUntil(() => (HuntTime -= Time.deltaTime) <= 0);

            if (NetworkManager.IsServer || NetworkManager.IsHost)
            {
                component.EndHuntServerRpc();
            }
        }
    }
}
