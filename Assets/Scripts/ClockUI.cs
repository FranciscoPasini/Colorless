using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ClockUI : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI textHora;
    public TextMeshProUGUI textDia;
    public LightManager lightingManager;

    private void Update()
    {
        if (lightingManager == null) return;

        float hora = lightingManager.horaActual;
        int h = Mathf.FloorToInt(hora);
        int m = Mathf.FloorToInt((hora - h) * 60f);

        if (textHora != null)
            textHora.text = $"{h:D2}:{m:D2}";

        if (textDia != null && DayManager.Instance != null)
            textDia.text = $"Día {DayManager.Instance.diaActual}";
    }
}
