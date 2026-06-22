using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day_1", menuName = "Colorless/DayData")]
public class DayData : ScriptableObject
{
    public string eventoAlIniciar;

    public List<ActivityStep> pasos = new List<ActivityStep>();
}
