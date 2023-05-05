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

    [Header("Pop Text Reference")]
    [SerializeField] private PopText popTextPrefab;

    public void SpawnPopText(Vector2 position, Color color, string message = "")
    {
        var popText = Instantiate(popTextPrefab, position, Quaternion.identity);

        popText.SetText(message);
        popText.SetColor(color);
    }
}
