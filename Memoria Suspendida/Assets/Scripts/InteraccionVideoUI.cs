using UnityEngine;
using UnityEngine.Video;

public class InteraccionVideoUI : MonoBehaviour
{
    [Header("Referencias (puedes dejarlas vacías para que se autoasignen)")]
    public GameObject panelVideoUI;          // RawImage con el video (UI panel)
    public VideoPlayer videoPlayer;          // El Video Player (componente)
    public GameObject burbujaInteractiva;    // Imagen + texto "Presiona E"
    public KeyCode teclaInteraccion = KeyCode.E;

    [Header("Distancia de interacción (fallback si no usas trigger)")]
    public float distanciaInteraccion = 2f;
    public bool usarTrigger2D = true; // si usas Collider2D con isTrigger, déjalo true

    private Transform jugador;
    private bool enRango = false;
    private bool videoActivo = false;

    private bool yaRecuperado = false;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (jugador == null)
            Debug.LogWarning("[InteraccionVideoUI] No se encontró ningún GameObject con tag 'Player'.");

        // Si no asignaste el VideoPlayer, busca uno en hijos o en escena
        if (videoPlayer == null)
            videoPlayer = GetComponentInChildren<VideoPlayer>();

        if (videoPlayer == null)
            videoPlayer = FindObjectOfType<VideoPlayer>(); // último recurso

        if (videoPlayer == null)
            Debug.LogWarning("[InteraccionVideoUI] No se encontró VideoPlayer. Asigna uno en el inspector.");

        if (panelVideoUI != null)
            panelVideoUI.SetActive(false);
        else
            Debug.LogWarning("[InteraccionVideoUI] panelVideoUI no asignado.");

        if (burbujaInteractiva != null)
            burbujaInteractiva.SetActive(false);
        else
            Debug.LogWarning("[InteraccionVideoUI] burbujaInteractiva no asignada.");
    }

    void Update()
    {
        // Si el jugador no existe, no hay nada que hacer
        if (jugador == null) return;

        // Si no usamos trigger, calculamos la distancia y manejamos enRango por Update
        if (!usarTrigger2D)
        {
            float distancia = Vector2.Distance(jugador.position, transform.position);
            enRango = distancia <= distanciaInteraccion;
        }

        // Mostrar / ocultar burbuja solo cuando no hay video activo
        if (burbujaInteractiva != null)
            burbujaInteractiva.SetActive(enRango && !videoActivo);

        // Interacción con tecla E
        if (enRango && Input.GetKeyDown(teclaInteraccion))
        {
            if (!videoActivo)
                MostrarVideo();
            else
                OcultarVideo();
        }
    }

    void MostrarVideo()
    {
        if (panelVideoUI != null)
            panelVideoUI.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }

        videoActivo = true;

        if (burbujaInteractiva != null)
            burbujaInteractiva.SetActive(false);

        Debug.Log("[InteraccionVideoUI] Video mostrado.");

        // 🔹 Registrar este objeto como recuperado (solo la primera vez)
        if (!yaRecuperado)
        {
            yaRecuperado = true;
            if (GameProgressManager.Instance != null)
                GameProgressManager.Instance.RegistrarObjetoRecuperado();
        }
    }

    // Agrega este campo arriba en el script:
    

    void OcultarVideo()
    {
        if (panelVideoUI != null)
            panelVideoUI.SetActive(false);

        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoActivo = false;

        // si seguimos en rango, mostrar la burbuja otra vez
        if (burbujaInteractiva != null)
            burbujaInteractiva.SetActive(enRango);

        Debug.Log("[InteraccionVideoUI] Video ocultado.");
    }

    // ----- Trigger 2D handlers -----
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!usarTrigger2D) return;
        if (other.CompareTag("Player"))
        {
            enRango = true;
            Debug.Log("[InteraccionVideoUI] Player entró en rango (TriggerEnter).");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!usarTrigger2D) return;
        if (other.CompareTag("Player"))
        {
            enRango = false;
            Debug.Log("[InteraccionVideoUI] Player salió del rango (TriggerExit).");
            if (burbujaInteractiva != null)
                burbujaInteractiva.SetActive(false);
            OcultarVideo();
        }
    }

    // Opcional: método público para forzar ocultar desde otros scripts
    public void ForzarOcultar()
    {
        OcultarVideo();
        enRango = false;
        if (burbujaInteractiva != null)
            burbujaInteractiva.SetActive(false);
    }
}
