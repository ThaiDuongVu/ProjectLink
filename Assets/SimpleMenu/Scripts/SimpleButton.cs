using UnityEngine;
using TMPro;

public class SimpleButton : MonoBehaviour
{
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text subText;

    public void SetMainText(string text)
    {
        mainText.SetText(text);
    }

    public void SetSubText(string text)
    {
        subText.SetText(text);
    }
}
