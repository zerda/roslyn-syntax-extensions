using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.Edm;
using Zerda.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Zerda.CodeAnalysis.CSharp.SyntaxStarter;

namespace EdmxExtractor
{
    public class EntityDefinition
    {
        /// <summary>
        /// Create entity class from EDMX entity definition.
        /// </summary>
        /// <param name="entityType">entity type from CSDL</param>
        /// <returns>class defination string</returns>
        /// <remarks>
        /// References
        /// - http://roslynquoter.azurewebsites.net/
        /// - https://gist.github.com/cmendible/9b8c7d7598f1ab0bc7ab5d24b2622622
        /// - https://joshvarty.com/2015/09/18/learn-roslyn-now-part-13-keeping-track-of-syntax-nodes-with-syntax-annotations/
        /// </remarks>
        public static CompilationUnitSyntax From(IEdmEntityType entityType)
        {
            var name = entityType.Name;

            // [Table("Foo")]
            // public class Foo { }
            var @class = ClassDeclaration(name)
                .AddModifiers(PublicModifier())
                .AddAttribute(Attribute("Table", name));

            foreach (var structuralProp in entityType.StructuralProperties())
            {
                @class = @class.AddMembers(GetPropertyDeclarationSyntax(structuralProp));
            }
            foreach (var navigationProp in entityType.NavigationProperties())
            {
                @class = @class.AddMembers(GetPropertyDeclarationSyntax(navigationProp));
            }

            // namespace SampleModel { }
            var @namespace = NamespaceDeclaration(entityType.Namespace)
                .AddMembers(@class);

            // using System;
            var root = CompilationUnit()
                .AddUsings("System",
                           "System.Collections.Generic",
                           "System.ComponentModel.DataAnnotations",
                           "System.ComponentModel.DataAnnotations.Schema")
                .AddMembers(@namespace);

            return root;
        }

        private static PropertyDeclarationSyntax GetPropertyDeclarationSyntax(IEdmProperty entityProp)
        {
            // public string Name { get; set; }
            TypeSyntax typeSyntax = GetTypeSyntax(entityProp.Type);
            var prop = ReadWritePropertyDeclaration(typeSyntax, entityProp.Name).AddModifiers(PublicModifier());
            if (entityProp.Type.IsString())
            {
                var edmStringType = entityProp.Type.AsString();
                // [Required]
                if (!edmStringType.IsNullable)
                {
                    prop = prop.AddAttribute(Attribute("Required"));
                }
                // [StringLength(100)]
                if (edmStringType.MaxLength.HasValue)
                {
                    prop = prop.AddAttribute(Attribute("StringLength", edmStringType.MaxLength.Value));
                }
            }
            return prop;
        }

        private static TypeSyntax GetTypeSyntax(IEdmTypeReference type)
        {
            if (type.IsPrimitive())
            {
                return GetPrimitiveTypeSyntax(type);
            }
            else if (type.IsCollection())
            {
                var elementType = type.AsCollection().ElementType();
                var elementTypeSyntax = GetTypeSyntax(elementType);
                return GenericName(Identifier("IEnumerable"))
                    .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList(elementTypeSyntax)));
            }
            else
            {
                return IdentifierName(type.Definition.ToString());
            }
        }

        private static TypeSyntax GetPrimitiveTypeSyntax(IEdmTypeReference type)
        {
            SyntaxToken token = default(SyntaxToken);
            var primitiveKind = type.PrimitiveKind();
            switch (primitiveKind)
            {
                case EdmPrimitiveTypeKind.Int32:
                    token = Token(SyntaxKind.IntKeyword);
                    break;
                case EdmPrimitiveTypeKind.String:
                    token = Token(SyntaxKind.StringKeyword);
                    break;
                case EdmPrimitiveTypeKind.Double:
                    token = Token(SyntaxKind.DoubleKeyword);
                    break;
                case EdmPrimitiveTypeKind.Boolean:
                    token = Token(SyntaxKind.BoolKeyword);
                    break;
                case EdmPrimitiveTypeKind.Decimal:
                    token = Token(SyntaxKind.DecimalKeyword);
                    break;
            }

            TypeSyntax typeSyntax;
            if (token == default(SyntaxToken))
            {
                typeSyntax = IdentifierName(primitiveKind.ToString());
            }
            else
            {
                typeSyntax = PredefinedType(token);
            }
            if (type.IsNullable && !type.IsString())
            {
                typeSyntax = NullableType(typeSyntax);
            }
            return typeSyntax;
        }
    }
}
