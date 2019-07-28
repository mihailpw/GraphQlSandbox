using GQL.Services.Infra;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    public abstract class EntityObjectBase
    {
        [GraphQlIdField(
            Description = "The identificator",
            IsRequired = true)]
        public string Id { get; set; }
    }
}