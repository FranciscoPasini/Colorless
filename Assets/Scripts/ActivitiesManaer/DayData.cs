using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dia_1", menuName = "Colorless/DayData")]
public class DayData : ScriptableObject
{
    [Tooltip("Clave de sintoma a disparar apenas empieza el dia (ej: Cansancio). Debe coincidir con una clave del SymptomManager. Vacio = nada.")]
    public string eventoAlIniciar;

    public List<ActivityStep> pasos = new List<ActivityStep>();
}
