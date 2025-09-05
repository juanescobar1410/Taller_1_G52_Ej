
using PackageProductos;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UsaProducto : MonoBehaviour
{
    public List<Producto> listaP = new List<Producto>();   // productos cargados desde el archivo
    public Stack<Producto> pilaProductos = new Stack<Producto>(); // productos generados (apilados)
    public TMP_Text pilaText;

    private bool generando = false;
    private bool despachando = false;
    private int totalDespachados = 0;
    private float tiempoTotalDespachados = 0f;
    private Dictionary<string, int> despachoporTipos = new Dictionary<string, int>();

    private float tiempoSiguienteDespacho = 0f;


    public void Start()
    {
        CargaArchivo();
        ActualizarTextoPila();
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

                pilaProductos.Push(copia);

                
            }

            ActualizarTextoPila();

            yield return new WaitForSeconds(1f); // espera 1 segundo antes de repetir
        }
    }

    public void DespacharProducto()
    {

        if(pilaProductos.Count > 0)
        {
            Producto despachado = pilaProductos.Pop();
            totalDespachados++;

            tiempoTotalDespachados += despachado.Tiempo;

            if (despachoporTipos.ContainsKey(despachado.Tipo))
            {
                despachoporTipos[despachado.Tipo]++;
            }
            else
            {
                despachoporTipos[despachado.Tipo] = 1;
            }

            Debug.Log("Producto despachado ? ID: " + despachado.Id +
                " | Nombre: " + despachado.Nombre +
                " | Tipo: " + despachado.Tipo +
                " | Peso: " + despachado.Peso +
                " | Precio: " + despachado.Precio +
                " | Tiempo: " + despachado.Tiempo
            );

            tiempoSiguienteDespacho = Time.time + despachado.Tiempo;
            ActualizarTextoPila();
        
        }
        else
        {
            Debug.Log("No hay productos para despachar.");
        }

        


    }
    public void ActualizarTextoPila()
    {
        string mostrar = "";

        foreach (var item in pilaProductos)
        {
            mostrar += item.ToString() + "\n";
        }
        pilaText.text = mostrar;
    }



}
