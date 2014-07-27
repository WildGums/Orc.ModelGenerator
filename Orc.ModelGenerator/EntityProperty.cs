using System;

namespace Orc.ModelGenerator
{
    public class EntityProperty
    {
        private readonly string _sourceName;
        private readonly Type _type;
        private readonly string _testValue;
        private string _friendlyTypeName;

        public EntityProperty(string name, Type type, string testValue)
        {
            _sourceName = name;
            _type = type;
            _testValue = testValue;
            _friendlyTypeName = GetFriendlyTypeName(_type);
        }

        public string FriendlyTypeName
        {
            get { return _friendlyTypeName; }
        }

        public string TestValue
        {
            get { return _testValue; }
        }
        public string SourceName
        {
            get { return _sourceName; }
        }
        public string Name
        {
            get
            {
                return _sourceName.Replace("ID", "Id")
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace(":", "")
                    .Replace(".", "")
                    .Replace("/", "")
                    .Replace("\\", "");
            }
        }

        public string ToCode()
        {
            return string.Format(@"    public {0} {1} {{ get; set; }}", _friendlyTypeName, Name);
        }

        private string GetFriendlyTypeName(Type type)
        {
            if (type == typeof (int)) return "int";
            if (type == typeof (string)) return "string";
            if (type == typeof(double)) return "double";
            return type.Name;
        }
    }
}