using UnityEngine;

/// <summary>
/// Vinieta de "enfoque" para VR. Crea un quad frente a la camara con un shader radial
/// (bordes oscuros, centro transparente) y anima su intensidad suavemente.
/// Sintoma del Dia 1. Llamar SetIntensidad(0..1) desde el SymptomManager.
/// </summary>
public class VignetteEffect : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Shader Colorless/Vignette. Arrastralo desde Assets/Shaders/Vignette.shader.")]
    [SerializeField] private Shader shader;
    [Tooltip("Camara a la que se engancha. Si se deja vacio usa Camera.main.")]
    [SerializeField] private Camera camara;

    [Header("Apariencia")]
    [SerializeField] private Color color = Color.black;
    [Range(0f, 1f)][SerializeField] private float radio = 0.35f;
    [Range(0.001f, 1f)][SerializeField] private float suavidad = 0.35f;

    [Header("Colocacion / animacion")]
    [Tooltip("Distancia del quad a la camara (debe ser mayor al near clip).")]
    [SerializeField] private float distancia = 0.3f;
    [Tooltip("Margen extra del quad para cubrir el FOV completo en ambos ojos.")]
    [SerializeField] private float margen = 1.4f;
    [Tooltip("Velocidad de transicion de la intensidad.")]
    [SerializeField] private float velocidad = 2f;

    private Material instancia;
    private Transform quad;
    private float intensidadActual;
    private float intensidadObjetivo;
    private float radioActual;
    private float radioObjetivo;

    private void Start()
    {
        if (camara == null) camara = Camera.main;
        if (camara == null || shader == null)
        {
            Debug.LogWarning("COLORLESS: VignetteEffect sin camara o sin shader asignado.");
            enabled = false;
            return;
        }
        CrearQuad();
        Aplicar(0f);
    }

    private void CrearQuad()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.name = "VignetteQuad";
        Destroy(go.GetComponent<Collider>());

        quad = go.transform;
        quad.SetParent(camara.transform, false);
        quad.localPosition = new Vector3(0f, 0f, distancia);
        quad.localRotation = Quaternion.identity;

        float fov = camara.fieldOfView <= 0f ? 60f : camara.fieldOfView;
        float alto = 2f * distancia * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float ancho = alto * Mathf.Max(camara.aspect, 1f);
        quad.localScale = new Vector3(ancho * margen, alto * margen, 1f);

        radioActual = radioObjetivo = radio;

        instancia = new Material(shader);
        instancia.SetColor("_Color", color);
        instancia.SetFloat("_Radio", radio);
        instancia.SetFloat("_Suavidad", suavidad);

        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        mr.sharedMaterial = instancia;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;
    }

    /// <summary>Setea la intensidad objetivo (0 = sin vinieta, 1 = maxima). Llamado por el SymptomManager.</summary>
    public void SetIntensidad(float valor)
    {
        intensidadObjetivo = Mathf.Clamp01(valor);
    }

    /// <summary>Setea el radio objetivo (mas chico = mas cerrada / tunel). Para el stack del Dia 5.</summary>
    public void SetRadio(float valor)
    {
        radioObjetivo = Mathf.Clamp01(valor);
    }

    private void Update()
    {
        bool cambia = false;

        if (!Mathf.Approximately(intensidadActual, intensidadObjetivo))
        {
            intensidadActual = Mathf.MoveTowards(intensidadActual, intensidadObjetivo, velocidad * Time.deltaTime);
            cambia = true;
        }
        if (!Mathf.Approximately(radioActual, radioObjetivo))
        {
            radioActual = Mathf.MoveTowards(radioActual, radioObjetivo, velocidad * Time.deltaTime);
            cambia = true;
        }

        if (cambia) Aplicar(intensidadActual);
    }

    private void Aplicar(float v)
    {
        if (instancia == null) return;
        instancia.SetFloat("_Intensidad", v);
        instancia.SetFloat("_Radio", radioActual);
        if (quad != null) quad.gameObject.SetActive(v > 0.001f);
    }
}
