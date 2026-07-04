using UnityEngine;

public class GeneradorCuevas
{
    private int ancho, alto;
    private float escala, umbral, offsetX, offsetY;
    private System.Random rand = new System.Random();
    private PerlinNoise ruido;

    public GeneradorCuevas(int ancho, int alto, float escala, float umbral)
    {
        this.ancho = ancho;
        this.alto = alto;
        this.escala = escala;
        this.umbral = umbral;
        offsetX = rand.Next(0, 100000);
        offsetY = rand.Next(0, 100000);

        ruido = new PerlinNoise(rand.Next());
    }
    public int[,] Generar()
    {
        int[,] cuevas = new int[ancho, alto];
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                float nx = (x + offsetX) * escala;
                float ny = (y + offsetY) * escala;
                float valor = ruido.Noise01(nx, ny);
                cuevas[x, y] = (valor < umbral) ? 0 : 1;

            }
        }
        return cuevas;
    }


}
