using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sintoma del Dia 7 ("la salida / renuncia"): revierte el burnout. Devuelve el color de forma
/// gradual y apaga los sintomas que venian (vinieta, parpadeo, audios). NO toca el desorden:
/// el ClutterManager queda como esta (eso se resuelve aparte).
/// Despues, espera un poco (para que se vea el color volver), funde a negro y recarga la escena,
/// que arranca de nuevo en el menu principal.
/// Se dispara con ActivityStep.eventoAlCompletar = "Salida" (via SymptomManager -> SalidaEffect.Aplicar),
/// despues de que el jugador selecciona el objeto de trabajar y piensa "Listo, renuncio".
/// </summary>
public class SalidaEffect : MonoBehaviour
{
    [Header("Referencias (vacio = no toca ese efecto)")]
    [SerializeField] private VignetteEffect vignette;
    [SerializeField] private BlinkEffect parpadeo;
    [SerializeField] private TirednessAudio audioCansancio;
    [SerializeField] private HeartbeatAudio palpitaciones;

    [Header("Reinicio")]
    [Tooltip("Segundos a esperar (con el color volviendo) antes de fundir y reiniciar.")]
    [SerializeField] private float esperaAntesDeReiniciar = 4f;
    [Tooltip("Escena a cargar. Vacio = recarga la escena actual (vuelve al menu, que vive en la misma escena).")]
    [SerializeField] private string escenaReinicio = "";

    /// <summary>Aplica la salida del Dia 7. Wirear en el SymptomManager bajo la clave "Salida".</summary>
    public void Aplicar()
    {
        DayManager.Instance?.RestaurarColor();   // el color vuelve gradual

        if (vignette != null) vignette.SetIntensidad(0f);   // al llegar a 0 el quad se apaga solo
        if (parpadeo != null) parpadeo.Desactivar();
        if (audioCansancio != null) audioCansancio.Desactivar();
        if (palpitaciones != null) palpitaciones.Desactivar();

        Debug.Log("COLORLESS: Salida (Dia 7) - color de vuelta y sintomas apagados. El desorden queda.");

        StartCoroutine(ReiniciarAlMenu());
    }

    private IEnumerator ReiniciarAlMenu()
    {
        yield return new WaitForSeconds(esperaAntesDeReiniciar);

        if (ScreenFade.Instance != null)
            yield return StartCoroutine(ScreenFade.Instance.FadeOut());   // se apaga la pantalla

        if (string.IsNullOrEmpty(escenaReinicio))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // recarga -> arranca el menu
        else
            SceneManager.LoadScene(escenaReinicio);
    }
}
