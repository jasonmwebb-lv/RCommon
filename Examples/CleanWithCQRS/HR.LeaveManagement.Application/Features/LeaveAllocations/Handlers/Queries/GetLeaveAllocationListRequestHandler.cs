﻿using AutoMapper;
using HR.LeaveManagement.Application.DTOs;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using RCommon.Mediator.Subscribers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Application.Constants;
using RCommon.Persistence;
using System.Collections;
using RCommon.Persistence.Crud;
using Microsoft.AspNetCore.Http;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationListRequestHandler : IAppRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
    {
        private readonly IGraphRepository<LeaveAllocation> _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public GetLeaveAllocationListRequestHandler(IGraphRepository<LeaveAllocation> leaveAllocationRepository,
             IMapper mapper,
             IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            this._leaveAllocationRepository.DataStoreName = DataStoreNamesConst.LeaveManagement;
            _mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._userService = userService;
        }

        public async Task<List<LeaveAllocationDto>> HandleAsync(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocations = new List<LeaveAllocation>();
            var allocations = new List<LeaveAllocationDto>();
            _leaveAllocationRepository.Include(x => x.LeaveType);
            if (request.IsLoggedInUser)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(
                    q => q.Type == CustomClaimTypes.Uid)?.Value;
                leaveAllocations = await _leaveAllocationRepository.FindAsync(x=>x.EmployeeId == userId) as List<LeaveAllocation>;

                var employee = await _userService.GetEmployee(userId);
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
                foreach (var alloc in allocations)
                {
                    alloc.Employee = employee;
                }
            }
            else
            {
                leaveAllocations = await _leaveAllocationRepository.FindAsync(x=>true) as List<LeaveAllocation>;
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
                foreach (var req in allocations)
                {
                    req.Employee = await _userService.GetEmployee(req.EmployeeId);
                }
            }

            return allocations;
        }
    }
}
