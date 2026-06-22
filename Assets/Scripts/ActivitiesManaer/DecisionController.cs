using System;
using System.Collections;
using UnityEngine;

public class DecisionController : MonoBehaviour
{
    public static DecisionController Instance { get; private set; }

    [Header("Hover config")]
    public float alturaHover = 0.15f;
    public float velocidadLerp = 5f;

    private Transform opcion1;
    private Transform opcion2;
    private Transform visual1;
    private Transform visual2;
    private Vector3 posBase1;
    private Vector3 posBase2;
    private Action<int> onDecision;

    private Transform bloqueada;
    private Action onBloqueadaIntento;
    private bool bloqueadaUsada;

    private bool activo = false;
    private Transform hovereada = null;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (!activo) return;
        AnimarOpciones();
    }

    public void Activar(Transform op1, Transform op2, Action<int> callback,
                        Transform opBloqueada = null, Action callbackBloqueada = null)
    {
        opcion1 = op1;
        opcion2 = op2;
        visual1 = GetVisual(op1);
        visual2 = GetVisual(op2);
        posBase1 = visual1.position;
        posBase2 = visual2.position;
        onDecision = callback;
        bloqueada = opBloqueada;
        onBloqueadaIntento = callbackBloqueada;
        bloqueadaUsada = false;
        activo = true;
        hovereada = null;

        op1.gameObject.SetActive(true);
        op2.gameObject.SetActive(true);

        ActivarHighlight(op1);
        ActivarHighlight(op2);
    }

    public void Desactivar()
    {
        activo = false;
        hovereada = null;

        if (opcion1 != null) { DesactivarHighlight(opcion1); opcion1.gameObject.SetActive(false); }
        if (opcion2 != null) { DesactivarHighlight(opcion2); opcion2.gameObject.SetActive(false); }

        if (bloqueada != null) GetHighlight(bloqueada)?.Desbloquear();

        opcion1 = null;
        opcion2 = null;
        visual1 = null;
        visual2 = null;
        bloqueada = null;
        onBloqueadaIntento = null;
        bloqueadaUsada = false;
    }

    private void ActivarHighlight(Transform target) => GetHighlight(target)?.Activar();
    private void DesactivarHighlight(Transform target) => GetHighlight(target)?.Desactivar();
    private InteractableHighlight GetHighlight(Transform target)
        => target != null ? target.GetComponentInChildren<InteractableHighlight>(true) : null;

    private Transform GetVisual(Transform raiz)
    {
        DecisionOption opt = raiz.GetComponent<DecisionOption>();
        return (opt != null && opt.Visual != null) ? opt.Visual : raiz;
    }

    public void NotifyHover(Transform option)
    {
        if (!activo) return;
        if (option == bloqueada && bloqueadaUsada) return;
        hovereada = option;
    }

    public void NotifyUnhover(Transform option)
    {
        if (!activo) return;
        if (hovereada == option) hovereada = null;
    }

    public void NotifySelected(Transform option)
    {
        if (!activo) return;

        if (option == bloqueada)
        {
            if (bloqueadaUsada) return;
            bloqueadaUsada = true;
            hovereada = null;
            GetHighlight(option)?.MarcarBloqueada();
            onBloqueadaIntento?.Invoke();
            return;
        }

        hovereada = option;
        Seleccionar();
    }

    private void AnimarOpciones()
    {
        if (opcion1 == null || opcion2 == null) return;
        AnimarUna(opcion1, visual1, posBase1);
        AnimarUna(opcion2, visual2, posBase2);
    }

    private void AnimarUna(Transform id, Transform visual, Vector3 posBase)
    {
        if (visual == null) return;

        Vector3 target;
        if (id == bloqueada && bloqueadaUsada)
            target = posBase;
        else if (id == hovereada)
            target = posBase + Vector3.up * alturaHover;
        else
            target = posBase - Vector3.up * (alturaHover * 0.5f);

        visual.position = Vector3.Lerp(visual.position, target, Time.deltaTime * velocidadLerp);
    }

    private void Seleccionar()
    {
        int indice = (hovereada == opcion1) ? 0 : 1;
        activo = false;

        if (visual1 != null) visual1.position = posBase1;
        if (visual2 != null) visual2.position = posBase2;

        onDecision?.Invoke(indice);
    }
}
