﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommon.Mediator
{
    public interface IMediatorBuilder
    {
        IServiceCollection Services { get; }
    }
}
