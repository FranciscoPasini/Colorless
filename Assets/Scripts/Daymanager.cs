  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;

  public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    [Header("Dias")]
    [Range(1, 7)]
    public int diaActual = 1;
    public int diasMaximos = 7;

    [Header("Transicion suave")]
    public bool transicionSuave = true;
    public float velocidadTransicion = 0.3f;

    [Header("Iluminacion")]
    public LightManager lightingManager;

    [Header("Hora de dormir")]
    [Tooltip("A partir de qué hora se puede dormir")]
    public float horaMinDormir = 22f;

    private readonly float[] saturacionPorDia = new float[]
    {
          1.0f, 0.85f, 0.65f, 0.45f, 0.30f, 0.15f, 0.0f
    };

    private float saturacionActual = 1f;
    private float saturacionObjetivo = 1f;
    private List<ObjectColor> objetosEnEscena = new List<ObjectColor>();
    private bool durmiendo = false;
    private float ultimaHora = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (lightingManager == null)
            lightingManager = FindFirstObjectByType<LightManager>();

        BuscarObjetos();
        AplicarDia(diaActual);
    }

    private void BuscarObjetos()
    {
        objetosEnEscena.Clear();
        ObjectColor[] todos = FindObjectsByType<ObjectColor>(FindObjectsSortMode.None);
        foreach (ObjectColor obj in todos)
        {
            if (obj.CompareTag("WorkObject")) continue;
            objetosEnEscena.Add(obj);
        }
        Debug.Log($"COLORLESS: {objetosEnEscena.Count} objetos registrados.");
    }

    private void Update()
    {
        if (!transicionSuave || durmiendo) return;

        if (!Mathf.Approximately(saturacionActual, saturacionObjetivo))
        {
            saturacionActual = Mathf.MoveTowards(
                saturacionActual, saturacionObjetivo, velocidadTransicion * Time.deltaTime);
            AplicarSaturacionATodos(saturacionActual);
        }

        DetectarMedianoche();
    }

    private void DetectarMedianoche()
    {
        if (lightingManager == null || durmiendo) return;

        float horaActual = lightingManager.horaActual;

        // Detecta cuando el reloj pasa de ~23.9 a ~0 (vuelta a medianoche)
        if (ultimaHora > 22f && horaActual < 1f)
            StartCoroutine(TransicionSueno());

        ultimaHora = horaActual;
    }

    public void TrySleep()
    {
        if (durmiendo) return;

        if (lightingManager != null && lightingManager.horaActual < horaMinDormir)
        {
            Debug.Log($"COLORLESS: Todavía no es hora de dormir. Hora: {lightingManager.horaActual:F1}");
            return;
        }

        StartCoroutine(TransicionSueno());
    }

    private IEnumerator TransicionSueno()
    {
        durmiendo = true;

        if (ScreenFade.Instance != null)
            yield return StartCoroutine(ScreenFade.Instance.FadeOut());

        AvanzarDia();

        yield return new WaitForSeconds(1f);

        if (ScreenFade.Instance != null)
            yield return StartCoroutine(ScreenFade.Instance.FadeIn());

        durmiendo = false;
    }

    private void AvanzarDia()
    {
        if (diaActual >= diasMaximos)
        {
            Debug.Log("COLORLESS: Último día.");
            return;
        }

        diaActual++;
        AplicarDia(diaActual);
        lightingManager?.IniciarDia();
        Debug.Log($"COLORLESS: Día {diaActual} — Saturación: {saturacionObjetivo}");
    }

    private void AplicarDia(int dia)
    {
        int index = Mathf.Clamp(dia - 1, 0, saturacionPorDia.Length - 1);
        saturacionObjetivo = saturacionPorDia[index];
        if (!transicionSuave)
        {
            saturacionActual = saturacionObjetivo;
            AplicarSaturacionATodos(saturacionActual);
        }
    }

    private void AplicarSaturacionATodos(float saturacion)
    {
        foreach (ObjectColor obj in objetosEnEscena)
        {
            if (obj != null)
                obj.AplicarSaturacion(saturacion);
        }
    }

    [ContextMenu("Avanzar Día (Test)")]
    private void TestAvanzar() => AvanzarDia();

    [ContextMenu("Resetear al Día 1 (Test)")]
    private void TestReset()
    {
        diaActual = 1;
        AplicarDia(1);
        lightingManager?.IniciarDia();
    }
}