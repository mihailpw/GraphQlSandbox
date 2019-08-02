namespace GQL.Services.Infra.Common
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