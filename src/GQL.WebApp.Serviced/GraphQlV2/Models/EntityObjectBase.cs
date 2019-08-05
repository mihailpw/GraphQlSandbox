using GraphQl.Server.Annotations;
using GraphQl.Server.Annotations.Attributes;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    public abstract class EntityObjectBase
    {
        [GraphQlField(Description = "The identificator")]
        public Id<string> Id { get; set; }
    }
}