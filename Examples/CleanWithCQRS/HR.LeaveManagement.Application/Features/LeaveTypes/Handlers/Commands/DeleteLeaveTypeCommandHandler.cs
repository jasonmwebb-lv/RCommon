﻿using AutoMapper;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Domain;
using RCommon.Mediator.Subscribers;
using RCommon.Persistence;
using RCommon.Persistence.Crud;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IAppRequestHandler<DeleteLeaveTypeCommand>
    {
        private readonly IMapper _mapper;
        private readonly IGraphRepository<LeaveType> _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(IMapper mapper, IGraphRepository<LeaveType> leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
            this._leaveTypeRepository.DataStoreName = DataStoreNamesConst.LeaveManagement;
        }

        public async Task HandleAsync(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepository.FindAsync(request.Id);

            if (leaveType == null)
                throw new NotFoundException(nameof(LeaveType), request.Id);

            await _leaveTypeRepository.DeleteAsync(leaveType);
        }
    }
}
