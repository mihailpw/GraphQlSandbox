using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.GraphQlClientCore
{
    public class Arguments : IEnumerable<Arguments.Entry>
    {
        private readonly string _builderName;

        private readonly List<string> _requiredArguments;
        private readonly List<Entry> _arguments;


        public Arguments(string builderName)
        {
            _builderName = builderName;

            _requiredArguments = new List<string>();
            _arguments = new List<Entry>();
        }


        IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }


        public void ThrowIfNotValid()
        {
            var addedRequiredArguments = _requiredArguments
                .Where(ra => _arguments.Any(a => a.Name == ra))
                .ToList();
            if (addedRequiredArguments.Count != _requiredArguments.Count)
            {
                var notAddedArguments = _requiredArguments.Except(addedRequiredArguments);
                throw new RequiredArgumentNotAddedException(_builderName, notAddedArguments);
            }
        }

        public void AddRequiredArgument(string fieldName)
        {
            _requiredArguments.Add(fieldName);
        }

        public void AddArgument(string fieldName, string typeName, object value)
        {
            _arguments.Add(new Entry(fieldName, typeName, value));
        }



        public class Entry
        {
            public string Name { get; }

            public string Type { get; }

            public object Value { get; }

            public string ArgumentName { get; }


            public Entry(string name, string type, object value)
            {
                Name = name;
                Type = type;
                Value = value;

                ArgumentName = $"{Name}_{Guid.NewGuid():N}";
            }
        }
    }
}