using System.Collections.Generic;

namespace Orc.ModelGenerator
{
    public class Entity : BaseGeneratorEntity
    {
        public Entity(string name, int rowCount=999)
        {
            Name = name;
            Properties = new List<EntityProperty>();
            IsEnabled = true;
            RowCount = rowCount;
        }

        public string Name { get; set; }
        public List<EntityProperty> Properties { get; private set; }

        public bool IsEnabled { get; set; }
        public int RowCount { get; set; }
    }
}