using UnityEngine;

public class DecisionOption : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Transform visual;

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
        DecisionController.Instance?.NotifyHover(transform);
        GetComponentInChildren<InteractableHighlight>(true)?.HoverOn();
    }

    public void OnHoverExit()
    {
        DecisionController.Instance?.NotifyUnhover(transform);
        GetComponentInChildren<InteractableHighlight>(true)?.HoverOff();
    }

    public void OnSelected() => DecisionController.Instance?.NotifySelected(transform);
}
