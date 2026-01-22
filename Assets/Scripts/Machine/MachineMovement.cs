using Deforestation.Dinosaurus;
using Deforestation.Recolectables;
using System;
using UnityEngine;
namespace Deforestation.Machine
{
	public class MachineMovement : MonoBehaviour
	{
		#region Fields
		[SerializeField] private float _speedForce = 50;
		[SerializeField] private float _speedRotation = 15;
        private Rigidbody _rb;
		private Vector3 _movementDirection;
        private Inventory _inventory => GameController.Instance.Inventory;

		[Header("Energy")]
		[SerializeField] private float energyDecayRate = 20f;
		private float energyTimer = 0f;

        #endregion

        #region Properties
        #endregion

        #region Unity Callbacks	
        private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			
            if (_inventory.HasResource(RecolectableType.HyperCrystal))
			{
				//Movement
				_movementDirection = new Vector3(Input.GetAxis("Vertical"), 0, 0);
				//transform.Rotate(Vector3.up * _speedRotation * Time.deltaTime * Input.GetAxis("Horizontal"));
				transform.Rotate(Vector3.up * _speedRotation * Time.deltaTime * Input.GetAxis("Horizontal"));
				//Debug.DrawRay(transform.position, transform.InverseTransformDirection(_movementDirection.normalized) * _speedForce);

				//Energy
				if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
				{
					energyTimer += Time.deltaTime;
					if (energyTimer >= energyDecayRate)
					{
						_inventory.UseResource(RecolectableType.HyperCrystal);
						energyTimer = 0f;

					}
                }
			}
			else
			{
				GameController.Instance.MachineController.StopMoving();
			}
            CheckGround();
		}

        private void FixedUpdate()
		{
			//Original:
				//_rb.AddRelativeForce(_movementDirection.normalized * _speedForce, ForceMode.Impulse);
			_rb.AddRelativeForce(_movementDirection.normalized * _speedForce, ForceMode.Impulse);
        }

		void CheckGround()
		{
			float rayDistance = 5f;
			float jumpForce = 60000000;
            // La variable "IsGrounded" almacenará:
				// La posición actual de la Máquina.
				// La dirección hacia abajo.
				// La distancia que recorrerá el rayo.
			// Devuelve "true" si el rayo colisiona con el "Terrain".
            bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance);
            
			// Si tenemos el Cristal de Salto y estamos tocando el suelo, :
			if (_inventory.HasResource(RecolectableType.JumpCrystal) && IsGrounded)
            {
                // Solamente podremos saltar si, aparte de pulsar la barra espaciadora, estamos tocando el suelo:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // Se restará 1 Cristal de Salto:
                    _inventory.UseResource(RecolectableType.JumpCrystal);
                    // Se añadirá una fuerza hacia arriba para que la Máquina salte:
                    _rb.AddRelativeForce(Vector3.up * jumpForce);
                }
            }
            else
                return;
        }









        private void OnTriggerEnter(Collider other)
		{
			//if (other.tag == "Tree")
			//{
			//	int index = other.GetComponent<Tree>().Index;
			//	GameController.Instance.TerrainController.DestroyTree(index, other.transform.position);
			//}



            // La variable "target" almacenará el componente "HealthSystem" del objeto con el que hemos colisionado:
            HealthSystem target = other.gameObject.GetComponent<HealthSystem>();
            // Si "target" es null, salimos del método:
            if (target == null)
				return;
            // Si "target" es "TheMachine" o  el "Player", salimos del método:
            if (target == GameController.Instance.MachineController.HealthSystem || target == GameController.Instance.PlayerHealth)
				return;
            // Si "target" no es ninguno de los dos anteriores, le quitamos 10 de vida:
            else
                target.TakeDamage(10);

		}
        
		
		
		
		
		
		
		
		
		
		
		// ESTO CAUSABA QUE LA PROPIA MÁQUINA SE HIRIERA A SI MISMA, LO QUE CAUSABA SU MUERTE: YA SE PORQUÉ, ERA UN "OnCollisionEnter" Y NO UN OnTriggerEnter":
			//private void OnCoollisionEnter(Collider collision)
			//      {
			//	//Hacemos daño por contacto a los Stegasaurus
			//	HealthSystem target = collision.gameObject.GetComponent<HealthSystem>();
			//          if (target == null)
			//	{
			//		return;
			//	}
			//	if (target == GameController.Instance.MachineController.HealthSystem || target == GameController.Instance.PlayerHealth)
			//	{
			//		target.TakeDamage(0);
			//	}
			//	else
			//	{
			//              target.TakeDamage(10);
			//		//if (target != null)
			//		//{
			//		//	target.TakeDamage(10);
			//		//}
			//	}
			//}

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        #endregion
	}
	
}
