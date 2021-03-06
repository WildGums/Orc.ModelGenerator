﻿using System;
using System.Collections.Generic;
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
            var itemCount = GetRowCount(_dataFile.FullFileName);
            using (var csvReader = CreateCsvReader(_dataFile.FullFileName))
            {
                csvReader.Read();
                var entity = new CsvEntity(entityName, _dataFile.FileName, itemCount);

                var headers = csvReader.FieldHeaders;
                for (int i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    var record = csvReader.CurrentRecord == null?String.Empty:csvReader.CurrentRecord[i];
                    entity.Properties.Add(new EntityProperty(header, DetectType(record), record));
                }
                yield return entity;
            }
        }

        private int GetRowCount(string fullFileName)
        {
            using (var csvReader = CreateCsvReader(fullFileName))
            {
                var i = 0;
                while (csvReader.Read())
                {
                    i++;
                }
                return i;
            }
        }


        private Type DetectType(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue)) return typeof (string);

            DateTime dateTimeValue;
            if (DateTime.TryParse(stringValue, out dateTimeValue)) return typeof (DateTime);
            int intValue;
            if (int.TryParse(stringValue, out intValue)) return typeof (int);
            double doubleValue;
            if (double.TryParse(stringValue, out doubleValue)) return typeof(double);

            return typeof (string);
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
