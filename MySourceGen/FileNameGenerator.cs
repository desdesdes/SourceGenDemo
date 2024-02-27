using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySourceGen
{
    [Generator(LanguageNames.CSharp)]
    internal class FileNameGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classesToImplement = context.SyntaxProvider.
                CreateSyntaxProvider(IsCandidate, GetCandidate).Where(i => i != null);

            context.RegisterSourceOutput(classesToImplement, AddSource);
        }

        private void AddSource(SourceProductionContext spc, ClassDeclarationSyntax cds)
        {
            var ns = GetNamespaceFrom(cds);
            var filePath = cds.GetLocation().GetLineSpan().Path;

            var sb = new StringBuilder();
            GeneratorsUtils.WriteGeneratedFileHeader(sb);

            sb.Append($@"
namespace {ns};

internal partial class {cds.Identifier}
{{
public static partial string GetFileName() => @""{filePath}"";
}}
");

            spc.AddSource(cds.Identifier.ToString() + ".generated.cs", sb.ToString());

            string GetNamespaceFrom(SyntaxNode s)
            {
                while (s.Parent != null)
                {
                    if (s.Parent is NamespaceDeclarationSyntax nds)
                    {
                        return nds.Name.ToString();
                    }
                }

                return null;
            }
        }

        private ClassDeclarationSyntax GetCandidate(GeneratorSyntaxContext gsc, CancellationToken ct)
        {
            if (gsc.Node is ClassDeclarationSyntax cds)
            {
                foreach (var member in cds.Members)
                {
                    if (member is MethodDeclarationSyntax mds && string.Equals(mds.Identifier.ToString(),
                        "GetFileName", StringComparison.OrdinalIgnoreCase) && mds.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword)))
                    {
                        return cds;
                    }
                }
            }

            return null;
        }

        private bool IsCandidate(SyntaxNode node, CancellationToken ct)
        {
            return (node is ClassDeclarationSyntax);
        }
    }
}
