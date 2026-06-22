using System.Collections;
using UnityEngine;

/// <summary>
/// Audio de palpitaciones (Dia 6): reproduce un latido cada cierto tiempo (~30s), no molesto.
/// Es aparte de los sonidos de cansancio (TirednessAudio). El AudioClip se asigna desde el
/// Inspector; si esta vacio no suena (sin error), asi se puede cablear y agregar el audio mas tarde.
/// Llamar Activar() desde el SymptomManager (clave "Palpitar"); Desactivar() para frenarlo.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class HeartbeatAudio : MonoBehaviour
{
    [Header("Clip (asignar mas tarde)")]
    [SerializeField] private AudioClip latido;

    [Header("Intervalo entre latidos (seg)")]
    [SerializeField] private float intervaloMin = 28f;
    [SerializeField] private float intervaloMax = 34f;

    [Range(0f, 1f)][SerializeField] private float volumen = 0.7f;

    private AudioSource source;
    private Coroutine rutina;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D: es un sonido del propio jugador
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

    /// <summary>Cambia el volumen de los proximos latidos (0..1).</summary>
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
