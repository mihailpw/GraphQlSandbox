using System;
using System.Collections.Generic;
using System.Reflection;
using GraphQL.Reflection;

namespace GQL.WebApp.Serviced.GraphQlV2._
{
    internal class SingleMethodAccessor : IAccessor
    {
        public MethodInfo MethodInfo { get; }
        public string FieldName => MethodInfo.Name;
        public Type ReturnType => MethodInfo.ReturnType;
        public Type DeclaringType => MethodInfo.DeclaringType;
        public ParameterInfo[] Parameters => MethodInfo.GetParameters();


        public SingleMethodAccessor(MethodInfo getter)
        {
            MethodInfo = getter;
        }


        public IEnumerable<T> GetAttributes<T>() where T : Attribute => MethodInfo.GetCustomAttributes<T>();
        public object GetValue(object target, object[] arguments) => MethodInfo.Invoke(target, arguments);
    }
}