using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TirednessAudio : MonoBehaviour
{
    [Header("Clips (assign later)")]
    [SerializeField] private AudioClip respiracion;
    [SerializeField] private AudioClip bostezo;

    [Header("Breathing interval (sec) - more frequent")]
    [SerializeField] private float respiracionMin = 12f;
    [SerializeField] private float respiracionMax = 22f;

    [Header("Yawn interval (sec) - more spaced")]
    [SerializeField] private float bostezoMin = 40f;
    [SerializeField] private float bostezoMax = 70f;

    [Range(0f, 1f)][SerializeField] private float volumen = 1f;
    [SerializeField] private bool respirarAlIniciar = true;

    private AudioSource source;
    private Coroutine rutinaResp;
    private Coroutine rutinaBost;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;
    }

    public void Activar()
    {
        if (respirarAlIniciar && respiracion != null)
            source.PlayOneShot(respiracion, volumen);

        if (rutinaResp == null)
            rutinaResp = StartCoroutine(Loop(() => respiracion, respiracionMin, respiracionMax));
        if (rutinaBost == null)
            rutinaBost = StartCoroutine(Loop(() => bostezo, bostezoMin, bostezoMax));
    }

    public void Desactivar()
    {
        if (rutinaResp != null) { StopCoroutine(rutinaResp); rutinaResp = null; }
        if (rutinaBost != null) { StopCoroutine(rutinaBost); rutinaBost = null; }
    }

    public void SetVolumen(float v)
    {
        volumen = Mathf.Clamp01(v);
    }

    private IEnumerator Loop(System.Func<AudioClip> clip, float min, float max)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(min, max));
            AudioClip c = clip();
            if (c != null) source.PlayOneShot(c, volumen);
        }
    }
}
