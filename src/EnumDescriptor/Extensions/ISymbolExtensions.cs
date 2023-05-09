using Microsoft.CodeAnalysis;

namespace EnumDescriptor.Extensions
{
    internal static class ISymbolExtensions
    {
        public static bool TryGetAttributeWithFullyQualifiedMetadataName(this ISymbol symbol, string name, out AttributeData? attributeData)
        {
            System.Collections.Immutable.ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
            foreach (AttributeData attribute in attributes)
            {
                if (attribute.AttributeClass?.HasFullyQualifiedMetadataName(name) == true)
                {
                    attributeData = attribute;
                    return true;
                }
            }

            attributeData = null;
            return false;
        }
    }
}