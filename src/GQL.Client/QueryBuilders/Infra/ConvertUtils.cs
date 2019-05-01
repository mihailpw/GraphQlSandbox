using System;

namespace GQL.Client.QueryBuilders.Infra
{
    public static class Utils
    {
        public static string ConvertRequestTypeToString(GraphQlRequestType requestType)
        {
            switch (requestType)
            {
                case GraphQlRequestType.Query:
                    return "query";
                case GraphQlRequestType.Mutation:
                    return "mutation";
                case GraphQlRequestType.Subscription: // TODO check this
                    return "subscription";
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
            }
        }

        public static string ConvertTypeToString(Type type)
        {
            var typeName = type.Name;

            switch (typeName)
            {
                case nameof(String):
                    return "String";
                case nameof(Int32):
                    return "Int";
                case nameof(Int64):
                    return "Int";
                case nameof(Double):
                    return "Float";
                case nameof(Boolean):
                    return "Boolean";
                default:
                    return typeName.Replace("QueryBuilder", string.Empty);
            }
        }
    }
}