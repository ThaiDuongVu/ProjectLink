using UnityEngine;
using UnityEngine.UI;

public class Cat : Character
{
    [Header("UI References")]
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
}
