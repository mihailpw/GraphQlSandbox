using System.Collections.Generic;

namespace GQL.Client.QueryBuilders.Infra
{
    public class GraphQlError
    {
        public string Message { get; }

        public IReadOnlyList<Location> Locations { get; set; }

        public IReadOnlyDictionary<string, object> AdditionalEntries { get; set; }


        public GraphQlError(
            string message,
            IReadOnlyList<Location> locations,
            IReadOnlyDictionary<string, object> additionalEntries)
        {
            Message = message;
            Locations = locations;
            AdditionalEntries = additionalEntries;
        }



        public class Location
        {
            public uint Column { get; }

            public uint Line { get; }


            public Location(uint column, uint line)
            {
                Column = column;
                Line = line;
            }
        }
    }
}