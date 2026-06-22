using UnityEngine;

/// <summary>
/// Sintoma del Dia 5 ("el pico"): sube la intensidad de los sintomas que ya venian.
/// Es un orquestador: NO crea efectos nuevos, solo le pega valores mas fuertes a los
/// efectos existentes. Se dispara con DayData.eventoAlIniciar = "Stack" (via SymptomManager
/// -> StackEffect.Aplicar). El desorden extra se hace a mano con el ClutterManager.
/// </summary>
public class StackEffect : MonoBehaviour
{
    [Header("Referencias (dejar vacio = no toca ese efecto)")]
    [SerializeField] private VignetteEffect vignette;
    [SerializeField] private TirednessAudio audioCansancio;

    [Header("Vinieta (mas intensa y mas cerrada)")]
    [Range(0f, 1f)][SerializeField] private float intensidadVignette = 0.7f;
    [Tooltip("Mas chico = mas cerrada (tunel).")]
    [Range(0f, 1f)][SerializeField] private float radioVignette = 0.15f;

    [Header("Audio (mas fuerte)")]
    [Range(0f, 1f)][SerializeField] private float volumenAudio = 1f;

    /// <summary>Aplica el stack del Dia 5. Wirear en el SymptomManager bajo la clave "Stack".</summary>
    public void Aplicar()
    {
        if (vignette != null)
        {
            vignette.SetIntensidad(intensidadVignette);
            vignette.SetRadio(radioVignette);
        }

        if (audioCansancio != null)
            audioCansancio.SetVolumen(volumenAudio);

        Debug.Log("COLORLESS: Stack del Dia 5 aplicado (vinieta + audio mas fuertes).");
    }
}
