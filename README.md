# DatabaseMapper

**DatabaseMapper** é uma ferramenta que mapeia a estrutura de entidades de um banco de dados em um grafo usando queries SQL. Isso permite que você visualize a estrutura do banco de dados de uma forma gráfica e eventualmente rode algoritmos úteis sobre grafos na estrutura do banco de dados.

## Uso

Para usar o DatabaseMapper, você pode executar a aplicação console através do terminal com os seguintes argumentos:

``` shell
DatabaseMapper.Console.exe -s <arquivos-queries-sql> -f <nome-arquivos-saída> [-o <diretório-saída>] [-i <arquivo-base-grafo>] [-v]
```

### Argumentos:

- `-i, --input-graph` (Opcional): Especifica o caminho para um arquivo com um grafo inicial. Este arquivo já contém a representação de um banco através de um grafo o que permite que o mapeamento seja incremental.

- `-o, --output-dir` (Opcional): Especifica o diretório onde os arquivos de saída serão gerados após o processamento. Os arquivos de saída incluirão um arquivo no formato `.graph` e um arquivo no formato `.dot` para a visualização do grafo gerado.

- `-f, --output-filename` (Obrigatório): Define o nome dos arquivos de exportação gerados após o processamento. Ex.: `--output-filename=batata` indica que os arquivos `batata.graph` e `batata.dot` serão gerados em `--output-dir` ao final do processamento.

- `-s, --source-files` (Obrigatório): Especifica os arquivos contendo queries SQL a serem processadas. Você pode fornecer uma lista de arquivos separados por vírgulas.

- `-v, --verbose` (Opcional): Habilita o modo verboso, imprimindo todas as mensagens na saída padrão. Use este argumento para obter informações detalhadas durante o processo.

## Exemplos de Uso

Aqui estão alguns exemplos de como usar o DatabaseMapper:


### Executar o DatabaseMapper com um arquivo de grafo de entrada, especificando um diretório de saída e arquivos de queries SQL

Para executar o DatabaseMapper com um arquivo de grafo de entrada, você deve fornecer o caminho para o arquivo de entrada (grafo), especificar o diretório onde os arquivos de saída serão gerados e fornecer os arquivos de queries SQL a serem processados. Você também pode habilitar o modo verboso para obter informações detalhadas durante o processo. Aqui está um exemplo:

```shell
DatabaseMapper.Console.exe -i graph_input.graph -o output_directory -f output_filename -s query1.sql,query2.sql
```


### Executar o DatabaseMapper com arquivos de queries SQL e habilitar o modo verboso

```shell
DatabaseMapper.Console.exe -s query1.sql,query2.sql -v
```
