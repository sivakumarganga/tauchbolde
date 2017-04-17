﻿using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class ConsoleNotificationSubmitter : INotificationSubmitter
    {
        /// <inheritdoc />
        public async Task SubmitAsync(ApplicationUser recipient, string content)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine($"Sent notification to {recipient.Email}: {content}.");

            Console.ForegroundColor = oldColor;
        }
    }
}
