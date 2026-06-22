using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    [Header("Modo dedicado (opcional)")]
    [Tooltip("Renderer propio del highlight (ej: una malla outline). Si se asigna, se enciende/apaga ESTE Renderer.")]
    [SerializeField] private Renderer highlightRenderer;

    [Header("Modo automatico")]
    [Tooltip("Material de highlight a superponer sobre la malla del objeto. Se usa SOLO si no hay highlightRenderer.")]
    [SerializeField] private Material highlightMaterial;

    [Header("Colores (se aplican sobre _HighlightColor)")]
    [Tooltip("Color normal de la silueta.")]
    [SerializeField] private Color colorBase = new Color(0f, 1f, 1f, 1f);
    [Tooltip("Color al apuntar con el control (hover).")]
    [SerializeField] private Color colorHover = new Color(1f, 0.9f, 0f, 1f);
    [Tooltip("Color cuando la opcion queda bloqueada (Dia 4).")]
    [SerializeField] private Color colorBloqueada = new Color(1f, 0f, 0f, 1f);

    private static readonly int IdHighlightColor = Shader.PropertyToID("_HighlightColor");

    private Renderer targetRenderer;
    private Material[] materialesOriginales;
    private Material matInstancia;   // instancia propia para colorear sin pisar otros objetos
    private bool usaModoDedicado;
    private bool activo;
    private bool bloqueada;          // si true, ignora el hover y se queda en colorBloqueada

    private void Awake()
    {
        usaModoDedicado = highlightRenderer != null;

        if (usaModoDedicado)
        {
            // .material instancia el material automaticamente: el color es propio de este objeto.
            matInstancia = highlightRenderer.material;
            highlightRenderer.enabled = false;
        }
        else
        {
            // Sin renderer dedicado: usamos la malla VISIBLE (el mesh prendido) y le sumamos el material.
            // Si hay un mesh duplicado apagado (ej: el del padre quieto) y otro prendido (el hijo que
            // levita), elegimos el prendido para que la silueta se dibuje sobre el que realmente se ve.
            targetRenderer = BuscarRendererVisible();
            if (targetRenderer == null || highlightMaterial == null)
                Debug.LogWarning($"COLORLESS: InteractableHighlight en '{name}' sin configurar " +
                                 "(falta un 'highlightRenderer', o un Renderer + 'highlightMaterial').");
            else
                matInstancia = new Material(highlightMaterial);   // instancia aislada
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

    /// <summary>Cambia el color al apuntar (true = hover/amarillo, false = base). Ignora si esta bloqueada.</summary>
    public void Hover(bool apuntando)
    {
        if (bloqueada) return;
        AplicarColor(apuntando ? colorHover : colorBase);
    }

    // Wrappers sin parametros para wirear facil desde el PointableUnityEventWrapper (WhenHover / WhenUnhover).
    public void HoverOn() => Hover(true);
    public void HoverOff() => Hover(false);

    /// <summary>Deja la silueta en rojo y bloqueada (no responde mas al hover). Dia 4.</summary>
    public void MarcarBloqueada()
    {
        bloqueada = true;
        AplicarColor(colorBloqueada);
    }

    /// <summary>Quita el bloqueo y vuelve al color base (para reutilizar / reversion dia 7).</summary>
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

    // Devuelve el primer Renderer VISIBLE (componente prendido y GameObject activo).
    // Si ninguno esta prendido, cae al primero que haya (para no romper).
    private Renderer BuscarRendererVisible()
    {
        Renderer[] rs = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in rs)
            if (r.enabled && r.gameObject.activeInHierarchy) return r;
        return rs.Length > 0 ? rs[0] : null;
    }
}
