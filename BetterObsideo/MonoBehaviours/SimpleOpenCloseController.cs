using BetterObsideo.Utility;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class SimpleOpenCloseController : NetworkBehaviour
    {
        private SimpleOpenClose component = null;

        private Animator Animator { get => component.myAnimator; }
        private AudioSource AudioSource { get => component.audioSource; }

        public AudioClip[] DoorClose { get => component.doorClose; }
        public AudioClip[] DoorOpen { get => component.doorOpen; }

        public bool IsOpen { get => component.objectOpen; set => component.objectOpen = value; }
        public bool CanOpen { get => component.canOpen; set => component.canOpen = value; }

        public bool ShowStats { get; private set; } = false;

        public bool HasAdditional { get => component.hasAdditional; }

        private AnimatorStateInfo animatorStateInfo;

        private void Awake()
        {
            component = gameObject.GetComponent<SimpleOpenClose>();

            if (component == null)
            {
                Destroy(this);
                return;
            }

            DebuggerUtility.WriteMessage($"{GetType().Name}.Awake ({GetInstanceID()})");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                ShowStats = false;
            }

            if (ShowStats)
            {
                //DebuggerUtility.ShowMessage($"{IsOpen}", $"{component.GetType().Name}.IsOpen ({component.GetInstanceID()})");
                //DebuggerUtility.ShowMessage($"{HasAdditional}", $"{component.GetType().Name}.HasAdditional ({component.GetInstanceID()})");
            }
        }

        public void FixedInteractServerRpc()
        {
            if (NetworkManager == null || !NetworkManager.IsListening)
            {
                return;
            }

            if (component.__rpc_exec_stage != __RpcExecStage.Server && (NetworkManager.IsClient || NetworkManager.IsHost))
            {
                using var writer = new FastBufferWriter(1285, Allocator.Temp, 63985);
                component.__sendServerRpc(writer, 2905143803u, default, RpcDelivery.Reliable);
            }

            if (component.__rpc_exec_stage != __RpcExecStage.Server || (!NetworkManager.IsServer && !NetworkManager.IsHost))
            {
                return;
            }

            if (CanOpen)
            {
                animatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

                if (animatorStateInfo.normalizedTime > animatorStateInfo.length + 0.1f)
                {
                    if (IsOpen)
                    {
                        Animator.Play("Close", 0, 0);
                        AudioSource.PlayOneShot(DoorClose[Random.Range(0, DoorClose.Length)]);
                    }
                    else
                    {
                        Animator.Play("Open", 0, 0);
                        AudioSource.PlayOneShot(DoorOpen[Random.Range(0, DoorOpen.Length)]);
                    }

                    IsOpen = !IsOpen;

                    ShowStats = true;
                }
            }
        }
    }
}
