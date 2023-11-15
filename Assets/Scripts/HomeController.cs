using UnityEngine;

public class HomeController : MonoBehaviour
{
    #region Unity Events

    private void Start()
    {
        Application.targetFrameRate = 60;
        SetCursorEnabled(false);
        SetTimeScale(1f);
    }

    #endregion

    private static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    public static void SetTimeScale(float scale = 1f)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;
    }
}
