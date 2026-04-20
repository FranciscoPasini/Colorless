using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    [Header("Referencias")]
    public Light directionalLight;
    public Gradient colorLuz;          
    public AnimationCurve intensidad;  

    [Header("Hora del día")]
    [Range(0, 24)] public float horaActual = 7f;
    public float velocidadTiempo = 1f; 

    [Header("Skybox")]
    public Gradient colorAmbiente;

    private void Update()
    {
        AvanzarTiempo();
        AplicarIluminacion();
    }

    [Header("Velocidad del tiempo")]
    public float segundosRealesPorMinutoDeJuego = 2f;

    private void AvanzarTiempo()
    {
        float minutosDeJuegoPorSegundo = 1f / segundosRealesPorMinutoDeJuego;
        horaActual += Time.deltaTime * minutosDeJuegoPorSegundo / 60f;
        if (horaActual >= 24f) horaActual = 0f;
    }

    private void AplicarIluminacion()
    {
        float t = horaActual / 24f;
        directionalLight.color = colorLuz.Evaluate(t);
        directionalLight.intensity = intensidad.Evaluate(t);

        // Rotación: amanecer 6h (t=0.25) ? mediodía (t=0.5) ? ocaso 18h (t=0.75)
        float angulo = (t - 0.25f) * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(angulo, -30f, 0f);

        RenderSettings.ambientLight = colorAmbiente.Evaluate(t);
    }

    // Llamado desde DayManager al comenzar un nuevo día
    public void IniciarDia()
    {
        horaActual = 7f; // el día arranca a las 7 am
    }
}

