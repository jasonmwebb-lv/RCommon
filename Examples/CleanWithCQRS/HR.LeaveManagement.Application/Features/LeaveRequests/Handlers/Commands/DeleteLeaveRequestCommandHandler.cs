﻿using AutoMapper;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Domain;
using RCommon.Mediator.Subscribers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RCommon.Persistence;
using RCommon.Persistence.Crud;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class DeleteLeaveRequestCommandHandler : IAppRequestHandler<DeleteLeaveRequestCommand>
    {
        private readonly IGraphRepository<LeaveRequest> _leaveRequestRepository;

        public DeleteLeaveRequestCommandHandler(IGraphRepository<LeaveRequest> leaveRequestRepository)
        {
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveRequestRepository.DataStoreName = DataStoreNamesConst.LeaveManagement;
        }

        public async Task HandleAsync(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.FindAsync(request.Id);

            if (leaveRequest == null)
                throw new NotFoundException(nameof(LeaveRequest), request.Id);

            await _leaveRequestRepository.DeleteAsync(leaveRequest);
        }
    }
}
