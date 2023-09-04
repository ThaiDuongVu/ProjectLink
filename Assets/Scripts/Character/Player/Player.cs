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
    }

    #endregion

    #region Interface Implementations

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint) { }

    public override void Die() { }

    #endregion

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            other.GetComponent<Collectible>().OnCollected(this);
        }
    }
}
