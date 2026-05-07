using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dia_1", menuName = "Colorless/DayData")]
public class DayData : ScriptableObject
{
    public List<ActivityStep> pasos = new List<ActivityStep>();
}
