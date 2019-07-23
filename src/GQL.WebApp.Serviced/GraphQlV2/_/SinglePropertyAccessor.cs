using System;
using System.Collections.Generic;
using System.Reflection;
using GraphQL.Reflection;

namespace GQL.WebApp.Serviced.GraphQlV2._
{
    internal class SinglePropertyAccessor : IAccessor
    {
        private readonly PropertyInfo _getter;


        public string FieldName => _getter.Name;
        public Type ReturnType => _getter.PropertyType;
        public Type DeclaringType => _getter.DeclaringType;
        public ParameterInfo[] Parameters => _getter.GetMethod.GetParameters();
        public MethodInfo MethodInfo => _getter.GetMethod;


        public SinglePropertyAccessor(PropertyInfo getter)
        {
            _getter = getter;
        }


        public IEnumerable<T> GetAttributes<T>() where T : Attribute => _getter.GetCustomAttributes<T>();
        public object GetValue(object target, object[] arguments) => _getter.GetValue(target);
    }
}