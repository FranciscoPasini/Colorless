using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Maneja el desorden acumulativo (Dia 3 en adelante). Cada grupo se aplica a partir de
/// 'diaDesde': ACTIVA los objetos de 'activar' y DESACTIVA los de 'desactivar'. Sirve para
/// hacer swaps (ej: ocultar los platos acomodados y mostrar los desacomodados).
/// Es acumulativo y reversible: si se vuelve a un dia anterior, revierte el swap.
/// DayManager lo llama durante el fundido a negro al dormir.
/// </summary>
public class ClutterManager : MonoBehaviour
{
    public static ClutterManager Instance { get; private set; }

    [Serializable]
    public class GrupoDesorden
    {
        [Tooltip("Dia a partir del cual se aplica este grupo (acumulativo).")]
        [Range(1, 7)] public int diaDesde = 3;

        [FormerlySerializedAs("objetos")]
        [Tooltip("Objetos que se ACTIVAN al llegar al dia (ej: platos desacomodados). Dejarlos DESACTIVADOS en la escena.")]
        public List<GameObject> activar = new List<GameObject>();

        [Tooltip("Objetos que se DESACTIVAN al llegar al dia (ej: platos acomodados). Dejarlos ACTIVADOS en la escena.")]
        public List<GameObject> desactivar = new List<GameObject>();
    }

    [SerializeField] private List<GrupoDesorden> grupos = new List<GrupoDesorden>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>
    /// Aplica todos los grupos segun el dia. Llamado por DayManager.AplicarDia.
    /// </summary>
    public void AplicarDia(int dia)
    {
        foreach (GrupoDesorden grupo in grupos)
        {
            bool alcanzado = dia >= grupo.diaDesde;
            SetActivo(grupo.activar, alcanzado);
            SetActivo(grupo.desactivar, !alcanzado);
        }
    }

    private void SetActivo(List<GameObject> lista, bool estado)
    {
        foreach (GameObject obj in lista)
        {
            if (obj != null && obj.activeSelf != estado)
                obj.SetActive(estado);
        }
    }
}
