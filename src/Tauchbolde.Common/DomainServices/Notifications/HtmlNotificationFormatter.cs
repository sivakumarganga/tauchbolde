﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Formats notification for a given recipient as HTML.
    /// </summary>
    public class HtmlNotificationFormatter : INotificationFormatter
    {
        /// <inheritdoc />
        public string Format(Diver recipient, IEnumerable<Notification> notifications)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            var sb = new StringBuilder();

            FormatHeader(sb, recipient, notifications);
            FormatNotification(sb, recipient, notifications);
            FormatFooter(sb);

            return sb.ToString();
        }

        private void FormatHeader(StringBuilder sb, Diver recipient, IEnumerable<Notification> notifications)
        {
            sb.AppendLine($"<h2>Hallo {recipient.Firstname},</h2>");

            sb.AppendLine("<p>");
            sb.AppendLine($"Auf der Tauchbolde-Webseite gibt es {notifications.Count()} News.");
            sb.AppendLine("</p>");
        }

        private void FormatNotification(StringBuilder sb, Diver recipient, IEnumerable<Notification> notifications)
        {
            sb.AppendLine("<ul>");

            foreach (var notification in notifications)
            {
                sb.AppendLine("<li>");

                sb.Append($"<small>{notification.OccuredAt.ToString("dd.MM.yyyy HH.mm")}</small> {NotificationTypeToString(notification.Type)}: ");
                sb.Append(notification.Message);
                sb.AppendLine();

                sb.AppendLine("</li>");
            }

            sb.AppendLine("</ul>");
        }

        private void FormatFooter(StringBuilder sb)
        {
            sb.AppendLine("<p>");
            sb.AppendLine("Guet Gas!");
            sb.AppendLine("</p>");
        }
        
        private string NotificationTypeToString(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.NewEvent:
                    return "Neue Aktivität";
                case NotificationType.CancelEvent:
                    return "Aktivität abgesagt";
                case NotificationType.EditEvent:
                    return "Aktivität geändert";
                case NotificationType.Commented:
                    return "Neuer Kommentar";
                case NotificationType.Accepted:
                    return "Zusage";
                case NotificationType.Declined:
                    return "Absage";
                case NotificationType.Tentative:
                    return "Vorbehalt";
                case NotificationType.Neutral:
                    return "Unklar";
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType));
            }
        }
    }
}