using GQL.DAL.Models;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public class ManagerUserType : UserTypeBase<ManagerUserModel>
    {
        public ManagerUserType()
        {
            Name = "Manager";
            Description = "Manager user type";

            Field(x => x.NumberOfSales, nullable: false).Description("Number of sales");

            Interface<UserInterface>();
        }
    }
}