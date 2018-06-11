# Roslyn Syntax Extensions
Manipulate the syntax tree in easy way.

# How to build an POCO class
```csharp
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Zerda.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Zerda.CodeAnalysis.CSharp.SyntaxStarter;

// [Table("Eating")]
// public class Food {}
var @class = ClassDeclaration("Food")
    .AddModifiers(PublicModifier())
    .AddAttributes(Attribute("Table", "Eating"));

@class = @class.AddMembers(
    // [Required]
    // [MaxLength(100)]
    // public string Name { get; set; }
    ReadWritePropertyDeclaration("string", "Name")
        .AddModifiers(PublicModifier())
        .AddAttribute(Attribute("Required"))
        .AddAttribute(Attribute("MaxLength", 100)),
    // public int? Calorie { get; set; }
    ReadWritePropertyDeclaration(NullableType(PredefinedType(Token(SyntaxKind.IntKeyword))), "Calorie")
        .AddModifiers(PublicModifier())
);

// namespace EFCore.Sample.Models {}
var @namespace = NamespaceDeclaration("EFCore.Sample.Models").AddMembers(@class);

// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
var root = CompilationUnit()
  .AddUsings(
    "System.ComponentModel.DataAnnotations", 
    "System.ComponentModel.DataAnnotations.Schema")
  .AddMembers(@namespace);

// Get Definition
root.NormalizeWhitespace().ToFullString();
```
