using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance { get; private set; }

    private OVRScreenFade ovrFade;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        ovrFade = FindFirstObjectByType<OVRScreenFade>();
        if (ovrFade == null)
            Debug.LogWarning("COLORLESS: No se encontró OVRScreenFade en la escena.");
    }

    public IEnumerator FadeOut()
    {
        if (ovrFade == null) yield break;
        ovrFade.FadeOut();
        yield return new WaitForSeconds(ovrFade.fadeTime);
    }

    public IEnumerator FadeIn()
    {
        if (ovrFade == null) yield break;
        ovrFade.FadeIn();
        yield return new WaitForSeconds(ovrFade.fadeTime);
    }
}