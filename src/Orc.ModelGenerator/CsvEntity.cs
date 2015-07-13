namespace Orc.ModelGenerator
{
    public class CsvEntity:Entity
    {
        public CsvEntity(string name, string fileName, int rowCount)
            : base(name, rowCount)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }
    }
}