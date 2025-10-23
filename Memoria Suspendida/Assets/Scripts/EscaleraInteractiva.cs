using UnityEngine;
using UnityEngine.SceneManagement;

public class EscaleraInteractiva : MonoBehaviour
{
    [Header("Configuraci�n de escena")]
    public string Arriba;

    [Header("UI de interacci�n")]
    public GameObject cartelTexto;     // Texto �Presiona Q�
    public GameObject burbujaImagen;   // Imagen burbuja

    private bool jugadorCerca = false;

    void Start()
    {
        if (cartelTexto != null) cartelTexto.SetActive(false);
        if (burbujaImagen != null) burbujaImagen.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(Arriba);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (cartelTexto != null) cartelTexto.SetActive(true);
            if (burbujaImagen != null) burbujaImagen.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (cartelTexto != null) cartelTexto.SetActive(false);
            if (burbujaImagen != null) burbujaImagen.SetActive(false);
        }
    }
}