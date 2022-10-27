﻿using Microsoft.Extensions.DependencyInjection;
using RCommon.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommon
{
    public static class PersistenceConfigurationExtensions
    {

        public static IObjectAccessConfiguration WithPersistence<T>(this IRCommonConfiguration config) where T: IObjectAccessConfiguration
        {
            var dataConfiguration = (T)Activator.CreateInstance(typeof(T), new object[] { config.Services });
            config = WithChangeTracking(config);
            return dataConfiguration;
        }

        public static IObjectAccessConfiguration WithPersistence<T>(this IRCommonConfiguration config, Action<T> actions) 
            where T : IObjectAccessConfiguration
        {
            
            var dataConfiguration = (T)Activator.CreateInstance(typeof(T), new object[] { config.Services });
            config = WithChangeTracking(config);
            actions(dataConfiguration);
            return dataConfiguration;
        }

        /// <summary>
        /// Right now we are always using change tracking due to requirements for publishing entity events and those events being
        /// somewhat tied to Change Tracking.
        /// </summary>
        /// <param name="config">Instance of <see cref="IRCommonConfiguration"/>passed in.</param>
        /// <returns>Updated instance of <see cref="IRCommonConfiguration"/>RCommon Configuration</returns>
        private static IRCommonConfiguration WithChangeTracking(this IRCommonConfiguration config)
        {
            config.Services.AddScoped<IChangeTracker, ChangeTracker>();
            return config;
        }



    }
}
