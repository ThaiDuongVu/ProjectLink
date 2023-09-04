using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character
{
    [Header("UI References")]
    [SerializeField] private TMP_Text coinText;
    private int _collectedCoins;
    public int CollectedCoins
    {
        get => _collectedCoins;
        set
        {
            _collectedCoins = value;
            coinText.SetText(value.ToString());
        }
    }

    private static readonly int FallAnimationBool = Animator.StringToHash("isFalling");

    [Header("Position References")]
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem groundTrail;
    [SerializeField] private ParticleSystem airTrail;
    private bool _tempGrounded;

    #region Unity Event

    protected override void Start()
    {
        base.Start();

        _tempGrounded = !IsGrounded;
    }

    protected override void Update()
    {
        base.Update();

        Animator.SetBool(FallAnimationBool, IsFalling);

        if (IsGrounded != _tempGrounded)
        {
            if (IsGrounded)
            {
                groundTrail.Play();
                airTrail.Stop();
            }
            else
            {
                groundTrail.Stop();
                airTrail.Play();
            }

            _tempGrounded = IsGrounded;
        }

        HandlePlayerBorder();
    }

    #endregion

    #region Interface Implementations

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint) { }

    public override void Die() { }

    #endregion

    private void HandlePlayerBorder()
    {
        var position = transform.position;

        if (position.x < minPosition.x) transform.position = new Vector2(maxPosition.x, position.y);
        else if (position.x > maxPosition.x) transform.position = new Vector2(minPosition.x, position.y);

        if (position.y < minPosition.y) transform.position = new Vector2(position.x, maxPosition.y);
        else if (position.y > maxPosition.y) transform.position = new Vector2(position.x, minPosition.y);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            other.GetComponent<Collectible>().OnCollected(this);
        }
    }
}
