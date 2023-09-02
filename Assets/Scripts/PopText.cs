using UnityEngine;
using TMPro;

public class PopText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetMessage(string message="")
    {
        text.SetText(message);
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }
}
