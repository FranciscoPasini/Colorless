  using UnityEngine;

  public class ObjectColor : MonoBehaviour
{
    [Tooltip("Color individual de este objeto")]
    public Color colorPropio = Color.white;

    [Header("Control de color")]
    public bool pierdeColor = true;
    [Range(1, 7)] public int diaInicio = 1;
    [Range(1, 7)] public int diaFinal = 7;
    public float velocidadTransicion = 0.5f;

    private Renderer rend;
    private MaterialPropertyBlock block;
    private float saturacionActual = 1f;
    private float saturacionObjetivo = 1f;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        AplicarSaturacion(saturacionActual);
    }

    private void OnValidate()
    {
        if (diaFinal < diaInicio) diaFinal = diaInicio;
        if (rend == null) rend = GetComponent<Renderer>();
        if (block == null) block = new MaterialPropertyBlock();
        AplicarSaturacion(saturacionActual);
    }

    private void Update()
    {
        if (Mathf.Approximately(saturacionActual, saturacionObjetivo)) return;
        saturacionActual = Mathf.MoveTowards(
            saturacionActual, saturacionObjetivo, velocidadTransicion * Time.deltaTime);
        AplicarSaturacion(saturacionActual);
    }

    public void ActualizarParaDia(int dia)
    {
        if (!pierdeColor)
        {
            saturacionObjetivo = 1f;
            return;
        }

        if (dia < diaInicio)
            saturacionObjetivo = 1f;
        else if (dia >= diaFinal)
            saturacionObjetivo = 0f;
        else
        {
            float t = (float)(dia - diaInicio) / (diaFinal - diaInicio);
            saturacionObjetivo = 1f - t;
        }
    }

    public void AplicarSaturacion(float saturacion)
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (block == null) block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);
        block.SetColor("_Color", colorPropio);
        block.SetFloat("_Saturation", saturacion);
        rend.SetPropertyBlock(block);
    }
}