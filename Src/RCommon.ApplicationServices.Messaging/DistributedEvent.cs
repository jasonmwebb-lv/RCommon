﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommon.ApplicationServices.Messaging
{
    public record DistributedEvent : IDistributedEvent
    {
        public DistributedEvent()
        {

        }
    }
}