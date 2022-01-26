using BetterObsideo.Utility;
using Obsideo.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace BetterObsideo.MonoBehaviours
{
    public class PlayerController : NetworkBehaviour
    {
        public static PlayerController Instance = null;

        private FirstPersonController component = null;
        public FirstPersonController Component { get => component; }

        public int InstanceID { get => component.GetInstanceID(); }

        public bool Exorcised { get => component.exorcised; set => component.exorcised = value; }
        public bool AntiRadPills { get => component.takenAntiRadPills; set => component.takenAntiRadPills = value; }
        public bool IsMoving { get => component.isMoving; set => component.isMoving = value; }
        public bool IsSprinting { get => component.canSprint; set => component.canSprint = value; }
        public bool IsCrouching { get => component.is_crouching; set => component.is_crouching = value; }

        public Image sprintBar = null;
        public Image SprintBar { get => sprintBar ??= component.sprintBar.GetComponent<Image>(); }
        public GameObject SprintBarContainer { get => component.sprintBarContainer; }

        public float HeartRate { get => component.heartRateActual; set => component.heartRateActual = value; }
        public float Stamina { get => component.stamina; set => component.stamina = value; }
        public float MaxStamina { get => component.maxStamina; set => component.maxStamina = value; }
        public float MovementSpeed { get => component.movementSpeed; set => component.movementSpeed = value; }

        public float CrounchMovementSpeed { get; set; } = 1.0f;
        public float BaseMovementSpeed { get; set; } = 1.8f;
        public float SprintMovementSpeed { get; set; } = 3.0f;

        public bool ShowStats { get; set; } = false;

        public override void OnDestroy()
        {
            if (this == Instance)
            {
                Instance = null;
            }
            base.OnDestroy();
        }

        private void Awake()
        {
            component = gameObject.GetComponent<FirstPersonController>();

            if (component == null || !component.IsLocalPlayer)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            DebuggerUtility.WriteMessage($"{GetType().Name}.Awake ({GetInstanceID()})");
        }

        private void Start()
        {
            MaxStamina = 1.5f;
            Stamina = MaxStamina;
        }

        private void Update()
        {
            if (IsLocalPlayer)
            {
                if (Input.GetKeyDown(KeyCode.RightControl))
                {
                    ShowStats = !ShowStats;
                    DebuggerUtility.ClearMessages();
                }

                if (ShowStats)
                {
                    if (AntiRadPills)
                    {
                        DebuggerUtility.ShowMessage($"{AntiRadPills}", $"Anti Rad Pills", false);
                    }
                    DebuggerUtility.ShowMessage($"{HeartRate:0}", $"Heart Rate", false);
                }

                if (Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    HeartRate++;
                }
            }
        }

        public void FixMovementAndStamina()
        {
            MovementSpeed = IsMoving ? (IsSprinting ? (IsCrouching ? BaseMovementSpeed : SprintMovementSpeed) : (IsCrouching ? CrounchMovementSpeed : BaseMovementSpeed)) : BaseMovementSpeed;
            SprintBarContainer.SetActive(Stamina < MaxStamina);
            SprintBar.fillAmount = Stamina / MaxStamina;
        }
    }
}
