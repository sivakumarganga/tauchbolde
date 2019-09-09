using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.InterfaceAdapters;
using Tauchbolde.InterfaceAdapters.Logbook.Details;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class GetLogbookEntryDetailsInteractorTests
    {
        private readonly Guid validId = new Guid("544E7E71-67F8-4676-9247-BA51ACC7884A");
        private readonly ILogbookEntryRepository repository = A.Fake<ILogbookEntryRepository>();
        private readonly IRelativeUrlGenerator relativeUrlGenerator = A.Fake<IRelativeUrlGenerator>();
        private readonly ILogbookDetailsUrlGenerator detailsUrlGenerator = A.Fake<ILogbookDetailsUrlGenerator>();
        private readonly ILogger<GetLogbookEntryDetailsInteractor> logger = A.Fake<ILogger<GetLogbookEntryDetailsInteractor>>();
        private readonly GetLogbookEntryDetailsInteractor interactor;
        private readonly MvcLogbookDetailsOutputPort outputPort;

        public GetLogbookEntryDetailsInteractorTests()
        {
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validId
                        ? new LogbookEntry
                            {
                                Id = validId,
                                Title = "Test",
                                OriginalAuthor = new Diver
                                {
                                    Fullname = "John Doe",
                                    AvatarId = "joe1",
                                    User = new IdentityUser
                                    {
                                        Email = "joe@doe.com"
                                    }
                                }
                            }
                        : null));

            outputPort = new MvcLogbookDetailsOutputPort(relativeUrlGenerator, detailsUrlGenerator);
            interactor = new GetLogbookEntryDetailsInteractor(logger, repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = new GetLogbookEntryDetails(validId, outputPort, false);

            var result = await interactor.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeTrue();
            outputPort.GetViewModel().Should().NotBeNull();
            outputPort.GetViewModel().Id.Should().Be(validId);
        }

        [Fact]
        public async Task Handle_NotFound()
        {
            var request = new GetLogbookEntryDetails(new Guid("D6401A2A-1C89-4CFD-8436-53D071B1673C"), outputPort, false);

            var result = await interactor.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeFalse();
        }
    }
}