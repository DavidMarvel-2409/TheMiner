using System.Text;
using UnityEngine;

public class BenchmarkUmbralPerlin : MonoBehaviour
{
    [SerializeField] private int ancho = 100;
    [SerializeField] private int alto = 100;
    [SerializeField] private int valoresRuido = 4;
    [SerializeField] private int refinaciones = 10;
    [SerializeField] private float escalaPerlin = 0.1f;

    [SerializeField] private float[] umbralesAProbar = { 0.2f, 0.25f, 0.3f, 0.4f, 0.5f };

    [SerializeField] private int repeticionesPorUmbral = 5;

    [ContextMenu("Correr Benchmark")]
    private void CorrerBenchmark()
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("umbralPerlin,porcentajeVacioPromedio");

        foreach (float umbral in umbralesAProbar)
        {
            float sumaPorcentajes = 0f;

            for (int r = 0; r < repeticionesPorUmbral; r++)
            {
                MapGenerator generador = new MapGenerator(alto, ancho);
                generador.generarRuido(valoresRuido);
                for (int i = 0; i < refinaciones; i++) generador.Refinar();

                GeneradorCuevas generadorCuevas = new GeneradorCuevas(ancho, alto, escalaPerlin, umbral);
                int[,] mapaCueva = generadorCuevas.Generar();
                generador.combinarCuevas(mapaCueva, -2);
                generador.ForzarBordes(-1);

                int[,] mapaFinal = generador.getMap();
                int vacias = 0;
                int totalInterior = 0;

                for (int x = 1; x < ancho - 1; x++)
                {
                    for (int y = 1; y < alto - 1; y++)
                    {
                        totalInterior++;
                        if (mapaFinal[x, y] == -2) vacias++;
                    }
                }

                sumaPorcentajes += (vacias * 100f) / totalInterior;
            }

            float promedio = sumaPorcentajes / repeticionesPorUmbral;
            string umbralStr = umbral.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string promedioStr = promedio.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            csv.AppendLine($"{umbralStr},{promedioStr}");
            Debug.Log($"umbralPerlin = {umbralStr} -> {promedioStr}% de celdas vacias " +
                      $"(promedio de {repeticionesPorUmbral} corridas)");
        }

        Debug.Log("=== Tabla completa (formato CSV, lista para copiar) ===\n" + csv.ToString());
    }
}