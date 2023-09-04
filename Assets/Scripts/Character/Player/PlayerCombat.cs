using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    [Header("Combat Stats")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float bounceForce;
    [SerializeField] private Color damageColor;

    private static readonly int JumpAnimationTrigger = Animator.StringToHash("jump");

    private Player _player;
    private PlayerMovement _playerMovement;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    #endregion

    private void Bounce()
    {
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
        Rigidbody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        Animator.SetTrigger(JumpAnimationTrigger);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy") && _player.IsFalling && _player.transform.position.y > other.transform.position.y)
        {
            // Deal damage to enemy
            var enemy = other.transform.GetComponent<Enemy>();
            var contactPoint = other.GetContact(0).point;
            enemy.TakeDamage(baseDamage, Rigidbody.velocity.normalized, contactPoint);

            // Refill air jumps on eliminations
            if (!enemy || enemy.IsDead) _playerMovement.RefillAirJumps();

            // Slightly bounce up
            Bounce();

            // Play some effects
            EffectsController.Instance.SpawnPopText(contactPoint, baseDamage.ToString(), damageColor);
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            GameController.Instance.PlaySlowMotionEffect();
        }
    }
}
