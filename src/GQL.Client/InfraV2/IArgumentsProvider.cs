using System.Collections.Generic;

namespace GQL.Client.InfraV2
{
    public interface IArgumentsProvider
    {
        IEnumerable<Argument> GetArguments();
    }
}