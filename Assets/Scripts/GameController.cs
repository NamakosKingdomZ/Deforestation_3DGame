using UnityEngine;
using Deforestation.Machine;
using Deforestation.UI;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Cinemachine;
using System;
using JetBrains.Annotations;

namespace Deforestation
{
	public class GameController : Singleton<GameController>
	{
		#region Properties
		public MachineController MachineController => _machine;
		public Inventory Inventory => _inventory;
		public InteractionSystem InteractionSystem => _interactionSystem;
		public TreeTerrainController TerrainController => _terrainController;
		public Camera MainCamera;
		public GameObject FakeWater => _fakeWater;

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
		[SerializeField] protected CharacterController _player;
		[SerializeField] protected HealthSystem _playerHealth;
		[SerializeField] protected Inventory _inventory;
		[SerializeField] protected InteractionSystem _interactionSystem;
		[SerializeField] private Transform _playerSpawn;


		[Header("Camera")]
		[SerializeField] protected CinemachineVirtualCamera _virtualCamera;
		[SerializeField] protected Transform _playerFollow;
		[SerializeField] protected Transform _machineFollow;

		[Header("Machine")]
		[SerializeField] protected MachineController _machine;
		[SerializeField] private Transform _machineSpawn;

		[Header("UI")]
		[SerializeField] protected UIGameController _uiController;
		[Header("Trees Terrain")]
		[SerializeField] protected TreeTerrainController _terrainController;
		[SerializeField] protected GameObject _fakeWater;
		[SerializeField] private float _waterHeight;





		private bool _machineModeOn;
		#endregion

		#region Unity Callbacks
		// Start is called before the first frame update
		void Start()
		{
			//UI Update
			_playerHealth.OnHealthChanged += _uiController.UpdatePlayerHealth;
			_machine.HealthSystem.OnHealthChanged += _uiController.UpdateMachineHealth;
			MachineModeOn = false;

		}

		// Update is called once per frame
		void Update()
		{
			//Water 
			if (_player.transform.position.y < _waterHeight)
			{
				RespawnPlayer();
			}

			if (_machine.transform.position.y < _waterHeight)
			{
				RespawnMachine();
			}

		}
		#endregion

		#region Public Methods
		public void TeleportPlayer(Vector3 target)
		{
			_player.enabled = false;
			_player.transform.position = target;
			_player.enabled = true;
		}

		internal void MachineMode(bool machineMode)
		{
			MachineModeOn = machineMode;
			//Player
			_player.gameObject.SetActive(!machineMode);
			_player.enabled = !machineMode;

			//Cursor + UI
			if (machineMode)
			{
				//Start Driving
				if (Inventory.HasResource(RecolectableType.HyperCrystal))
					_machine.StartDriving(machineMode);

				_player.transform.parent = _machineFollow;
				_uiController.HideInteraction();
				Cursor.lockState = CursorLockMode.None;
				//Camera
				_virtualCamera.Follow = _machineFollow;

				_machine.enabled = true;
				_machine.WeaponController.enabled = true;
				_machine.GetComponent<MachineMovement>().enabled = true;

			}
			else
			{
				_machine.enabled = false;
				_machine.WeaponController.enabled = false;
				_machine.GetComponent<MachineMovement>().enabled = false;
				_player.transform.parent = null;

				//Camera
				_virtualCamera.Follow = _playerFollow;
				Cursor.lockState = CursorLockMode.Locked;

			}
			Cursor.visible = machineMode;
		}



		private void RespawnPlayer()
		{
			Rigidbody rb = _player.GetComponent<Rigidbody>();

			if (rb != null)
			{
				rb.position = _playerSpawn.position;
				rb.rotation = _playerSpawn.rotation;
				rb.angularVelocity = Vector3.zero;
			}
			else
			{
				_player.transform.position = _playerSpawn.position;
				_player.transform.rotation = _playerSpawn.rotation;
			}
		}

		public void RespawnMachine()
		{
			Rigidbody rb = _machine.GetComponent<Rigidbody>();

			if (rb != null)
			{
				rb.position = _machineSpawn.position;
				rb.rotation = _machineSpawn.rotation;
				rb.angularVelocity = Vector3.zero;
			}
			else
			{
				_machine.transform.position = _machineSpawn.position;
				_machine.transform.rotation = _machineSpawn.rotation;
			}







			#endregion

			#region Private Methods

			#endregion

		}

	}
}