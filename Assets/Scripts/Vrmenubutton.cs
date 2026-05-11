using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Botón minimalista para VR. Maneja hover y click con feedback visual sutil.
/// Requiere: Image en el mismo GameObject, OVRInputModule en el EventSystem.
/// </summary>
[RequireComponent(typeof(Button))]
public class VRMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Colores (paleta monocromática)")]
    public Color colorNormal = new Color(1f, 1f, 1f, 0f);   // transparente
    public Color colorHover = new Color(1f, 1f, 1f, 0.08f); // blanco muy sutil
    public Color colorClick = new Color(1f, 1f, 1f, 0.18f);

    [Header("Texto del botón")]
    public TMPro.TMP_Text label;
    public Color textoNormal = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color textoHover = Color.white;

    [Header("Animación")]
    public float velocidadTransicion = 8f;

    private Image bg;
    private Button btn;
    private Color bgObjetivo;
    private Color textoObjetivo;

    private void Awake()
    {
        bg = GetComponent<Image>();
        btn = GetComponent<Button>();

        // Deshabilitar transición nativa de Unity (usamos la nuestra)
        btn.transition = Selectable.Transition.None;

        bgObjetivo = colorNormal;
        textoObjetivo = textoNormal;

        if (bg != null) bg.color = colorNormal;
        if (label != null) label.color = textoNormal;
    }

    private void Update()
    {
        if (bg != null) bg.color = Color.Lerp(bg.color, bgObjetivo, velocidadTransicion * Time.deltaTime);
        if (label != null) label.color = Color.Lerp(label.color, textoObjetivo, velocidadTransicion * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData _)
    {
        bgObjetivo = colorHover;
        textoObjetivo = textoHover;
    }

    public void OnPointerExit(PointerEventData _)
    {
        bgObjetivo = colorNormal;
        textoObjetivo = textoNormal;
    }

    public void OnPointerClick(PointerEventData _)
    {
        StartCoroutine(AnimacionClick());
    }

    private IEnumerator AnimacionClick()
    {
        if (bg != null) bg.color = colorClick;
        yield return new WaitForSeconds(0.1f);
        bgObjetivo = colorNormal;
    }
}