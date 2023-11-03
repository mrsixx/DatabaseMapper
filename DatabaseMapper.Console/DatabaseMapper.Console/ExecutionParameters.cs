using CommandLine.Text;
using CommandLine;

namespace DatabaseMapper.Console
{
    internal class ExecutionParameters
    {
        [Option('i', "input", Required = false, HelpText = "Caminho para o grafo de origem.")]
        public string BaseGraphPathName { get; set; }

        [Option('o', "output", Required = true, HelpText = "Caminho onde o será gerado o arquivo de saída com o grafo obtido.")]
        public string OutputPathName { get; set; }

        [Option('s', "source", Required = true, HelpText = "Arquivos que serão processados.")]
        public IEnumerable<string> SourceFilesPathName { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Imprime todas as mensagens na saída padrão.")]
        public bool Verbose { get; set; }

    }
}
