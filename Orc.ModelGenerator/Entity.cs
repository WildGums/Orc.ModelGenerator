using System.Collections.Generic;

namespace Orc.ModelGenerator
{
    public class Entity
    {
        public Entity(string name)
        {
            Name = name;
            Properties = new List<EntityProperty>();
            IsEnabled = true;
        }

        public string Name { get; private set; }
        public List<EntityProperty> Properties { get; private set; }

        public bool IsEnabled { get; set; }
    }
}