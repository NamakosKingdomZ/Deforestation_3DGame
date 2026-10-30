using UnityEngine;
using TMPro;
using Deforestation.Recolectables;
using System;
using Deforestation.Interaction;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Deforestation.UI
{
	public class UIGameController : MonoBehaviour
	{
		#region Properties
		#endregion

		#region Fields
		private Inventory _inventory;
		private InteractionSystem _interactionSystem;

		[Header("Settings")]
		[SerializeField] private AudioMixer _mixer;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private GameObject _settingsPanel;
		[SerializeField] private Slider _musicSlider;
		[SerializeField] private Slider _fxSlider;

		[Header("Inventory")]
		[SerializeField] private TextMeshProUGUI _crystal1Text;
		[SerializeField] private TextMeshProUGUI _crystal2Text;
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
			if (_settingsPanel != null)
				_settingsPanel.SetActive(false);

			//My Events
			if (Singleton<GameController>.TryGetInstance(out GameController gameController))
			{
				_inventory = gameController.Inventory;
				_interactionSystem = gameController.InteractionSystem;
				if (_inventory != null)
					_inventory.OnInventoryUpdated += UpdateUIInventory;
				if (_interactionSystem != null)
				{
					_interactionSystem.OnShowInteraction += ShowInteraction;
					_interactionSystem.OnHideInteraction += HideInteraction;
				}
			}
			//Settings events
			if (_settingsButton != null)
				_settingsButton.onClick.AddListener(SwitchSettings);
			if (_musicSlider != null)
				_musicSlider.onValueChanged.AddListener(MusicVolumeChange);
			if (_fxSlider != null)
				_fxSlider.onValueChanged.AddListener(FXVolumeChange);
		}		

		private void SwitchSettings()
		{
			_settingsOn = !_settingsOn;
			if (_settingsPanel != null)
				_settingsPanel.SetActive(_settingsOn);
		}

		internal void UpdateMachineHealth(float value)
		{
			if (_machineSlider != null)
				_machineSlider.value = value;
		}

		internal void UpdatePlayerHealth(float value)
		{
			if (_playerSlider != null)
				_playerSlider.value = value;
		}

		#endregion

		#region Public Methods
		public void ShowInteraction(string message)
		{
			if (_interactionPanel != null)
				_interactionPanel.Show(message);
		}
		public void HideInteraction()
		{
			if (_interactionPanel != null)
				_interactionPanel.Hide();

		}

		#endregion

		#region Private Methods
		private void UpdateUIInventory()
		{
			if (_inventory == null)
				return;

			if (_crystal1Text != null && _inventory.InventoryStack.ContainsKey(RecolectableType.SuperCrystal))
				_crystal1Text.text = _inventory.InventoryStack[RecolectableType.SuperCrystal].ToString();
			else if (_crystal1Text != null)
				_crystal1Text.text = "0";
			if (_crystal2Text != null && _inventory.InventoryStack.ContainsKey(RecolectableType.HyperCrystal))
				_crystal2Text.text = _inventory.InventoryStack[RecolectableType.HyperCrystal].ToString();
			else if (_crystal2Text != null)
				_crystal2Text.text = "0";
		}

		private void FXVolumeChange(float value)
		{
			if (_mixer != null)
				_mixer.SetFloat("FXVolume", Mathf.Lerp(-60f, 0f, value));
		}

		private void MusicVolumeChange(float value)
		{
			if (_mixer != null)
				_mixer.SetFloat("MusicVolume", Mathf.Lerp(-60f, 0f, value));

		}
		#endregion
	}

}
