// See https://aka.ms/new-console-template for more information
using DatabaseMapper.Core.Graph;
using DatabaseMapper.Core.Mapper;

var graph = new TableGraph();
var mapper = new DbMapperService();
var query = @"SELECT E.FirstName, E.LastName, D.Name AS DepartmentName, EH.StartDate, EH.EndDate, J.JobTitle
                            FROM HumanResources.Employee AS E
                            JOIN HumanResources.EmployeeDepartmentHistory AS EDH ON E.EmployeeID = EDH.EmployeeID
                            JOIN HumanResources.Department AS D ON EDH.DepartmentID = D.DepartmentID
                            JOIN HumanResources.EmployeeJobHistory AS EH ON E.EmployeeID = EH.EmployeeID
                            LEFT JOIN HumanResources.JobTitle AS J ON EH.JobTitleID = J.JobTitleID;";

mapper.IncrementModel(graph, query);
var graphExporter = new GraphExporter();
graphExporter.ExportTableGraphToGraphviz(graph, @"C:\Users\mathe\Documents\adventure_works_graph.txt");