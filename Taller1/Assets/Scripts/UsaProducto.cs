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


                //string[] lineaproductos = contenidoTexto.Split('|');


                //Producto p = new Producto(
                //    lineaproductos[0], lineaproductos[1], lineaproductos[2], double.Parse(lineaproductos[3]), double.Parse(lineaproductos[4]), int.Parse(lineaproductos[5])

                //);

                //listaP.Add(p);
                string[] lineas = contenidoTexto.Split('\n');

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    string[] datos = linea.Split('|');

                    Producto p = new Producto(
                        datos[0], datos[1], datos[2],
                        double.Parse(datos[3]), double.Parse(datos[4]), int.Parse(datos[5])
                    );

                    listaP.Add(p);
                }






                //Debug.Log("Producto leido" + listaP);

                for (int i = 0; i < listaP.Count; i++)
                {
                    Debug.Log(
                        " ID: " + listaP[i].Id +
                        " | Nombre: " + listaP[i].Nombre +
                        " | Tipo: " + listaP[i].Tipo +
                        " | Peso: " + listaP[i].Peso +
                        " | Precio: " + listaP[i].Precio +
                        " | Tiempo: " + listaP[i].Tiempo
                    );
                }


            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al leer el texto" + e.Message);
            }
        }
    }













}
