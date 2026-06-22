using System.Collections;
using UnityEngine;

/// <summary>
/// Audio de cansancio (Dia 2): reproduce respiraciones profundas (intervalo corto) y
/// bostezos (intervalo mas largo) de forma periodica y aleatoria.
/// Los AudioClip se asignan desde el Inspector; si estan vacios no suena nada (sin error),
/// asi se puede dejar cableado y agregar el audio mas tarde.
/// Llamar Activar() desde el SymptomManager; Desactivar() para frenarlo.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TirednessAudio : MonoBehaviour
{
    [Header("Clips (asignar mas tarde)")]
    [SerializeField] private AudioClip respiracion;
    [SerializeField] private AudioClip bostezo;

    [Header("Intervalo respiracion (seg) - mas seguido")]
    [SerializeField] private float respiracionMin = 12f;
    [SerializeField] private float respiracionMax = 22f;

    [Header("Intervalo bostezo (seg) - mas espaciado")]
    [SerializeField] private float bostezoMin = 40f;
    [SerializeField] private float bostezoMax = 70f;

    [Range(0f, 1f)][SerializeField] private float volumen = 1f;
    [Tooltip("Reproducir una respiracion apenas se activa el cansancio.")]
    [SerializeField] private bool respirarAlIniciar = true;

    private AudioSource source;
    private Coroutine rutinaResp;
    private Coroutine rutinaBost;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D: son sonidos del propio jugador
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
