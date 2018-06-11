using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Zerda.CodeAnalysis.CSharp
{
    public static class SyntaxStarter
    {
        public static NamespaceDeclarationSyntax NamespaceDeclaration(string name)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(name));
        }

        public static SyntaxToken PublicModifier()
        {
            return Token(SyntaxKind.PublicKeyword);
        }

        #region PropertyDeclaration
        public static PropertyDeclarationSyntax ReadWritePropertyDeclaration(TypeSyntax type, string name)
        {
            return SyntaxFactory.PropertyDeclaration(type, name).AddAccessorListAccessors(GetAccessorDeclaration(), SetAccessorDeclaration());
        }

        public static PropertyDeclarationSyntax ReadWritePropertyDeclaration(string type, string name)
        {
            return ReadWritePropertyDeclaration(ParseTypeName(type), name);
        }

        public static AccessorDeclarationSyntax GetAccessorDeclaration()
        {
            return AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

        public static AccessorDeclarationSyntax SetAccessorDeclaration()
        {
            return AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }
        #endregion

        #region Attribute
        public static AttributeSyntax Attribute(string name, string value = null)
        {
            if (value == null)
                return SyntaxFactory.Attribute(IdentifierName(name));
            else
                return SyntaxStarter.Attribute(name, SyntaxStarter.AttributeArgument(value));
        }

        public static AttributeSyntax Attribute(string name, int value)
        {            
            return SyntaxStarter.Attribute(name, SyntaxStarter.AttributeArgument(value));
        }

        public static AttributeSyntax Attribute(string name, AttributeArgumentSyntax value)
        {
            return SyntaxFactory.Attribute(IdentifierName(name), 
                AttributeArgumentList(SingletonSeparatedList(value)));
        }

        public static AttributeArgumentSyntax AttributeArgument(string value)
        {
            return SyntaxFactory.AttributeArgument(
                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value)));
        }

        public static AttributeArgumentSyntax AttributeArgument(int value)
        {
            return SyntaxFactory.AttributeArgument(
                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)));
        }

        public static AttributeArgumentSyntax AttributeArgument(ExpressionSyntax value)
        {
            return SyntaxFactory.AttributeArgument(value);
        }
        #endregion
    }
}
