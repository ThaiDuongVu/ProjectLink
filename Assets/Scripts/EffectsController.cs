using UnityEngine;

public class EffectsController : MonoBehaviour
{
    #region Singleton

    private static EffectsController _effectsControllerInstance;

    public static EffectsController Instance
    {
        get
        {
            if (_effectsControllerInstance == null) _effectsControllerInstance = FindObjectOfType<EffectsController>();
            return _effectsControllerInstance;
        }
    }

    #endregion
}
