using UnityEngine;
using TMPro;

public class PlayerCombo : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float comboDuration;
    private Timer _timer;
    public int Multiplier { get; protected set; }

    [Header("References")]
    [SerializeField] private TMP_Text text;
    private RectTransform _textTransform;
    private const float TextScaleInterpolationRatio = 0.2f;

    private bool _isPaused;

    private void Awake()
    {
        _textTransform = text.GetComponent<RectTransform>();
    }

    private void Start()
    {
        Cancel();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (Multiplier <= 1) return;
        
        _textTransform.localScale = Vector2.Lerp(_textTransform.localScale,
            Vector2.one * (_timer.Progress / _timer.MaxProgress), TextScaleInterpolationRatio);

        if (!_isPaused && _timer.IsReached()) Cancel();
    }

    public void Add(int amount = 1)
    {
        Multiplier += amount;
        _timer = new Timer(comboDuration);

        _textTransform.localRotation = new Quaternion(0f, 0f, Random.Range(-0.2f, 0.2f), 1f);
        text.SetText($"x{Multiplier.ToString()}");
    }

    public void Cancel()
    {
        Multiplier = 1;
        _timer = new Timer(comboDuration);
        _textTransform.localScale = Vector2.zero;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Unpause()
    {
        _isPaused = false;
    }
}