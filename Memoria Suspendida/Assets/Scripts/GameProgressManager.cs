using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    [Header("Progreso del jugador")]
    public int objetosTotales = 3;
    private int objetosRecuperados = 0;

    [Header("UI Final")]
    public GameObject burbujaFinal; // Imagen con TMP dentro
    public string escenaFinal = "Final";

    private bool puedeFinalizar = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameProgressManager] Instance creada y marcada como DontDestroyOnLoad.");

            // Escuchar evento de cambio de escena para volver a encontrar la burbuja
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Intentar encontrar una nueva burbuja final cuando cambia de escena
        if (burbujaFinal == null)
        {
            burbujaFinal = GameObject.FindWithTag("BurbujaFinal");

            if (burbujaFinal != null)
            {
                burbujaFinal.SetActive(false);
                Debug.Log($"[GameProgressManager] burbujaFinal encontrada en la escena '{scene.name}'.");
            }
            else
            {
                Debug.Log("[GameProgressManager] No se encontró burbujaFinal en la nueva escena.");
            }
        }
    }

    void Start()
    {
        if (burbujaFinal != null)
            burbujaFinal.SetActive(false);
    }

    public void RegistrarObjetoRecuperado(string objetoName = null)
    {
        objetosRecuperados++;
        Debug.Log($"[GameProgressManager] Objeto recuperado ({objetosRecuperados}/{objetosTotales}) {(string.IsNullOrEmpty(objetoName) ? "" : "-> " + objetoName)}");

        if (objetosRecuperados >= objetosTotales)
        {
            Debug.Log("[GameProgressManager] ¡Todos los objetos recuperados! El final está desbloqueado.");
            DesbloquearFinal();
        }
    }

    void DesbloquearFinal()
    {
        puedeFinalizar = true;

        if (burbujaFinal == null)
        {
            // Intentar encontrarla por tag una última vez
            burbujaFinal = GameObject.FindWithTag("BurbujaFinal");
        }

        if (burbujaFinal != null)
        {
            burbujaFinal.SetActive(true);
            Debug.Log("[GameProgressManager] burbujaFinal ACTIVADA correctamente.");
        }
        else
        {
            Debug.LogWarning("[GameProgressManager] No se encontró ninguna burbujaFinal en esta escena.");
        }
    }

    void Update()
    {
        if (puedeFinalizar && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log($"[GameProgressManager] Cargando escena final: {escenaFinal}");
            SceneManager.LoadScene(escenaFinal);
        }
    }

    public bool HaRecuperadoTodos()
    {
        return objetosRecuperados >= objetosTotales;
    }
}
