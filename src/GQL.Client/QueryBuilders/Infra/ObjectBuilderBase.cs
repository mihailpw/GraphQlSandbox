using System.Collections.Generic;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class ObjectBuilderBase : BuilderBase
    {
        protected readonly List<BuilderBase> Includes;


        protected ObjectBuilderBase()
        {
            Includes = new List<BuilderBase>();
        }


        public override IEnumerable<ArgumentData> EnumerateArguments()
        {
            foreach (var builder in Includes)
            {
                foreach (var argument in builder.EnumerateArguments())
                {
                    yield return argument;
                }
            }
        }


        protected void Include(string fieldName, bool include = true)
        {
            Include(new FieldBuilder(fieldName), include);
        }

        protected void Include(BuilderBase builder, bool include = true)
        {
            if (!include)
            {
                return;
            }

            builder.ThrowIfNotValid();
            Includes.Add(builder);
        }
    }
}