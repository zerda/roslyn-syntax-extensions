using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Zerda.CodeAnalysis.CSharp.Syntax
{
    public static class CompilationUnitSyntaxExtensions
    {
        public static CompilationUnitSyntax AddUsings(this CompilationUnitSyntax source, params string[] items)
        {
            return source.AddUsings(items.Select(str => UsingDirective(ParseName(str))).ToArray());
        }
    }
}
