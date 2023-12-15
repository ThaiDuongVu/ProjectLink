using UnityEngine;
using UnityEngine.UI;

public class RatingDisplay : MonoBehaviour
{
    private Image[] _icons;

    #region Unity Events

    private void Awake()
    {
        _icons = GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    #endregion

    public void UpdateRating(int rating)
    {
        gameObject.SetActive(false);
        for (int i = 0; i < rating; i++) _icons[i].gameObject.SetActive(true);
        for (int i = rating; i < 3; i++) _icons[i].gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
