using System.Collections;
using TMPro;
using UnityEngine;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNameText;

    private Level[] _allLevels;
    private int _selectedLevelIndex;

    private Level SelelctedLevel => _allLevels[_selectedLevelIndex];

    private const float BufferDuration = 0.1f;
    private bool _isBuffering;

    #region Unity Events

    private void Awake()
    {
        _allLevels = GetComponentsInChildren<Level>(true);
    }

    private void Start()
    {
        _selectedLevelIndex = 0;
        UpdateLevelPreview();

        Application.targetFrameRate = 60;
        GameController.SetCursorEnabled(false);
        GameController.SetTimeScale(1f);
    }

    #endregion

    private void UpdateLevelPreview()
    {
        for (var i = 0; i < _allLevels.Length; i++) _allLevels[i].gameObject.SetActive(i == _selectedLevelIndex);
        levelNameText.SetText($"< Level {_selectedLevelIndex + 1} >");
    }

    public void SelectNextLevel()
    {
        if (_isBuffering) return;

        if (_selectedLevelIndex + 1 <= _allLevels.Length - 1) _selectedLevelIndex++;
        else _selectedLevelIndex = 0;

        UpdateLevelPreview();

        _isBuffering = true;
        StartCoroutine(ProcessBuffer());
    }

    public void SelectPreviousLevel()
    {
        if (_isBuffering) return;

        if (_selectedLevelIndex - 1 >= 0) _selectedLevelIndex--;
        else _selectedLevelIndex = _allLevels.Length - 1;

        UpdateLevelPreview();

        _isBuffering = true;
        StartCoroutine(ProcessBuffer());
    }

    public void LoadSelectedLevel()
    {
        SceneLoader.Instance.Load(SelelctedLevel.name);
    }

    // Buffer period between setting switches
    private IEnumerator ProcessBuffer()
    {
        yield return new WaitForSecondsRealtime(BufferDuration);
        _isBuffering = false;
    }
}
