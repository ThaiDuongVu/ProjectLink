using UnityEngine;

public class Portal : MonoBehaviour
{
    public virtual void OnEnter()
    {
        StartCoroutine(Level.Instance.Reload());
    }

    public virtual void OnExit()
    {
        
    }
}
