#region base classes

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

public class FieldMetadata
{
    public string Name { get; set; }
    public bool IsComplex { get; set; }
    public Type QueryBuilderType { get; set; }
}

public enum Formatting
{
    None,
    Indented
}

internal static class GraphQlQueryHelper
{
    public static string GetIndentation(int level, byte indentationSize)
    {
        return new String(' ', level * indentationSize);
    }

    public static string BuildArgumentValue(object value, Formatting formatting, int level, byte indentationSize)
    {
        if (value is Enum @enum)
            return ConvertEnumToString(@enum);

        if (value is bool @bool)
            return @bool ? "true" : "false";

        if (value is IGraphQlInputObject inputObject)
            return BuildInputObject(inputObject, formatting, level + 2, indentationSize);

        var argumentValue = Convert.ToString(value, CultureInfo.InvariantCulture);
        if (value is String || value is Guid)
            return $"\"{argumentValue}\"";

        if (value is IEnumerable enumerable)
        {
            var builder = new StringBuilder();
            builder.Append("[");
            var delimiter = String.Empty;
            foreach (var item in enumerable)
            {
                builder.Append(delimiter);

                if (formatting == Formatting.Indented)
                {
                    builder.AppendLine();
                    builder.Append(GetIndentation(level + 1, indentationSize));
                }

                builder.Append(BuildArgumentValue(item, formatting, level, indentationSize));
                delimiter = ",";
            }

            builder.Append("]");
            return builder.ToString();
        }

        return argumentValue;
    }

    public static string BuildInputObject(IGraphQlInputObject inputObject, Formatting formatting, int level, byte indentationSize)
    {
        var builder = new StringBuilder();
        builder.Append("{");

        var isIndentedFormatting = formatting == Formatting.Indented;
        string valueSeparator;
        if (isIndentedFormatting)
        {
            builder.AppendLine();
            valueSeparator = ": ";
        }
        else
            valueSeparator = ":";

        var separator = String.Empty;
        foreach (var propertyValue in inputObject.GetPropertyValues().Where(p => p.Value != null))
        {
            var value = BuildArgumentValue(propertyValue.Value, formatting, level, indentationSize);
            builder.Append(isIndentedFormatting ? GetIndentation(level, indentationSize) : separator);
            builder.Append(propertyValue.Name);
            builder.Append(valueSeparator);
            builder.Append(value);

            separator = ",";

            if (isIndentedFormatting)
                builder.AppendLine();
        }

        if (isIndentedFormatting)
            builder.Append(GetIndentation(level - 1, indentationSize));

        builder.Append("}");

        return builder.ToString();
    }

    private static string ConvertEnumToString(Enum @enum)
    {
        var enumMember = @enum.GetType().GetTypeInfo().GetField(@enum.ToString());
        if (enumMember == null)
            throw new InvalidOperationException("enum member resolution failed");

        var enumMemberAttribute = (EnumMemberAttribute)enumMember.GetCustomAttribute(typeof(EnumMemberAttribute));

        return enumMemberAttribute == null
            ? @enum.ToString()
            : enumMemberAttribute.Value;
    }
}

public struct InputPropertyInfo
{
    public string Name { get; set; }
    public object Value { get; set; }
}

internal interface IGraphQlInputObject
{
    IEnumerable<InputPropertyInfo> GetPropertyValues();
}

public abstract class GraphQlQueryBuilder
{
    private readonly Dictionary<string, GraphQlFieldCriteria> _fieldCriteria = new Dictionary<string, GraphQlFieldCriteria>();

    protected virtual string Prefix { get { return null; } }

    protected abstract IList<FieldMetadata> AllFields { get; }

    public void Clear()
    {
        _fieldCriteria.Clear();
    }

    public void IncludeAllFields()
    {
        IncludeFields(AllFields);
    }

    public string Build(Formatting formatting = Formatting.None, byte indentationSize = 2)
    {
        return Build(formatting, 1, indentationSize);
    }

