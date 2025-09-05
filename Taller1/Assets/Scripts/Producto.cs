using System;
using UnityEngine;

namespace PackageProductos
{
    [Serializable]
    public class Producto
    {
        [SerializeField]
        private string id;
        [SerializeField]
        private string nombre;
        [SerializeField]
        private string tipo;
        [SerializeField]
        private double peso;
        [SerializeField]
        private double precio;
        [SerializeField]
        private int tiempo;

        public string Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public double Peso { get => peso; set => peso = value; }
        public double Precio { get => precio; set => precio = value; }
        public int Tiempo { get => tiempo; set => tiempo = value; }

        public Producto()
        {
        }

        public Producto(string id, string nombre, string tipo, double peso, double precio, int tiempo)
        {
            this.id = id;
            this.nombre = nombre;
            this.tipo = tipo;
            this.peso = peso;
            this.precio = precio;
            this.tiempo = tiempo;
        }

        public override string ToString()
        {
            return $"ID: {id}, Nombre: {nombre}, Tipo: {tipo}, Peso: {peso}, Precio: {precio}, Tiempo: {tiempo}";
        }


    }

}