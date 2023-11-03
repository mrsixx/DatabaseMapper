// See https://aka.ms/new-console-template for more information
using CommandLine;
using DatabaseMapper.Console;
using DatabaseMapper.Core.Graph;
using DatabaseMapper.Core.Mapper;
using System.Diagnostics;

Parser.Default.ParseArguments<ExecutionParameters>(args)
    .WithParsed(p =>
    {

        ConsoleWriteIfInVerboseMode(p.Verbose, "Modo verboso habilitado");

        var mapper = new DbMapperService();
        var graphExporter = new GraphExporter();
        int initialVertexCount = 0, initialEdgesCount = 0;
        TableGraph graph = null;
        if (p.HasBaseGraph)
        {
            ConsoleWriteIfInVerboseMode(p.Verbose, $"Lendo o grafo em {p.BaseGraphFilePath}...");
            graph = graphExporter.ImportTableGraphFromFile(p.BaseGraphFilePath);
            initialVertexCount = graph.VertexCount;
            initialEdgesCount = graph.EdgeCount;
            ConsoleWriteIfInVerboseMode(p.Verbose, $"Leitura completa: {initialVertexCount} vértices e {initialEdgesCount} arestas encontradas.");
        }
        else
            graph = new TableGraph();
  
        foreach(var sourceFile in p.SourceFilesPathName)
        {
            try
            {
                // Open the text file using a stream reader.
                ConsoleWriteIfInVerboseMode(p.Verbose, $"Lendo {sourceFile}...");
                using var sr = new StreamReader(sourceFile);
                // Read the stream as a string, and write the string to the console.
                mapper.IncrementModel(graph, sr.ReadToEnd());
            }
            catch (IOException e)
            {
                Console.WriteLine($"O arquivo {sourceFile} não pode ser lido:");
                Console.WriteLine(e.Message);
            }
        }
        

        ConsoleWriteIfInVerboseMode(p.Verbose, $"Salvando arquivos do grafo em: {p.OutputPathDir}...");
        graphExporter.ExportTableGraphToGraphviz(graph, p.OutputPathDir, p.OutputFileName);
        graphExporter.ExportTableGraphToFile(graph, p.OutputPathDir, p.OutputFileName);
        Console.WriteLine($"Mapeamento concluído: {graph.VertexCount - initialVertexCount} vértices e {graph.EdgeCount - initialEdgesCount} arestas foram adicionadas ao grafo!");
    });


static void ConsoleWriteIfInVerboseMode(bool write, string message) {
    if (write)
        Console.WriteLine(message);
}