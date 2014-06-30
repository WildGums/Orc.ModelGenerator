using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace Orc.ModelGenerator.DataProviders
{
    internal class CsvDataProvider: DataProvider
    {
        private readonly DataFile _dataFile;

        public CsvDataProvider(DataFile dataFile)
        {
            _dataFile = dataFile;
        }

        public override IEnumerable<Entity> Generate()
        {
            var entityName = CreateEntityName(_dataFile.FileName.Replace(".csv", string.Empty));
            using (var csvReader = CreateCsvReader(_dataFile.FullFileName))
            {
                csvReader.Read();
                var headers = csvReader.FieldHeaders;
                var entity = new Entity(entityName);

                for (int i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    entity.Properties.Add(new EntityProperty(header, DetectType(csvReader.CurrentRecord[i])));
                }
                yield return entity;
            }
        }

        private Type DetectType(string stringValue)
        {
            var expectedTypes = new List<Type> { typeof(DateTime), typeof(int), typeof(double), typeof(string) };
            foreach (var type in expectedTypes)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        // You'll have to think about localization here
                        object newValue = converter.ConvertFromInvariantString(stringValue);
                        if (newValue != null)
                        {
                            return type;
                        }
                    }
                    catch
                    {
                        // Can't convert given string to this type
                        continue;
                    }

                }
            }

            return null;
        }

        private CsvReader CreateCsvReader(string path)
        {
            var csvReader = new CsvReader(new StreamReader(path));
            csvReader.Configuration.CultureInfo = new CultureInfo("en-AU");
            csvReader.Configuration.WillThrowOnMissingField = false;
            csvReader.Configuration.HasHeaderRecord = true;
            return csvReader;
        }
    }
}
