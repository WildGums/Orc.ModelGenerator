using System.Text;

namespace Orc.ModelGenerator.EntityCodeGenerators
{
    public class RepositoryUnitTestGenerator : EntityCodeGenerator
    {
        public override string Generate(Entity entity)
        {
            var properties = GetUnitTestRowsCode(entity);
            return string.Format(
                @"[TestMethod]
public void Load{0}s()
{{
    using (var repo = new ChickenRepository(DataFolder))
    {{
        var records = repo.Load{0}s();
        Assert.AreEqual(999, records.Length);

{1}
    }}
}}
",
                entity.Name, properties);
        }

        private string GetUnitTestRowsCode(Entity entity)
        {
            var sb = new StringBuilder();
            foreach (var entityProperty in entity.Properties)
            {
                sb.AppendLine(string.Format(@"        Assert.AreEqual(999, records[0].{0});",
                    entityProperty.Name));
            }
            return sb.ToString();
        }
    }
}