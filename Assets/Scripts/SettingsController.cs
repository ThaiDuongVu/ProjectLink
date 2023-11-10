using System.Collections;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private SimpleButton fullscreenButton;
    private const string FullscreenKey = "Fullscreen";

    [SerializeField] private SimpleButton resolutionButton;
    private const string ResolutionKey = "Resolution";

    private const float BufferDuration = 0.2f;
    private bool _isBuffering;

    #region Unity Events

    private void Start()
    {
        Apply();
    }

    #endregion

    public void ToggleFullscreen()
    {
        if (_isBuffering) return;

        // Toggle settings
        var fullscreenValue = PlayerPrefs.GetInt(FullscreenKey, 0);
        PlayerPrefs.SetInt(FullscreenKey, fullscreenValue == 0 ? 1 : 0);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(ProcessBuffer());
    }

    public void ToggleResolution()
    {
        if (_isBuffering) return;

        // Toggle settings
        var resolutionValue = PlayerPrefs.GetInt(ResolutionKey, 2);
        if (resolutionValue < 2) PlayerPrefs.SetInt(ResolutionKey, resolutionValue + 1);
        else PlayerPrefs.SetInt(ResolutionKey, 0);

        // Apply settings
        Apply();

        _isBuffering = true;
        StartCoroutine(ProcessBuffer());
    }

    // Buffer period between setting switches
    private IEnumerator ProcessBuffer()
    {
        yield return new WaitForSecondsRealtime(BufferDuration);
        _isBuffering = false;
    }

    private void Apply()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        // 0: fullscreen
        // 1: windowed
        var fullscreenValue = PlayerPrefs.GetInt(FullscreenKey, 0);
        fullscreenButton.SetMainText(fullscreenValue == 0 ? "Fullscreen" : "Windowed");

        // 0: 640x360
        // 1: 1280x720
        // 3: 1920x1080
        var resolutionValue = PlayerPrefs.GetInt(ResolutionKey, 2);
        switch (resolutionValue)
        {
            case 0:
                Screen.SetResolution(640, 360, fullscreenValue == 0);
                resolutionButton.SetMainText("640 x 360");
                break;

            case 1:
                Screen.SetResolution(1280, 720, fullscreenValue == 0);
                resolutionButton.SetMainText("1280 x 720");
                break;

            case 2:
                Screen.SetResolution(1920, 1080, fullscreenValue == 0);
                resolutionButton.SetMainText("1920 x 1080");
                break;

            default:
                break;
        }
    }
}
