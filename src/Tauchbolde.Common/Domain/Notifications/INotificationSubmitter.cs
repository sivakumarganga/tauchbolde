﻿using System.Threading.Tasks;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Domain.Notifications
{
    /// <summary>
    /// Allows to send out notification.
    /// </summary>
    public interface INotificationSubmitter
    {
        /// <summary>
        /// Submits one single Notification asynchronous.
        /// </summary>
        /// <param name="recipient">Recipient to submit the Notification to.</param>
        /// <param name="content">Text content of the notification.</param>
        Task SubmitAsync(Diver recipient, string content);
    }
}
