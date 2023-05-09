using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using EnumDescriptor.Extensions;

namespace EnumDescriptor.Models
{
    internal static class Emitter
    {
        /// <summary>
        /// Using Microsoft Syntax generator, we build our extension methods for the enum tagged.
        /// The extension method will be called 'ToStringFast'
        /// We also must decorate our code with tags to exclude from any test plugins that may track code coverage
        /// we can't expect users to test our generated code.
        /// All types must be defined as "global" scope to reduce the chances of conflicts with other generated code
        /// We also tell the compiler what analyzer emitted this code, to allow users to navigate and see generated code within
        /// Visual studio
        /// </summary>
        public static MemberDeclarationSyntax GetSyntax(INamedTypeSymbol enumSymbol)
        {
            var memberDeclarationSyntax = MethodDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)), Identifier("ToStringFast"))
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                .AddAttributeLists(
                    AttributeList(SingletonSeparatedList(
                           Attribute(IdentifierName("global::System.CodeDom.Compiler.GeneratedCode"))
                           .AddArgumentListArguments(
                               AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(typeof(EnumDescriptorGenerator).FullName))),
                               AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression,
                                Literal(typeof(EnumDescriptorGenerator).Assembly.GetName().Version.ToString()))))))
                    .WithOpenBracketToken(Token(TriviaList(Comment($"/// <summary>Convert values for <see cref=\"{enumSymbol.GetFullyQualifiedMetadataName()}\" /> to string value</summary>"), Comment($"/// <returns>Returns <see cref=\"string\" /> value of {enumSymbol.MetadataName}</returns>")), SyntaxKind.OpenBracketToken, TriviaList())),
                    AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage")))))
                .WithParameterList(ParameterList(
                    SingletonSeparatedList(
                        Parameter(Identifier(enumSymbol.MetadataName))
                        .WithModifiers(TokenList(Token(SyntaxKind.ThisKeyword)))
                        .WithType(IdentifierName(enumSymbol.GetFullyQualifiedMetadataName()))
                    )))
                .WithExpressionBody(
                    ArrowExpressionClause(
                        SwitchExpression(IdentifierName(enumSymbol.MetadataName))
                        .WithArms(GetEnumReturns(enumSymbol))))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                .NormalizeWhitespace();

            return memberDeclarationSyntax;
        }

        /// <summary>
        /// Build the return for the extension method from the enum given
        /// In this case we are using `nameof` instead of typing in the name, this is just as performant as
        /// manually typing out the value we wish to see
        /// </summary>
        private static SeparatedSyntaxList<SwitchExpressionArmSyntax> GetEnumReturns(INamedTypeSymbol enumSymbol)
        {
            var members = enumSymbol.GetMembers()
                .Where(m => m is IFieldSymbol field && field.ConstantValue is not null)
                .Cast<IFieldSymbol>()
                .Select(static m =>
                    SwitchExpressionArm(
                        ConstantPattern(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName((m.ContainingSymbol as ITypeSymbol)?.GetFullyQualifiedMetadataName()!),
                                IdentifierName(m.MetadataName))),

                        LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(m.MetadataName)))).ToList();
            members.Add(SwitchExpressionArm(
                                    DiscardPattern(),
                                    ThrowExpression(
                                        ObjectCreationExpression(
                                            IdentifierName("global::System.ArgumentOutOfRangeException"))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList<ArgumentSyntax>(
                                                    Argument(
                                                        InvocationExpression(
                                                            IdentifierName(
                                                                Identifier(
                                                                    TriviaList(),
                                                                    SyntaxKind.NameOfKeyword,
                                                                    "nameof",
                                                                    "nameof",
                                                                    TriviaList())))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SingletonSeparatedList<ArgumentSyntax>(
                                                                    Argument(
                                                                        IdentifierName(enumSymbol.MetadataName))))))))))));
            var separatedList = SeparatedList(members);

            return separatedList;
        }
    }
}