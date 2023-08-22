using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;

namespace CIncludeGenerator;

public class SourceGenerator : ILibrary
{
    public SourceGenerator(Inc)
    
    public void Preprocess(Driver driver, ASTContext ctx) => throw new NotImplementedException();

    public void Postprocess(Driver driver, ASTContext ctx) => throw new NotImplementedException();

    public void Setup(Driver driver)
    {
        driver.Options.GeneratorKind = GeneratorKind.CSharp;
        driver.Options.AddModule("")
    }

    public void SetupPasses(Driver driver) => throw new NotImplementedException();
}