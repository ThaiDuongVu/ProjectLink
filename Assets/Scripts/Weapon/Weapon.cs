using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Weapon General Stats")]
    public WeaponFireMode fireMode;
    public float range;
    public int fireRate;

    [Header("Hitscan Damage Stats")]
    public float bodyDamagePerShot;
    public float headDamagePerShot;
    public float knockbackPerShot;

    [Header("Weapon References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private SpriteRenderer crosshair;
    [SerializeField] private LineRenderer hitLine;

    [Header("Projectile References")]
    [SerializeField] private Projectile projectilePrefab;

    private bool _canFire = true;
    private Timer _fireTimer;

    [Header("Ammo References")]
    public int maxAmmo;
    public int ammoRechargeTime;
    private Timer _ammoRechargeTimer;
    [SerializeField] private Transform ammoDisplay;
    private Image[] _ammoIcons;
    private int _currentAmmo;
    public int CurrentAmmo
    {
        get => _currentAmmo;
        set
        {
            _currentAmmo = value;
            for (var i = 0; i < value; i++) _ammoIcons[i].gameObject.SetActive(true);
            for (var i = value; i < _ammoIcons.Length; i++) _ammoIcons[i].gameObject.SetActive(false);
        }
    }

    [Header("Name References")]
    public new string name;
    [SerializeField] private TMP_Text nameText;

    [Header("Color References")]
    public Color regularColor;
    public Color hitColor;

    public Color CrosshairColor => crosshair.color;

    public IDamageable CurrentDamageTarget { get; protected set; }
    public Zombie CurrentZombieTarget { get; protected set; }
    private Vector2 _currentDamageTargetContactPoint;
    private IKnockbackable _currentKnockbackTarget;

    #region Unity Events

    protected virtual void Awake()
    {
        _ammoIcons = ammoDisplay.GetComponentsInChildren<Image>();
    }

    protected virtual void Start()
    {
        crosshair.transform.position = transform.position + transform.up * range;

        hitLine.SetPosition(0, firePoint.localPosition);
        hitLine.SetPosition(1, crosshair.transform.localPosition);
        hitLine.gameObject.SetActive(false);

        CurrentAmmo = maxAmmo;
        _ammoRechargeTimer = new Timer(ammoRechargeTime);

        nameText.SetText(name);
    }

    protected virtual void Update()
    {
        if (!_canFire && _fireTimer.IsReached()) _canFire = true;

        GetDamageableFromHitscan();

        if (fireMode != WeaponFireMode.Projectile)
            crosshair.color = hitLine.startColor = hitLine.endColor = CurrentDamageTarget == null ? regularColor : hitColor;

        if (_ammoRechargeTimer.IsReached())
        {
            if (CurrentAmmo < maxAmmo) CurrentAmmo++;
            _ammoRechargeTimer.Reset();
        }
    }

    #endregion

    #region Fire Methods

    public virtual void Fire()
    {
        if (!_canFire) return;
        if (CurrentAmmo <= 0) return;

        switch (fireMode)
        {
            case WeaponFireMode.NonAutomatic:
                FireNonAutomatic();
                break;

            case WeaponFireMode.Projectile:
                FireProjectile();
                break;

            default:
                return;
        }

        _canFire = false;
        _fireTimer = new Timer(1f / fireRate);
    }

    public virtual void FireNonAutomatic()
    {
        DealDamageToCurrentTarget();
        CurrentAmmo--;

        muzzle.Play();
        StartCoroutine(PlayHitLineEffect(0.1f));
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    public virtual void FireAutomatic()
    {

    }

    public virtual void FireBurst()
    {

    }

    public virtual void FireProjectile()
    {
        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).CurrentDirection = transform.up;
        CurrentAmmo--;

        muzzle.Play();
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    public virtual void CancelFire()
    {

    }

    #endregion

    private IEnumerator PlayHitLineEffect(float duration)
    {
        hitLine.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        hitLine.gameObject.SetActive(false);
    }

    #region Damage Methods

    private void GetDamageableFromHitscan()
    {
        // Get raycast target
        var hit = Physics2D.Raycast(firePoint.position, transform.up, range);

        // Get damage & knockback target from raycast hit
        CurrentDamageTarget = hit ? hit.transform.GetComponent<IDamageable>() : null;
        _currentKnockbackTarget = hit ? hit.transform.GetComponent<IKnockbackable>() : null;
        CurrentZombieTarget = hit ? hit.transform.GetComponent<Zombie>() : null;

        _currentDamageTargetContactPoint = hit ? hit.point : new Vector2(-20f, -20f);
    }

    private void DealDamageToCurrentTarget(bool isHeadDamage = false)
    {
        if (CurrentDamageTarget == null) return;

        var damageDealt = CurrentDamageTarget?.CurrentHealth <= bodyDamagePerShot ? CurrentDamageTarget?.CurrentHealth : bodyDamagePerShot;
        CurrentDamageTarget?.TakeDamage(bodyDamagePerShot, transform.up, _currentDamageTargetContactPoint);
        _currentKnockbackTarget?.Knockback(transform.up, knockbackPerShot);

        if (CurrentDamageTarget.CurrentHealth <= 0f)
        {
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            GameController.Instance.PlaySlowMotionEffect();
        }

        EffectsController.Instance.SpawnPopText(_currentDamageTargetContactPoint, hitColor, damageDealt.ToString());
    }

    #endregion
}
