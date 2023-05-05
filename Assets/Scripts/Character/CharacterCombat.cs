using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    protected Animator Animator;
    protected Rigidbody2D Rigidbody;

    #region Unity Events

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    #endregion
}
