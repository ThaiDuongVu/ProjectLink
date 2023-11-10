using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    #region Singleton

    private static GameController _gameControllerInstance;

    public static GameController Instance
    {
        get
        {
            if (_gameControllerInstance == null) _gameControllerInstance = FindObjectOfType<GameController>();
            return _gameControllerInstance;
        }
    }

    #endregion

    public GameState State { get; set; }

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player pause/resume input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion

    #region Input Handlers

    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        if (State == GameState.InProgress) Pause();
        else if (State == GameState.Paused) Resume();
    }

    #endregion

    #region Pause/Resume Methods

    public void Pause()
    {
        Time.timeScale = 0f;
        PostProcessingController.Instance.SetDepthOfField(true);

        // pauseMenu.SetActive(true);
        State = GameState.Paused;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PostProcessingController.Instance.SetDepthOfField(false);

        // pauseMenu.SetActive(false);
        State = GameState.InProgress;
    }

    #endregion
}
