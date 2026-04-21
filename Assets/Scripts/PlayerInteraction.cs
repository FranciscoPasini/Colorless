using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Config")]
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI (opcional)")]
    public TextMeshProUGUI hintText;

    private Interactable currentTarget;

    private void Update()
    {
        DetectTarget();

        if (currentTarget != null && (Input.GetKeyDown(interactKey)))
            currentTarget.Interact();
    }

    private void DetectTarget()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Interactable found = hit.collider.GetComponentInParent<Interactable>();
            if (found != currentTarget)
            {
                currentTarget = found;
                UpdateHint();
            }
        }
        else if (currentTarget != null)
        {
            currentTarget = null;
            UpdateHint();
        }
    }

    private void UpdateHint()
    {
        if (hintText == null) return;
        hintText.text = currentTarget != null ? $"[E] {currentTarget.hintLabel}" : "";
    }
}
