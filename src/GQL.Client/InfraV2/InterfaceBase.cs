namespace GQL.Client.InfraV2
{
    public abstract class InterfaceBase : TypeBase
    {
        protected InterfaceBase(string name) : base(name)
        {
        }


        protected void IncludeOnTypeField(TypeBase type)
        {
            Include(new OnTypeField(type));
        }


        // TODO remove comments
        //public TInterface OnType<T>(T type) where T : TypeBase, TInterface
        //{
        //    var thisType = GetType();
        //    if (type.GetType() == thisType)
        //    {
        //        throw new InvalidOperationException($"Should be provided delivered type of '{((INameProvider) this).Name}' (class name: {thisType.Name}).");
        //    }

        //    Include(new OnTypeField(type));

        //    return (TInterface) this;
        //}
    }
}