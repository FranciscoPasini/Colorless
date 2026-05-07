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
}
