namespace GQL.Services.Infra.Core
{
    internal class Config : IConfig
    {
        private readonly bool _isDebug;

        public bool ThrowIfPropertyNotFound => _isDebug;
        public bool ThrowIfPropertiesTypesDifferent => _isDebug;


        public Config()
        {
#if DEBUG
            _isDebug = true;
#endif
        }

    }
}