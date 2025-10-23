using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuertaInteractiva : MonoBehaviour
{
    [Header("Configuración de escena")]
    [Tooltip("Nombre o índice de la escena a la que se cambiará")]
    public string nombreEscenaDestino;

    [Header("UI del mensaje")]
    public GameObject cartelUI; // cartel que dice "Presiona Q"

    bool jugadorCerca = false;

    void Start()
    {
        if (cartelUI != null)
            cartelUI.SetActive(false);
    }

    void Update()
    {
        // Si el jugador está cerca y presiona Q
        if (jugadorCerca && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }

    // Detecta cuando el jugador entra en el área de la puerta
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (cartelUI != null)
                cartelUI.SetActive(true);
        }
    }

    // Detecta cuando el jugador sale del área
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (cartelUI != null)
                cartelUI.SetActive(false);
        }
    }
}