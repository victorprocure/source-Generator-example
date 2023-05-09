﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace EnumDescriptor.Models;

internal sealed record TypeInfo(string QualifiedName, TypeKind Kind, bool IsRecord)
{
    public TypeDeclarationSyntax GetSyntax()
    {
        return Kind switch
        {
            TypeKind.Struct => StructDeclaration(QualifiedName),
            TypeKind.Interface => InterfaceDeclaration(QualifiedName),
            TypeKind.Class when IsRecord =>
                RecordDeclaration(Token(SyntaxKind.RecordKeyword), QualifiedName)
                    .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken)),
            _ => ClassDeclaration(QualifiedName)
        };
    }
}