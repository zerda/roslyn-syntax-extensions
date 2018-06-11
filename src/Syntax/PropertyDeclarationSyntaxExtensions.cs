using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Zerda.CodeAnalysis.CSharp.Syntax
{
    public static class PropertyDeclarationSyntaxExtensions
    {
        public static bool HasGetAccessor(this PropertyDeclarationSyntax source)
        {
            if (source.AccessorList == null)
                return false;
            return source.AccessorList.Accessors.IndexOf(item => item.Keyword.RawKind == (int)SyntaxKind.GetKeyword) >= 0;
        }

        public static bool HasSetAccessor(this PropertyDeclarationSyntax source)
        {
            if (source.AccessorList == null)
                return false;
            return source.AccessorList.Accessors.IndexOf(item => item.Keyword.RawKind == (int)SyntaxKind.SetKeyword) >= 0;
        }

        public static PropertyDeclarationSyntax AddAttribute(this PropertyDeclarationSyntax source, AttributeSyntax attributes)
        {
            return source.AddAttributeLists(AttributeList(SingletonSeparatedList(attributes)));
        }

        public static PropertyDeclarationSyntax AddAttributes(this PropertyDeclarationSyntax source, params AttributeSyntax[] attributes)
        {
            return source.AddAttributeLists(AttributeList(SeparatedList(attributes)));
        }
    }
}
