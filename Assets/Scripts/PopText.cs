using UnityEngine;
using TMPro;

public class PopText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float destroyDelay;

    #region Unity Events

    private void Awake()
    {
        Invoke(nameof(SelfDestruct), destroyDelay);
    }

    #endregion

    public void SetText(string message)
    {
        text.SetText(message);
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
