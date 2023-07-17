using UnityEngine;

public class DashLine : MonoBehaviour
{
    private LineRenderer _line0;
    private LineRenderer _line1;

    #region Unity Events

    private void Awake()
    {
        _line0 = GetComponentsInChildren<LineRenderer>()[0];
        _line1 = GetComponentsInChildren<LineRenderer>()[1];
    }

    #endregion

    public void SetDirection(Vector2 direction)
    {
        transform.right = direction;
    }

    public void SetLength(float length)
    {
        _line0.SetPosition(1, new Vector2(length / 2f, 0f));
        _line1.SetPosition(1, new Vector2(length / 2f, 0f));
        _line1.transform.localPosition = new Vector2(length / 2f, 0f);
    }

    public void SetColor(Color color)
    {
        _line0.startColor = _line0.endColor = _line1.startColor = _line1.endColor = color;
    }
}
