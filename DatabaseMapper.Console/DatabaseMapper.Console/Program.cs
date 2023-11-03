// See https://aka.ms/new-console-template for more information
using CommandLine;
using DatabaseMapper.Console;
using DatabaseMapper.Core.Graph;
using DatabaseMapper.Core.Mapper;
using System.Diagnostics;

Parser.Default.ParseArguments<ExecutionParameters>(args)
    .WithParsed<ExecutionParameters>(p =>
    {

        ConsoleWriteIfInVerboseMode(p.Verbose, "Modo verboso habilitado");

        var graph = new TableGraph();
        var mapper = new DbMapperService();
        var graphExporter = new GraphExporter();

        var query = @"SELECT E.FirstName, E.LastName, D.Name AS DepartmentName, EH.StartDate, EH.EndDate, J.JobTitle
                            FROM HumanResources.Employee AS E
                            JOIN HumanResources.EmployeeDepartmentHistory AS EDH ON E.EmployeeID = EDH.EmployeeID
                            JOIN HumanResources.Department AS D ON EDH.DepartmentID = D.DepartmentID
                            JOIN HumanResources.EmployeeJobHistory AS EH ON E.EmployeeID = EH.EmployeeID
                            LEFT JOIN HumanResources.JobTitle AS J ON EH.JobTitleID = J.JobTitleID;";

        foreach(var sourceFile in p.SourceFilesPathName)
        {
            try
            {
                // Open the text file using a stream reader.
                ConsoleWriteIfInVerboseMode(p.Verbose, $"Lendo {sourceFile}...");
                using (var sr = new StreamReader(sourceFile))
                {
                    // Read the stream as a string, and write the string to the console.
                    mapper.IncrementModel(graph, sr.ReadToEnd());
                }
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


static void ConsoleWriteIfInVerboseMode(bool write, string message) => Console.WriteLine(message);