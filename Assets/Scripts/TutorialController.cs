using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    #region Singleton

    private static TutorialController _TutorialControllerInstance;

    public static TutorialController Instance
    {
        get
        {
            if (_TutorialControllerInstance == null) _TutorialControllerInstance = FindObjectOfType<TutorialController>();
            return _TutorialControllerInstance;
        }
    }

    #endregion
    
    [SerializeField] private GameObject[] tutorialDisplays;
    private int _tutorialPhase;
    private bool _isTutorialActive;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        // Handle fire input
        _inputManager.Player.Fire.performed += FireOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private IEnumerator Start()
    {
        for (int i = 0; i < tutorialDisplays.Length; i++) tutorialDisplays[i].SetActive(false);
        tutorialDisplays[_tutorialPhase].SetActive(true);

        yield return new WaitForSeconds(2f);
        _isTutorialActive = true;
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        if (_tutorialPhase == 0 && _isTutorialActive) AdvanceTutorial();
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
    }

    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        if (_tutorialPhase == 1) AdvanceTutorial();
    }

    #endregion

    private void AdvanceTutorial()
    {
        if (_tutorialPhase < tutorialDisplays.Length - 1) _tutorialPhase++;

        for (int i = 0; i < tutorialDisplays.Length; i++) tutorialDisplays[i].SetActive(false);
        tutorialDisplays[_tutorialPhase].SetActive(true);
    }
}
