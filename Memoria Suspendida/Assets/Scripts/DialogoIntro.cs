using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro
using UnityEngine.SceneManagement;

public class DialogoIntro : MonoBehaviour
{
    [TextArea(3, 5)]
    public string[] lineas; // Lista de textos

    public TMP_Text textoUI; // Referencia al texto en pantalla
    public GameObject botonContinuar;

    private int indice = 0;

    void Start()
    {
        textoUI.text = lineas[indice];
        botonContinuar.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AvanzarTexto();
    }

    void AvanzarTexto()
    {
        if (indice < lineas.Length - 1)
        {
            indice++;
            textoUI.text = lineas[indice];
        }
        else
        {
            botonContinuar.SetActive(true);
        }
    }

    // Pon esto en el botón "Continuar"
    public void IrAEscenaExterior()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia el nombre según tu escena
    }
}