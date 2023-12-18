using System.Collections;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    #region Unity Events

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    #endregion

    public void SetText(string message)
    {
        text.SetText(message);
    }
}
