using UnityEngine;

public class EffectsController : MonoBehaviour
{
    #region Singleton

    private static EffectsController _effectsControllerInstance;

    public static EffectsController Instance
    {
        get
        {
            if (_effectsControllerInstance == null) _effectsControllerInstance = FindFirstObjectByType<EffectsController>();
            return _effectsControllerInstance;
        }
    }

    #endregion

    [SerializeField] private PopText popTextPrefab;
    [SerializeField] private SpeechBubble speechBubblePrefab;

    public void SpawnPopText(Vector2 position, string message, Color color)
    {
        var text = Instantiate(popTextPrefab, position, Quaternion.identity);
        text.SetText(message);
        text.SetColor(color);
    }

    public void SpawnSpeechBubble(Transform parent, Vector2 localPosition, string message)
    {
        var speechBubble = Instantiate(speechBubblePrefab, transform.position, Quaternion.identity);
        speechBubble.SetText(message);
        speechBubble.transform.SetParent(parent);
        speechBubble.transform.localPosition = localPosition;
    }
}
