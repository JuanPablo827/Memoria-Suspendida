using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("Panel del menú de pausa")]
    [SerializeField] private GameObject panelMenu;

    private bool juegoPausado = false;

    [Header("Control de nivel actual")]
    [SerializeField] private string nivelActual;


    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    private void Start()
    {
        nivelActual = SceneManager.GetActiveScene().name;
        BuscarElementosUI();

        if (panelMenu != null)
            panelMenu.SetActive(false);
    }

    private void Update()
    {
        // Pausar / Reanudar con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
                ReanudarJuego();
            else
                PausarJuego();
        }
    }

    // ---------------- MÉTODOS PRINCIPALES ----------------

    public void CargarNivel(string nombreEscena)
    {
        Debug.Log("Cargando nivel: " + nombreEscena);
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }

    public void PausarJuego()
    {
        if (panelMenu != null)
            panelMenu.SetActive(true);

        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void ReanudarJuego()
    {
        if (panelMenu != null)
            panelMenu.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void SalirDelJuego()
    {
        string pantallaInicio = "Inicio"; // 👈 nombre de tu escena inicial

        if (Application.CanStreamedLevelBeLoaded(pantallaInicio))
        {
            CargarNivel(pantallaInicio);
        }
        else
        {
            Debug.LogWarning("⚠️ La escena 'Inicio' no está en el Build Settings.");
        }
    }

    // ---------------- SOPORTE ----------------

    public bool IsPausado() => juegoPausado;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        BuscarElementosUI();

        if (panelMenu != null)
            panelMenu.SetActive(false);
    }

    private void BuscarElementosUI()
    {
        // Buscar el panel dentro de tu jerarquía (PanelMenuPausa > Canvas > Menu > FondoMenu)
        Transform panel = GameObject.Find("PanelMenuPausa")?.transform.Find("Canvas/Menu/FondoMenu");
        if (panel != null)
            panelMenu = panel.gameObject;

        // Buscar los botones dentro del panel encontrado
        Button btnReanudar = panelMenu?.transform.Find("Reanudar")?.GetComponent<Button>();
        Button btnSalir = panelMenu?.transform.Find("Salir")?.GetComponent<Button>();

        // Asignar eventos
        if (btnReanudar != null)
        {
            btnReanudar.onClick.RemoveAllListeners();
            btnReanudar.onClick.AddListener(ReanudarJuego);
        }

        if (btnSalir != null)
        {
            btnSalir.onClick.RemoveAllListeners();
            btnSalir.onClick.AddListener(SalirDelJuego);
        }
    }
}
