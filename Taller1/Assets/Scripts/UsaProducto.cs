using UnityEngine;
using PackageProductos;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;
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




        if (File.Exists(filePath) )
        {
            string[] lineas = File.ReadAllLines(filePath);
            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                try
                {
                    string contenidoTexto = File.ReadAllText(filePath);
                    Debug.Log(contenidoTexto);



                    string[] lineaproductos = linea.Split("|");
                    string.IsNullOrWhiteSpace(contenidoTexto);

                    Producto p = new Producto(lineaproductos[1], lineaproductos[2], lineaproductos[3], double.Parse(lineaproductos[4]), double.Parse(lineaproductos[5]), int.Parse(lineaproductos[0]));

                    listaP.Add(p);
                    Debug.Log("Producto leido" + listaP.Count);


                }

            }        
            catch (System.Exception e)
            {
                Debug.LogError("Error al leer el texto" + e.Message);
            }
        }

    }













}
