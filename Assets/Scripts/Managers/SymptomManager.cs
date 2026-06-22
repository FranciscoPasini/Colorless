using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mapea claves de sintoma (string) a acciones (UnityEvent) configurables desde el Inspector.
/// Los pasos del dia (ActivityStep.eventoAlCompletar) disparan estas claves al completarse.
/// Sirve para conectar TODOS los sintomas de los 7 dias sin tocar codigo.
/// </summary>
public class SymptomManager : MonoBehaviour
{
    public static SymptomManager Instance { get; private set; }

    [Serializable]
    public class Sintoma
    {
        [Tooltip("Clave que debe coincidir con 'eventoAlCompletar' del paso (ej: Enfoque).")]
        public string clave;
        [Tooltip("Que hacer al disparar esta clave (ej: VignetteEffect.SetIntensidad).")]
        public UnityEvent accion;
    }

    [SerializeField] private List<Sintoma> sintomas = new List<Sintoma>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Disparar(string clave)
    {
        if (string.IsNullOrEmpty(clave)) return;

        bool encontrado = false;
        foreach (Sintoma s in sintomas)
        {
            if (s.clave == clave)
            {
                s.accion?.Invoke();
                encontrado = true;
            }
        }

        if (encontrado)
            Debug.Log($"COLORLESS: Sintoma disparado '{clave}'.");
        else
            Debug.LogWarning($"COLORLESS: SymptomManager no tiene un sintoma con clave '{clave}'.");
    }
}
