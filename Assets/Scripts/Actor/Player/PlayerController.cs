using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPausable
{
    void OnPause();
    void OnUnpause();
}

public interface ICheckPaused
{
    bool CheckIsPaused();
}

public interface IPlayerInitialiser
{
    void InitialisePlayer(SceneController sceneController);
}

namespace PlayerSystems
{
    public class PlayerController : MonoBehaviour, IPlayerInitialiser, IPausable, ICheckPaused
    {
        public UnityEngine.InputSystem.PlayerInput playerInput;
        public bool isPaused = false;

        // ================================
        // Initialisers
        // ================================

        /// <summary>
        /// Initialises classes systems to the player
        /// </summary>
        public void InitialisePlayer(SceneController sceneController)
        {
            // Initialises and sets primary data for vessel
            InitiateShipStatHandler();

            // Initialises and activates related handlers and controls
            InitiateInputSystem();
            InitiateMovement();
            InitiateWeapons();

            sceneController.OnGameCompletion.AddListener(RemoveInputSystems);
        }

        /// <summary>
        /// Initialises ship stats using the servicer to retrieve data.
        /// </summary>
        private void InitiateShipStatHandler()
        {
            IStatHandler statHandler = this.GetComponent<IStatHandler>();
            statHandler.InitialiseStats(SessionData.instance.selectedShip);
            PlayerHeathComponent heathComponent = this.GetComponent<PlayerHeathComponent>();
            heathComponent.InitialiseHealth(SessionData.instance.selectedShip.maxHealth);
            PlayerShieldComponent shieldComponent = this.GetComponent<PlayerShieldComponent>();
            shieldComponent.InitialiseShield(SessionData.instance.selectedShip.maxSheild);
        }

        /// <summary>
        /// Intiailises player related input classes
        /// </summary>
        public void InitiateInputSystem()
        {
            playerInput = this.GetComponent<UnityEngine.InputSystem.PlayerInput>();

            if (Application.isMobilePlatform)
            {
                Debug.Log("Is ported to mobile");
                BeginMobileInputSystem();
            }
            else
            {
                Debug.Log("Is ported to desktop");
                BeginDesktopInputSystem();
            }
        }

        /// <summary>
        /// Initiates the mobile input systerm to this player
        /// </summary>
        private void BeginMobileInputSystem()
        {
            //Spawn UI HUD
            UISettings uiSettings = GameManager.Instance.uiSettings;
            GameObject mobileHUD = Instantiate(uiSettings.mobileUIHUDPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            playerInput.SwitchCurrentActionMap("Mobile");
            DesktopInputManager desktopinput = this.GetComponent<DesktopInputManager>();
            desktopinput.enabled = false;

            IMobileInput mobileInput = this.GetComponent<IMobileInput>();
            mobileInput.InitialiseInput(mobileHUD);
        }


        /// <summary>
        /// Initiates the desktop input system for this player
        /// </summary>
        private void BeginDesktopInputSystem()
        {
            playerInput.SwitchCurrentActionMap("Desktop");
            MobileInputManager mobileInput = this.GetComponent<MobileInputManager>();
            mobileInput.enabled = false;

            IDesktopInput deskTopInput = this.GetComponent<IDesktopInput>();
            deskTopInput.InitialiseDesktop();
        }

        /// <summary>
        /// Initialises movement related classes.
        /// </summary>
        public void InitiateMovement()
        {
            PlayerMovementController playerMovement = this.GetComponent<PlayerMovementController>();
            playerMovement.InitialiseMovement();
        }

        public void InitiateWeapons()
        {
            PlayerWeaponController playerWeapons = this.GetComponent<PlayerWeaponController>();
            playerWeapons.InitialiseWeaponController();
            IDamageable damager = this.GetComponent<IDamageable>();
            damager.InitialiseComponent();
        }

        // ================================
        // Pausers and Checkers
        // ================================

        public void OnPause()
        {
            isPaused = true;
        }

        public void OnUnpause()
        {
            isPaused = false;
        }

        public bool CheckIsPaused()
        {
            return isPaused;
        }

        // ================================
        // Game Completion
        // ================================

        /// <summary>
        /// Called to disable input control in the event of game events or game completion
        /// </summary>
        public void RemoveInputSystems()
        {
            Debug.LogWarning("Lockout Triggered");
            DesktopInputManager desktopinput = this.GetComponent<DesktopInputManager>();
            Destroy(desktopinput);

            MobileInputManager mobileInput = this.GetComponent<MobileInputManager>();
            Destroy(mobileInput);
        }
    }
}