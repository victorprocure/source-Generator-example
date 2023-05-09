using System.Text;

using Microsoft.CodeAnalysis;

namespace EnumDescriptor.Extensions
{
    public static class ITypeSymbolExtensions
    {
        /// <summary>
        /// Try and get the fully qualified name of the symbol given
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string GetFullyQualifiedMetadataName(this ITypeSymbol symbol)
        {
            static void BuildFrom(ISymbol? symbol, in StringBuilder builder)
            {
                switch (symbol)
                {
                    case INamespaceSymbol { ContainingNamespace.IsGlobalNamespace: false }:
                        BuildFrom(symbol.ContainingNamespace, in builder);
                        _ = builder.Append('.');
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    case INamespaceSymbol { IsGlobalNamespace: false }:
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    case ITypeSymbol { ContainingSymbol: INamespaceSymbol { IsGlobalNamespace: true } }:
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    case ITypeSymbol { ContainingSymbol: INamespaceSymbol namespaceSymbol }:
                        BuildFrom(namespaceSymbol, in builder);
                        _ = builder.Append('.');
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    case ITypeSymbol { ContainingType: ITypeSymbol typeSymbol }:
                        BuildFrom(typeSymbol, in builder);
                        _ = builder.Append('.');
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    default:
                        break;
                }
            }

            StringBuilder stringBuilder = new();
            BuildFrom(symbol, in stringBuilder);

            return stringBuilder.ToString();
        }

        public static bool HasFullyQualifiedMetadataName(this ITypeSymbol symbol, string name)
        {
            StringBuilder builder = new();

            symbol.AppendFullyQualifiedMetadataName(in builder);

            string fullyQualifiedName = builder.ToString();

            return fullyQualifiedName.AsSpan().SequenceEqual(name.AsSpan());
        }

        private static void AppendFullyQualifiedMetadataName(this ITypeSymbol symbol, in StringBuilder builder)
        {
            static void BuildFrom(ISymbol? symbol, in StringBuilder builder)
            {
                switch (symbol)
                {
                    // Namespaces that are nested also append a leading '.'
                    case INamespaceSymbol { ContainingNamespace.IsGlobalNamespace: false }:
                        BuildFrom(symbol.ContainingNamespace, in builder);
                        _ = builder.Append('.');
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    // Other namespaces (ie. the one right before global) skip the leading '.'
                    case INamespaceSymbol { IsGlobalNamespace: false }:
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    // Types with no namespace just have their metadata name directly written
                    case ITypeSymbol { ContainingSymbol: INamespaceSymbol { IsGlobalNamespace: true } }:
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    // Types with a containing non-global namespace also append a leading '.'
                    case ITypeSymbol { ContainingSymbol: INamespaceSymbol namespaceSymbol }:
                        BuildFrom(namespaceSymbol, in builder);
                        _ = builder.Append('.');
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    // Nested types append a leading '+'
                    case ITypeSymbol { ContainingSymbol: ITypeSymbol typeSymbol }:
                        BuildFrom(typeSymbol, in builder);
                        _ = builder.Append('+');
                        _ = builder.Append(symbol.MetadataName);
                        break;

                    default:
                        break;
                }
            }

            BuildFrom(symbol, in builder);
        }
    }
}