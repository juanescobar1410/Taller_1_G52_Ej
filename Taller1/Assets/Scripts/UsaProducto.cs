using UnityEngine;
using PackageProductos;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

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
                string[] lineas = File.ReadAllLines(filePath); // 👈 leer por líneas
                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue; // evitar líneas vacías

                    string[] datos = linea.Split('|');
                    if (datos.Length >= 6)
                    {
                        if (double.TryParse(datos[3], out double valor1) &&
                            double.TryParse(datos[4], out double valor2) &&
                            int.TryParse(datos[5], out int cantidad))
                        {
                            var p = new Producto(
                                datos[0],
                                datos[1],
                                datos[2],
                                valor1,
                                valor2,
                                cantidad
                            );
                            Debug.Log("Producto leído: " + p.Id);
                            listaP.Add(p);
                        }
                        else
                        {
                            Debug.LogError($"Error al convertir números en la línea: {linea}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Datos insuficientes en la línea: {linea}");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al leer el texto: " + e.Message);
            }
        }
        // Agregar la llave de cierre faltante para la clase UsaProducto
    }

}   