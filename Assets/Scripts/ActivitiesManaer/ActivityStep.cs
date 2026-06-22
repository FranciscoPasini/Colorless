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
    [Tooltip("Clave de sintoma a disparar si se elige la opcion 1. Vacio = nada.")]
    public string opcion1Evento;
    [Tooltip("Clave de sintoma a disparar si se elige la opcion 2. Vacio = nada.")]
    public string opcion2Evento;

    [Header("Decision bloqueada (Dia 4+: la 'buena' no se puede elegir)")]
    [Tooltip("Nombre del objeto (debe ser opcion1 u opcion2) que queda BLOQUEADO: al seleccionarlo se pone rojo, vuelve a su lugar y no avanza. Vacio = ninguno bloqueado.")]
    public string opcionBloqueadaNombre;
    [Tooltip("Pensamiento que aparece al intentar elegir la opcion bloqueada (ej: 'Hoy no tengo tiempo...'). Vacio = nada.")]
    public string pensamientoBloqueada;

    [Header("Sintoma / Evento")]
    [Tooltip("Clave del sintoma a disparar al completar este paso. Debe coincidir con una clave del SymptomManager (ej: Enfoque). Vacio = nada.")]
    public string eventoAlCompletar;
}
