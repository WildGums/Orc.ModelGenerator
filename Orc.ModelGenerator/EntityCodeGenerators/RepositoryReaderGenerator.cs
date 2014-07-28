namespace Orc.ModelGenerator.EntityCodeGenerators
{
    public class RepositoryReaderGenerator : EntityCodeGenerator
    {
        public override string Generate(Entity entity)
        {
            var fileName = "TODO.csv";
            if (entity is CsvEntity)
            {
                fileName = (entity as CsvEntity).FileName;
            }
            var outputString = string.Format(
@"private const string {0}sCsv = @""{1}"";
public {0}[] Load{0}s()
{{
    using (var csvReader = CreateCsvReader({0}sCsv, typeof({0}Map)))
    {{
        var records = csvReader.GetRecords<{0}>();
        return records.ToArray();
    }}
}}
",
                entity.Name, fileName);

            return outputString;
        }
    }
}