using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    /// <summary>
    /// Formats the header as HTML.
    /// </summary>
    public interface IHtmlHeaderFormatter
    {
        /// <summary>
        /// Formats the header as HTML and write it into <paramref name="htmlBuilder"/>.
        /// </summary>
        /// <param name="recipient">The receiver of the notifications.</param>
        /// <param name="notificationCount">The total number of notifications.</param>
        /// <param name="htmlBuilder">The <see cref="StringBuilder"/> to write the HTML into.</param>
        void Format(Diver recipient, int notificationCount, StringBuilder htmlBuilder);
    }
}