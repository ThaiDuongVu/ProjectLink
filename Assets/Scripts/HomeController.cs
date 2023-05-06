using UnityEngine;

public class HomeController : MonoBehaviour
{
    #region Singleton

    private static HomeController _homeControllerInstance;

    public static HomeController Instance
    {
        get
        {
            if (_homeControllerInstance == null) _homeControllerInstance = FindObjectOfType<HomeController>();
            return _homeControllerInstance;
        }
    }

    #endregion

    #region Unity Events

    private void Start()
    {
        SaveLoadController.DeleteTempData();
    }

    #endregion
}
