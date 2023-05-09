
Input:
using System;
namespace TestNameSpace
{
    public sealed class TestClass
    {
        private readonly string testValue;
        
        public TestClass(string testValue)
        {
            this.testValue = testValue;
        }
        
        public string GetValue()
        {
            return testValue;
        }
    }
}

output:
CompilationUnit()
.WithUsings(
    SingletonList<UsingDirectiveSyntax>(
        UsingDirective(
            IdentifierName("System"))))
.WithMembers(
    SingletonList<MemberDeclarationSyntax>(
        NamespaceDeclaration(
            IdentifierName("TestNameSpace"))
        .WithMembers(
            SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration("TestClass")
                .WithModifiers(
                    TokenList(
                        new []{
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.SealedKeyword)}))
                .WithMembers(
                    List<MemberDeclarationSyntax>(
                        new MemberDeclarationSyntax[]{
                            FieldDeclaration(
                                VariableDeclaration(
                                    PredefinedType(
                                        Token(SyntaxKind.StringKeyword)))
                                .WithVariables(
                                    SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        VariableDeclarator(
                                            Identifier("testValue")))))
                            .WithModifiers(
                                TokenList(
                                    new []{
                                        Token(SyntaxKind.PrivateKeyword),
                                        Token(SyntaxKind.ReadOnlyKeyword)})),
                            ConstructorDeclaration(
                                Identifier("TestClass"))
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword)))
                            .WithParameterList(
                                ParameterList(
                                    SingletonSeparatedList<ParameterSyntax>(
                                        Parameter(
                                            Identifier("testValue"))
                                        .WithType(
                                            PredefinedType(
                                                Token(SyntaxKind.StringKeyword))))))
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ExpressionStatement(
                                            AssignmentExpression(
                                                SyntaxKind.SimpleAssignmentExpression,
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    ThisExpression(),
                                                    IdentifierName("testValue")),
                                                IdentifierName("testValue")))))),
                            MethodDeclaration(
                                PredefinedType(
                                    Token(SyntaxKind.StringKeyword)),
                                Identifier("GetValue"))
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword)))
                            .WithBody(
                                Block(
                                    SingletonList<StatementSyntax>(
                                        ReturnStatement(
                                            IdentifierName("testValue")))))}))))))
.NormalizeWhitespace()