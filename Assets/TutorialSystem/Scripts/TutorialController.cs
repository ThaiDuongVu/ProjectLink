using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TutorialController : MonoBehaviour
{
    #region Singleton

    private static TutorialController _tutorialControllerInstance;

    public static TutorialController Instance
    {
        get
        {
            if (_tutorialControllerInstance == null) _tutorialControllerInstance = FindObjectOfType<TutorialController>();
            return _tutorialControllerInstance;
        }
    }

    #endregion

    [SerializeField] private GameObject tutorialDisplay;
    [SerializeField] private TMP_Text tutorialText;

    public bool IsTutorialDisplayed { get; set; }

    private TutorialInputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new TutorialInputManager();

        // Handle tutorial input
        _inputManager.Tutorial.Skip.performed += SkipOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager?.Disable();
    }

    private void Start()
    {
        tutorialDisplay.gameObject.SetActive(false);
    }

    #endregion

    #region Input Handlers

    private void SkipOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance?.State != GameState.InProgress) return;

        HideTutorial();
    }

    #endregion

    public void DisplayTutorial(string message)
    {
        if (PlayerPrefs.GetInt("ShowTutorial") == 1) return;
        if (IsTutorialDisplayed) return;

        tutorialText.text = message;
        tutorialDisplay.gameObject.SetActive(true);
        IsTutorialDisplayed = true;

        PostProcessingController.Instance.SetDepthOfField(true);
        GameController.SetTimeScale(0f);
    }

    public void HideTutorial()
    {
        if (!IsTutorialDisplayed) return;

        tutorialDisplay.gameObject.SetActive(false);
        IsTutorialDisplayed = false;

        PostProcessingController.Instance.SetDepthOfField(false);
        GameController.SetTimeScale(1f);
    }
}
