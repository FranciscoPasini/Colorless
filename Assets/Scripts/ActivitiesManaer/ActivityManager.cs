using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour
{
    public static ActivityManager Instance { get; private set; }

    [Header("Dias")]
    public List<DayData> dias = new List<DayData>();

    [Header("Referencias")]
    public LightManager lightManager;

    private int pasoActual;
    private DayData diaActual;
    private ActivityStep pasoEnCurso;
    private Interactable interactableActivo;
    private bool esperandoInteraccion;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (lightManager == null)
            lightManager = FindFirstObjectByType<LightManager>();

        IniciarDia(DayManager.Instance.diaActual);
    }

    public void IniciarDia(int dia)
    {
        int indice = dia - 1;
        if (indice < 0 || indice >= dias.Count)
        {
            Debug.LogWarning($"COLORLESS: No hay DayData para el día {dia}.");
            return;
        }
        pasoActual = 0;
        diaActual = dias[indice];
        Debug.Log($"COLORLESS: ActivityManager iniciando día {dia} con {diaActual.pasos.Count} pasos.");
        EjecutarPasoActual();
    }

    private void EjecutarPasoActual()
    {
        if (diaActual == null || pasoActual >= diaActual.pasos.Count) return;

        pasoEnCurso = diaActual.pasos[pasoActual];

        if (!string.IsNullOrEmpty(pasoEnCurso.pensamiento))
            ThoughtDisplay.Instance?.MostrarPensamiento(pasoEnCurso.pensamiento, alTerminar: ActivarPaso);
        else
            ActivarPaso();
    }

    private void ActivarPaso()
    {
        interactableActivo = BuscarInteractable(pasoEnCurso.nombreInteractable);
        esperandoInteraccion = interactableActivo != null;

        if (esperandoInteraccion)
        {
            interactableActivo.onInteract.AddListener(OnInteractuado);
            ActivarHighlight(interactableActivo);
        }
        else
        {
            StartCoroutine(CompletarPaso());
        }
    }

    private void OnInteractuado()
    {
        if (!esperandoInteraccion) return;
        esperandoInteraccion = false;

        if (interactableActivo != null)
        {
            interactableActivo.onInteract.RemoveListener(OnInteractuado);
            DesactivarHighlight(interactableActivo);
            interactableActivo = null;
        }

        StartCoroutine(CompletarPaso());
    }

    private IEnumerator CompletarPaso()
    {
        if (pasoEnCurso.esDormir)
        {
            DayManager.Instance.TrySleep();
            yield break;
        }

        if (pasoEnCurso.avanzarHoraA >= 0f && ScreenFade.Instance != null)
        {
            yield return StartCoroutine(ScreenFade.Instance.FadeOut());
            if (lightManager != null) lightManager.horaActual = pasoEnCurso.avanzarHoraA;
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(ScreenFade.Instance.FadeIn());
        }

        pasoActual++;
        EjecutarPasoActual();
    }

    private Interactable BuscarInteractable(string nombre)
    {
        if (string.IsNullOrEmpty(nombre)) return null;
        GameObject go = GameObject.Find(nombre);
        if (go == null)
        {
            Debug.LogWarning($"COLORLESS: No se encontró el GameObject '{nombre}' en la escena.");
            return null;
        }
        return go.GetComponent<Interactable>();
    }

    private void ActivarHighlight(Interactable target)
    {
        target?.GetComponent<InteractableHighlight>()?.Activar();
    }

    private void DesactivarHighlight(Interactable target)
    {
        target?.GetComponent<InteractableHighlight>()?.Desactivar();
    }
}
