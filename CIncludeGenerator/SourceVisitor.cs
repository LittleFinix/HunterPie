using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CIncludeGenerator;

public class SourceVisitor : ISyntaxReceiver
{
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0)
        {
            var syntaxAttributes = cds.AttributeLists.SelectMany(e => e.Attributes)
                .Where(e => e.Name.NormalizeWhitespace().ToFullString() == "IncludeHeader");

            if (!syntaxAttributes.Any())
                return;
            
            if (cds.Modifiers.All(m => m.ToFullString() != "partial"))
                return;
            
        }
    }
}