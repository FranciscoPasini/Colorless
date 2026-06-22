using UnityEngine;

public class StackEffect : MonoBehaviour
{
    [Header("References (empty = don't touch that effect)")]
    [SerializeField] private VignetteEffect vignette;
    [SerializeField] private TirednessAudio audioCansancio;

    [Header("Vignette (more intense and tighter)")]
    [Range(0f, 1f)][SerializeField] private float intensidadVignette = 0.7f;
    [Range(0f, 1f)][SerializeField] private float radioVignette = 0.15f;

    [Header("Audio (louder)")]
    [Range(0f, 1f)][SerializeField] private float volumenAudio = 1f;

    public void Aplicar()
    {
        if (vignette != null)
        {
            vignette.SetIntensidad(intensidadVignette);
            vignette.SetRadio(radioVignette);
        }

        if (audioCansancio != null)
            audioCansancio.SetVolumen(volumenAudio);
    }
}
