using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaEffect : MonoBehaviour
{
    [Header("References (empty = don't touch that effect)")]
    [SerializeField] private VignetteEffect vignette;
    [SerializeField] private BlinkEffect parpadeo;
    [SerializeField] private TirednessAudio audioCansancio;
    [SerializeField] private HeartbeatAudio palpitaciones;

    [Header("Restart")]
    [SerializeField] private float esperaAntesDeReiniciar = 4f;
    [SerializeField] private string escenaReinicio = "";

    public void Aplicar()
    {
        DayManager.Instance?.RestaurarColor();

        if (vignette != null) vignette.SetIntensidad(0f);
        if (parpadeo != null) parpadeo.Desactivar();
        if (audioCansancio != null) audioCansancio.Desactivar();
        if (palpitaciones != null) palpitaciones.Desactivar();

        StartCoroutine(ReiniciarAlMenu());
    }

    private IEnumerator ReiniciarAlMenu()
    {
        yield return new WaitForSeconds(esperaAntesDeReiniciar);

        if (ScreenFade.Instance != null)
            yield return StartCoroutine(ScreenFade.Instance.FadeOut());

        if (string.IsNullOrEmpty(escenaReinicio))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            SceneManager.LoadScene(escenaReinicio);
    }
}
