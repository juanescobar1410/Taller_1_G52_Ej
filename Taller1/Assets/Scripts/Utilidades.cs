using System;
using System.IO;
using System.Text;
using UnityEngine;


public class Utilidades: MonoBehaviour
{
    String path;
    String contenidoTexto;


    void Start()
    {

        path = Path.Combine(Application.streamingAssetsPath, "productos");
        contenidoTexto = File.ReadAllText(path, Encoding.UTF8);

        Debug.Log(contenidoTexto);

    }

}
