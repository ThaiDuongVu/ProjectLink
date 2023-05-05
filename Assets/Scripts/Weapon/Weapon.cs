using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float range;
    public int fireRate;
    public float damagePerShot;
    public float knockbackPerShot;

    [Header("Weapon References")]
    [SerializeField] private SpriteRenderer crosshair;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private LineRenderer hitLine;
    [SerializeField] private Transform firePoint;

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

    [Header("Color References")]
    public Color regularColor;
    public Color hitColor;

    public Color CrosshairColor => crosshair.color;

    private IDamageable _currentDamageTarget;
    private Vector2 _currentDamageTargetContactPoint;
    private IKnockbackable _currentKnockbackTarget;

    public Zombie ZombieTarget { get; protected set; }

    #region Unity Events

    protected virtual void Awake()
    {
        _ammoIcons = ammoDisplay.GetComponentsInChildren<Image>();
    }

    protected virtual void Start()
    {
        crosshair.transform.position = transform.position + transform.up * range;

        hitLine.SetPosition(1, crosshair.transform.localPosition);
        hitLine.gameObject.SetActive(false);

        CurrentAmmo = maxAmmo;
        _ammoRechargeTimer = new Timer(ammoRechargeTime);
    }

    protected virtual void Update()
    {
        if (!_canFire && _fireTimer.IsReached()) _canFire = true;

        GetDamageableFromRaycast();
        crosshair.color = hitLine.startColor = hitLine.endColor = _currentDamageTarget == null ? regularColor : hitColor;

        if (_ammoRechargeTimer.IsReached())
        {
            if (CurrentAmmo < maxAmmo) CurrentAmmo++;
            _ammoRechargeTimer.Reset();
        }
    }

    #endregion

    #region Fire Methods

    public void Fire()
    {
        if (!_canFire) return;
        if (CurrentAmmo <= 0) return;

        DealDamageToCurrentTarget();
        CurrentAmmo--;

        _canFire = false;
        _fireTimer = new Timer(1f / fireRate);

        muzzle.Play();
        StartCoroutine(PlayHitLineEffect(0.1f));
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    public void CancelFire()
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

    private void GetDamageableFromRaycast()
    {
        var hit = Physics2D.Raycast(firePoint.position, transform.up, range);

        _currentDamageTarget = hit ? hit.transform.GetComponent<IDamageable>() : null;
        _currentKnockbackTarget = hit ? hit.transform.GetComponent<IKnockbackable>() : null;
        _currentDamageTargetContactPoint = hit ? hit.point : new Vector2(-20f, -20f);

        ZombieTarget = hit ? hit.transform.GetComponent<Zombie>() : null;
    }

    private void DealDamageToCurrentTarget()
    {
        if (_currentDamageTarget == null) return;

        _currentDamageTarget?.TakeDamage(damagePerShot, transform.up, _currentDamageTargetContactPoint);
        _currentKnockbackTarget?.Knockback(transform.up, knockbackPerShot);

        if (_currentDamageTarget.CurrentHealth <= 0f)
        {
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            GameController.Instance.PlaySlowMotionEffect();
        }

        EffectsController.Instance.SpawnPopText(_currentDamageTargetContactPoint, hitColor, damagePerShot.ToString());
    }

    #endregion
}
