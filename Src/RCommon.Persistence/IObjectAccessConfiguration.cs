using System;

namespace RCommon
{
    /// <summary>
    /// Base interface implemented by specific data configurators that configure RCommon data providers.
    /// </summary>
    public interface IObjectAccessConfiguration
    {
        IObjectAccessConfiguration SetDefaultDataStore(Action<DefaultDataStoreOptions> options);
    }
}
