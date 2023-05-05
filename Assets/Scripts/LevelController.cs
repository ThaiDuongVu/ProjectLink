using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region Unity Events

    private void Start()
    {
        var levels = Resources.LoadAll<Level>("Levels");
        Instantiate(levels[Random.Range(0, levels.Length)], transform.position, Quaternion.identity).transform.SetParent(transform);
    }

    #endregion
}
