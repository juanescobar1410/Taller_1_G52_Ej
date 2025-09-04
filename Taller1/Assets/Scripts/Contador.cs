using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;


public class Contador : MonoBehaviour
{
    private bool activo;
    private float tiempoTranscurrido;
    public TMP_Text tiempoTexto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiempoTranscurrido = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (activo)
        {
            tiempoTranscurrido += Time.deltaTime;
        }
        
        TimeSpan tiempo = TimeSpan.FromSeconds(tiempoTranscurrido);

        tiempoTexto.text = tiempo.ToString(@"mm\:ss");
    }
    
    public void IniciarContador()
    {
        activo = true;
    }
    public void DetenerContador()
    {
        activo = false;
    }
}
