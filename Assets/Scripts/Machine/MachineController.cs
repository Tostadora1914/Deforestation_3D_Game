using UnityEngine;
using System;
using Deforestation.Machine.Weapon;

namespace Deforestation.Machine
{
	[RequireComponent(typeof(HealthSystem))]
	public class MachineController : MonoBehaviour
	{
		#region Properties
		public HealthSystem HealthSystem => _health;
		public WeaponController WeaponController;
		public Action<bool> OnMachineDriveChange;

		#endregion

		#region Fields
		private HealthSystem _health;
		private MachineMovement _movement;
		private Animator _anim;
        // Ángulo máximo para que la máquina explote(al estar volcada).
        private float _explosionAngle = 140f;

        #endregion

        #region Unity Callbacks
        private void Awake()
		{
			_health = GetComponent<HealthSystem>();
			_movement = GetComponent<MachineMovement>();
			_anim = GetComponent<Animator>();
		}
		// Start is called before the first frame update
		void Start()
		{
			_movement.enabled = false;
		}

		// Update is called once per frame
		void Update()
		{
            // Si se presiona la tecla "º", dejarás de estar en Modo Máquina:
            if (Input.GetKeyUp(KeyCode.BackQuote))
				StopDriving();

            // En la variable "rightAngle", se almacenará el ángulo actual, entre el "up" del mundo (Vector3.up) y el "up" de la máquina (transform.up):
            float rightAngle = Vector3.Angle(Vector3.up, transform.up);
            //Debug.Log("Ángulo de la máquina: " + rightAngle);

			// Si el ángulo normal es mayor o igual al de la explosión, :
            if (rightAngle >= _explosionAngle)
				// "TheMachine" morirá:
                GameController.Instance.DieMessage();
        }

        #endregion

        #region Public Methods
        public void StopDriving()
		{
			GameController.Instance.MachineMode(false);
			StopMoving();
			OnMachineDriveChange?.Invoke(false);

		}

		public void StartDriving(bool machineMode)
		{
			enabled = machineMode;
			_movement.enabled = machineMode;
			_anim.SetTrigger("WakeUp");
			_anim.SetBool("Move", machineMode);
			OnMachineDriveChange?.Invoke(true);
		}

		public void StopMoving()
		{
			_movement.enabled = false;
			_anim.SetBool("Move", false);
		}

		#endregion

		#region Private Methods
		#endregion
	}

}