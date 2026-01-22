using UnityEngine;

namespace Deforestation.Dinosaurus
{
	public class Stegasaurus : Dinosaur
	{
		#region Fields
		[SerializeField] private float _radiusMovement = 100f;
		#endregion

		#region Properties
		#endregion

		#region Unity Callbacks	
		private void Start()
		{
			_health.OnHealthChanged += Damage;
		}

		void Update()
		{
			if (!_agent.pathPending)
			{ // Asegura que el agente haya calculado el camino
				if (_agent.remainingDistance <= _agent.stoppingDistance)
				{ // Comprueba si la distancia restante es menor que la distancia de parada
					if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
					{
						_anim.SetBool("Fleeing", false);

					}
				}
			}
            // Si la vida del Stegasaurus es 0, llama al método Die():
            if (_health.CurrentHealth == 0)
            {
                Die();
            }
        }

		#endregion

		#region Private Methods
		private void Damage(float health)
		{
			MoverAdestinoAleatorio();
		}
		void MoverAdestinoAleatorio()
		{
			if (_agent != null) // Lo puse yo.
			{
				Vector3 destinoAleatorio = Random.insideUnitSphere * _radiusMovement;
				destinoAleatorio += transform.position;
				UnityEngine.AI.NavMeshHit hit;
				if (UnityEngine.AI.NavMesh.SamplePosition(destinoAleatorio, out hit, _radiusMovement, 1)) 
				{
					if (_agent.isOnNavMesh == true) // Lo puse yo.
                        _agent.SetDestination(hit.position);
				}
				_anim.SetBool("Fleeing", true);
			}
		}

		#endregion

		#region Public Methods
		#endregion

	}
}
