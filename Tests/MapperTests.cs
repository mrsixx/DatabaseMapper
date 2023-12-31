using DatabaseMapper.Core.Graph;
using DatabaseMapper.Core.Mapper;
using Xunit;

namespace Tests
{
    public class MapperTests
    {
        [Fact]
        public void QueryWithMultipleJoins()
        {
            var query = @"SELECT m.maquina AS maquina,
                                   o.nome AS operador,
                                   t.inicio AS inicio,
                                   t.os AS os,
                                   procs.referencia AS processo
                            FROM tapont t
                            JOIN eventos e ON (e.codseq = t.codevento)
                            JOIN operador o ON (o.codseq = t.codoperador)
                            JOIN maquinas m ON (m.codseq = t.codmaquina)
                            JOIN procs ON (procs.codseq = t.codproc)
                            WHERE (1=1)
                              AND e.prod = 0;";

            var graph = new TableGraph();
            var mapper = new DbMapperService();

            Assert.Equal(0, graph.EdgeCount);
            Assert.Equal(0, graph.VertexCount);

            mapper.IncrementModel(graph, query);

            Assert.Equal(4, graph.EdgeCount);
            Assert.Equal(5, graph.VertexCount);

            Assert.Contains(graph.Vertices, v => v.Table == "TAPONT");
            Assert.Contains(graph.Vertices, v => v.Table == "EVENTOS");
            Assert.Contains(graph.Vertices, v => v.Table == "OPERADOR");
            Assert.Contains(graph.Vertices, v => v.Table == "MAQUINAS");
            Assert.Contains(graph.Vertices, v => v.Table == "PROCS");

            Assert.Contains(graph.Edges, e => e.SourceLabel == "EVENTOS.CODSEQ" && e.TargetLabel == "TAPONT.CODEVENTO");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "OPERADOR.CODSEQ" && e.TargetLabel == "TAPONT.CODOPERADOR");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "MAQUINAS.CODSEQ" && e.TargetLabel == "TAPONT.CODMAQUINA");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "PROCS.CODSEQ" && e.TargetLabel == "TAPONT.CODPROC");
        }
        [Fact]
        public void QueryAdventureWorks()
        {
            var query = @"SELECT E.FirstName, E.LastName, D.Name AS DepartmentName, EH.StartDate, EH.EndDate, J.JobTitle
                            FROM HumanResources.Employee AS E
                            JOIN HumanResources.EmployeeDepartmentHistory AS EDH ON E.EmployeeID = EDH.EmployeeID
                            JOIN HumanResources.Department AS D ON EDH.DepartmentID = D.DepartmentID
                            JOIN HumanResources.EmployeeJobHistory AS EH ON E.EmployeeID = EH.EmployeeID
                            LEFT JOIN HumanResources.JobTitle AS J ON EH.JobTitleID = J.JobTitleID;";

            var graph = new TableGraph();
            var mapper = new DbMapperService();

            Assert.Equal(0, graph.EdgeCount);
            Assert.Equal(0, graph.VertexCount);

            mapper.IncrementModel(graph, query);

            Assert.Equal(4, graph.EdgeCount);
            Assert.Equal(5, graph.VertexCount);

            Assert.Contains(graph.Vertices, v => v.Table == "HUMANRESOURCES.EMPLOYEE");
            Assert.Contains(graph.Vertices, v => v.Table == "HUMANRESOURCES.EMPLOYEEDEPARTMENTHISTORY");
            Assert.Contains(graph.Vertices, v => v.Table == "HUMANRESOURCES.DEPARTMENT");
            Assert.Contains(graph.Vertices, v => v.Table == "HUMANRESOURCES.EMPLOYEEJOBHISTORY");
            Assert.Contains(graph.Vertices, v => v.Table == "HUMANRESOURCES.JOBTITLE");

            Assert.Contains(graph.Edges, e => e.SourceLabel == "EMPLOYEE.EMPLOYEEID" && e.TargetLabel == "EMPLOYEEDEPARTMENTHISTORY.EMPLOYEEID");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "EMPLOYEEDEPARTMENTHISTORY.DEPARTMENTID" && e.TargetLabel == "DEPARTMENT.DEPARTMENTID");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "EMPLOYEE.EMPLOYEEID" && e.TargetLabel == "EMPLOYEEJOBHISTORY.EMPLOYEEID");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "EMPLOYEEJOBHISTORY.JOBTITLEID" && e.TargetLabel == "JOBTITLE.JOBTITLEID");

            var graphExporter = new GraphExporter();
            graphExporter.ExportTableGraphToGraphviz(graph, @"C:\Users\mathe\Documents\", "graph.txt");
        }
    }
}
