using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThoughtDisplay : MonoBehaviour
{
    public static ThoughtDisplay Instance { get; private set; }

    [Header("Referencia")]
    [SerializeField] private TMP_Text textoDisplay;
    [Tooltip("Panel/canvas a mostrar mientras hay texto. Si se deja vacio se usa el GameObject del textoDisplay. NO debe ser este mismo GameObject.")]
    [SerializeField] private GameObject panel;

    [Header("Config")]
    [SerializeField] private float delayPorCaracter = 0.04f;
    [SerializeField] private float tiempoAutoOcultar = 3f;

    private Coroutine coroutineActual;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (panel == null && textoDisplay != null) panel = textoDisplay.gameObject;
        if (textoDisplay != null) textoDisplay.text = "";
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (panel != null && panel != gameObject) panel.SetActive(visible);
    }

    public void MostrarPensamiento(string texto, Action alTerminar = null, float ocultarDespuesDe = -1f)
    {
        if (textoDisplay == null)
        {
            Debug.LogWarning("COLORLESS: ThoughtDisplay no tiene textoDisplay asignado.");
            return;
        }
        if (coroutineActual != null) StopCoroutine(coroutineActual);
        SetVisible(true);
        float delay = ocultarDespuesDe >= 0f ? ocultarDespuesDe : tiempoAutoOcultar;
        coroutineActual = StartCoroutine(RutinaTypewriter(texto, delay, alTerminar));
    }

    public void Ocultar()
    {
        if (coroutineActual != null) { StopCoroutine(coroutineActual); coroutineActual = null; }
        if (textoDisplay != null) textoDisplay.text = "";
        SetVisible(false);
    }

    private IEnumerator RutinaTypewriter(string texto, float tiempoOcultar, Action alTerminar)
    {
        textoDisplay.text = "";
        foreach (char c in texto)
        {
            textoDisplay.text += c;
            yield return new WaitForSeconds(delayPorCaracter);
        }
        alTerminar?.Invoke();
        if (tiempoOcultar > 0f)
        {
            yield return new WaitForSeconds(tiempoOcultar);
            textoDisplay.text = "";
            SetVisible(false);
        }
        coroutineActual = null;
    }

    [ContextMenu("TEST - Mostrar pensamiento")]
    private void TestMostrar() => MostrarPensamiento("Voy a prepararme el desayuno...");
}
