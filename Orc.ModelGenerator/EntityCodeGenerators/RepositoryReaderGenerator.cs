namespace Orc.ModelGenerator.EntityCodeGenerators
{
    public class RepositoryReaderGenerator : EntityCodeGenerator
    {
        public override string Generate(Entity entity)
        {
            return string.Format(
                @"

private const string {0}sCsv = @""TODO.csv"";
public {0}[] Load{0}s()
{{
    using (var csvReader = CreateCsvReader({0}sCsv, typeof({0}Map)))
    {{
        var records = csvReader.GetRecords<{0}>();
        return records.ToArray();
    }}
}}
",
                entity.Name);
        }
    }
}