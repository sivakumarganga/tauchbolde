using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.NewCommentUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class NewCommentHandlerTests
    {
        private readonly Guid validEventId = new Guid("6BE85F8D-3898-488C-92A9-D979648B742B");
        private readonly Guid validAuthorId = new Guid("E072BCB8-36E6-4E36-A241-31E4DDB14435");
        private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly ICommentRepository commentRepository = A.Fake<ICommentRepository>();
        private readonly NewCommentHandler handler;

        public NewCommentHandlerTests()
        {
            A.CallTo(() => eventRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validEventId
                        ? new Tauchbolde.Domain.Entities.Event { Id = validEventId }
                        : null
                    ));

            handler = new NewCommentHandler(eventRepository, commentRepository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = new NewComment(validEventId, validAuthorId, "The answer is 42!");
            
            var useCaseResult = await handler.Handle(request, CancellationToken.None);

            useCaseResult.Should().NotBeNull();
            useCaseResult.IsSuccessful.Should().BeTrue();
            useCaseResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_InvalidEventId()
        {
            var request = new NewComment(
                new Guid("15D08C24-FD8F-43E8-989E-69B98E8257B0"),
                validAuthorId, 
                "The answer is 42!");
            
            var useCaseResult = await handler.Handle(request, CancellationToken.None);

            useCaseResult.Should().NotBeNull();
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.NotFound);
            useCaseResult.Errors.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ErrorWhileInserting()
        {
            var request = new NewComment(validEventId, validAuthorId, "The answer is 42!");
            A.CallTo(() => commentRepository.InsertAsync(A<Comment>._))
                .Invokes(() => throw new InvalidOperationException());
            
            var useCaseResult = await handler.Handle(request, CancellationToken.None);

            useCaseResult.Should().NotBeNull();
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.GeneralFailure);
            useCaseResult.Errors.Should().HaveCount(1);
        }
    }
}