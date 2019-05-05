using System;

namespace GQL.Client.InfraV2
{
    public sealed class Argument
    {
        public string Id { get; }
        public string Name { get; }
        public string Type { get; }
        public object Value { get; }


        public Argument(string name, string type, object value)
        {
            Name = name;
            Type = type;
            Value = value;

            Id = $"{Name}_{Guid.NewGuid():N}";
        }
    }
}