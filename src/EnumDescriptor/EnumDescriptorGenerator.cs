using System.Text;

using EnumDescriptor.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumDescriptor;

[Generator(LanguageNames.CSharp)]
public sealed class EnumDescriptorGenerator : IIncrementalGenerator
{
    /// <summary>
    /// Initialize Method on incremental generator is called exactly once per host,
    /// regardless of whether the generator is shared across multiple projects in a solution
    /// </summary>
    /// <remarks>
    /// This means any changes to the generator require the project to be reloaded, i.e Visual Studio closed and reopened
    /// </remarks>
    /// <param name="context"></param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // This is the code to generate our source definitions for any enum's that are tagged with our custom attribute
        // 'DescribeEnumAttribute'. The tag we're looking for must be fully qualified.
        // We create a custom dictionary with our 'HierarchyInfo' to store information about the enum for later use
        IncrementalValuesProvider<(HierarchyInfo HierarchyInfo, INamedTypeSymbol EnumSymbol)> classDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName("EnumDescriptor.DescribeEnumAttribute",
                static (node, _) => node is EnumDeclarationSyntax { AttributeLists.Count: > 0 },
                static (context, token) =>
                {
                    // Because we are using static code generation via expressions and switch statements in outputted code,
                    // we must check the compilation is being done against a supported csharp language version
                    if (context.SemanticModel.Compilation is not CSharpCompilation { LanguageVersion: >= LanguageVersion.CSharp8 })
                    {
                        return default;
                    }

                    INamedTypeSymbol enumSymbol = (INamedTypeSymbol)context.TargetSymbol;
                    HierarchyInfo hierarchy = HierarchyInfo.From(enumSymbol);

                    return (Hierarchy: hierarchy, EnumSymbol: enumSymbol);
                })
            .Where(static item => item.Hierarchy is not null)!;

        // This registers our source generator with the compiler for execution when the compiler is run
        context.RegisterSourceOutput(classDeclarations, static (context, source) => Execute(context, source));
    }

    /// <summary>
    /// This is run as part of the incremental build pipeline and is registered in the `Initialize` method
    /// via 'RegisterSourceOutput'
    /// </summary>
    /// <param name="context"></param>
    /// <param name="item"></param>
    private static void Execute(SourceProductionContext context, (HierarchyInfo HierarchyInfo, INamedTypeSymbol enumSymbol) item)
    {
        MemberDeclarationSyntax syntax = Emitter.GetSyntax(item.enumSymbol);
        CompilationUnitSyntax compilationUnit = item.HierarchyInfo.GetCompilationUnit(syntax);
        context.AddSource($"{item.HierarchyInfo.FilenameHint}.{item.HierarchyInfo.MetadataName}Extensions.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }
}