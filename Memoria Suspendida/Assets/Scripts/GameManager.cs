using UnityEngine;
using UnityEngine.SceneManagement;

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

    // ---------------- MÉTODOS PÚBLICOS ----------------

    public void CargarNivel(string nombreEscena)
    {
        Debug.Log("Cargando nivel: " + nombreEscena);
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté normal
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
        string pantallaInicio = "Inicio"; // 👈 nombre de tu escena de inicio

        if (Application.CanStreamedLevelBeLoaded(pantallaInicio))
        {
            CargarNivel(pantallaInicio);
        }
        else
        {
            Debug.LogWarning("⚠️ La escena 'Inicio' no está en el Build Settings.");
        }
    }

    public void EstadoDeJuego(string estado)
    {
        switch (estado)
        {
            case "Pausa":
                PausarJuego();
                break;
            case "Jugando":
                ReanudarJuego();
                break;
            case "Salir":
                SalirDelJuego();
                break;
        }
    }

    public bool IsPausado()
    {
        return juegoPausado;
    }

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
        // Asegurar que al cargar una escena el juego esté activo
        Time.timeScale = 1f;
        juegoPausado = false;

        // Ocultar menú si no debe estar visible
        if (panelMenu != null)
            panelMenu.SetActive(false);
    }

}