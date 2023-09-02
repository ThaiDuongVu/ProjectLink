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

    [SerializeField] private PopText popTextPrefab;

    public void SpawnPopText(Vector2 position, string message, Color color, float duration = 0.5f)
    {
        var popText = Instantiate(popTextPrefab, position, Quaternion.identity);
        popText.SetMessage(message);
        popText.SetColor(color);
        Destroy(popText.gameObject, duration);
    }
}
