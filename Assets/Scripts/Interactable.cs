using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string hintLabel = "Interactuar";
    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract.Invoke();
    }
}
