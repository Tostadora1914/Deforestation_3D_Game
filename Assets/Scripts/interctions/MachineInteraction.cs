using UnityEngine;
using System;
using System.Collections;
namespace Deforestation.Interaction
{
	public enum MachineInteractionType
	{
		Door,
		Stairs,
		Machine
	}
	public class MachineInteraction : MonoBehaviour, IInteractable
	{
		#region Properties
		#endregion

		#region Fields
		[SerializeField] protected MachineInteractionType _type;

        // La posición de la puerta abierta:
        [SerializeField] protected Transform _firstTarget;
        // La posición de la puerta cerrada:
        [SerializeField] protected Transform _secondTarget;
        // Tiempo para que la puerta se cierre automáticamente:
        private float _timeToCloseDoor = 4;
		// Bool para saber si la puerta está cerrada o abierta (al comienzo está cerrada):
		private bool _isOpened = false;

        [SerializeField] protected InteractableInfo _interactableInfo;


        #endregion

        #region Public Methods
        public InteractableInfo GetInfo()
		{
			_interactableInfo.Type = _type.ToString();
			return _interactableInfo;
		}

		public virtual void Interact()
		{
			if (_type == MachineInteractionType.Door)
			{
				//Move Door
				transform.position = _firstTarget.transform.position;
				// La puerta está abierta:
				_isOpened = true;
				// Que se inice la corrutina:
				StartCoroutine(CloseDoorAfterSomeTime());

            }
			if (_type == MachineInteractionType.Stairs)
			{
				//Teleport Player
				GameController.Instance.TeleportPlayer(_firstTarget.position);
			}
			if (_type == MachineInteractionType.Machine)
			{
				GameController.Instance.MachineMode(true);
			}
		}

        #endregion
        #region Private Methods
        // Corrutina para cerrar la puerta después de un tiempo:
        private IEnumerator CloseDoorAfterSomeTime()
		{
            // Esperar el tiempo que almacena la variable "_timeToCloseDoor":
            yield return new WaitForSeconds(_timeToCloseDoor);
			// Si la puerta está abierta, :
			if (_isOpened == true)
			{
                // Que la puerta vuelva a su primera posición (cerrada):
                transform.position = _secondTarget.transform.position;
                // La puerta está cerrada:
                _isOpened = false;
            
			}
        }
        #endregion
    }

}