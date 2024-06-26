#region license
//Copyright 2010 Ritesh Rao 

//Licensed under the Apache License, Version 2.0 (the "License"); 
//you may not use this file except in compliance with the License. 
//You may obtain a copy of the License at 

//http://www.apache.org/licenses/LICENSE-2.0 

//Unless required by applicable law or agreed to in writing, software 
//distributed under the License is distributed on an "AS IS" BASIS, 
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
//See the License for the specific language governing permissions and 
//limitations under the License. 
#endregion
#region license compliance
//Substantial changes to the original code have been made in the form of namespace reorganization, 
//and method signature.
//Original code here: https://github.com/riteshrao/ncommon/blob/v1.2/NCommon/src/Data/Impl/TransactionScopeHelper.cs
#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Transactions;

namespace RCommon.Persistence.Transactions
{
    /// <summary>
    /// Helper class to create <see cref="TransactionScope"/> instances.
    /// </summary>
    public static class TransactionScopeHelper
    {

        public static TransactionScope CreateScope(ILogger<UnitOfWork> logger, IUnitOfWork unitOfWork)
        {
            if (unitOfWork.TransactionMode == TransactionMode.New)
            {
                logger.LogDebug("Creating a new TransactionScope with TransactionScopeOption.RequiresNew");
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = unitOfWork.IsolationLevel }, TransactionScopeAsyncFlowOption.Enabled);
            }
            if (unitOfWork.TransactionMode == TransactionMode.Supress)
            {
                logger.LogDebug("Creating a new TransactionScope with TransactionScopeOption.Supress");
                return new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            }
            logger.LogDebug("Creating a new TransactionScope with TransactionScopeOption.Required");
            return new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
