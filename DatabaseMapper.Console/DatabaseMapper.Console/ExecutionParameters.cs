using CommandLine.Text;
using CommandLine;

namespace DatabaseMapper.Console
{
    internal class ExecutionParameters
    {
        [Option('i', "input-graph", Required = false, HelpText = "Arquivo com o grafo inicial.")]
        public string BaseGraphFilePath { get; set; }

        [Option('o', "output-dir", Required = true, HelpText = "Diretório onde serão gerados os arquivos de saída após o processamento (.graph e .dot).")]
        public string OutputPathDir { get; set; }
        
        [Option('f', "output-filename", Required = true, HelpText = "Nome dos arquivos de exportação.")]
        public string OutputFileName { get; set; }

        [Option('s', "source-files", Required = true, HelpText = "Arquivos contendo queries SQL para serem processadas.", Separator = ',')]
        public IEnumerable<string> SourceFilesPathName { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Habilita o modo verboso, imprimindo todas as mensagens na saída padrão.")]
        public bool Verbose { get; set; }


        public bool HasBaseGraph => !String.IsNullOrWhiteSpace(BaseGraphFilePath);
    }
}
