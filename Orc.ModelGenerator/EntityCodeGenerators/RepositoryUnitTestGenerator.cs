using System.Text;

namespace Orc.ModelGenerator.EntityCodeGenerators
{
    public class RepositoryUnitTestGenerator : EntityCodeGenerator
    {
        private string repositoryName = "RawCsvRepository";
        public override string Generate(Entity entity)
        {
            var properties = GetUnitTestRowsCode(entity);
            var outputString = string.Format(
@"[Test]
public void Load{0}s()
{{
    var repo = new {2}(DataFolder);
    var records = repo.Load{0}s();
    Assert.AreEqual({3}, records.Length);
{1}
}}
",
                entity.Name, properties, repositoryName, entity.RowCount);

            return outputString;
        }

        private string GetUnitTestRowsCode(Entity entity)
        {
            var sb = new StringBuilder();
            foreach (var entityProperty in entity.Properties)
            {
                sb.AppendLine(string.Format(@"    Assert.AreEqual({1}, records[0].{0});",
                    entityProperty.Name, entityProperty.TestValue));
            }
            return sb.ToString();
        }
    }
}