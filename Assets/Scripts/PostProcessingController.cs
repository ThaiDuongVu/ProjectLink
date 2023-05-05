using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    #region Singleton

    private static PostProcessingController _postprocessingControllerInstance;

    public static PostProcessingController Instance
    {
        get
        {
            if (_postprocessingControllerInstance == null) _postprocessingControllerInstance = FindObjectOfType<PostProcessingController>();
            return _postprocessingControllerInstance;
        }
    }

    #endregion

    private VolumeProfile _volumeProfile;
    private DepthOfField _depthOfField;
    private ChromaticAberration _chromaticAberration;
    private Vignette _vignette;

    public const float DefaultVignetteIntensity = 0.3f;
    public const float DefaultChromaticAberrationIntensity = 0.2f;

    #region Unity Event

    private void Awake()
    {
        _volumeProfile = GetComponent<Volume>().profile;
        _volumeProfile.TryGet(out _depthOfField);
        _volumeProfile.TryGet(out _chromaticAberration);
        _volumeProfile.TryGet(out _vignette);
    }

    private void Start()
    {
        SetChromaticAberration(false);
        SetChromaticAberrationIntensity(DefaultChromaticAberrationIntensity);
        SetVignetteIntensity(DefaultVignetteIntensity);
    }

    #endregion

    public void SetDepthOfField(bool value)
    {
        _depthOfField.active = value;
    }

    public void SetChromaticAberration(bool value)
    {
        _chromaticAberration.active = value;
    }

    private void SetChromaticAberrationIntensity(float value)
    {
        _chromaticAberration.intensity.value = value;
    }

    public void SetVignetteIntensity(float value)
    {
        _vignette.intensity.value = value;
    }

    public void SetVignetteCenter(Vector2 center)
    {
        _vignette.center.value = center;
    }
}