using UnityEngine;

public class HomeController : MonoBehaviour
{
    #region Unity Events

    private void Start()
    {
        Application.targetFrameRate = 60;
        GameController.SetTimeScale(1f);
    }

    #endregion
}
