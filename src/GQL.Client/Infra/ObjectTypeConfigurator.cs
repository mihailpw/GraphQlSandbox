using System;
using System.Collections.Generic;

namespace GQL.Client.Infra
{
    public interface ITypeConfigurator<out TParentType, out TType>
    {
        TParentType With(Action<TType> setupAction, bool include = true);
    }

    public class ObjectTypeConfigurator<TParentType, TType> : ITypeConfigurator<TParentType, TType>
    {
        private readonly TParentType _parentType;
        private readonly string _fieldName;
        private readonly List<Argument> _arguments;
        private readonly Func<TType> _typeFactory;
        private readonly Action<FieldType> _addFieldTypeAction;


        public ObjectTypeConfigurator(
            TParentType parentType,
            string fieldName,
            List<Argument> arguments,
            Func<TType> typeFactory,
            Action<FieldType> addFieldTypeAction)
        {
            _parentType = parentType;
            _fieldName = fieldName;
            _arguments = arguments;
            _typeFactory = typeFactory;
            _addFieldTypeAction = addFieldTypeAction;
        }


        public TParentType With(Action<TType> action, bool include = true)
        {
            if (include)
            {
                var type = _typeFactory();
                action(type);
                var fieldType = new FieldType(_fieldName, _arguments, type as TypeBase);
                _addFieldTypeAction(fieldType);
            }

            return _parentType;
        }
    }
}