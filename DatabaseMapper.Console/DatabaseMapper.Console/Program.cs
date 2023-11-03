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

        var graph = new TableGraph();
        var mapper = new DbMapperService();
        var graphExporter = new GraphExporter();
  
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
        

        ConsoleWriteIfInVerboseMode(p.Verbose, $"Salvando grafo resultante em: {p.OutputPathName}...");
        graphExporter.ExportTableGraphToGraphviz(graph, p.OutputPathName);
        Console.WriteLine("Mapeamento concluído!");
    });


static void ConsoleWriteIfInVerboseMode(bool write, string message) {
    if (write)
        Console.WriteLine(message);
}