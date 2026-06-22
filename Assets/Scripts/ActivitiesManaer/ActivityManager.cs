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

        if (!string.IsNullOrEmpty(diaActual.eventoAlIniciar))
            SymptomManager.Instance?.Disparar(diaActual.eventoAlIniciar);

        EjecutarPasoActual();
    }

    private void EjecutarPasoActual()
    {
        if (diaActual == null || pasoActual >= diaActual.pasos.Count) return;

        pasoEnCurso = diaActual.pasos[pasoActual];

        if (!string.IsNullOrEmpty(pasoEnCurso.pensamiento) && ThoughtDisplay.Instance != null)
            ThoughtDisplay.Instance.MostrarPensamiento(pasoEnCurso.pensamiento, alTerminar: ActivarPaso);
        else
            ActivarPaso();
    }

    private void ActivarPaso()
    {
        if (pasoEnCurso.esDecision)
        {
            ActivarDecision();
            return;
        }

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

    private void ActivarDecision()
    {
        GameObject go1 = BuscarIncluyendoInactivos(pasoEnCurso.opcion1Nombre);
        GameObject go2 = BuscarIncluyendoInactivos(pasoEnCurso.opcion2Nombre);

        if (go1 == null || go2 == null)
        {
            Debug.LogWarning($"COLORLESS: No se encontraron los objetos de decisión '{pasoEnCurso.opcion1Nombre}' o '{pasoEnCurso.opcion2Nombre}'.");

            StartCoroutine(CompletarPaso());
            return;
        }

        Transform bloqueada = null;
        if (!string.IsNullOrEmpty(pasoEnCurso.opcionBloqueadaNombre))
        {
            GameObject goBloq = BuscarIncluyendoInactivos(pasoEnCurso.opcionBloqueadaNombre);
            if (goBloq != null) bloqueada = goBloq.transform;
            else Debug.LogWarning($"COLORLESS: No se encontró la opción bloqueada '{pasoEnCurso.opcionBloqueadaNombre}'.");
        }

        DecisionController.Instance.Activar(go1.transform, go2.transform, OnDecisionHecha,
                                           bloqueada, OnIntentoBloqueada);
    }

    private void OnIntentoBloqueada()
    {
        if (!string.IsNullOrEmpty(pasoEnCurso.pensamientoBloqueada) && ThoughtDisplay.Instance != null)
            ThoughtDisplay.Instance.MostrarPensamiento(pasoEnCurso.pensamientoBloqueada);
    }

    private GameObject BuscarIncluyendoInactivos(string nombre)
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.name == nombre && go.scene.isLoaded)
                return go;
        }
        return null;
    }

    private void OnDecisionHecha(int indice)
    {
        string elegida = indice == 0 ? pasoEnCurso.opcion1Nombre : pasoEnCurso.opcion2Nombre;
        Debug.Log($"COLORLESS: Jugador eligió opción {indice + 1}: {elegida}");

        string eventoOpcion = indice == 0 ? pasoEnCurso.opcion1Evento : pasoEnCurso.opcion2Evento;
        if (!string.IsNullOrEmpty(eventoOpcion))
            SymptomManager.Instance?.Disparar(eventoOpcion);

        DecisionController.Instance.Desactivar();
        StartCoroutine(CompletarPaso());
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

        if (!string.IsNullOrEmpty(pasoEnCurso.eventoAlCompletar))
            SymptomManager.Instance?.Disparar(pasoEnCurso.eventoAlCompletar);

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
        target?.GetComponentInChildren<InteractableHighlight>(true)?.Activar();
    }

    private void DesactivarHighlight(Interactable target)
    {
        target?.GetComponentInChildren<InteractableHighlight>(true)?.Desactivar();
    }
}
