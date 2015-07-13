using System;
using System.Collections.Generic;

namespace Orc.ModelGenerator.DataProviders
{
    public abstract class DataProvider
    {
        public static DataProvider CreateDataProvider(DataFile datafile)
        {
            if (datafile.DataFileType == DataFileType.Csv)
                return new CsvDataProvider(datafile);
            else if (datafile.DataFileType == DataFileType.Excel)
                return new ExcelDataProvider(datafile);
            else
                throw new Exception("Invalid data file type");
        }

        public abstract IEnumerable<Entity> Generate();

        protected string CreatePropertyName(string header)
        {
            header = header.Replace(" ", string.Empty);
            return header;
        }

        protected string CreateEntityName(string entityName)
        {
            entityName = entityName.Replace(" ", string.Empty);
            if (entityName.EndsWith("s"))
                entityName = entityName.Substring(0, entityName.Length - 1);
            return entityName;
        }
    }
}