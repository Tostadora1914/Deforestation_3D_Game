using Deforestation.Dinosaurus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

namespace Deforestation.Dinosaurus
{

    public class Velociraptor : Dinosaur
    {
        #region Properties
        #endregion

        #region Fields
        // Distancia de detección de la Máquina:
        private float _machineDetectionDistance = 50;
        // Distancia de detección del Player:
        private float _playerAttackDistance = 15;
        private float _playerDetectionDistance = 25;

        // Booleana -> Huir:
        private bool _flee;
        // Booleana -> Perseguir al Player:
        private bool _chasePlayer;
        // Booleana -> Atacar al Player:
        private bool _attackPlayer;

        // Tiempo entre ataques ataque:
        [SerializeField] private float _attackTime = 2;
        // Daño de ataque:
        private float _attackDamage = 5f;

        // Cooldown de ataque:
        private float _attackColdDown;
        // El radio por el cual se pueden mover (para huir de la Máquina):
        private float _radiusMovement = 150;

        // Posición de la Máquina:
        private Vector3 _machinePosition => GameController.Instance.MachineController.transform.position;
        // Posición del Player:
        private Vector3 _playerPosition => GameController.Instance.Player.transform.position;
        #endregion

        #region Unity Callbacks
        // Start is called before the first frame update
        void Start()
        {
            // El cooldown tendrá el mismo valor que el tiempo de ataque:
            _attackColdDown = _attackTime;
        }

        // Update is called once per frame
        void Update()
        {
            // En la variable "distanceToMachine", se almacenará un vector (línea) que calculará la distancia entre la posición del Velociraptor y la posición de la Máquina:
            float distanceToMachine = Vector3.Distance(transform.position, _machinePosition);
            float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition);
            //Debug.Log("Velocidad del Velociraptor: " + _agent.velocity.magnitude);


            if (_health.CurrentHealth <= 0)
                Die();
            // Si la distancia a la Máquina es menor a la distancia de detección de la Máquina:
            if (distanceToMachine < _machineDetectionDistance)
            {
                // Si no estás huyendo, :
                if (!_flee) // Solo será "false" si ha pasado por el "else".
                {
                    // Ejecuta la función "FleeFromMachine()":
                    FleeFromMachine();
                }
            }
            // Si la distancia a la Máquina es mayor a la distancia de detección de la Máquina:
            else
            {
                // Si el agente existe (o está activado), y, el agente está en un "NavMesh", :
                // Si el "NavMesh" a calculado la ruta, y la distancia restante es menor o igual a la distancia de parada, :
                if (_agent.enabled && _agent.isOnNavMesh && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    // No estás huyendo:
                    _flee = false;
                    _anim.SetBool("Flee", false);

                }
            }

            if (distanceToPlayer < _playerDetectionDistance)
            {
                DetectPlayer();
            }
            else
            {
                _chasePlayer = false;
                _anim.SetBool("RunTowardPlayer", false);
                
            }
            //if (distanceToPlayer < _playerDetectionDistance)
            //{
            //    if (!_chasePlayer)
            //    {
            //        DetectPlayer();
            //    }
            //}
            //else
            //{
            //    _chasePlayer = false;
            //    _anim.SetBool("RunTowardPlayer", false);

            //}
            if (distanceToPlayer < _playerAttackDistance)
            {
                AttackPlayer(); 
            }
            else
            {
                if (_agent.enabled && _agent.isOnNavMesh)
                {
                    _agent.isStopped = false;
                    _attackPlayer = false;
                    _anim.SetBool("Attack", false);
                }
            }
        }

        //private void DetectPlayer()
        //{
        //    _chasePlayer = true;
        //    _anim.SetBool("RunTowardPlayer", true);
        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(_playerPosition, out hit, _playerDetectionDistance, 1))
        //        _agent.SetDestination(hit.position);
        //    _agent.SetDestination(_playerPosition);
        //    _flee = false;
        //    _attackPlayer = false;

        //}
        private void DetectPlayer()
        {
            _chasePlayer = true;
            _anim.SetBool("RunTowardPlayer", true);
            _agent.SetDestination(_playerPosition);
        }

        #endregion
        #region Private Methods
        private void AttackPlayer()
        {
            //_chasePlayer = false;
            //_flee = false;
            _attackPlayer = true;
            _anim.SetBool("RunTowardPlayer", false);
            _anim.SetBool("Attack", true);
            _agent.isStopped = true;
            _attackColdDown -= Time.deltaTime;
            if (_attackColdDown <= 0)
            {
                _attackColdDown = _attackTime;
                GameController.Instance.PlayerHealth.TakeDamage(_attackDamage);
                if (GameController.Instance.PlayerHealth.CurrentHealth == 0)
                {
                    GameController.Instance.PlayerHealth.Die();
                    _anim.SetBool("Attack", false);

                }

            }
        }
        private void FleeFromMachine()
        {
            _flee = true;
            Vector3 destiny = RandomPlace(transform.position, _radiusMovement);

            _agent.SetDestination(destiny);

            _anim.SetBool("Flee", true);
            _attackPlayer = false;
            _chasePlayer = false;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _machineDetectionDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _playerDetectionDistance);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _playerAttackDistance);

        }
        private Vector3 RandomPlace(Vector3 origin, float radius) // A esta función se la debe de pasar un vector3 (origen) y un float (radio).
        {
            // La variable "randomDirection" hará:
            // EL centro de la esfera será la posición del Velociraptor.
            // El radio de la esfera será inicialmente de 1.
            // Le pasaremos un valor (radio) para que el radio de búsqueda sea mayor/menor.
            Vector3 randomDirection = transform.position + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                if (_agent.isOnNavMesh == true)
                    // Devuelve la posición válida más cercana en el NavMesh:
                    return hit.position;
            }
            // Devuelve el origen si no encuentra una posición válida:
            return origin;
        }
        #endregion
    }
}
