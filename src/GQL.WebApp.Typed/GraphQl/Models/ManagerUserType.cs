using GQL.DAL.Models;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public class ManagerUserType : UserTypeBase<ManagerUserModel>
    {
        public ManagerUserType()
        {
            Name = "Manager";

            Field(x => x.NumberOfSales, nullable: false);

            Interface<UserInterface>();
        }
    }
}