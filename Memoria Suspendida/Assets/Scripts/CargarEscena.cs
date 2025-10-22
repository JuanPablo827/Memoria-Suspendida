using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    public string nombreEscena;

    public void Cargar()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
