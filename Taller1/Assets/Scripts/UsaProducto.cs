using PackageProductos;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class UsaProducto : MonoBehaviour
{
    public List<Producto> listaP = new List<Producto>();



    public void Start()
    {
        CargaArchivo();
    }



    public void CargaArchivo()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "productos.txt");

        if (File.Exists(filePath))
        {
            string contenidoTexto = File.ReadAllText(filePath);

            
                try
                {
                   

                    string[] lineaproductos = contenidoTexto.Split("|");


                Producto p = new Producto(
                    lineaproductos[1], lineaproductos[2], lineaproductos[3], double.Parse(lineaproductos[4]), double.Parse(lineaproductos[5]), int.Parse(lineaproductos[6])

                );

                    listaP.Add(p);
                    Debug.Log("Producto leido" + listaP);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error al leer el texto" + e.Message);
                }
        }
    }













}
