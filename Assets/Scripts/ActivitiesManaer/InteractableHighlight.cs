using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    [SerializeField] private Renderer highlightRenderer;
    private void Awake()
    {
        if (highlightRenderer != null)
            highlightRenderer.enabled = false;
    }

    public void Activar()
    {
        if (highlightRenderer != null) highlightRenderer.enabled = true;
    }

    public void Desactivar()
    {
        if (highlightRenderer != null) highlightRenderer.enabled = false;
    }
}
