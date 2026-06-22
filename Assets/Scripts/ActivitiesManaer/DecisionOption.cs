using UnityEngine;

public class DecisionOption : MonoBehaviour
{
    [Header("Animacion")]
    [Tooltip("Malla a levitar al apuntar. Debe ser un hijo SIN el collider (el collider queda quieto para que el rayo no pierda el objeto). Vacio = busca un Renderer hijo, o usa este mismo objeto.")]
    [SerializeField] private Transform visual;

    /// <summary>Transform que se mueve al levitar (la malla), no el de la raiz/collider.</summary>
    public Transform Visual
    {
        get
        {
            if (visual != null) return visual;
            Renderer r = GetComponentInChildren<Renderer>();
            return (r != null && r.transform != transform) ? r.transform : transform;
        }
    }

    public void OnHoverEnter()
    {
        DecisionController.Instance?.NotifyHover(transform);          // levitar
        GetComponentInChildren<InteractableHighlight>(true)?.HoverOn(); // color
    }

    public void OnHoverExit()
    {
        DecisionController.Instance?.NotifyUnhover(transform);
        GetComponentInChildren<InteractableHighlight>(true)?.HoverOff();
    }

    public void OnSelected() => DecisionController.Instance?.NotifySelected(transform);
}