    protected string Build(Formatting formatting, int level, byte indentationSize)
    {
        var isIndentedFormatting = formatting == Formatting.Indented;

        var builder = new StringBuilder();

        if (!String.IsNullOrEmpty(Prefix))
        {
            builder.Append(Prefix);

            if (isIndentedFormatting)
                builder.Append(" ");
        }

        builder.Append("{");

        if (isIndentedFormatting)
            builder.AppendLine();

        var separator = String.Empty;
        foreach (var criteria in _fieldCriteria.Values)
        {
            var fieldCriteria = criteria.Build(formatting, level, indentationSize);
            if (isIndentedFormatting)
                builder.AppendLine(fieldCriteria);
            else if (!String.IsNullOrEmpty(fieldCriteria))
            {
                builder.Append(separator);
                builder.Append(fieldCriteria);
            }

            separator = ",";
        }

        if (isIndentedFormatting)
            builder.Append(GraphQlQueryHelper.GetIndentation(level - 1, indentationSize));

        builder.Append("}");
        return builder.ToString();
    }

    protected void IncludeScalarField(string fieldName, IDictionary<string, object> args)
    {
        _fieldCriteria[fieldName] = new GraphQlScalarFieldCriteria(fieldName, args);
    }

    protected void IncludeObjectField(string fieldName, GraphQlQueryBuilder objectFieldQueryBuilder, IDictionary<string, object> args)
    {
        _fieldCriteria[fieldName] = new GraphQlObjectFieldCriteria(fieldName, objectFieldQueryBuilder, args);
    }

    protected void IncludeFields(IEnumerable<FieldMetadata> fields)
    {
        foreach (var field in fields)
        {
            if (field.QueryBuilderType == null)
                IncludeScalarField(field.Name, null);
            else
            {
                var queryBuilder = (GraphQlQueryBuilder)Activator.CreateInstance(field.QueryBuilderType);
                queryBuilder.IncludeAllFields();
                IncludeObjectField(field.Name, queryBuilder, null);
            }
        }
    }

    private abstract class GraphQlFieldCriteria
    {
        protected readonly string FieldName;
        private readonly IDictionary<string, object> _args;

        protected GraphQlFieldCriteria(string fieldName, IDictionary<string, object> args)
        {
            FieldName = fieldName;
            _args = args;
        }

        public abstract string Build(Formatting formatting, int level, byte indentationSize);

        protected string BuildArgumentClause(Formatting formatting, int level, byte indentationSize)
        {
            var separator = formatting == Formatting.Indented ? " " : null;
            return
                _args?.Count > 0
                    ? $"({String.Join($",{separator}", _args.Select(kvp => $"{kvp.Key}:{separator}{GraphQlQueryHelper.BuildArgumentValue(kvp.Value, formatting, level, indentationSize)}"))}){separator}"
                    : String.Empty;
        }
    }

    private class GraphQlScalarFieldCriteria : GraphQlFieldCriteria
    {
        public GraphQlScalarFieldCriteria(string fieldName, IDictionary<string, object> args) : base(fieldName, args)
        {
        }

        public override string Build(Formatting formatting, int level, byte indentationSize)
        {
            var builder = new StringBuilder();
            if (formatting == Formatting.Indented)
                builder.Append(GraphQlQueryHelper.GetIndentation(level, indentationSize));

            builder.Append(FieldName);
            builder.Append(BuildArgumentClause(formatting, level, indentationSize));
            return builder.ToString();
        }
    }

    private class GraphQlObjectFieldCriteria : GraphQlFieldCriteria
    {
        private readonly GraphQlQueryBuilder _objectQueryBuilder;

        public GraphQlObjectFieldCriteria(string fieldName, GraphQlQueryBuilder objectQueryBuilder, IDictionary<string, object> args) : base(fieldName, args)
        {
            _objectQueryBuilder = objectQueryBuilder;
        }

        public override string Build(Formatting formatting, int level, byte indentationSize)
        {
            if (_objectQueryBuilder._fieldCriteria.Count == 0)
                return String.Empty;

            var builder = new StringBuilder();
            var fieldName = FieldName;
            if (formatting == Formatting.Indented)
                fieldName = $"{GraphQlQueryHelper.GetIndentation(level, indentationSize)}{FieldName} ";

            builder.Append(fieldName);
            builder.Append(BuildArgumentClause(formatting, level, indentationSize));
            builder.Append(_objectQueryBuilder.Build(formatting, level + 1, indentationSize));
            return builder.ToString();
        }
    }
}

