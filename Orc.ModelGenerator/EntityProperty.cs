using System;
using System.Text;

namespace Orc.ModelGenerator
{
    public class EntityProperty : BaseGeneratorEntity
    {

        private Type _type;

        public EntityProperty(string name, Type type, string testValue)
        {
            SourceName = name;
            _type = type;
            TestValue = testValue;
            Name = GetName(CreateFieldName(name));
            PropertyType = GetPropertyType(type);
        }

        private string CreateFieldName(string header)
        {
            if (header.IndexOfAny(new[] { '|' }) != -1)
            {
                return header.Substring(0, header.IndexOfAny(new[] { '|' }));
            }
            return header;
        }
        private EntityPropertyType GetPropertyType(Type type)
        {
            if (type == typeof(int)) return EntityPropertyType.Int;
            if (type == typeof(double)) return EntityPropertyType.Double;
            if (type == typeof(string)) return EntityPropertyType.String;
            if (type == typeof(DateTime)) return EntityPropertyType.DateTime;
            if (type == typeof(TimeSpan)) return EntityPropertyType.TimeSpan;
            return EntityPropertyType.None;
        }

        private string GetName(string sourceName)
        {
            sourceName = MakeCamel(sourceName);
            return sourceName.Replace("ID", "Id")
                .Replace("-", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace("/", "")
                .Replace("\\", "");
        }

        private string MakeCamel(string sourceName)
        {
            if (string.IsNullOrWhiteSpace(sourceName)) return string.Empty;
            var parts = sourceName.Trim().Split(' ');
            StringBuilder sb = new StringBuilder();
            foreach (var part in parts)
            {
                var capital = part[0].ToString().ToUpper();
                var rest = part.Substring(1);
                sb.Append(capital);
                sb.Append(rest);
            }
            return sb.ToString();
        }

        public EntityPropertyType PropertyType { get; set; }

        public string FriendlyTypeName
        {
            get
            {
                if (PropertyType == EntityPropertyType.Int) return "int";
                if (PropertyType == EntityPropertyType.Double) return "double";
                if (PropertyType == EntityPropertyType.String) return "string";
                if (PropertyType == EntityPropertyType.DateTime) return "DateTime";
                if (PropertyType == EntityPropertyType.TimeSpan) return "TimeSpan";
                return "N/A";
            }
        }

        public string TestValue { get; set; }
        public string SourceName { get; set; }
        public string Name { get; set; }
 
        public string ToCode()
        {
            return string.Format(@"    public {0} {1} {{ get; set; }}", FriendlyTypeName, Name);
        }
    }
}