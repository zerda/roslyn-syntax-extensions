using System;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;

namespace EdmxExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading from EDMX file...");

            // reading from edmx file
            var edmxPath = "./Sample_v3.edmx";
            using (var reader = XmlReader.Create(edmxPath))
            {
                const string basePath = "bin/Output/Models";
                Directory.CreateDirectory(basePath);

                // select from all entity sets
                var model = EdmxReader.Parse(reader);
                var entityTypes = model.EntityContainers().First().EntitySets().Select(set => set.ElementType);
                foreach (var entityType in entityTypes)
                {
                    var entity = EntityDefinition.From(entityType);
                    File.WriteAllText(Path.Combine(basePath, $"{entityType.Name}.cs"), entity.NormalizeWhitespace().ToFullString());
                    Console.WriteLine($"{entityType.Name}");
                }
            }

            Console.WriteLine("All done.");
        } 
    }
}
