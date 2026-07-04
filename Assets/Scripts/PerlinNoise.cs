using UnityEngine;

public class PerlinNoise
{
    private int[] permutacion;
    private System.Random rand;

    public PerlinNoise(int semilla)
    {
        rand = new System.Random(semilla);
        int[] p = new int[256];
        for (int i = 0; i < 256; i++) p[i] = i;

        for (int i = 255; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = p[i];
            p[i] = p[j];
            p[j] = temp;
        }

        permutacion = new int[512];
        for (int i = 0; i < 512; i++)
        {
            permutacion[i] = p[i % 256];
        }

    }
    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Lerp(float t, float a, float b)
    {
        return a + t * (b - a);
    }
    private static float Grad(int hash, float x, float y)
    {
        int h = hash & 3;
        float u = (h < 2) ? x : y;
        float v = (h < 2) ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
    public float Noise(float x, float y)
    {
        int xi = (int)Mathf.Floor(x) & 255;
        int yi = (int)Mathf.Floor(y) & 255;

        float xf = x - (float)Mathf.Floor(x);
        float yf = y - (float)Mathf.Floor(y);

        float u = Fade(xf);
        float v = Fade(yf);

        int aa = permutacion[permutacion[xi] + yi];
        int ab = permutacion[permutacion[xi] + yi + 1];
        int ba = permutacion[permutacion[xi + 1] + yi];
        int bb = permutacion[permutacion[xi + 1] + yi + 1];

        float x1 = Lerp(u, Grad(aa, xf, yf), Grad(ba, xf - 1, yf));
        float x2 = Lerp(u, Grad(ab, xf, yf - 1), Grad(bb, xf - 1, yf - 1));

        return Lerp(v, x1, x2);
    }
    public float Noise01(float x, float y)
    {
        return (Noise(x, y) + 1f) / 2f;
    }
}
