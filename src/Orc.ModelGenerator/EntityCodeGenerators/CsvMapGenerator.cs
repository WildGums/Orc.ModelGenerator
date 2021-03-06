using System.Text;

namespace Orc.ModelGenerator.EntityCodeGenerators
{
    public class CsvMapGenerator : EntityCodeGenerator
    {
        public override string Generate(Entity entity)
        {
            var properties = GetMapRowsCode(entity);
            var outputString = string.Format(
@"public sealed class {0}Map: CsvClassMap<{0}>
{{
    public {0}Map()
    {{
{1}    }}
}}
",
                entity.Name, properties);

            return outputString;
        }

        private string GetMapRowsCode(Entity entity)
        {
            var sb = new StringBuilder();
            foreach (var entityProperty in entity.Properties)
            {
                sb.AppendLine(string.Format(@"        Map(x => x.{0}).Name(""{1}"");",
                    entityProperty.Name, entityProperty.SourceName));
            }
            return sb.ToString();
        }
    }
}