using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public abstract class EntityBaseType<T> : ObjectGraphType<T>
        where T : EntityModelBase
    {
        protected EntityBaseType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The identificator");
        }
    }

    public abstract class InterfaceBaseType<T> : InterfaceGraphType<T>
        where T : EntityModelBase
    {
        protected InterfaceBaseType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The identificator");
        }
    }
}