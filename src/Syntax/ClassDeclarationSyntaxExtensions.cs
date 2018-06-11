using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Zerda.CodeAnalysis.CSharp.Syntax
{
    public static class ClassDeclarationSyntaxExtensions
    {
        public static ClassDeclarationSyntax BaseFrom(this ClassDeclarationSyntax source, params string[] bases)
        {
            return source.AddBaseListTypes(bases.Select(str => SimpleBaseType(ParseTypeName(str))).ToArray());
        }

        public static ClassDeclarationSyntax AddAttribute(this ClassDeclarationSyntax source, AttributeSyntax attributes)
        {
            return source.AddAttributeLists(AttributeList(SingletonSeparatedList(attributes)));
        }

        public static ClassDeclarationSyntax AddAttributes(this ClassDeclarationSyntax source, params AttributeSyntax[] attributes)
        {
            return source.AddAttributeLists(AttributeList(SeparatedList(attributes)));
        }
    }
}
