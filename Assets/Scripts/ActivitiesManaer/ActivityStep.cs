using System;
using UnityEngine;

[Serializable]
public class ActivityStep
{
    [Header("Text")]
    public string pensamiento;

    [Header("Interaction")]
    public string nombreInteractable;

    [Header("Time")]
    public float avanzarHoraA = -1f;

    [Header("Flags")]
    public bool esDormir = false;

    [Header("Decision (leave empty if not a decision)")]
    public bool esDecision = false;
    public string opcion1Nombre;
    public string opcion2Nombre;
    public string opcion1Evento;
    public string opcion2Evento;

    [Header("Blocked decision (Day 4+: the 'good' one can't be picked)")]
    public string opcionBloqueadaNombre;
    public string pensamientoBloqueada;

    [Header("Symptom / Event")]
    public string eventoAlCompletar;
}
