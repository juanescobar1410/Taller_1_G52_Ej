
using PackageProductos;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class UsaProducto : MonoBehaviour
{
    public List<Producto> listaP = new List<Producto>();   
    public Stack<Producto> pilaProductos = new Stack<Producto>();
    List<string> listaMetricas = new List<string>();
    public TMP_Text TextoProductos;
    public TMP_Text TextoTamaño;
    public TMP_Text TextoDespachados;
    public TMP_Text TextoGenerados;
    public TMP_Text TextoTope;
    public TMP_Text TextoContador;

    public GameObject PanelResultados;
    public TMP_Text TextoResultados;

    private bool ContadorActivo;
    private float TiempoTranscurrido;
    private bool generando = false;
    private bool despachando = false;
    private int totalDespachados = 0;
    private int totalGenerados = 0;
    private int totalNoDespachados = 0;
    private float tiempoTotalDespachados = 0f;
    private float TiempoTotalGeneracion = 0f;
    private Dictionary<string, int> despachoporTipos = new Dictionary<string, int>();

    private float tiempoInicioGeneracion = 0f;
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

        if (ContadorActivo)
        {
            TiempoTranscurrido += Time.deltaTime;
            int minutos = Mathf.FloorToInt(TiempoTranscurrido / 60f);
            int segundos = Mathf.FloorToInt(TiempoTranscurrido % 60f);
            TextoContador.text = " Minutos " + minutos + " : " + segundos + " Segundos ";
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

    
    public void IniciarGeneracion()
    {
  
        if (PanelResultados != null)
        {
            PanelResultados.SetActive(false);
            if (TextoResultados != null)
                TextoResultados.text = "";
        }

        ContadorActivo = true;
        TiempoTranscurrido = 0f;
        tiempoInicioGeneracion = Time.time;
        if (!generando)
        {
            generando = true;
            despachando = true;

            totalNoDespachados = 0;
            totalGenerados = 0;
            totalDespachados = 0;
            TiempoTotalGeneracion = 0f;
            tiempoTotalDespachados = 0f;
            pilaProductos.Clear();

            tiempoInicioGeneracion = Time.time;
            tiempoSiguienteDespacho = Time.time + 1f;
            StartCoroutine(GenerarProductos());
        }
    }

    public void CerrarPanelResultados()
    {
        if (PanelResultados != null)
        {
            PanelResultados.SetActive(false);
        }
    }

    public void DetenerGeneracion()
    {
        ContadorActivo = false;
        generando = false;
        despachando = false;
        totalNoDespachados = totalGenerados - totalDespachados; 
        StopAllCoroutines();

        calcularMostrarResultados();
        
        Utilidades.guardarMetricas(listaMetricas);

    }

   
    private IEnumerator GenerarProductos()
    {
        while (generando)
        {
            int cantidad = Random.Range(1, 4); 

            for (int i = 0; i < cantidad; i++)
            {
               
                Producto elegido = listaP[Random.Range(0, listaP.Count)];

                
                Producto copia = new Producto(
                    elegido.Id,
                    elegido.Nombre,
                    elegido.Tipo,
                    elegido.Peso,
                    elegido.Precio,
                    elegido.Tiempo
                );

                TiempoTotalGeneracion += copia.Tiempo;

                pilaProductos.Push(copia);
                totalGenerados++;

            }

            ActualizarTextoPila();

            yield return new WaitForSeconds(2f); 
        }
    }



    public void DespacharProducto()
    {

        if (pilaProductos.Count > 0)
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
        TextoProductos.text = mostrar;
       
        TextoDespachados.text = totalDespachados.ToString();
        TextoGenerados.text = totalGenerados.ToString();
        TextoTamaño.text = pilaProductos.Count.ToString();
        if (pilaProductos.Count > 0)
            TextoTope.text = pilaProductos.Peek().ToString();
        else
            TextoTope.text = "Pila vacía";
    }




    public void calcularMostrarResultados()
    {

        float promedioTiempo = totalDespachados > 0 ? tiempoTotalDespachados / totalDespachados : 0f;

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


        string resultado = "RESULTADOS\n ";
        resultado += "Total generados : " + totalGenerados + "\n";
        resultado += "Total despachados :" + totalDespachados + "\n";
        resultado += "Tamaño de la pila : " + pilaProductos.Count + "\n";
        resultado += "Tiempo promedio despacho : " + promedioTiempo + "\n";

        resultado += "DESPACHADOS POR TIPO\n ";
        resultado += "Despachados por tipo : Basico: " + despachoporTipos["Basico"]
              + ", Fragil: " + despachoporTipos["Fragil"]
              + ", Pesado: " + despachoporTipos["Pesado"] + "\n";
        resultado += "Tipo mas despachado : " + tipoMasDespachado + "\n";
        resultado += "No Despachados : " + totalNoDespachados + "\n";

        resultado += "TIEMPOS\n";
        resultado += "Tiempo total generacion : " + totalGenerados + "\n";
        resultado += "Tiempo total despacho : " + tiempoTotalDespachados + " segundos \n";
        resultado += "Tiempo total de generacion de productos : " + TiempoTranscurrido + " segundos \n";
        var lineas = resultado.Split('\n');
        Debug.Log(resultado);
        listaMetricas.Add(resultado);
        listaMetricas.Clear();
        listaMetricas.AddRange(lineas);

        if (TextoResultados != null)
            TextoResultados.text = resultado;

        if (PanelResultados != null)
            PanelResultados.SetActive(true);



    }


}

