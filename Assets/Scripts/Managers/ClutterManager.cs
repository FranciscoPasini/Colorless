using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClutterManager : MonoBehaviour
{
    public static ClutterManager Instance { get; private set; }

    [Serializable]
    public class GrupoDesorden
    {
        [Range(1, 7)] public int diaDesde = 3;

        [FormerlySerializedAs("objetos")]
        public List<GameObject> activar = new List<GameObject>();

        public List<GameObject> desactivar = new List<GameObject>();
    }

    [SerializeField] private List<GrupoDesorden> grupos = new List<GrupoDesorden>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

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
