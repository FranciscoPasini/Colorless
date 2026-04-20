using UnityEngine;
public class ObjectColor : MonoBehaviour
{
    [Tooltip("Color individual de este objeto")]
    public Color colorPropio = Color.white;

    private Renderer rend;
    private MaterialPropertyBlock block;
    private Color colorAnterior;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        AplicarColor();
    }

    private void OnValidate()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (block == null) block = new MaterialPropertyBlock();
        AplicarColor();
    }

    private void AplicarColor()
    {
        if (rend == null) return;

        rend.GetPropertyBlock(block);
        block.SetColor("_Color", colorPropio);
        rend.SetPropertyBlock(block);

        colorAnterior = colorPropio;
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