using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPainter : MonoBehaviour
{
    [SerializeField] private Camera _camara;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tilePared, tileVacio;
    [SerializeField] private List<TileBase> tiles = new List<TileBase>();
    [SerializeField] private int ancho, alto;

    [SerializeField] private int valoresRuido, refinaciones;
    [SerializeField] private int valorBorde = 1;

    [Header("Ruido Perlin (forma de las cuevas)")]
    [SerializeField] private float escalaPerlin = 0.1f;
    [SerializeField] private float umbralPerlin = 0.5f;

    private void Start()
    {
        if (tiles.Count < valoresRuido)
        {
            Debug.LogError($"La lista 'tiles' tiene {tiles.Count} elementos, pero " + $"valoresRuido = {valoresRuido}. Deben coincidir para que " + $"cada valor del mapa tenga un tile asignado.");
            return;
        }
        _camara.orthographicSize = masAlto(alto, ancho) / 2f;
        //_camara.transform.position = new Vector3(ancho / 2f, alto / 2f, _camara.transform.position.z);

        MapGenerator generador = new MapGenerator(alto, ancho);
        generador.generarRuido(valoresRuido);

        for (int i = 0; i < refinaciones; i++)
        {
            generador.Refinar();
        }

        GeneradorCuevas generadorCuevas = new GeneradorCuevas(ancho, alto, escalaPerlin, umbralPerlin);
        int[,] mapaCueva = generadorCuevas.Generar();

        generador.combinarCuevas(mapaCueva, -2);

        //generador.ConvertirABinario();
        generador.ForzarBordes(-1);

        PintarMapa(generador.getMap());
    }
    void PintarMapa(int[,] mapa)
    {
        tilemap.ClearAllTiles();
        for (int x = 0; x < mapa.GetLength(0); x++)
        {
            for (int y = 0; y < mapa.GetLength(1); y++)
            {
                Vector3Int pos = new Vector3Int(x - (mapa.GetLength(0) / 2), y - (mapa.GetLength(1) / 2), 0);
                int valor = mapa[x, y];
                if (valor >= 0) tilemap.SetTile(pos, tiles[valor]);
                else
                {
                    switch (valor)
                    {
                        case -1: tilemap.SetTile(pos, tilePared); break;
                        case -2: tilemap.SetTile(pos, tileVacio); break;
                    }
                }
                    
            }
        }
    }
    private float masAlto(int a, int b)
    {
        return a > b ? a : b;
    }
}
