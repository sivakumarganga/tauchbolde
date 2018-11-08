using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace Tauchbolde.Tests.DomainServices.Notifications
{
    public class NotificationSenderTests
    {
        [Fact]
        public async Task No_Check_Without_Pending_Notifications()
        {
            // Arrange
            var notificationRepository = A.Fake<INotificationRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<Diver>._, A<IGrouping<Diver, Notification>>._)).MustNotHaveHappened();
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task No_Check_Interval_Not_Reached()
        {
            // Arrange
            var userHansMeier = CreateTestUser(DateTime.Now.AddMinutes(-1), 1);
            var notifications = CreateTestNotifications(userHansMeier);
            var notificationRepository = A.Fake<INotificationRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<Diver>._, A<IGrouping<Diver, Notification>>._)).MustNotHaveHappened();
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task No_Check_Interval_Reached()
        {
            // Arrange
            var lastNotificationCheckAt = DateTime.Now.AddHours(-1).AddMinutes(-1);
            var userHansMeier = CreateTestUser(lastNotificationCheckAt, 1);
            var notifications = CreateTestNotifications(userHansMeier);
            var notificationRepository = A.Fake<INotificationRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));
            A.CallTo(() => formatter.Format(A<Diver>._, A<IGrouping<Diver, Notification>>._)).Returns("Some content!");

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<Diver>._, A<IGrouping<Diver, Notification>>._)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._)).MustHaveHappened(Repeated.Exactly.Once);

            notifications.First().CountOfTries.Should().Be(1);
            userHansMeier.LastNotificationCheckAt.Should().HaveValue();
            userHansMeier.LastNotificationCheckAt.Should().BeAfter(lastNotificationCheckAt);
        }

        [Fact]
        public async Task No_Multiple_Notifications_For_One_User_When_Interval_Reached()
        {
            // Arrange
            var lastNotificationCheckAt = DateTime.Now.AddHours(-1).AddMinutes(-1);
            var userHansMeier = CreateTestUser(lastNotificationCheckAt, 1);
            var notifications = CreateTestNotifications(userHansMeier, 3);
            var notificationRepository = A.Fake<INotificationRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));
            A.CallTo(() => formatter.Format(A<Diver>._, A<IGrouping<Diver, Notification>>._)).Returns("Some content!");

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(
                A<Diver>._,
                A<IGrouping<Diver, Notification>>.That.Matches(g => g.Count() == 3)))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => submitter.SubmitAsync(A<Diver>._, A<string>._)).MustHaveHappened(Repeated.Exactly.Once);

            foreach (var notification in notifications)
            {
                notification.CountOfTries.Should().Be(1);
            }

            userHansMeier.LastNotificationCheckAt.Should().BeAfter(lastNotificationCheckAt);
        }

        private static Diver CreateTestUser(DateTime lastNotificationCheckAt, int notificationIntervalInHours)
        {
            return new Diver
            {
                Id = Guid.NewGuid(),
                Firstname = "Hans",
                Lastname = "Meier",
                LastNotificationCheckAt = lastNotificationCheckAt,
                NotificationIntervalInHours = notificationIntervalInHours,
                User = new IdentityUser
                {
                    Id = "1",
                    UserName = "hans.meier@test.com",
                },
            };
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