using System;

namespace GQL.Client.Infra
{
    public class Argument
    {
        public string Name { get; }

        public string Type { get; }

        public object Value { get; }

        public string ArgumentName { get; }


        public Argument(string name, string type, object value)
        {
            Name = name;
            Type = type;
            Value = value;

            ArgumentName = $"{Name}_{Guid.NewGuid():N}";
        }
    }
}