﻿using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using Moq;
using NUnit.Framework;
using RCommon.ApplicationServices.Validation;
using RCommon.Persistence;
using RCommon.Persistence.Crud;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    [TestFixture()]
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;

        private readonly CreateLeaveTypeDto _leaveTypeDto;
        private readonly CreateLeaveTypeCommandHandler _handler;

        public CreateLeaveTypeCommandHandlerTests()
        {

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            var testData = new List<LeaveType>();
            var mock = new Mock<IGraphRepository<LeaveType>>();
            var validationMock = new Mock<IValidationService>();
            mock.Setup(x => x.AddAsync(TestDataActions.CreateLeaveTypeStub(), CancellationToken.None))
                .Returns(() => Task.FromResult(new BaseCommandResponse()));


            _leaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 15,
                Name = "Test DTO"
            };

            validationMock.Setup(x => x.ValidateAsync(_leaveTypeDto, false, CancellationToken.None))
                .Returns(() => Task.FromResult(new ValidationOutcome()));
            _handler = new CreateLeaveTypeCommandHandler(_mapper, mock.Object, validationMock.Object);
        }

        [Test]
        public async Task Valid_LeaveType_Added()
        {
            var result = await _handler.HandleAsync(new CreateLeaveTypeCommand() { LeaveTypeDto = _leaveTypeDto }, CancellationToken.None);

            result.ShouldBeOfType<BaseCommandResponse>();
        }

        [Test]
        public async Task InValid_LeaveType_Added()
        {
            _leaveTypeDto.DefaultDays = -1;

            var result = await _handler.HandleAsync(new CreateLeaveTypeCommand() { LeaveTypeDto = _leaveTypeDto }, CancellationToken.None);

            //leaveTypes.Count.ShouldBe(3);

            result.ShouldBeOfType<BaseCommandResponse>();
            
        }
    }
}
