
using PackageProductos;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using JetBrains.Annotations;
public class UsaProducto : MonoBehaviour
{
    List<string> listaMetricas = new List<string>();
    public List<Producto> listaP = new List<Producto>();   // productos cargados desde el archivo
    public Stack<Producto> pilaProductos = new Stack<Producto>(); // productos generados (apilados)
    public TMP_Text TextoProductos;
    public TMP_Text TextoTamaño;
    public TMP_Text TextoDespachados;
    public TMP_Text TextoTope;
    public TMP_Text TextoContador;

    private bool ContadorActivo;
    private float TiempoTranscurrido;
    private bool generando = false;
    private bool despachando = false;
    private int totalDespachados = 0;
    private int totalGenerados = 0;
    private int totalNoDespachados;
    private float tiempoTotalDespachados = 0f;
    private Dictionary<string, int> despachoporTipos = new Dictionary<string, int>();

    private float tiempoSiguienteDespacho = 0f;


    public void Start()
    {
        TiempoTranscurrido = 0f;
        CargaArchivo();

        despachoporTipos.Add("Basico", 0);
        despachoporTipos.Add("Fragil", 0);
        despachoporTipos.Add("Pesado", 0);
        
        ActualizarTextoPila();
    }

    public void Update()
    {
        
        if(ContadorActivo)
        {
            TiempoTranscurrido += Time.deltaTime;
            int minutos = Mathf.FloorToInt(TiempoTranscurrido / 60f);
            int segundos = Mathf.FloorToInt(TiempoTranscurrido % 60f);
            TextoContador.text = $"{minutos:00}:{segundos:00}";
        }

        if (despachando && Time.time >= tiempoSiguienteDespacho && pilaProductos.Count > 0)
        {
            DespacharProducto();
        }
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

    //  botón "Iniciar"
    public void IniciarGeneracion()
    {
        ContadorActivo = true;
        TiempoTranscurrido = 0f;
        if (!generando)
        {
            generando = true;
            despachando = true;

            tiempoSiguienteDespacho = Time.time + 1f; 
            StartCoroutine(GenerarProductos());
        }
    }

    // Método para detener
    public void DetenerGeneracion()
    {
        ContadorActivo = false;
        generando = false;
        despachando = false;
        totalNoDespachados = totalGenerados - totalDespachados; //no se en donde poner esto);
        StopAllCoroutines();
        calcularMostrarResultados();
        Utilidades.guardarMetricas(listaMetricas);
    }

    // Corutina que genera productos aleatorios 
    private IEnumerator GenerarProductos()
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
                totalGenerados++;

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

            tiempoSiguienteDespacho = Time.time + 1f;
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
        TextoProductos.text = mostrar;
    }


    public void calcularMostrarResultados()
    {

        float promedioTiempo = totalDespachados > 0 ? tiempoTotalDespachados / totalDespachados : 0f;
        float productosNoDespachados = totalGenerados - totalDespachados;

        string tipoMasDespachado = "";
        int maxDespachados = 0;

        foreach (var kvp in despachoporTipos)
        {
            if (kvp.Value > maxDespachados)
            {
                maxDespachados = kvp.Value;
                tipoMasDespachado = kvp.Key;
            }
        }

       

        string resultado = "RESULTADOS ";
        resultado += "Total generados = " + totalGenerados + "";
        resultado += "Total despachados =" + totalDespachados + "";
        resultado += "Tamaño de la pila =" + pilaProductos.Count + "";
        resultado += "Tiempo promedio despacho =" + promedioTiempo + "";

        resultado += "DESPACHADOS POR TIPO";
        resultado += "Despachados por tipo = Basico:" + despachoporTipos + "Fragil:" + despachoporTipos + "Pesado:" + despachoporTipos + "";
        resultado += "Tipo mas despachado =" + tipoMasDespachado + "";
        resultado += "No Despachados = + " + productosNoDespachados + "";

        resultado += "TIEMPOS";
        resultado += "Tiempo total generacion ="+ totalGenerados + ""; 
        resultado += "Tiempo total despacho =" + tiempoTotalDespachados + " segundos";
        resultado += "Tiempo total de generacion de productos =" + TiempoTranscurrido +  "segundos";
        var lineas = resultado.Replace("\r\n", "\n").Split('\n');
        Debug.Log(resultado);
        listaMetricas.Add(resultado);
        listaMetricas.Clear();
        listaMetricas.AddRange(lineas);


    }


}
