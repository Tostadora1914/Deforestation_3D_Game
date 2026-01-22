using System;
using UnityEngine;

namespace Deforestation
{

	public class HealthSystem : MonoBehaviour
	{
		//public float CurrentHealth => _currentHealth;
        public event Action<float> OnHealthChanged;
		public event Action OnDeath;
		public float CurrentHealth => _currentHealth;

        [SerializeField]
		public float MaxHealth = 100f;
		private float _currentHealth;

		private void Awake()
		{
            _currentHealth = MaxHealth;
		}

		public void TakeDamage(float damage)
		{
            _currentHealth -= damage;
			OnHealthChanged?.Invoke(CurrentHealth);

			if (CurrentHealth <= 0)
			{
				Die();
			}
		}

		public void Heal(float amount)
		{
            _currentHealth += amount;
            _currentHealth = Mathf.Min(CurrentHealth, MaxHealth);
			OnHealthChanged?.Invoke(CurrentHealth);
		}

		public void SetHealth(float value)
		{
            _currentHealth = value;
            _currentHealth = Mathf.Min(CurrentHealth, MaxHealth);
			OnHealthChanged?.Invoke(CurrentHealth);
		}
		// La función de este método es:
			// Quitar cualquier tipo de movimiento al Player.
		public void WithoutPlayerControl()
		{
            //Desactivamos el script de movimeinto del Player:
				// Es cierto que dijisteis que no sería lo suyo modificar los scripts que provengan de assets externos.
            GameController.Instance.Player.MoveSpeed = 0;
            GameController.Instance.Player.RotationSpeed = 0;
            GameController.Instance.Player.SprintSpeed = 0;
        }
        // La función de este método es:
			// Quitar cualquier tipo de movimiento a "TheMachine".
        public void WithoutMachineControl()
		{
            //Desactivamos el script de movimeinto de "TheMachine":
            GameController.Instance.MachineMovement.enabled = false;
			GameController.Instance.MachineController.WeaponController.enabled = false;
        }
        public void Die()
		{
			if (GameController.Instance.MachineController.HealthSystem.CurrentHealth == 0)
			{
                OnDeath?.Invoke();
                // Impedimos el movimiento a "TheMachine":
                WithoutMachineControl();
                // La vida actual será 0 (para que no sea negativa (no sigua bajando la vida)):
                _currentHealth = 0;
                // Si alguien llama a este método, se activará el "Die Panel":
                GameController.Instance.UIGameController.ShowDieText();
				// Se te desbloquea el cursor, para poder hacer clic al botón:
				Cursor.lockState = CursorLockMode.None;
                return;

            }
            // Solamente podrás morir si tu vida actual es menor o igual 0, :
            if (GameController.Instance.PlayerHealth.CurrentHealth == 0)
            {
                OnDeath?.Invoke();
				// Impedimos el movimiento al Player:
				WithoutPlayerControl();
                // La vida actual será 0 (para que no sea negativa (no sigua bajando la vida)):
                _currentHealth = 0;
                // Si alguien llama a este método, se activará el "Die Panel":
                GameController.Instance.UIGameController.ShowDieText();
                // Se te desbloquea el cursor, para poder hacer clic al botón:
                Cursor.lockState = CursorLockMode.None;
                return;

            }
            OnDeath?.Invoke();
            _currentHealth = 0;
        }
    }

}