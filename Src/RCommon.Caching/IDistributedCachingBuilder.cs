﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommon.Caching
{
    public interface IDistributedCachingBuilder
    {
        IServiceCollection Services { get; }
    }
}