public abstract class GraphQlQueryBuilder<TQueryBuilder> : GraphQlQueryBuilder where TQueryBuilder : GraphQlQueryBuilder<TQueryBuilder>
{
    public TQueryBuilder WithAllFields()
    {
        IncludeAllFields();
        return (TQueryBuilder)this;
    }

    public TQueryBuilder WithAllScalarFields()
    {
        IncludeFields(AllFields.Where(f => !f.IsComplex));
        return (TQueryBuilder)this;
    }

    protected TQueryBuilder WithScalarField(string fieldName, IDictionary<string, object> args = null)
    {
        IncludeScalarField(fieldName, args);
        return (TQueryBuilder)this;
    }

    protected TQueryBuilder WithObjectField(string fieldName, GraphQlQueryBuilder queryBuilder, IDictionary<string, object> args = null)
    {
        IncludeObjectField(fieldName, queryBuilder, args);
        return (TQueryBuilder)this;
    }
}
#endregion

#region shared types
#endregion

#region builder classes
public class UsersQueryQueryBuilder : GraphQlQueryBuilder<UsersQueryQueryBuilder>
{
    private static readonly FieldMetadata[] AllFieldMetadata =
        new[]
        {
            new FieldMetadata { Name = "customers", IsComplex = true, QueryBuilderType = typeof(CustomerQueryBuilder) },
            new FieldMetadata { Name = "user" },
            new FieldMetadata { Name = "users", IsComplex = true, QueryBuilderType = typeof(UserInterfaceQueryBuilder) }
        };

    protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

    public UsersQueryQueryBuilder WithCustomers(CustomerQueryBuilder customerQueryBuilder)
    {
        return WithObjectField("customers", customerQueryBuilder);
    }

    public UsersQueryQueryBuilder WithUser(UserInterfaceQueryBuilder userInterfaceQueryBuilder, Guid id)
    {
        var args = new Dictionary<string, object>();
        args.Add("id", id);
        return WithObjectField("user", userInterfaceQueryBuilder, args);
    }

    public UsersQueryQueryBuilder WithUsers(UserInterfaceQueryBuilder userInterfaceQueryBuilder)
    {
        return WithObjectField("users", userInterfaceQueryBuilder);
    }
}

public class CustomerQueryBuilder : GraphQlQueryBuilder<CustomerQueryBuilder>
{
    private static readonly FieldMetadata[] AllFieldMetadata =
        new[]
        {
            new FieldMetadata { Name = "email" },
            new FieldMetadata { Name = "friends", IsComplex = true, QueryBuilderType = typeof(UserInterfaceQueryBuilder) },
            new FieldMetadata { Name = "id" },
            new FieldMetadata { Name = "isActive" },
            new FieldMetadata { Name = "name" },
            new FieldMetadata { Name = "roles", IsComplex = true }
        };

    protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

    public CustomerQueryBuilder WithEmail()
    {
        return WithScalarField("email");
    }

    public CustomerQueryBuilder WithFriends(UserInterfaceQueryBuilder userInterfaceQueryBuilder, string email = null)
    {
        var args = new Dictionary<string, object>();
        if (email != null)
            args.Add("email", email);

        return WithObjectField("friends", userInterfaceQueryBuilder, args);
    }

    public CustomerQueryBuilder WithId()
    {
        return WithScalarField("id");
    }

    public CustomerQueryBuilder WithIsActive()
    {
        return WithScalarField("isActive");
    }

    public CustomerQueryBuilder WithName()
    {
        return WithScalarField("name");
    }

    public CustomerQueryBuilder WithRoles()
    {
        return WithScalarField("roles");
    }
}

