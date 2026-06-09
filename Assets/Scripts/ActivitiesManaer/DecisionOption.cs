using UnityEngine;

public class DecisionOption : MonoBehaviour
{
    public void OnHoverEnter() => DecisionController.Instance?.NotifyHover(transform);
    public void OnHoverExit() => DecisionController.Instance?.NotifyUnhover(transform);
    public void OnSelected() => DecisionController.Instance?.NotifySelected(transform);
}
