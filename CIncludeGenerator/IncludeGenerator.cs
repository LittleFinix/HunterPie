using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CIncludeGenerator;

[Generator]
public class IncludeGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SourceVisitor());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var attrName = context.Compilation.GetTypeByMetadataName(typeof(IncludeHeaderAttribute).FullName);
        
        var declarations = context.Compilation.SyntaxTrees.SelectMany(tree =>
            tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());

        declarations = declarations
            .Where(d =>
                d.AttributeLists.Any(al =>
                    al.Attributes.Any(a =>
                        a.Name.ToFullString() == "IncludeHeader")));

        foreach (var declaration in declarations)
        {
            var semanticModel = context.Compilation.GetSemanticModel(declaration.SyntaxTree);
            var type = semanticModel.GetTypeInfo(declaration.Parent!);
            
            
        }

    }
}
