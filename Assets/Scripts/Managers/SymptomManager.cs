using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SymptomManager : MonoBehaviour
{
    public static SymptomManager Instance { get; private set; }

    [Serializable]
    public class Sintoma
    {
        public string clave;
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

        foreach (Sintoma s in sintomas)
        {
            if (s.clave == clave)
                s.accion?.Invoke();
        }
    }
}
