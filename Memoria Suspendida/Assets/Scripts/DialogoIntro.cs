using UnityEngine;
using TMPro;
using System.Collections;

public class DialogoIntro : MonoBehaviour
{
    [TextArea(3, 5)]
    public string[] lineas; // Lista de textos

    public TMP_Text textoUI;          // Referencia al texto en pantalla
    public GameObject burbujaUI;      // Referencia a la burbuja de chat (el fondo)
    public float tiempoAntesDeDesaparecer = 3f; // Segundos antes de ocultar al final

    private int indice = 0;
    private bool dialogoTerminado = false;

    void Start()
    {
        textoUI.text = lineas[indice];
        burbujaUI.SetActive(true);
    }

    void Update()
    {
        if (!dialogoTerminado && Input.GetKeyDown(KeyCode.Space))
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
            dialogoTerminado = true;
            StartCoroutine(DesaparecerDespuesDeTiempo());
        }
    }

    IEnumerator DesaparecerDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoAntesDeDesaparecer);
        textoUI.text = "";             // Borra el texto
        burbujaUI.SetActive(false);    // Oculta la burbuja
    }
}
