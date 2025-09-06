using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class Utilidades : MonoBehaviour
{
    

    public static bool guardarMetricas(List<string> listaMetricas)
    {
        try
        {
            string json = JsonUtility.ToJson(new metricWrapper { Metricas = listaMetricas },true);
            string folderP = Application.streamingAssetsPath;

            if (Directory.Exists(folderP))
            {
                Directory.CreateDirectory(folderP);
            }

            string fileP = Path.Combine(folderP, "datosMetricasProductos.json");
            File.WriteAllText(fileP, json);
   
            return true;
        }
        catch (SystemException ex)
        {
            Debug.LogError("Ayuda error" + ex.Message);
            return false;
        }
    }
}

public class metricWrapper
{
    public List<string> Metricas;
}
