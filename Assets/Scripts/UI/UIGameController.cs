using UnityEngine;
using TMPro;
using Deforestation.Recolectables;
using System;
using Deforestation.Interaction;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

namespace Deforestation.UI
{
	public class UIGameController : MonoBehaviour
	{
        #region Properties

        // Para que el Game Controller pueda acceder a estas dos variables:
        public Button SettingsButton => _settingsButton;
		public GameObject SettingsPanel => _settingsPanel;
		public GameObject DiePanel => _diePanel;
        #endregion

        #region Fields
        private Inventory _inventory => GameController.Instance.Inventory;		
		private InteractionSystem _interactionSystem => GameController.Instance.InteractionSystem;
		 

        [Header("Settings")]
        [Header("Settings Buttons")]
        // Referencia serializada al botón "Confirm Settings Button":
        [SerializeField] private Button _confirmSettingsButton;
        // Referencia serializada al botón "Restart Button":
        [SerializeField] private Button _retryGameButton;
        
		[SerializeField] private GameObject _diePanel;
        [SerializeField] private AudioMixer _mixer;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private GameObject _settingsPanel;

        [SerializeField] private Slider _musicSlider;
		[SerializeField] private Slider _fxSlider;

		[Header("Inventory")]
		[SerializeField] private TextMeshProUGUI _crystal1Text;
		[SerializeField] private TextMeshProUGUI _crystal2Text;
		[SerializeField] private TextMeshProUGUI _crystal3Text;
		[Header("Interacytion")]
		[SerializeField] private InteractionPanel _interactionPanel;
		[Header("Live")]
		[SerializeField] private Slider _machineSlider;
		[SerializeField] private Slider _playerSlider;


        private bool _settingsOn = false;
		#endregion

		#region Unity Callbacks
		// Start is called before the first frame update
		void Start()
		{
            // Inicialmente, el "Settings Panel" y el "Die Panel" estarán desactivados:
            _settingsPanel.SetActive(false);
			_diePanel.gameObject.SetActive(false);

            //My Events
            _inventory.OnInventoryUpdated += UpdateUIInventory;
			_interactionSystem.OnShowInteraction += ShowInteraction;
			_interactionSystem.OnHideInteraction += HideInteraction;
			//Settings events
			_settingsButton.onClick.AddListener(SwitchSettings);
            // Cuando se haga clic en el botón "Confirm Settings Button", se llamará al método "ConfirmSettings()":
            _confirmSettingsButton.onClick.AddListener(ConfirmSettings);
			// Cuando se haga clic en el botón "Restart Button", se llamará al método "RestartFromBeginning()":
			_retryGameButton.onClick.AddListener(RetryFromBeginning);

            _musicSlider.onValueChanged.AddListener(MusicVolumeChange);
			_fxSlider.onValueChanged.AddListener(FXVolumeChange);
            // Para que, cuando tengas el cursor desbloqueado, no puedas modificar las barras de vida (a tu antojo):
            _machineSlider.interactable = false;
			_playerSlider.interactable = false;
        }

        // Metodo al que se acudirá cuando se haga clic en el botón "Restart Button":
        private void RetryFromBeginning()
        {
            // Destruye el "Game Controller" actual de la escena (debido al cambio de escenas):
            Destroy(GameController.Instance.gameObject);
			// La partida se vuelve a empezar de cero:
            SceneManager.LoadScene("InGame Scene");
        }

        private void SwitchSettings()
		{
            _settingsOn = !_settingsOn;
			_settingsPanel.SetActive(_settingsOn);
			// Que se desactive el botón del engranaje (para que no se vea):
			_settingsButton.gameObject.SetActive(!_settingsOn);

		}
		private void ConfirmSettings()
		{
            // Activa el botón del engranaje:
            _settingsButton.gameObject.SetActive(_settingsOn);
            // Desactiva el "Settings Panel":
            _settingsPanel.SetActive(!_settingsOn);

        }
		public void ShowDieText()
		{
			_diePanel.gameObject.SetActive(true);

        }

        internal void UpdateMachineHealth(float value)
		{
			_machineSlider.value = value;
		}

		internal void UpdatePlayerHealth(float value)
		{
			_playerSlider.value = value;
		}

		#endregion

		#region Public Methods
		public void ShowInteraction(string message)
		{
			_interactionPanel.Show(message);
		}
		public void HideInteraction()
		{
			_interactionPanel.Hide();

		}

		#endregion

		#region Private Methods
		private void UpdateUIInventory()
		{
			if (_inventory.InventoryStack.ContainsKey(RecolectableType.SuperCrystal))
				_crystal1Text.text = _inventory.InventoryStack[RecolectableType.SuperCrystal].ToString();
			else
				_crystal1Text.text = "0";
			if (_inventory.InventoryStack.ContainsKey(RecolectableType.HyperCrystal))
				_crystal2Text.text = _inventory.InventoryStack[RecolectableType.HyperCrystal].ToString();
			else
				_crystal2Text.text = "0";
			if (_inventory.InventoryStack.ContainsKey(RecolectableType.JumpCrystal))
                _crystal3Text.text = _inventory.InventoryStack[RecolectableType.JumpCrystal].ToString();
            else
				_crystal3Text.text = "0";
        }

		private void FXVolumeChange(float value)
		{
			_mixer.SetFloat("FXVolume", Mathf.Lerp(-60f, 0f, value));
		}

		private void MusicVolumeChange(float value)
		{
			_mixer.SetFloat("MusicVolume", Mathf.Lerp(-60f, 0f, value));

		}
		#endregion
	}

}