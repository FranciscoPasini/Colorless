using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    [Header("Dedicated mode (optional)")]
    [SerializeField] private Renderer highlightRenderer;

    [Header("Automatic mode")]
    [SerializeField] private Material highlightMaterial;

    [Header("Colors (applied to _HighlightColor)")]
    [SerializeField] private Color colorBase = new Color(0f, 1f, 1f, 1f);
    [SerializeField] private Color colorHover = new Color(1f, 0.9f, 0f, 1f);
    [SerializeField] private Color colorBloqueada = new Color(1f, 0f, 0f, 1f);

    private static readonly int IdHighlightColor = Shader.PropertyToID("_HighlightColor");

    private Renderer targetRenderer;
    private Material[] materialesOriginales;
    private Material matInstancia;
    private bool usaModoDedicado;
    private bool activo;
    private bool bloqueada;

    private void Awake()
    {
        usaModoDedicado = highlightRenderer != null;

        if (usaModoDedicado)
        {
            matInstancia = highlightRenderer.material;
            highlightRenderer.enabled = false;
        }
        else
        {
            targetRenderer = BuscarRendererVisible();
            if (targetRenderer != null && highlightMaterial != null)
                matInstancia = new Material(highlightMaterial);
        }

        AplicarColor(colorBase);
    }

    public void Activar()
    {
        if (activo) return;
        activo = true;

        if (!bloqueada) AplicarColor(colorBase);

        if (usaModoDedicado)
        {
            highlightRenderer.enabled = true;
            return;
        }

        if (targetRenderer == null || matInstancia == null) return;

        materialesOriginales = targetRenderer.sharedMaterials;
        var conHighlight = new Material[materialesOriginales.Length + 1];
        materialesOriginales.CopyTo(conHighlight, 0);
        conHighlight[conHighlight.Length - 1] = matInstancia;
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

    public void Hover(bool apuntando)
    {
        if (bloqueada) return;
        AplicarColor(apuntando ? colorHover : colorBase);
    }

    public void HoverOn() => Hover(true);
    public void HoverOff() => Hover(false);

    public void MarcarBloqueada()
    {
        bloqueada = true;
        AplicarColor(colorBloqueada);
    }

    public void Desbloquear()
    {
        bloqueada = false;
        AplicarColor(colorBase);
    }

    public bool EstaBloqueada => bloqueada;

    private void AplicarColor(Color c)
    {
        if (matInstancia != null) matInstancia.SetColor(IdHighlightColor, c);
    }

    private Renderer BuscarRendererVisible()
    {
        Renderer[] rs = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in rs)
            if (r.enabled && r.gameObject.activeInHierarchy) return r;
        return rs.Length > 0 ? rs[0] : null;
    }
}
