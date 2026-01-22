using UnityEngine;
using Deforestation.Machine;
using Deforestation.UI;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Cinemachine;
using System;
using UnityEngine.SceneManagement;
using StarterAssets;

namespace Deforestation
{
	public class GameController : Singleton<GameController>
	{
		#region Properties
		public MachineController MachineController => _machineController;
		public MachineMovement MachineMovement => _machineMovement;
		public Inventory Inventory => _inventory;
		public InteractionSystem InteractionSystem => _interactionSystem;
		public TreeTerrainController TerrainController => _terrainController;
		public Camera MainCamera;
		public UIGameController UIGameController => _uiController;
        // Delegate, para acceder al Health System del Player:
        public HealthSystem PlayerHealth => _playerHealth;
        // Delegate, para acceder al script (de manera externa (revisa método HealthSystem/Die())) que controla el movimiento del Player (First Person Controller):
        public FirstPersonController Player => _player;


        //Events
        public Action<bool> OnMachineModeChange;

		public bool MachineModeOn
		{
			get
			{
				return _machineModeOn;
			}
			private set
			{
				_machineModeOn = value;
				OnMachineModeChange?.Invoke(_machineModeOn);
			}
		}
		#endregion

		#region Fields
		[Header("Player")]
		[SerializeField] protected CharacterController _playerCharacterController;

        [SerializeField] protected FirstPersonController _player;
		[SerializeField] protected HealthSystem _playerHealth;
		[SerializeField] protected Inventory _inventory;
		[SerializeField] protected InteractionSystem _interactionSystem;

		[Header("Camera")]
		[SerializeField] protected CinemachineVirtualCamera _virtualCamera;

		[SerializeField] protected Transform _playerFollow;
		[SerializeField] protected Transform _machineFollow;

		[Header("Machine")]
		[SerializeField] protected MachineController _machineController;
		[Header("UI")]
		[SerializeField] protected UIGameController _uiController;
		[SerializeField] protected MainMenuInteraction _mainMenuInteraction;
		[Header("Trees Terrain")]
		[SerializeField] protected TreeTerrainController _terrainController;
		[SerializeField] private MachineMovement _machineMovement;


        private bool _machineModeOn;
        #endregion

        #region Unity Callbacks
        // Start is called before the first frame update
        void Start()
		{
            // Desactivar el canvas del "Main Menu" al iniciar la escena:
            _mainMenuInteraction.gameObject.SetActive(false);
            //UI Update
            _playerHealth.OnHealthChanged += _uiController.UpdatePlayerHealth;
			_machineController.HealthSystem.OnHealthChanged += _uiController.UpdateMachineHealth;
			MachineModeOn = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
		{
            //Si, presionas la tecla "Escape", volverás al "Main Menu Scene" (da igual si estas en Modo Máquina o no):
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                // Al haber un cambio de escenas (InGame a Main Menu), el Singleton (o su heredero), se mantenía con vida (no se destruía debido a los atributos del Singleton).

                // A consecuencia de esto, cuando regresabas al "InGame Scene" desde el "Main Menu Scene", se generaban errores "NullReferenceExcepiton", debido a lo siguiente:

                // El heredero de Singleton (GameController) seguía teniendo referencias de objetos que (en el cambio de escena), se volvieron null, por eso daban errores.

                // Tuve una conversación con ChatGPT (con explicaciones correctas y detalladas) sobre este problema, y me recomendó destruir el Singleton (su heredero) manualmente, antes de cambiar de escena.

                // Porque, se regeneraría al cambiar de escena (Main Menu a Ingame -> InGame a Main Menu -> Main Menu a InGame).

                // El único inconveniente acerca de esto, es que, no se guardará el proceso.
                Destroy(GameController.Instance.gameObject);
                //Se cambia de escena al "Main Menu Scene":
                SceneManager.LoadScene("Main Menu Scene");
            }
        }
        #endregion

        #region Public Methods

        // Método para manejar el mensaje de muerte del Player/TheMachine:
        public void DieMessage()
		{
			_playerHealth.Die();
			_machineController.HealthSystem.Die();
        }
		public void TeleportPlayer(Vector3 target)
		{
			_playerCharacterController.enabled = false;
			_playerCharacterController.transform.position = target;
			_playerCharacterController.enabled = true;
		}

		internal void MachineMode(bool machineMode)
		{
			MachineModeOn = machineMode;
			//Player
			_playerCharacterController.gameObject.SetActive(!machineMode);
			_playerCharacterController.enabled = !machineMode;
           


            //Cursor + UI
            if (machineMode)
			{
                // Activamos el Game Object del "Settings Button" (engranaje):
                _uiController.SettingsButton.gameObject.SetActive(true);
                //Start Driving
                if (Inventory.HasResource(RecolectableType.HyperCrystal))
                    _machineController.StartDriving(machineMode);
				if (Inventory.HasResource(RecolectableType.JumpCrystal))
					//_machineController.(machineMode);

                _playerCharacterController.transform.parent = _machineFollow;
				_uiController.HideInteraction();
				Cursor.lockState = CursorLockMode.None;
				//Camera
				_virtualCamera.Follow = _machineFollow;

				_machineController.enabled = true;
				_machineController.WeaponController.enabled = true;
				_machineController.GetComponent<MachineMovement>().enabled = true;

                //Si la vida de "TheMachine" es menor o igual a 0, : 
                if (_machineController.HealthSystem.CurrentHealth <= 0)
					DieMessage();


            }
			else
			{
				_machineController.enabled = false;
				_machineController.WeaponController.enabled = false;
				_machineController.GetComponent<MachineMovement>().enabled = false;
				_playerCharacterController.transform.parent = null;

                // Si no estas en modo máquina, el panel se desactivará:
                // Si presionas "º" para salir de la Máquina mientras tienes el "Settings Panel" activo, se desactivará automáticamente:
                _uiController.SettingsPanel.SetActive(false);
                // Y se desactivará el botón del engranaje:
                _uiController.SettingsButton.gameObject.SetActive(false);
                // Si la vida del Player es menor o igual a 0, :
                if (_playerHealth.CurrentHealth <= 0)
                    DieMessage();
           
				//Camera
				_virtualCamera.Follow = _playerFollow;
				Cursor.lockState = CursorLockMode.Locked;
			}
			Cursor.visible = machineMode;
		}
        #endregion

        #region Private Methods
        #endregion
    }

}