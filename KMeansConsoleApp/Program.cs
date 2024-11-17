using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Accord.MachineLearning;

class Program
{
    static void Main(string[] args)
    {
        // Caminho absoluto para o arquivo dentro da pasta "data" (ajustar conforme necessário)
        string filePath = @"C:\Users\Cliente\Desktop\kmeans\KMeansConsoleApp\KMeansConsoleApp\KMeansConsoleApp\data\iris_formatted.csv";

        // Verificar se o arquivo existe para evitar erros
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Arquivo CSV não encontrado. Verifique o caminho.");
            return;
        }

        // Ler os dados do CSV
        var data = new List<double[]>();
        using (var reader = new StreamReader(filePath))
        {
            string header = reader.ReadLine(); // Ignorar o cabeçalho
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                // Ignorar linhas vazias
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Dividir a linha por vírgulas e converter para números
                var values = line.Split(',');
                if (values.Length >= 4) 
                {
                    try
                    {
                        var numericValues = values.Take(4).Select(double.Parse).ToArray();
                        data.Add(numericValues);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Erro ao converter a linha para números. Linha ignorada: " + line);
                    }
                }
            }
        }

        // Converter para matriz
        double[][] dataArray = data.ToArray();

        // Inicializar KMeans com 3 clusters (especificamente para a base Iris)
        int k = 3;
        KMeans kmeans = new KMeans(k);

        // Treinar o modelo
        KMeansClusterCollection clusters = kmeans.Learn(dataArray);

        // Predizer os clusters para os dados
        int[] labels = clusters.Decide(dataArray);

        // Exibir os centróides
        Console.WriteLine("Centróides dos Clusters:");
        foreach (var centroid in clusters.Centroids)
        {
            Console.WriteLine(string.Join(", ", centroid.Select(c => c.ToString("F2"))));
        }

        // Exibir as classificações
        Console.WriteLine("\nClassificações:");
        for (int i = 0; i < labels.Length; i++)
        {
            Console.WriteLine($"Ponto {i + 1}: Cluster {labels[i]}");
        }

        // Exportar os resultados para um arquivo CSV
        using (var writer = new StreamWriter("clusters_result.csv"))
        {
            writer.WriteLine("X,Y,Cluster"); // Cabeçalho
            for (int i = 0; i < dataArray.Length; i++)
            {
                var point = dataArray[i];
                int cluster = labels[i];
                writer.WriteLine($"{point[0]},{point[1]},{cluster}");
            }
        }
        Console.WriteLine("Resultados exportados para clusters_result.csv");
    }
}
