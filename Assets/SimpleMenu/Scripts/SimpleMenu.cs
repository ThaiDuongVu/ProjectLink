using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SimpleMenu : MonoBehaviour
{
    [SerializeField] private bool disableOnStart;

    private SimpleButton[] _buttons;

    private EventSystem _eventSystem;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();
        _inputManager.UI.Enter.performed += EnterOnPerformed;
        _inputManager.Enable();

        StartCoroutine(SelectFirstButton());
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        _buttons = GetComponentsInChildren<SimpleButton>();
        _eventSystem = EventSystem.current;
    }

    private void Start()
    {
        SetActive(!disableOnStart);
    }

    #endregion

    #region Input Handlers

    private void EnterOnPerformed(InputAction.CallbackContext context)
    {
        _eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }

    #endregion

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    private IEnumerator SelectFirstButton()
    {
        yield return new WaitForEndOfFrame();

        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_buttons[0].gameObject);
    }
}
