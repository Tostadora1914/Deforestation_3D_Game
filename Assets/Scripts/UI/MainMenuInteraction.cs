using Deforestation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuInteraction : MonoBehaviour
{
    #region Properties
    #endregion

    #region Fields
    // Referencias a los botones del "Main Menu":
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _exitGameButton;
    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Activar el canvas del "Main Menu" al iniciar la escena:
        _startGameButton.onClick.AddListener(StartGame);
        _exitGameButton.onClick.AddListener(ExitGame);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("InGame Scene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}

