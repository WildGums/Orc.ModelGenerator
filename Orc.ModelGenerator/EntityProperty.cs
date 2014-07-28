using System;

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
            Name = GetName(SourceName);
            PropertyType = GetPropertyType(type);
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
            return sourceName.Replace("ID", "Id")
                .Replace(" ", "")
                .Replace("-", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace("/", "")
                .Replace("\\", "");
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