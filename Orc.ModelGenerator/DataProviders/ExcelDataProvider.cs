using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

namespace Orc.ModelGenerator.DataProviders
{
    internal class ExcelDataProvider : DataProvider
    {
        private readonly DataFile _dataFile;

        public ExcelDataProvider(DataFile dataFile)
        {
            _dataFile = dataFile;
        }

        public override IEnumerable<Entity> Generate()
        {
            using (var stream = File.Open(_dataFile.FullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                excelDataReader.IsFirstRowAsColumnNames = true;
                var excelDataSet = excelDataReader.AsDataSet();
                foreach (DataTable table in excelDataSet.Tables)
                {
                    yield return CreateEntity(table);
                }
            }
        }

        private Entity CreateEntity(DataTable table)
        {
            var entity = new Entity(CreateEntityName(table.TableName));
            foreach (DataColumn column in table.Columns)
            {
                entity.Properties.Add(new EntityProperty(CreatePropertyName(column.ColumnName), column.DataType));
            }
            return entity;
        }
    }
}