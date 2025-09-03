using UnityEngine;
using PackageProductos;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;
public class UsaProducto : MonoBehaviour
{
    List<Producto> listaP = new List<Producto>();



    public void Start()
    {
        CargaArchivo();
    }


    public void CargaArchivo()
    {

        string filePath = Path.Combine(Application.streamingAssetsPath, "productos.txt");




        if (File.Exists(filePath))
        {


            try
            {
                string contenidoTexto = File.ReadAllText(filePath);
                Debug.Log(contenidoTexto);

                Producto p = new Producto(contenidoTexto[1], contenidoTexto[2], contenidoTexto[3]);


                Debug.Log("Producto leido" + p.Id);
                listaP.Add(p);

            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al leer el texto" + e.Message);
            }
        }

    }













}
