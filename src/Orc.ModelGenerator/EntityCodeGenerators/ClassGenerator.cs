using System.Text;

namespace Orc.ModelGenerator.EntityCodeGenerators
{
    public class ClassGenerator : EntityCodeGenerator
    {
        public override string Generate(Entity entity)
        {
            var properties = GetPropertiesCode(entity);
            var outputString = string.Format(
@"public class {0}
{{
{1}}}
",
                entity.Name, properties);

            return outputString;
        }

        private string GetPropertiesCode(Entity entity)
        {
            var sb = new StringBuilder();
            foreach (var entityProperty in entity.Properties)
            {
                sb.AppendLine(GetPropertyCode(entityProperty));
            }
            return sb.ToString();
        }

        private string GetPropertyCode(EntityProperty entityProperty)
        {
            return string.Format(@"    public {0} {1} {{ get; set; }}", entityProperty.FriendlyTypeName, entityProperty.Name);
        }
    }
}