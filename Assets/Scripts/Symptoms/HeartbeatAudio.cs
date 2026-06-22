using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeartbeatAudio : MonoBehaviour
{
    [Header("Clip (assign later)")]
    [SerializeField] private AudioClip latido;

    [Header("Interval between beats (sec)")]
    [SerializeField] private float intervaloMin = 28f;
    [SerializeField] private float intervaloMax = 34f;

    [Range(0f, 1f)][SerializeField] private float volumen = 0.7f;

    private AudioSource source;
    private Coroutine rutina;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;
    }

    public void Activar()
    {
        if (rutina != null) return;
        rutina = StartCoroutine(Loop());
    }

    public void Desactivar()
    {
        if (rutina != null) { StopCoroutine(rutina); rutina = null; }
    }

    public void SetVolumen(float v) => volumen = Mathf.Clamp01(v);

    private IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervaloMin, intervaloMax));
            if (latido != null) source.PlayOneShot(latido, volumen);
        }
    }
}
