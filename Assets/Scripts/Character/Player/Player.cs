using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character
{
    [Header("Player UI References")]
    [SerializeField] private Image healthBar;
    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            healthBar.transform.localScale = new Vector2(value / baseHealth, 1f);
        }
    }

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

    private static readonly int HurtAnimationTrigger = Animator.StringToHash("hurt");

    private bool _movementDisabled;
    public bool MovementDisabled
    {
        get => _movementDisabled;
        set
        {
            _movementDisabled = value;
            _playerMovement.StopImmediate();
        }
    }

    private bool _combatDisabled;
    public bool CombatDisabled
    {
        get => _combatDisabled;
        set => _combatDisabled = value;
    }

    public int ComboMultiplier => _playerCombo.Multiplier;

    private Portal _enteredPortal;
    private const float PortalEnterInterpolationRatio = 0.25f;
    private static readonly int PortalEnterAnimationBool = Animator.StringToHash("isInPortal");

    public PlayerCombat PlayerCombat { get; private set; }
    private PlayerMovement _playerMovement;
    private PlayerCombo _playerCombo;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        PlayerCombat = GetComponent<PlayerCombat>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCombo = GetComponent<PlayerCombo>();
    }

    protected override void Start()
    {
        base.Start();

        var tempData = SaveLoadController.LoadTempData();
        Health = tempData != null ? tempData.playerHealth : baseHealth;
        CollectedCoins = tempData != null ? tempData.playerCollectedCoins : 0;

        MovementDisabled = CombatDisabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (_enteredPortal)
            transform.position = Vector3.Lerp(transform.position, _enteredPortal.transform.position, PortalEnterInterpolationRatio);
    }

    #endregion

    #region Interface Implementations

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {
        base.TakeDamage(damage, direction, contactPoint);

        _playerCombo.Cancel();

        Animator.SetTrigger(HurtAnimationTrigger);
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }

    public override void Die()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        GameController.Instance.PlaySlowMotionEffect();
        GameController.Instance.StartCoroutine(GameController.Instance.GameOver("You died"));

        base.Die();
    }

    #endregion

    private void EnterPortal(Portal portal)
    {
        _enteredPortal = portal;
        _enteredPortal.OnEnter();

        MovementDisabled = CombatDisabled = true;
        Animator.SetBool(PortalEnterAnimationBool, true);

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
    }

    private void ExitPortal()
    {
        _enteredPortal.OnExit();
        _enteredPortal = null;

        MovementDisabled = CombatDisabled = false;
        Animator.SetBool(PortalEnterAnimationBool, false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Collectible")) other.GetComponent<Collectible>().OnCollected(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal")) EnterPortal(other.GetComponent<Portal>());
    }
}
