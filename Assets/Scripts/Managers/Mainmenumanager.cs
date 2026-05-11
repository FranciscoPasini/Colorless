using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona el menú principal de COLORLESS.
/// Adjuntalo al GameObject raíz del menú en la escena MainMenu.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Nombre de la escena de juego")]
    [Tooltip("Debe coincidir exactamente con el nombre en Build Settings")]
    public string nombreEscenaJuego = "Game";

    [Header("Canvas del menú (para animación de entrada)")]
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    [Tooltip("Duración del fade de entrada y salida")]
    public float tiempoFade = 1f;

    private bool cargando = false;

    private void Start()
    {
        // Fade de entrada al abrir el menú
        if (canvasGroup != null)
            StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Llamado desde el botón Play (OnClick en el Inspector).
    /// </summary>
    public void OnClickPlay()
    {
        if (cargando) return;
        cargando = true;
        StartCoroutine(IrAlJuego());
    }

    private IEnumerator IrAlJuego()
    {
        // Fade out del canvas
        if (canvasGroup != null)
            yield return StartCoroutine(FadeOutCanvas());

        // Usar OVRScreenFade si está disponible
        OVRScreenFade ovrFade = FindFirstObjectByType<OVRScreenFade>();
        if (ovrFade != null)
        {
            ovrFade.FadeOut();
            yield return new WaitForSeconds(ovrFade.fadeTime);
        }

        SceneManager.LoadScene(nombreEscenaJuego);
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < tiempoFade)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / tiempoFade);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutCanvas()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;
        while (elapsed < tiempoFade)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / tiempoFade);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
