﻿using AutoMapper;
using HR.LeaveManagement.Application.DTOs;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using RCommon.Mediator.Subscribers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using HR.LeaveManagement.Application.Constants;
using RCommon.Persistence;
using RCommon.Persistence.Crud;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListRequestHandler : IAppRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDto>>
    {
        private readonly IGraphRepository<LeaveRequest> _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public GetLeaveRequestListRequestHandler(IGraphRepository<LeaveRequest> leaveRequestRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _leaveRequestRepository = leaveRequestRepository;
            this._leaveRequestRepository.DataStoreName = DataStoreNamesConst.LeaveManagement;
            _mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._userService = userService;
        }

        public async Task<List<LeaveRequestListDto>> HandleAsync(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<LeaveRequest>();
            var requests = new List<LeaveRequestListDto>();
            _leaveRequestRepository.Include(x => x.LeaveType);

            if (request.IsLoggedInUser)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(
                    q => q.Type == CustomClaimTypes.Uid)?.Value;
                leaveRequests = await _leaveRequestRepository.FindAsync(x=>x.RequestingEmployeeId == userId) as List<LeaveRequest>;

                var employee = await _userService.GetEmployee(userId);
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
                foreach (var req in requests)
                {
                    req.Employee = employee;
                }
            }
            else
            {
                leaveRequests = await _leaveRequestRepository.FindAsync(x => true) as List<LeaveRequest>;
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
                foreach (var req in requests)
                {
                    req.Employee = await _userService.GetEmployee(req.RequestingEmployeeId);
                }
            }

            return requests;

            
        }
    }
}
