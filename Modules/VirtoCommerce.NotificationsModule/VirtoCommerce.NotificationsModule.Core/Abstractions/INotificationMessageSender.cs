using System.Threading.Tasks;
using VirtoCommerce.NotificationsModule.Core.Model;

namespace VirtoCommerce.NotificationsModule.Core.Abstractions
{
    /// <summary>
    /// The sender wich send a email/sms message via smtp/sendgrid/twilio senders
    /// </summary>
    public interface INotificationMessageSender
    {
        Task SendNotificationAsync(NotificationMessage message);
    }
}
