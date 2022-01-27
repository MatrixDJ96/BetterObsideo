using BetterObsideo.Utility;
using Obsideo.Ghosts;
using System;
using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class GhostAreaController : AbstractController<GhostArea>
    {
        private BibleStart bible = null;
        public BibleStart Bible
        {
            get
            {
                if (bible == null)
                {
                    if (GameObject.FindGameObjectWithTag("Bible") is GameObject gameObject)
                    {
                        bible = gameObject.GetComponent<BibleStart>();
                    }
                }
                return bible;
            }
        }

        public float HuntTime { get; set; } = 0;
        public float HuntChance { get => component.chanceOfHunt / 101 * 100; }

        public bool CanHunt { get => component.canHunt; set => component.canHunt = value; }
        public bool IsInHunt { get => component.isInHunt; set => component.isInHunt = value; }

        public GameObject[] MainDoors { get => component.mainDoor = GameObject.FindGameObjectsWithTag("MainDoor"); }

        public string ChosenGhost { get => component.chosenGhost; }
        public string[] GhostTypes { get => component.ghostTypes; }

        public bool ShowStats { get; set; } = true;

        private void Start()
        {
            DebuggerUtility.WriteMessage($"{GetType().Name}.Start ({GetInstanceID()})");
        }

        private void Update()
        {
            // Fix wrong heart rate for non-host player after successful exorcism
            if (PlayerController.Instance.Exorcised != component.exorcismSuccess)
            {
                PlayerController.Instance.Exorcised = component.exorcismSuccess;
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

                if ((HuntTime > 0 || IsInHunt) && (NetworkManager.IsServer || NetworkManager.IsHost))
                {
                    DebuggerUtility.ShowMessage($"{HuntTime}", $"Hunt Time", false);
                }
            }

            //if (SteamIdUtility.IsMatrixDJ96Player())
            {
                if (Input.GetKeyDown(KeyCode.Y) && !(PlayerController.Instance.IsTabletOpen || PlayerController.Instance.IsEscapeMenuOpen) && !IsInHunt)
                {
                    CanHunt = true;
                    IsInHunt = true;
                    DebuggerUtility.WriteMessage($"Ghost is in hunt!");
                }

                if (Input.GetKeyDown(KeyCode.O) && !(PlayerController.Instance.IsTabletOpen || PlayerController.Instance.IsEscapeMenuOpen) && Bible != null)
                {
                    Bible.ExercismOutcomeClientRpc(true);
                    DebuggerUtility.WriteMessage($"Ghost has been exorcised!");
                }
            }
        }

        public void FixedGracePeriodClientRpc()
        {
            if (NetworkManager == null || !NetworkManager.IsListening)
            {
                return;
            }

            DebuggerUtility.WriteMessage($"Server: {NetworkManager.IsServer} | Host: {NetworkManager.IsHost} | Client: {NetworkManager.IsClient}", $"{GetType().Name}.FixedGracePeriodClientRpc ({component.__rpc_exec_stage})");

            if (component.__rpc_exec_stage != __RpcExecStage.Client && (NetworkManager.IsServer || NetworkManager.IsHost))
            {
                DebuggerUtility.WriteMessage($"SendClientRpc()", $"{GetType().Name}.FixedGracePeriodClientRpc ({component.__rpc_exec_stage})");

                using var writer = new FastBufferWriter(1285, Allocator.Temp, 63985);
                component.__sendClientRpc(writer, 1226921989u, default, RpcDelivery.Reliable);
            }

            if (component.__rpc_exec_stage == __RpcExecStage.Client && (NetworkManager.IsClient || NetworkManager.IsHost))
            {
                DebuggerUtility.WriteMessage($"Exec()", $"{GetType().Name}.FixedGracePeriodClientRpc ({component.__rpc_exec_stage})");

                PlayerController.Instance.ShowHuntWarning = true;

                foreach (var flashlight in component.playerFlashlight)
                {
                    flashlight.flashlight.color = Color.red;
                    //flashlight.FlickeringLightServerRpc(doFlicker: true);
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

        public IEnumerator FixedGracePeriod(int ghostId)
        {
            yield return new WaitForSeconds(3f);
            component.SpawnGhostServerRpc(ghostId);
        }

        public IEnumerator FixedHunt()
        {
            //yield return new WaitUntil(() => (HuntTime -= Time.deltaTime) <= 0);

            yield return new WaitForSeconds(6f);

            foreach (var flashlight in component.playerFlashlight)
            {
                flashlight.flashlight.color = Color.white;
            }

            if (NetworkManager.IsServer || NetworkManager.IsHost)
            {
                component.EndHuntServerRpc();
            }
        }
    }
}
