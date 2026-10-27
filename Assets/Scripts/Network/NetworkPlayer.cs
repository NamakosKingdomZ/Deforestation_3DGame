
using Deforestation.Interaction;
using Deforestation.Recolectables;
using Photon.Pun;
using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Deforestation.Network
{
	public class NetworkPlayer : MonoBehaviourPun
	{
		#region Fields

		[SerializeField] private HealthSystem _health;
		[SerializeField] private Inventory _inventory;
		[SerializeField] private InteractionSystem _interactions;
		[SerializeField] private CharacterController _controller;
		[SerializeField] private FirstPersonController _fps;
		[SerializeField] private StarterAssetsInputs _inputs;
		[SerializeField] private PlayerInput _inputsPlayer;
		[SerializeField] private GameObject _3dAvatar;
		[SerializeField] private Transform _playerFollow;
		private NetworkGameController _gameController;
		private Animator _anim;
		#endregion

		#region Properties
		#endregion

		#region Unity Callbacks	
		private void Awake()
		{
			_anim = _3dAvatar.GetComponent<Animator>();
		}
		private void Start()
		{
			_gameController = FindObjectOfType<NetworkGameController>(true);
			if (photonView.IsMine)
			{
				_gameController.InitializePlayer(_health, _controller, _inventory, _interactions, _playerFollow);
				_health.OnHealthChanged += Hit;
				_health.OnDeath += Die;
				_health.enabled = true;
				_inventory.enabled = true;
				_interactions.enabled = true;
				_fps.enabled = true;
				_controller.enabled = true;

				Invoke(nameof(AddInitialCrystals), 1);
				
			}
			else
			{
				DisconectPlayer();
			}
		}

		private void AddInitialCrystals()
		{
			_inventory.AddRecolectable(RecolectableType.SuperCrystal, 7);
			_inventory.AddRecolectable(RecolectableType.HyperCrystal, 3);
		}

		private void DisconectPlayer()
		{
			Destroy(_health);
			Destroy(_inventory);
			Destroy(_interactions);
			Destroy(_fps);
			Destroy(_controller);
			Destroy(_inputs);
			Destroy(_inputsPlayer);
		}

		private void Update()
		{
			if (photonView.IsMine)
			{
				//TODO: MOVER A inputcontroller
				if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
				{
					_anim.SetBool("Run", true);
				}
				else
				{
					_anim.SetBool("Run", false);
				}
				if (Input.GetKeyUp(KeyCode.Space))
					_anim.SetTrigger("Jump");
			}
		}

		#endregion

		#region Private Methods
		private void Die()
		{
			_anim.SetTrigger("Die");
			DisconectPlayer();
			this.enabled = false;

		}

		private void Hit(float obj)
		{
			_anim.SetTrigger("Hit");
		}
		#endregion

		#region Public Methods
		#endregion

	}

}