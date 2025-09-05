//using PackageProductos;
//using System.Collections.Generic;
//using System.IO;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UIElements;
//public class UsaProducto : MonoBehaviour
//{
//    public List<Producto> listaP = new List<Producto>();



//    public void Start()
//    {
//        CargaArchivo();
//    }



//    public void CargaArchivo()
//    {
//        string filePath = Path.Combine(Application.streamingAssetsPath, "productos.txt");

//        if (File.Exists(filePath))
//        {
//            string contenidoTexto = File.ReadAllText(filePath);


//            try
//            {


//                //string[] lineaproductos = contenidoTexto.Split('|');


//                //Producto p = new Producto(
//                //    lineaproductos[0], lineaproductos[1], lineaproductos[2], double.Parse(lineaproductos[3]), double.Parse(lineaproductos[4]), int.Parse(lineaproductos[5])

//                //);

//                //listaP.Add(p);
//                string[] lineas = contenidoTexto.Split('\n');

//                foreach (string linea in lineas)
//                {
//                    if (string.IsNullOrWhiteSpace(linea)) continue;
//                    string[] datos = linea.Split('|');

//                    Producto p = new Producto(
//                        datos[0], datos[1], datos[2],
//                        double.Parse(datos[3]), double.Parse(datos[4]), int.Parse(datos[5])
//                    );

//                    listaP.Add(p);
//                }






//                //Debug.Log("Producto leido" + listaP);

//                for (int i = 0; i < listaP.Count; i++)
//                {
//                    Debug.Log(
//                        " ID: " + listaP[i].Id +
//                        " | Nombre: " + listaP[i].Nombre +
//                        " | Tipo: " + listaP[i].Tipo +
//                        " | Peso: " + listaP[i].Peso +
//                        " | Precio: " + listaP[i].Precio +
//                        " | Tiempo: " + listaP[i].Tiempo
//                    );
//                }


//            }
//            catch (System.Exception e)
//            {
//                Debug.LogError("Error al leer el texto" + e.Message);
//            }
//        }
//    }








//}

using PackageProductos;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UsaProducto : MonoBehaviour
{
    public List<Producto> listaP = new List<Producto>();   // productos cargados desde el archivo
    private List<Producto> pilaProductos = new List<Producto>(); // productos generados (apilados)

    private bool generando = false;

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

                Debug.Log("Productos cargados: " + listaP.Count);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al leer el texto: " + e.Message);
            }
        }
    }

    // Método que se llamará desde el botón "Iniciar"
    public void IniciarGeneracion()
    {
        if (!generando)
        {
            generando = true;
            StartCoroutine(GenerarProductos());
        }
    }

    // Método opcional para detener
    public void DetenerGeneracion()
    {
        generando = false;
        StopAllCoroutines();
    }

    // Corutina que genera productos aleatorios cada segundo
    private System.Collections.IEnumerator GenerarProductos()
    {
        while (generando)
        {
            int cantidad = Random.Range(1, 4); // entre 1 y 3 productos

            for (int i = 0; i < cantidad; i++)
            {
                // Elegir un producto aleatorio de la lista cargada
                Producto elegido = listaP[Random.Range(0, listaP.Count)];

                // Crear una copia para no modificar el catálogo
                Producto copia = new Producto(
                    elegido.Id,
                    elegido.Nombre,
                    elegido.Tipo,
                    elegido.Peso,
                    elegido.Precio,
                    elegido.Tiempo
                );

                pilaProductos.Add(copia);

                Debug.Log(
                    "Producto apilado ? ID: " + copia.Id +
                    " | Nombre: " + copia.Nombre +
                    " | Tipo: " + copia.Tipo +
                    " | Peso: " + copia.Peso +
                    " | Precio: " + copia.Precio +
                    " | Tiempo: " + copia.Tiempo
                );
            }

            yield return new WaitForSeconds(1f); // espera 1 segundo antes de repetir
        }
    }
}
