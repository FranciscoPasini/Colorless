using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    [Header("Modo dedicado (opcional)")]
    [Tooltip("Renderer propio del highlight (ej: una malla outline). Si se asigna, se enciende/apaga ESTE Renderer.")]
    [SerializeField] private Renderer highlightRenderer;

    [Header("Modo automatico")]
    [Tooltip("Material de highlight a superponer sobre la malla del objeto. Se usa SOLO si no hay highlightRenderer.")]
    [SerializeField] private Material highlightMaterial;

    private Renderer targetRenderer;
    private Material[] materialesOriginales;
    private bool usaModoDedicado;
    private bool activo;

    private void Awake()
    {
        usaModoDedicado = highlightRenderer != null;

        if (usaModoDedicado)
        {
            highlightRenderer.enabled = false;
        }
        else
        {
            // Sin renderer dedicado: usamos la malla propia (o de un hijo) y le sumamos el material de highlight.
            targetRenderer = GetComponentInChildren<Renderer>();
            if (targetRenderer == null || highlightMaterial == null)
                Debug.LogWarning($"COLORLESS: InteractableHighlight en '{name}' sin configurar " +
                                 "(falta un 'highlightRenderer', o un Renderer + 'highlightMaterial').");
        }
    }

    public void Activar()
    {
        if (activo) return;
        activo = true;

        if (usaModoDedicado)
        {
            highlightRenderer.enabled = true;
            return;
        }

        if (targetRenderer == null || highlightMaterial == null) return;

        materialesOriginales = targetRenderer.sharedMaterials;
        var conHighlight = new Material[materialesOriginales.Length + 1];
        materialesOriginales.CopyTo(conHighlight, 0);
        conHighlight[conHighlight.Length - 1] = highlightMaterial;
        targetRenderer.sharedMaterials = conHighlight;
    }

    public void Desactivar()
    {
        if (!activo) return;
        activo = false;

        if (usaModoDedicado)
        {
            highlightRenderer.enabled = false;
            return;
        }

        if (targetRenderer != null && materialesOriginales != null)
            targetRenderer.sharedMaterials = materialesOriginales;
    }
}