public class UsersMutationQueryBuilder : GraphQlQueryBuilder<UsersMutationQueryBuilder>
{
    private static readonly FieldMetadata[] AllFieldMetadata =
        new[]
        {
            new FieldMetadata { Name = "createUser" },
            new FieldMetadata { Name = "createUsers" }
        };

    protected override string Prefix { get { return "mutation"; } }

    protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

    public UsersMutationQueryBuilder WithCreateUser(UserInterfaceQueryBuilder userInterfaceQueryBuilder, UserInput user)
    {
        var args = new Dictionary<string, object>();
        args.Add("user", user);
        return WithObjectField("createUser", userInterfaceQueryBuilder, args);
    }

    public UsersMutationQueryBuilder WithCreateUsers(UserInterfaceQueryBuilder userInterfaceQueryBuilder, IEnumerable<UserInput> users)
    {
        var args = new Dictionary<string, object>();
        args.Add("users", users);
        return WithObjectField("createUsers", userInterfaceQueryBuilder, args);
    }
}

public class UsersSubscriptionQueryBuilder : GraphQlQueryBuilder<UsersSubscriptionQueryBuilder>
{
    private static readonly FieldMetadata[] AllFieldMetadata =
        new[]
        {
            new FieldMetadata { Name = "addUser" }
        };

    protected override string Prefix { get { return "subscription"; } }

    protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

    public UsersSubscriptionQueryBuilder WithAddUser(UserInterfaceQueryBuilder userInterfaceQueryBuilder)
    {
        return WithObjectField("addUser", userInterfaceQueryBuilder);
    }
}

public class ManagerQueryBuilder : GraphQlQueryBuilder<ManagerQueryBuilder>
{
    private static readonly FieldMetadata[] AllFieldMetadata =
        new[]
        {
            new FieldMetadata { Name = "email" },
            new FieldMetadata { Name = "friends", IsComplex = true, QueryBuilderType = typeof(UserInterfaceQueryBuilder) },
            new FieldMetadata { Name = "id" },
            new FieldMetadata { Name = "name" },
            new FieldMetadata { Name = "numberOfSales" },
            new FieldMetadata { Name = "roles", IsComplex = true }
        };

    protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

    public ManagerQueryBuilder WithEmail()
    {
        return WithScalarField("email");
    }

    public ManagerQueryBuilder WithFriends(UserInterfaceQueryBuilder userInterfaceQueryBuilder, string email = null)
    {
        var args = new Dictionary<string, object>();
        if (email != null)
            args.Add("email", email);

        return WithObjectField("friends", userInterfaceQueryBuilder, args);
    }

    public ManagerQueryBuilder WithId()
    {
        return WithScalarField("id");
    }

    public ManagerQueryBuilder WithName()
    {
        return WithScalarField("name");
    }

    public ManagerQueryBuilder WithNumberOfSales()
    {
        return WithScalarField("numberOfSales");
    }

    public ManagerQueryBuilder WithRoles()
    {
        return WithScalarField("roles");
    }
}
#endregion

#region input classes
public class UserInput : IGraphQlInputObject
{
    public string Name { get; set; }
    public string Email { get; set; }

    IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
    {
        yield return new InputPropertyInfo { Name = "name", Value = Name };
        yield return new InputPropertyInfo { Name = "email", Value = Email };
    }
}
#endregion

#region data classes
public class UsersQuery
{
    public ICollection<Customer> Customers { get; set; }
    public string User { get; set; }
    public ICollection<UserInterface> Users { get; set; }
}

public class Customer
{
    public string Email { get; set; }
    public ICollection<UserInterface> Friends { get; set; }
    public Guid? Id { get; set; }
    public bool? IsActive { get; set; }
    public string Name { get; set; }
    public ICollection<string> Roles { get; set; }
}

public class UsersMutation
{
    public string CreateUser { get; set; }
    public string CreateUsers { get; set; }
}

public class UsersSubscription
{
    public string AddUser { get; set; }
}

public class Manager
{
    public string Email { get; set; }
    public ICollection<UserInterface> Friends { get; set; }
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int? NumberOfSales { get; set; }
    public ICollection<string> Roles { get; set; }
}
#endregion
