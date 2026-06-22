using System;
using System.Collections;
using UnityEngine;

public class DecisionController : MonoBehaviour
{
    public static DecisionController Instance { get; private set; }

    [Header("Config hover")]
    public float alturaHover = 0.15f;
    public float velocidadLerp = 5f;

    private Transform opcion1;
    private Transform opcion2;
    private Vector3 posBase1;
    private Vector3 posBase2;
    private Action<int> onDecision;
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

    public void Activar(Transform op1, Transform op2, Action<int> callback)
    {
        opcion1 = op1;
        opcion2 = op2;
        posBase1 = op1.position;
        posBase2 = op2.position;
        onDecision = callback;
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

        opcion1 = null;
        opcion2 = null;
    }

    private void ActivarHighlight(Transform target)
    {
        target.GetComponentInChildren<InteractableHighlight>(true)?.Activar();
    }

    private void DesactivarHighlight(Transform target)
    {
        target.GetComponentInChildren<InteractableHighlight>(true)?.Desactivar();
    }

    public void NotifyHover(Transform option)
    {
        if (!activo) return;
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
        hovereada = option;
        Seleccionar();
    }

    private void AnimarOpciones()
    {
        if (opcion1 == null || opcion2 == null) return;

        Vector3 targetPos1 = hovereada == opcion1
            ? posBase1 + Vector3.up * alturaHover
            : posBase1 - Vector3.up * (alturaHover * 0.5f);

        Vector3 targetPos2 = hovereada == opcion2
            ? posBase2 + Vector3.up * alturaHover
            : posBase2 - Vector3.up * (alturaHover * 0.5f);

        opcion1.position = Vector3.Lerp(opcion1.position, targetPos1, Time.deltaTime * velocidadLerp);
        opcion2.position = Vector3.Lerp(opcion2.position, targetPos2, Time.deltaTime * velocidadLerp);
    }

    private void Seleccionar()
    {
        int indice = (hovereada == opcion1) ? 0 : 1;
        activo = false;

        if (opcion1 != null) opcion1.position = posBase1;
        if (opcion2 != null) opcion2.position = posBase2;

        onDecision?.Invoke(indice);
    }
}
