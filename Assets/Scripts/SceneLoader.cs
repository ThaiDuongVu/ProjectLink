using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton

    private static SceneLoader _sceneLoaderInstance;

    public static SceneLoader Instance
    {
        get
        {
            if (_sceneLoaderInstance == null) _sceneLoaderInstance = FindFirstObjectByType<SceneLoader>();
            return _sceneLoaderInstance;
        }
    }

    #endregion

    private Animator _cameraAnimator;
    private static readonly int OutroTrigger = Animator.StringToHash("outro");
    [SerializeField] private AnimationClip cameraOutroAnimationClip;
    [SerializeField] private AudioSource loadLevelAudio;

    private string _sceneToLoad = "";

    #region Unity Event

    private void Awake()
    {
        if (Camera.main is { }) _cameraAnimator = Camera.main.GetComponent<Animator>();
    }

    #endregion

    private IEnumerator Load()
    {
        // Load scene in background but don't allow transition
        // var asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
        // asyncOperation.allowSceneActivation = false;

        // Play camera animation
        _cameraAnimator.SetTrigger(OutroTrigger);

        // Wait for camera animation to complete
        yield return new WaitForSecondsRealtime(cameraOutroAnimationClip.averageDuration);

        // Allow transition to new scene
        // asyncOperation.allowSceneActivation = true;
        SceneManager.LoadScene(_sceneToLoad, LoadSceneMode.Single);
    }

    public void Load(string scene)
    {
        _sceneToLoad = scene;
        loadLevelAudio.Play();
        StartCoroutine(Load());
    }

    public void Restart()
    {
        Load(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        var nextLevelIndex = Convert.ToInt32(SceneManager.GetActiveScene().name[5..]) + 1;
        var nextLevelName = $"Level{(nextLevelIndex < 10 ? "0" : "")}{nextLevelIndex}";
        Load(nextLevelName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}