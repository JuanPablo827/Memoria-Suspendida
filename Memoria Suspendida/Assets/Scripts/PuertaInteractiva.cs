using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuertaInteractiva : MonoBehaviour
{
    [Header("Configuraci�n de escena")]
    [Tooltip("Nombre o �ndice de la escena a la que se cambiar�")]
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
        // Si el jugador est� cerca y presiona Q
        if (jugadorCerca && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }

    // Detecta cuando el jugador entra en el �rea de la puerta
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (cartelUI != null)
                cartelUI.SetActive(true);
        }
    }

    // Detecta cuando el jugador sale del �rea
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