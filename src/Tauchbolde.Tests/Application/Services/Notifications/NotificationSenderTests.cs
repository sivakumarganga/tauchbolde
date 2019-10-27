using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel.Services;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.Services.Notifications
{
    public class NotificationSenderTests
    {
        private readonly ILoggerFactory loggerFactory = A.Fake<ILoggerFactory>();
        private readonly INotificationRepository repository = A.Fake<INotificationRepository>();
        private readonly INotificationFormatter formatter = A.Fake<INotificationFormatter>();
        private readonly INotificationSubmitter submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
        private readonly NotificationSender sender;

        public NotificationSenderTests()
        {
            sender = new NotificationSender(loggerFactory, repository);
        }

        [Fact]
        public async Task No_Check_Without_Pending_Notifications()
        {
            // Act
            await sender.SendAsync(formatter, submitter, () => Task.CompletedTask);

            // Assert
            A.CallTo(() => repository.GetPendingNotificationByUserAsync())
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => formatter.FormatAsync(A<Diver>._, A<IGrouping<Diver, Notification>>._))
                .MustNotHaveHappened();
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task No_Check_Interval_Not_Reached()
        {
            // Arrange
            var lastNotificationCheckAt = DateTime.UtcNow.AddMinutes(-1);
            SystemClock.SetTime(lastNotificationCheckAt);
            var userHansMeier = CreateTestUser(lastNotificationCheckAt, 1);
            var notifications = CreateTestNotifications(userHansMeier);

            A.CallTo(() => repository.GetPendingNotificationByUserAsync())
                .Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));

            // Act
            await sender.SendAsync(formatter, submitter, () => Task.CompletedTask);

            // Assert
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task No_Check_When_Interval_Reached()
        {
            // Arrange
            var lastNotificationCheckAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-1);
            var userHansMeier = CreateTestUser(lastNotificationCheckAt, 1);
            var notifications = CreateTestNotifications(userHansMeier);
            
            A.CallTo(() => repository.GetPendingNotificationByUserAsync())
                .Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));
            A.CallTo(() => formatter.FormatAsync(A<Diver>._, A<IGrouping<Diver, Notification>>._))
                .Returns("Some content!");

            // Act
            await sender.SendAsync(formatter, submitter, () => Task.CompletedTask);

            // Assert
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._))
                .MustHaveHappenedOnceExactly();
            notifications.First().CountOfTries.Should().Be(1);
            userHansMeier.LastNotificationCheckAt.Should().BeAfter(lastNotificationCheckAt);
        }

        [Fact]
        public async Task No_Multiple_Notifications_For_One_User_When_Interval_Reached()
        {
            // Arrange
            var lastNotificationCheckAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-1);
            var userHansMeier = CreateTestUser(lastNotificationCheckAt, 1);
            var notifications = CreateTestNotifications(userHansMeier, 3);

            A.CallTo(() => repository.GetPendingNotificationByUserAsync())
                .Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));
            A.CallTo(() => formatter.FormatAsync(A<Diver>._, A<IGrouping<Diver, Notification>>._))
                .Returns("Some content!");

            // Act
            await sender.SendAsync(formatter, submitter, () => Task.CompletedTask);

            // Assert
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._))
                .MustHaveHappenedOnceExactly();
            
            foreach (var notification in notifications)
            {
                notification.CountOfTries.Should().Be(1);
            }
            
            userHansMeier.LastNotificationCheckAt.Should().BeAfter(lastNotificationCheckAt);
        }

        private static Diver CreateTestUser(DateTime lastNotificationCheckAt, int notificationIntervalInHours)
        {
            var result = DiverFactory.CreateJohnDoe();
            result.LastNotificationCheckAt = lastNotificationCheckAt;
            result.NotificationIntervalInHours = notificationIntervalInHours;

            return result;
        }

        private static List<Notification> CreateTestNotifications(Diver userHansMeier, int countNotifications = 1)
        {
            var result = new List<Notification>();

            for (var numberOfNotification = 0; numberOfNotification < countNotifications; numberOfNotification++)
            {
                result.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    Recipient = userHansMeier,
                    AlreadySent = false,
                    CountOfTries = 0,
                    Message = "This is a test notification!",
                    Type = NotificationType.NewEvent,
                    Event = new Event
                    {
                        Name = "Testevent",
                        Id = Guid.NewGuid(),
                    }
                });
            }

            return result;
        }
    }
}
