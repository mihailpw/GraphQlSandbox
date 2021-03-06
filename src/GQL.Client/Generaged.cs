﻿namespace GraphQlClient
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using GraphQlClient.Infra;

    #region Dtos

    public class UserInterfaceDto
    {
        public string Email { get; set; }
        public List<UserInterfaceDto> Friends { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
        public UserType? Type { get; set; }
        public bool? IsActive { get; set; }
        public int? NumberOfSales { get; set; }
    }

    public enum UserType
    {
        [EnumMember(Value = "GOOD_GUY")]
        GoodGuy,
        [EnumMember(Value = "BAD_GUY")]
        BadGuy,
        [EnumMember(Value = "NOBODY")]
        Nobody,
    }

    public class CustomerDto
    {
        public string Email { get; set; }
        public List<UserInterfaceDto> Friends { get; set; }
        public string Id { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
        public UserType? Type { get; set; }
    }

    public class ManagerDto
    {
        public string Email { get; set; }
        public List<UserInterfaceDto> Friends { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int? NumberOfSales { get; set; }
        public List<string> Roles { get; set; }
        public UserType? Type { get; set; }
    }

    public class ManagerInputDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class CustomerInputDto
    {
        public string Email { get; set; }
        public string Name { get; set; } = null;
        public List<string> Roles { get; set; } = null;
    }

    public class UsersQueryDto
    {
        public List<CustomerDto> Customers { get; set; }
        public UserInterfaceDto User { get; set; }
        public List<UserInterfaceDto> Users { get; set; }
        public int? UsersCount { get; set; }
    }

    public class UsersMutationDto
    {
        public List<CustomerDto> CreateCustomers { get; set; }
        public ManagerDto CreateManager { get; set; }
    }

    public class UsersSubscriptionDto
    {
        public UserInterfaceDto AddUser { get; set; }
    }

    #endregion

    #region Builders

    public class UserInterfaceBuilder : TypeBase
    {
        public UserInterfaceBuilder() : base("UserInterface")
        {
        }


        public UserInterfaceBuilder Email()
        {
            IncludeField(
                "email",
                new List<Argument>(0),
                null);
            return this;
        }

        public Func<Action<UserInterfaceBuilder>, UserInterfaceBuilder> Friends(
            string email = null)
        {
            IncludingField("friends");
            return __ =>
            {
                var _ = new UserInterfaceBuilder();
                __(_);
                IncludeField(
                    "friends",
                    new List<Argument>
                    {
                        new Argument("email", "String", email),
                    },
                    _);
                return this;
            };
        }

        public UserInterfaceBuilder Id()
        {
            IncludeField(
                "id",
                new List<Argument>(0),
                null);
            return this;
        }

        public UserInterfaceBuilder Name()
        {
            IncludeField(
                "name",
                new List<Argument>(0),
                null);
            return this;
        }

        public UserInterfaceBuilder Roles()
        {
            IncludeField(
                "roles",
                new List<Argument>(0),
                null);
            return this;
        }

        public UserInterfaceBuilder Type()
        {
            IncludeField(
                "type",
                new List<Argument>(0),
                null);
            return this;
        }

        public UserInterfaceBuilder OnCustomer(Action<CustomerBuilder> setupAction)
        {
            var type = new CustomerBuilder();
            setupAction(type);
            IncludeOnTypeField(type);
            return this;
        }

        public UserInterfaceBuilder OnManager(Action<ManagerBuilder> setupAction)
        {
            var type = new ManagerBuilder();
            setupAction(type);
            IncludeOnTypeField(type);
            return this;
        }
    }

    public class CustomerBuilder : TypeBase
    {
        public CustomerBuilder() : base("Customer")
        {
        }


        public CustomerBuilder Email()
        {
            IncludeField(
                "email",
                new List<Argument>(0),
                null);
            return this;
        }

        public Func<Action<UserInterfaceBuilder>, CustomerBuilder> Friends(
            string email = null)
        {
            IncludingField("friends");
            return __ =>
            {
                var _ = new UserInterfaceBuilder();
                __(_);
                IncludeField(
                    "friends",
                    new List<Argument>
                    {
                        new Argument("email", "String", email),
                    },
                    _);
                return this;
            };
        }

        public CustomerBuilder Id()
        {
            IncludeField(
                "id",
                new List<Argument>(0),
                null);
            return this;
        }

        public CustomerBuilder IsActive()
        {
            IncludeField(
                "isActive",
                new List<Argument>(0),
                null);
            return this;
        }

        public CustomerBuilder Name()
        {
            IncludeField(
                "name",
                new List<Argument>(0),
                null);
            return this;
        }

        public CustomerBuilder Roles()
        {
            IncludeField(
                "roles",
                new List<Argument>(0),
                null);
            return this;
        }

        public CustomerBuilder Type()
        {
            IncludeField(
                "type",
                new List<Argument>(0),
                null);
            return this;
        }
    }

    public class ManagerBuilder : TypeBase
    {
        public ManagerBuilder() : base("Manager")
        {
        }


        public ManagerBuilder Email()
        {
            IncludeField(
                "email",
                new List<Argument>(0),
                null);
            return this;
        }

        public Func<Action<UserInterfaceBuilder>, ManagerBuilder> Friends(
            string email = null)
        {
            IncludingField("friends");
            return __ =>
            {
                var _ = new UserInterfaceBuilder();
                __(_);
                IncludeField(
                    "friends",
                    new List<Argument>
                    {
                        new Argument("email", "String", email),
                    },
                    _);
                return this;
            };
        }

        public ManagerBuilder Id()
        {
            IncludeField(
                "id",
                new List<Argument>(0),
                null);
            return this;
        }

        public ManagerBuilder Name()
        {
            IncludeField(
                "name",
                new List<Argument>(0),
                null);
            return this;
        }

        public ManagerBuilder NumberOfSales()
        {
            IncludeField(
                "numberOfSales",
                new List<Argument>(0),
                null);
            return this;
        }

        public ManagerBuilder Roles()
        {
            IncludeField(
                "roles",
                new List<Argument>(0),
                null);
            return this;
        }

        public ManagerBuilder Type()
        {
            IncludeField(
                "type",
                new List<Argument>(0),
                null);
            return this;
        }
    }

    public class UsersQueryBuilder : TypeBase
    {
        public UsersQueryBuilder() : base("UsersQuery")
        {
        }


        public Func<Action<CustomerBuilder>, UsersQueryBuilder> Customers()
        {
            IncludingField("customers");
            return __ =>
            {
                var _ = new CustomerBuilder();
                __(_);
                IncludeField(
                    "customers",
                    new List<Argument>(0),
                    _);
                return this;
            };
        }

        public Func<Action<UserInterfaceBuilder>, UsersQueryBuilder> User(
            string id,
            UserType? type = null)
        {
            IncludingField("user");
            return __ =>
            {
                var _ = new UserInterfaceBuilder();
                __(_);
                IncludeField(
                    "user",
                    new List<Argument>
                    {
                        new Argument("id", "ID!", id),
                        new Argument("type", "UserType", type),
                    },
                    _);
                return this;
            };
        }

        public Func<Action<UserInterfaceBuilder>, UsersQueryBuilder> Users(
            UserType? type = null)
        {
            IncludingField("users");
            return __ =>
            {
                var _ = new UserInterfaceBuilder();
                __(_);
                IncludeField(
                    "users",
                    new List<Argument>
                    {
                        new Argument("type", "UserType", type),
                    },
                    _);
                return this;
            };
        }

        public UsersQueryBuilder UsersCount(
            string position = null)
        {
            IncludeField(
                "usersCount",
                new List<Argument>
                {
                    new Argument("position", "String", position),
                },
                null);
            return this;
        }
    }

    public class UsersMutationBuilder : TypeBase
    {
        public UsersMutationBuilder() : base("UsersMutation")
        {
        }


        public Func<Action<CustomerBuilder>, UsersMutationBuilder> CreateCustomers(
            List<CustomerInputDto> customers = null)
        {
            IncludingField("createCustomers");
            return __ =>
            {
                var _ = new CustomerBuilder();
                __(_);
                IncludeField(
                    "createCustomers",
                    new List<Argument>
                    {
                        new Argument("customers", "[CustomerInput]", customers),
                    },
                    _);
                return this;
            };
        }

        public Func<Action<ManagerBuilder>, UsersMutationBuilder> CreateManager(
            ManagerInputDto manager)
        {
            IncludingField("createManager");
            return __ =>
            {
                var _ = new ManagerBuilder();
                __(_);
                IncludeField(
                    "createManager",
                    new List<Argument>
                    {
                        new Argument("manager", "ManagerInput!", manager),
                    },
                    _);
                return this;
            };
        }
    }

    public class UsersSubscriptionBuilder : TypeBase
    {
        public UsersSubscriptionBuilder() : base("UsersSubscription")
        {
        }


        public Func<Action<UserInterfaceBuilder>, UsersSubscriptionBuilder> AddUser()
        {
            IncludingField("addUser");
            return __ =>
            {
                var _ = new UserInterfaceBuilder();
                __(_);
                IncludeField(
                    "addUser",
                    new List<Argument>(0),
                    _);
                return this;
            };
        }
    }

    #endregion

    public interface IAppClientFactory
    {
        IGraphQlQueryClient<UsersQueryDto> CreateQueryClient(Action<UsersQueryBuilder> setupAction);

        IGraphQlMutationClient<UsersMutationDto> CreateMutationClient(Action<UsersMutationBuilder> setupAction);

        IGraphQlSubscriptionClient<UsersSubscriptionDto> CreateSubscriptionClient(Action<UsersSubscriptionBuilder> setupAction);
    }


    public class AppClientFactory : ClientFactoryBase, IAppClientFactory
    {
        public AppClientFactory(string url, JsonSerializerSettings jsonSerializerSettings = null)
            : base(url, jsonSerializerSettings)
        {
        }


        public IGraphQlQueryClient<UsersQueryDto> CreateQueryClient(Action<UsersQueryBuilder> setupAction)
        {
            var type = new UsersQueryBuilder();
            setupAction(type);
            return CreateQueryClient<UsersQueryDto>(type);
        }

        public IGraphQlMutationClient<UsersMutationDto> CreateMutationClient(Action<UsersMutationBuilder> setupAction)
        {
            var type = new UsersMutationBuilder();
            setupAction(type);
            return CreateMutationClient<UsersMutationDto>(type);
        }

        public IGraphQlSubscriptionClient<UsersSubscriptionDto> CreateSubscriptionClient(Action<UsersSubscriptionBuilder> setupAction)
        {
            var type = new UsersSubscriptionBuilder();
            setupAction(type);
            return CreateSubscriptionClient<UsersSubscriptionDto>(type);
        }
    }
}

namespace GraphQlClient.Infra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQL.Client;
    using GraphQL.Client.Http;
    using GraphQL.Common.Request;
    using GraphQL.Common.Response;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;


    #region Client

    public interface IResponse<out T>
    {
        bool IsCompleted { get; }

        bool IsFailed { get; }

        T Data { get; }

        IReadOnlyList<Error> Errors { get; }
    }

    public interface ISubscription<T> : IDisposable
    {
        event EventHandler<IResponse<T>> Received;

        IResponse<T> LastResponse { get; }
    }

    public class Response<T> : IResponse<T>
    {
        private T _data;


        public bool IsCompleted { get; }

        public bool IsFailed => !IsCompleted;

        public T Data
        {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Can not get data from failed response.");
                }

                return _data;
            }
            protected set => _data = value;
        }

        public IReadOnlyList<Error> Errors { get; }


        private Response(T data)
        {
            _data = data;

            IsCompleted = true;
            Errors = Array.Empty<Error>();
        }

        private Response(IReadOnlyList<Error> errors)
        {
            Errors = errors;

            IsCompleted = false;
        }


        public static Response<T> CreateFrom(GraphQLResponse graphQlResponse)
        {
            if (graphQlResponse == null)
            {
                return null;
            }

            if (graphQlResponse.Errors != null && graphQlResponse.Errors.Length > 0)
            {
                var errors = graphQlResponse.Errors.Select(
                        e => new Error(
                            e.Message,
                            e.Locations?.Select(l => new Error.Location(l.Column, l.Line)).ToList(),
                            e.AdditionalEntries?.ToDictionary(p => p.Key, p => (object)p.Value)))
                    .ToList();

                return new Response<T>(errors);
            }
            else
            {
                var jData = (JToken)graphQlResponse.Data;
                var dto = jData.ToObject<T>();

                return new Response<T>(dto);
            }
        }
    }

    public class Subscription<T> : ISubscription<T>
    {
        private readonly IGraphQLSubscriptionResult _graphQlSubscriptionResult;


        public event EventHandler<IResponse<T>> Received;


        public IResponse<T> LastResponse { get; }


        public Subscription(IGraphQLSubscriptionResult graphQlSubscriptionResult)
        {
            _graphQlSubscriptionResult = graphQlSubscriptionResult;
            _graphQlSubscriptionResult.OnReceive += r => Received?.Invoke(this, Response<T>.CreateFrom(r));
            LastResponse = Response<T>.CreateFrom(graphQlSubscriptionResult.LastResponse);
        }


        public void Dispose()
        {
            _graphQlSubscriptionResult?.Dispose();
        }
    }

    public class Error
    {
        public string Message { get; }

        public IReadOnlyList<Location> Locations { get; set; }

        public IReadOnlyDictionary<string, object> AdditionalEntries { get; set; }


        public Error(
            string message,
            IReadOnlyList<Location> locations,
            IReadOnlyDictionary<string, object> additionalEntries)
        {
            Message = message;
            Locations = locations;
            AdditionalEntries = additionalEntries;
        }



        public class Location
        {
            public uint Column { get; }

            public uint Line { get; }


            public Location(uint column, uint line)
            {
                Column = column;
                Line = line;
            }
        }
    }

    public interface IGraphQlQueryClient : IDisposable
    {
        Task<IResponse<TDto>> SendAsync<TDto>(CancellationToken cancellationToken = default);
    }

    public interface IGraphQlQueryClient<TDto> : IGraphQlQueryClient
    {
        Task<IResponse<TDto>> SendAsync(CancellationToken cancellationToken = default);
    }

    public interface IGraphQlMutationClient : IDisposable
    {
        Task<IResponse<TDto>> SendAsync<TDto>(CancellationToken cancellationToken = default);
    }

    public interface IGraphQlMutationClient<TDto> : IGraphQlMutationClient
    {
        Task<IResponse<TDto>> SendAsync(CancellationToken cancellationToken = default);
    }

    public interface IGraphQlSubscriptionClient : IDisposable
    {
        Task<ISubscription<TDto>> SendAsync<TDto>(CancellationToken cancellationToken = default);
    }

    public interface IGraphQlSubscriptionClient<TDto> : IGraphQlSubscriptionClient
    {
        Task<ISubscription<TDto>> SendAsync(CancellationToken cancellationToken = default);
    }

    public abstract class GraphQlClientBase : IDisposable
    {
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings;


        protected GraphQLHttpClient Client { get; }

        protected GraphQLRequest Request { get; }


        static GraphQlClientBase()
        {
            DefaultJsonSerializerSettings = new JsonSerializerSettings();
            DefaultJsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }


        protected GraphQlClientBase(
            string url,
            string query,
            Dictionary<string, object> variables,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            Client = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(url),
                JsonSerializerSettings = jsonSerializerSettings ?? DefaultJsonSerializerSettings,
            });
            Request = new GraphQLRequest
            {
                Query = query,
                Variables = variables,
            };
        }


        public void Dispose()
        {
            Client.Dispose();
        }
    }

    public class GraphQlQueryClient<TDto> : GraphQlClientBase, IGraphQlQueryClient<TDto>
    {
        public GraphQlQueryClient(
            string url,
            string query,
            Dictionary<string, object> variables,
            JsonSerializerSettings jsonSerializerSettings = null)
            : base(url, query, variables, jsonSerializerSettings)
        {
        }


        public Task<IResponse<TDto>> SendAsync(CancellationToken cancellationToken)
        {
            return SendAsync<TDto>(cancellationToken);
        }

        public async Task<IResponse<T>> SendAsync<T>(CancellationToken cancellationToken)
        {
            var graphQlResponse = await Client.SendQueryAsync(Request, cancellationToken);
            var response = Response<T>.CreateFrom(graphQlResponse);

            return response;
        }
    }

    public class GraphQlMutationClient<TDto> : GraphQlClientBase, IGraphQlMutationClient<TDto>
    {
        public GraphQlMutationClient(
            string url,
            string query,
            Dictionary<string, object> variables,
            JsonSerializerSettings jsonSerializerSettings = null)
            : base(url, query, variables, jsonSerializerSettings)
        {
        }


        public Task<IResponse<TDto>> SendAsync(CancellationToken cancellationToken)
        {
            return SendAsync<TDto>(cancellationToken);
        }

        public async Task<IResponse<T>> SendAsync<T>(CancellationToken cancellationToken)
        {
            var graphQlResponse = await Client.SendMutationAsync(Request, cancellationToken);
            var response = Response<T>.CreateFrom(graphQlResponse);

            return response;
        }
    }

    public class GraphQlSubscriptionClient<TDto> : GraphQlClientBase, IGraphQlSubscriptionClient<TDto>
    {
        public GraphQlSubscriptionClient(
            string url,
            string query,
            Dictionary<string, object> variables,
            JsonSerializerSettings jsonSerializerSettings = null)
            : base(url, query, variables, jsonSerializerSettings)
        {
        }

        public Task<ISubscription<TDto>> SendAsync(CancellationToken cancellationToken)
        {
            return SendAsync<TDto>(cancellationToken);
        }

        public async Task<ISubscription<T>> SendAsync<T>(CancellationToken cancellationToken)
        {
            var graphQlSubscriptionResult = await Client.SendSubscribeAsync(Request, cancellationToken);
            var subscription = new Subscription<T>(graphQlSubscriptionResult);

            return subscription;
        }
    }

    public abstract class ClientFactoryBase
    {
        private readonly string _url;
        private readonly JsonSerializerSettings _jsonSerializerSettings;


        protected ClientFactoryBase(string url, JsonSerializerSettings jsonSerializerSettings = null)
        {
            _url = url;
            _jsonSerializerSettings = jsonSerializerSettings;
        }


        protected IGraphQlQueryClient<TDto> CreateQueryClient<TDto>(TypeBase type)
        {
            var (requestQuery, variables) = PrepareRequestData("query", type);
            return new GraphQlQueryClient<TDto>(_url, requestQuery, variables, _jsonSerializerSettings);
        }

        protected IGraphQlMutationClient<TDto> CreateMutationClient<TDto>(TypeBase type)
        {
            var (requestQuery, variables) = PrepareRequestData("mutation", type);
            return new GraphQlMutationClient<TDto>(_url, requestQuery, variables, _jsonSerializerSettings);
        }

        protected IGraphQlSubscriptionClient<TDto> CreateSubscriptionClient<TDto>(TypeBase type)
        {
            var (requestQuery, variables) = PrepareRequestData("subscription", type);
            return new GraphQlSubscriptionClient<TDto>(_url, requestQuery, variables, _jsonSerializerSettings);
        }


        private static (string requestQuery, Dictionary<string, object> variables) PrepareRequestData(string requestType, TypeBase type)
        {
            ((ITypeValidator)type).ThrowIfNotValid();

            var arguments = ((IArgumentsProvider)type).GetArguments().ToList();
            var requestQuery = BuildQuery(requestType, type, arguments);
            var variables = arguments.ToDictionary(a => a.Id, a => a.Value);

            return (requestQuery, variables);
        }

        private static string BuildQuery(string requestType, IRequestBuilder requestBuilder, IReadOnlyCollection<Argument> arguments)
        {
            var stringBuilder = new StringBuilder(requestType);

            if (arguments.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var argument in arguments)
                {
                    stringBuilder.Append($"${argument.Id}:{argument.Type},");
                }
                stringBuilder.Length--;
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            requestBuilder.AppendRequest(stringBuilder);
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }

    #endregion

    #region Providers

    public interface IArgumentsProvider
    {
        IEnumerable<Argument> GetArguments();
    }

    public interface INameProvider
    {
        string Name { get; }
    }

    public interface IRequestBuilder
    {
        void AppendRequest(StringBuilder builder);
    }

    public interface ITypeValidator
    {
        void ThrowIfNotValid();
    }

    #endregion

    #region Fields

    public sealed class Argument
    {
        public string Id { get; }
        public string Name { get; }
        public string Type { get; }
        public object Value { get; }


        public Argument(string name, string type, object value)
        {
            Name = name;
            Type = type;
            Value = value;

            Id = $"{Name}_{Guid.NewGuid():N}";
        }
    }

    public class Field : FieldBase
    {
        private readonly List<Argument> _arguments;


        public Field(string key, List<Argument> arguments, TypeBase type)
            : base(key, type)
        {
            _arguments = arguments;
        }


        protected sealed override void AppendArguments(StringBuilder builder)
        {
            if (_arguments.Count > 0)
            {
                builder.Append("(");
                foreach (var argument in _arguments)
                {
                    builder.Append($"{argument.Name}:${argument.Id},");
                }
                builder.Length--;
                builder.Append(")");
            }
        }

        protected override IEnumerable<Argument> GetArguments()
        {
            foreach (var argument in base.GetArguments())
            {
                yield return argument;
            }

            foreach (var argument in _arguments)
            {
                yield return argument;
            }
        }
    }

    public class OnTypeField : FieldBase
    {
        public OnTypeField(TypeBase type)
            : base($"... on {((INameProvider)type).Name}", type)
        {
        }

        protected override void AppendArguments(StringBuilder builder)
        {
        }
    }

    public abstract class FieldBase : IRequestBuilder, IArgumentsProvider, ITypeValidator
    {
        private readonly string _key;


        protected TypeBase Type { get; }


        protected FieldBase(string key, TypeBase type)
        {
            _key = key;
            Type = type;
        }


        void IRequestBuilder.AppendRequest(StringBuilder builder)
        {
            builder.Append(_key);

            AppendArguments(builder);

            if (Type is IRequestBuilder typeRequestBuilder)
            {
                builder.Append("{");
                typeRequestBuilder.AppendRequest(builder);
                builder.Append("}");
            }
        }

        IEnumerable<Argument> IArgumentsProvider.GetArguments()
        {
            return GetArguments();
        }

        void ITypeValidator.ThrowIfNotValid()
        {
            ((ITypeValidator)Type)?.ThrowIfNotValid();
        }


        protected abstract void AppendArguments(StringBuilder builder);

        protected virtual IEnumerable<Argument> GetArguments()
        {
            return Type != null
                ? ((IArgumentsProvider)Type).GetArguments()
                : Enumerable.Empty<Argument>();
        }
    }

    #endregion

    #region Types

    public abstract class TypeBase : IRequestBuilder, IArgumentsProvider, INameProvider, ITypeValidator
    {
        private readonly string _name;
        private readonly List<FieldBase> _fields;
        private readonly List<string> _includingFieldNames;


        string INameProvider.Name => _name;


        protected TypeBase(string name)
        {
            _name = name;
            _fields = new List<FieldBase>();
            _includingFieldNames = new List<string>();
        }


        void IRequestBuilder.AppendRequest(StringBuilder builder)
        {
            foreach (var field in _fields)
            {
                var requestBuilder = (IRequestBuilder)field;
                if (requestBuilder != null)
                {
                    requestBuilder.AppendRequest(builder);
                    builder.Append(" ");
                }
            }
        }

        IEnumerable<Argument> IArgumentsProvider.GetArguments()
        {
            foreach (var field in _fields)
            {
                var argumentsProvider = (IArgumentsProvider)field;
                if (argumentsProvider != null)
                {
                    foreach (var argument in argumentsProvider.GetArguments())
                    {
                        yield return argument;
                    }
                }
            }
        }

        void ITypeValidator.ThrowIfNotValid()
        {
            if (_includingFieldNames.Count > 0)
            {
                throw new InvalidOperationException($"Following fields from '{GetType().Name}' not setup: {string.Join(", ", _includingFieldNames)}");
            }

            foreach (var field in _fields)
            {
                if (field is ITypeValidator typeValidator)
                {
                    typeValidator.ThrowIfNotValid();
                }
            }
        }


        protected void IncludingField(string fieldName)
        {
            _includingFieldNames.Add(fieldName);
        }

        protected void IncludeField(string fieldName, List<Argument> arguments, TypeBase type)
        {
            _includingFieldNames.Remove(fieldName);
            _fields.Add(new Field(fieldName, arguments, type));
        }

        protected void IncludeOnTypeField(TypeBase type)
        {
            _fields.Add(new OnTypeField(type));
        }
    }

    #endregion

}
