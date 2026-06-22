using UnityEngine;

public class VignetteEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Shader shader;
    [SerializeField] private Camera camara;

    [Header("Appearance")]
    [SerializeField] private Color color = Color.black;
    [Range(0f, 1f)][SerializeField] private float radio = 0.35f;
    [Range(0.001f, 1f)][SerializeField] private float suavidad = 0.35f;

    [Header("Placement / animation")]
    [SerializeField] private float distancia = 0.3f;
    [SerializeField] private float margen = 1.4f;
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

    public void SetIntensidad(float valor)
    {
        intensidadObjetivo = Mathf.Clamp01(valor);
    }

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
