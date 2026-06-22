using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string hintLabel = "Interact";
    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract.Invoke();
    }
}
