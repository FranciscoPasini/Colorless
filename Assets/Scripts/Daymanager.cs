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

    [Header("Iluminacion")]
    public LightManager lightingManager;

    [Header("Hora de dormir")]
    [Tooltip("A partir de qué hora se puede dormir")]
    public float horaMinDormir = 22f;

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
        if (!durmiendo)
            DetectarMedianoche();
    }

    private void DetectarMedianoche()
    {
        if (lightingManager == null) return;
        float horaActual = lightingManager.horaActual;
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
        Debug.Log($"COLORLESS: Día {diaActual}");
    }

    private void AplicarDia(int dia)
    {
        foreach (ObjectColor obj in objetosEnEscena)
        {
            if (obj != null)
                obj.ActualizarParaDia(dia);
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