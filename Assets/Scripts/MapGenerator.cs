using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MapGenerator
{
    private int[,] mapa;
    private int valores, ancho, alto;
    private System.Random rand = new System.Random();
    public MapGenerator(int alto, int ancho)
    {
        this.ancho = ancho;
        this.alto = alto;
        mapa = new int[ancho, alto];
    }

    public int[,] getMap()
    {
        return mapa;
    }

    public void generarRuido(int countE)
    {
        valores = countE;
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                mapa[x, y] = rand.Next(0, valores);
            }
        }
    }
    
    private float porcent(float el100, float current)
    {
        return (current * 100) / el100;
    }

    private int getMayor(List<int> ops, int totalVecinos)
    {
        int index = 0;
        int current = -1;
        for (int i = 0; i < ops.Count; i++)
        {
            if (ops[i] > current)
            {
                current = ops[i];
                index = i;
            }
        }
        //float por = porcent(totalVecinos, ops[index]);
        //if (por > 50 && por < 60) return index;
        if (ops[index] * 2 > totalVecinos) return index;
        return -1;
    }
    private bool posExiste(int x, int y)
    {
        if (x < 0 || x > ancho - 1 || y < 0 || y > alto - 1) return false;
        return true;
    }
    public void Refinar()
    {
        (int dx, int dy)[] vecinos = { (-1, 0), (1, 0), (0, 1), (0, -1) };

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                List<int> cant = new List<int>(new int[valores]);
                int vecinosDis = 0;

                foreach (var v in vecinos)
                {
                    int nx = x + v.dx;
                    int ny = y + v.dy;
                    if (posExiste(nx, ny))
                    {
                        vecinosDis++;
                        int valorVecino = mapa[nx, ny];
                        cant[valorVecino]++;
                    }
                }

                int indexMayor = getMayor(cant, vecinosDis);
                if (indexMayor != -1) mapa[x, y] = indexMayor;
            }
        }
    }
    public void ForzarBordes(int valorBorde)
    {
        for (int x = 0; x < ancho; x++)
        {
            mapa[x, 0] = valorBorde;
            //mapa[x, alto - 1] = valorBorde;
        }
        for (int y = 0; y < alto; y++)
        {
            mapa[0, y] = valorBorde;
            mapa[ancho - 1, y] = valorBorde;
        }
    }

    public void combinarCuevas(int[,] mapaCueva, int valorVacio)
    {
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                if (mapaCueva[x, y] == 0)
                {
                    mapa[x, y] = valorVacio;
                }
            }
        }
    }

    public int numeroDominante()
    {
        int[] count = new int[valores];
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                count[mapa[x, y]]++;
            }
        }

        int dominante = -1, index = 0;
        for (int k = 0; k < count.Length; k++)
        {
            if (count[k] > dominante)
            {
                dominante = count[k];
                index = k;
            }
        }
        return index;
    }

    public void ConvertirABinario()
    {
        int val0 = numeroDominante();
        Debug.Log($"dominante: {val0}");
        int[,] nuevoMapa = new int[ancho, alto];
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                nuevoMapa[x, y] = (mapa[x, y] == val0) ? 0 : 1;
            }
        }
        mapa = nuevoMapa;
    }
}
