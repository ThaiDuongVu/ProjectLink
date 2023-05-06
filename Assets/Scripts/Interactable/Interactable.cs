using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Animator Animator;

    #region Unity Events

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    #endregion

    public virtual void OnInteracted(Transform activator)
    {

    }
}
