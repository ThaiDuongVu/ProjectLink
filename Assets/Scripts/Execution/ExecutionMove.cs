using UnityEngine;

public class ExecutionMove : MonoBehaviour
{
    [Header("Animation")]
    public AnimationClip animationClip;
    public string animationTrigger;

    [Header("Stats")]
    public float durationInFrames;
    public float hitFrame;
}
