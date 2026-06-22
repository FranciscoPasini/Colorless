using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    [Header("References")]
    public Light directionalLight;
    public Gradient colorLuz;
    public AnimationCurve intensidad;

    [Header("Time of day")]
    [Range(0, 24)] public float horaActual = 7f;
    public float velocidadTiempo = 1f;

    [Header("Skybox")]
    public Gradient colorAmbiente;

    private void Update()
    {
        AvanzarTiempo();
        AplicarIluminacion();
    }

    [Header("Time speed")]
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

        float angulo = (t - 0.25f) * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(angulo, -30f, 0f);

        RenderSettings.ambientLight = colorAmbiente.Evaluate(t);
    }

    public void IniciarDia()
    {
        horaActual = 7f;
    }
}
