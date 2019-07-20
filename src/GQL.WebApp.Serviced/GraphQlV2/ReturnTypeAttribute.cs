using System;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class ReturnTypeAttribute : Attribute
    {
        public Type ReturnType { get; }


        public ReturnTypeAttribute(Type returnType)
        {
            ReturnType = returnType;
        }
    }
}