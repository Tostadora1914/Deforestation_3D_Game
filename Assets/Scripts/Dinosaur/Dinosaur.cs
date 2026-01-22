using System;
using UnityEngine;
using UnityEngine.AI;

namespace Deforestation.Dinosaurus
{

	public class Dinosaur : MonoBehaviour
	{
		#region Fields
		protected Animator _anim;
		protected NavMeshAgent _agent;
		protected HealthSystem _health;
		
		#endregion

		#region Properties
		public HealthSystem Health => _health;
		#endregion

		#region Unity Callbacks	
		private void Awake()
		{
			_health = GetComponent<HealthSystem>();
			_anim = GetComponent<Animator>();
			_agent = GetComponent<NavMeshAgent>();

			_health.OnDeath += Die;
		}

		public void Die() // Cuando matabas al cualquier dinosaurio, y le volvias a disparar (estando muerto), volvia a ejecutar la animación de muerte (desde el principio).
		{
			if (_health != null)
			{
				_anim.SetTrigger("Die");
				Destroy(_agent);
				Destroy(this);
				// Para que se destruya el gameObject (cualquier dinosaurio que herede de este script) después de 1 segundo:
				if (_agent != null)
					Destroy(gameObject, 1f);

			}
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        #endregion

    }

}