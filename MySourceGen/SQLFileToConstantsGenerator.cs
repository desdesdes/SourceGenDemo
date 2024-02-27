using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace MySourceGen
{
    [Generator(LanguageNames.CSharp)]
    public class SQLFileToConstantsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var sqlFiles = context.AdditionalTextsProvider.Where(f => f.Path.EndsWith(".sql"))
                .Collect();

            context.RegisterSourceOutput(sqlFiles, AddSource);
        }

        private void AddSource(SourceProductionContext spc, ImmutableArray<AdditionalText> sqlFiles)
        {
            var sb = new StringBuilder();
            GeneratorsUtils.WriteGeneratedFileHeader(sb);

            GeneratorsUtils.WriteGeneratedAttribute(sb);
            sb.AppendLine("internal static class SQL");
            sb.AppendLine("{");
            foreach (var file in sqlFiles)
            {
                var content = file.GetText().ToString();

                sb.Append("public const string ");
                sb.Append(Path.GetFileNameWithoutExtension(file.Path));
                sb.Append(" = @\"");
                sb.Append(content.Replace("\"", "\"\""));
                sb.AppendLine("\";");
            }
            sb.AppendLine("}");
            var sourceText = SourceText.From(sb.ToString(), Encoding.UTF8);

            spc.AddSource("SQL.generated.cs", sourceText);
        }
    }
}
