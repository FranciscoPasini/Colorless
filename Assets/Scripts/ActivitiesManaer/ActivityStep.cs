using System;
using UnityEngine;

[Serializable]
public class ActivityStep
{
    [Header("Texto")]
    public string pensamiento;

    [Header("Interaccion")]
    [Tooltip("Nombre exacto del GameObject en la escena")]
    public string nombreInteractable;

    [Header("Tiempo")]
    [Tooltip("Hora del juego a la que salta el tiempo al completar. -1 = no saltar")]
    public float avanzarHoraA = -1f;

    [Header("Flags")]
    public bool esDormir = false;

    [Header("Decision (dejar vacio si no es decision)")]
    public bool esDecision = false;
    [Tooltip("Nombre exacto del GameObject opcion 1 en la escena (ej: Hamburguesa)")]
    public string opcion1Nombre;
    [Tooltip("Nombre exacto del GameObject opcion 2 en la escena (ej: PolloEnsalada)")]
    public string opcion2Nombre;

    [Header("Sintoma / Evento")]
    [Tooltip("Clave del sintoma a disparar al completar este paso. Debe coincidir con una clave del SymptomManager (ej: Enfoque). Vacio = nada.")]
    public string eventoAlCompletar;
}
