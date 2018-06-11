using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using Xunit;
using Zerda.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Zerda.CodeAnalysis.CSharp.SyntaxStarter;

namespace Zerda.CodeAnalysis.CSharp.CodeGenerator.Tests
{
    public class EFCoreScenario
    {
        [Fact]
        public void DbContext()
        {
            // public class MyContext : DbContext {}
            var @class = ClassDeclaration("MyContext").BaseFrom("DbContext").AddModifiers(PublicModifier());

            // public DbSet<Food> Foods { get; set; }
            @class = @class.AddMembers(
                ReadWritePropertyDeclaration("DbSet<Food>", "Foods").AddModifiers(PublicModifier())
            );

            // namespace EFCore.Sample {}
            var @namespace = NamespaceDeclaration("EFCore.Sample").AddMembers(@class);

            // using EFCore.Sample.Models;
            // using Microsoft.EntityFrameworkCore;
            var root = CompilationUnit();
            root = root.AddUsings("EFCore.Sample.Models", "Microsoft.EntityFrameworkCore").AddMembers(@namespace);

            var expected = File.ReadAllText("./Assets/MyContext.cs");
            Assert.Equal(expected, root.NormalizeWhitespace().ToFullString());
        }

        [Fact]
        public void Entity()
        {
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
            var root = CompilationUnit();
            root = root.AddUsings("System.ComponentModel.DataAnnotations", "System.ComponentModel.DataAnnotations.Schema").AddMembers(@namespace);

            var expected = File.ReadAllText("./Assets/Food.cs");
            Assert.Equal(expected, root.NormalizeWhitespace().ToFullString());
        }
    }
}
