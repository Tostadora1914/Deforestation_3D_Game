using Deforestation;
using Deforestation.Dinosaurus;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Deforestation
{
    public class Water : MonoBehaviour
    {
        #region Properties
        #endregion

        #region Fields
        private HealthSystem _playerHealth;
        private HealthSystem _dinosaurHealth;
        #endregion

        #region Unity Callbacks
        // Start is called before the first frame update
        void Awake()
        {
            // TODO: Por ahora, no hay dinosaurios ->   Velociraptor velociraptor = GameObject.FindObjectOfType<Velociraptor>();
            // TODO: Por ahora, no hay dinosaurios ->   _dinosaurHealth = velociraptor.gameObject.GetComponent<HealthSystem>();
            // Si no hay un Player, regresa:
            if (_playerHealth == null)
                return;
            else
                _playerHealth = GameController.Instance?.PlayerHealth.GetComponent<HealthSystem>();
        }

        // Update is called once per frame
        void Start()
        {
            
        }
        void Update()
        {

        }
        private void OnTriggerStay(Collider other)
        {
            //TODO: Error en la línea 43: Object reference not set to an instance of an object (me parece que se debe a que se inicia este script ANTES de que se asigne el PlayerHealth en GameController).
            //if (other.CompareTag("Player") && _playerHealth.CurrentHealth <= 100f)
            //{
            //    _playerHealth.TakeDamage(1f);
            //    if (_playerHealth.CurrentHealth == 1)
            //    {
            //        GameController.Instance.DieMessage();
            //        _playerHealth.SetHealth(0);
            //    }
            //}
            //if (other.CompareTag("Dinosaur") && _dinosaurHealth.CurrentHealth <= 200f)
            //{
            //    _dinosaurHealth.TakeDamage(1f);
            //    if (_dinosaurHealth != null && _dinosaurHealth.CurrentHealth == 1)
            //    {
            //        _dinosaurHealth.SetHealth(0);
            //    }

            //}

        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
