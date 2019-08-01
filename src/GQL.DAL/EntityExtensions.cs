using System;
using GQL.DAL.Models;

namespace GQL.DAL
{
    public static class EntityExtensions
    {
        public static TModel WithId<TModel>(this TModel model, string id = null)
            where TModel : EntityModelBase
        {
            model.Id = id ?? Guid.NewGuid().ToString();
            return model;
        }
    }
}