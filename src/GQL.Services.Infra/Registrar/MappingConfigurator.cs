﻿using System;
using GQL.Services.Infra.Core;

namespace GQL.Services.Infra.Registrar
{
    internal class MappingConfigurator : IMappingConfigurator
    {
        private readonly Type _graphQlType;
        private readonly IGraphQlTypeRegistry _typeRegistry;


        public MappingConfigurator(
            Type graphQlType,
            IGraphQlTypeRegistry typeRegistry)
        {
            _graphQlType = graphQlType;
            _typeRegistry = typeRegistry;
        }


        public IMappingConfigurator Map<T>()
        {
            _typeRegistry.DirectRegister(typeof(T), _graphQlType);
            return this;
        }
    }
}