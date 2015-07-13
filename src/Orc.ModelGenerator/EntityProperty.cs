using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Orc.ModelGenerator
{
    public class EntityProperty : BaseGeneratorEntity
    {

        private Type _type;
        private readonly string _testStringValue;
        private EntityPropertyType _propertyType;
        private string _name;
        private bool _nullable;

        public EntityProperty(string name, Type type, string testStringValue)
        {
            SourceName = name;
            _type = type;
            _testStringValue = testStringValue;
            Name = GetName(CreateFieldName(name));
            PropertyType = GetPropertyType(type);
            ExampleValues = new Collection<string>();
            ExampleValues.Add(testStringValue);
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
                .Replace("%", "")
                .Replace("~", "")
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

        public EntityPropertyType PropertyType
        {
            get { return _propertyType; }
            set
            {
                if (_propertyType == value) return;
                _propertyType = value;
                RaisePropertyChanged(() => PropertyType);
                RaisePropertyChanged(() => FriendlyFullTypeName);
            }
        }

        public bool Nullable
        {
            get { return _nullable; }
            set
            {
                if (_nullable == value) return;
                _nullable = value;
                RaisePropertyChanged(() => Nullable);
                RaisePropertyChanged(() => FriendlyFullTypeName);
            }
        }

        public string FriendlyTypeName
        {
            get
            {
                var nullable = Nullable ? "?" : "";
                if (PropertyType == EntityPropertyType.Int) return "int" + nullable;
                if (PropertyType == EntityPropertyType.Double) return "double" + nullable;
                if (PropertyType == EntityPropertyType.String) return "string";
                if (PropertyType == EntityPropertyType.DateTime) return "DateTime" + nullable;
                if (PropertyType == EntityPropertyType.TimeSpan) return "TimeSpan" + nullable;
                return "N/A";
            }
        }

        public string FriendlyFullTypeName
        {
            get
            {
                return Name + " : " + FriendlyTypeName;
            }
        }
        public Collection<string> ExampleValues { get; private set; } 
        public string TestValue
        {
            get
            {
                if (Nullable && _testStringValue.Trim().Length == 0)
                {
                    return "null";
                }
                if (PropertyType == EntityPropertyType.Int)
                {
                    if (_testStringValue.Contains("0") && _testStringValue.Length > 1)
                        return _testStringValue.Substring(_testStringValue.LastIndexOf("0") + 1);
                    return _testStringValue;
                }

                if (PropertyType == EntityPropertyType.Double) return _testStringValue;
                if (PropertyType == EntityPropertyType.String) return string.Format(@"""{0}""", _testStringValue);
                if (PropertyType == EntityPropertyType.DateTime)
                {
                    DateTime dateTime;
                    DateTime.TryParse(_testStringValue, out dateTime);
                    return string.Format(@"new DateTime({0}, {1}, {2})", dateTime.Year, dateTime.Month, dateTime.Day);
                }
                if (PropertyType == EntityPropertyType.TimeSpan) return "new TimeSpan(10,10,0)";
                throw new Exception();
            }
        }

        public string SourceName { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => FriendlyFullTypeName);
            }
        }

        public string ToCode()
        {
            return string.Format(@"    public {0} {1} {{ get; set; }}", FriendlyTypeName, Name);
        }
    }
}