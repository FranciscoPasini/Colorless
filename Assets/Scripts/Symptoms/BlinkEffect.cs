using System.Collections;
using UnityEngine;

/// <summary>
/// Parpadeo periodico para VR (sintoma de cansancio, Dia 2). Crea un quad negro a pantalla
/// completa frente a la camara y lo cierra/abre rapido cada cierto intervalo.
/// Llamar Activar() desde el SymptomManager; Desactivar() para frenarlo.
/// </summary>
public class BlinkEffect : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Shader Colorless/ScreenOverlay. Arrastralo desde Assets/Shaders/ScreenOverlay.shader.")]
    [SerializeField] private Shader shader;
    [Tooltip("Camara a la que se engancha. Si se deja vacio usa Camera.main.")]
    [SerializeField] private Camera camara;

    [Header("Apariencia")]
    [SerializeField] private Color color = Color.black;
    [Range(0f, 1f)][SerializeField] private float opacidadMax = 1f;

    [Header("Tiempos")]
    [Tooltip("Intervalo base entre parpadeos (seg).")]
    [SerializeField] private float intervalo = 90f;
    [Tooltip("Variacion aleatoria +/- sobre el intervalo (seg).")]
    [SerializeField] private float variacion = 15f;
    [SerializeField] private float tiempoCierre = 0.1f;
    [SerializeField] private float tiempoApertura = 0.18f;

    [Header("Colocacion")]
    [SerializeField] private float distancia = 0.3f;
    [SerializeField] private float margen = 1.5f;

    private Material instancia;
    private Transform quad;
    private Coroutine rutina;

    private void Start()
    {
        if (camara == null) camara = Camera.main;
        if (camara == null || shader == null)
        {
            Debug.LogWarning("COLORLESS: BlinkEffect sin camara o sin shader asignado.");
            enabled = false;
            return;
        }
        CrearQuad();
        SetAlpha(0f);
    }

    private void CrearQuad()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.name = "BlinkQuad";
        Destroy(go.GetComponent<Collider>());

        quad = go.transform;
        quad.SetParent(camara.transform, false);
        quad.localPosition = new Vector3(0f, 0f, distancia);
        quad.localRotation = Quaternion.identity;

        float fov = camara.fieldOfView <= 0f ? 60f : camara.fieldOfView;
        float alto = 2f * distancia * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float ancho = alto * Mathf.Max(camara.aspect, 1f);
        quad.localScale = new Vector3(ancho * margen, alto * margen, 1f);

        instancia = new Material(shader);
        instancia.SetColor("_Color", color);

        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        mr.sharedMaterial = instancia;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;

        go.SetActive(false);
    }

    public void Activar()
    {
        if (rutina != null) return;
        rutina = StartCoroutine(LoopParpadeo());
    }

    public void Desactivar()
    {
        if (rutina != null) { StopCoroutine(rutina); rutina = null; }
        SetAlpha(0f);
        if (quad != null) quad.gameObject.SetActive(false);
    }

    /// <summary>Dispara un parpadeo unico (util para testear o reusar en otros momentos).</summary>
    public void ParpadearUnaVez()
    {
        StartCoroutine(UnParpadeo());
    }

    private IEnumerator LoopParpadeo()
    {
        while (true)
        {
            float espera = Mathf.Max(0.1f, intervalo + Random.Range(-variacion, variacion));
            yield return new WaitForSeconds(espera);
            yield return StartCoroutine(UnParpadeo());
        }
    }

    private IEnumerator UnParpadeo()
    {
        if (quad != null) quad.gameObject.SetActive(true);
        yield return Fade(0f, opacidadMax, tiempoCierre);
        yield return Fade(opacidadMax, 0f, tiempoApertura);
        if (quad != null) quad.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float desde, float hasta, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(desde, hasta, t / dur));
            yield return null;
        }
        SetAlpha(hasta);
    }

    private void SetAlpha(float a)
    {
        if (instancia != null) instancia.SetFloat("_Alpha", a);
    }
}
