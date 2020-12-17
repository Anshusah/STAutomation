using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Cicero.Service
{
    public class Enums
    {
        public enum RecipientType
        {
            [Description("Case Owner")]
            CaseOwner = 1,
            [Description("Case Field")]
            CaseField = 2,
            [Description("Assigned User")]
            AssignedUser = 3,
            [Description("Email Group")]
            EmailGroup = 4,
            [Description("Users in Role")]
            UsersInRole = 5
        }

        public enum AuditLogOperation
        {
            Insert = 0,
            Update = 1,
            Delete = 2
        }

        public enum ElementEventType
        {
            [Description("On Load Event")]
            OnLoad = 0,
            [Description("On Click Event")]
            OnClick = 1,
            [Description("On Change Event")]
            OnChange = 2,
            [Description("On Key Up Event")]
            OnKeyUp = 3,
            [Description("On Focus Event")]
            OnFocus = 4
        }

        public enum PayeeType
        {
            Individual = 1,
            Company = 2
        }

        public enum OnfidoCheckResults
        {
            consider,
            clear,
        }
        public enum NotificationTypesToastr
        {
            Success,
            Warning,
            Info,
            Error,
        }

        public enum NotificationTypesNoty
        {
            Success,
            Warning,
            Info,
            Error,
            Alert

        }
        public enum EmailTemplateType
        {
            [Description("Account Notification")]
            AccountNotification = 1,
            [Description("Claim Notification")]
            ClaimNotification = 2
        }
        public enum ButtonAction
        {
            [Description("Deleted")]
            delete,
            [Description("Activated")]
            active,
            [Description("Deactivated")]
            inactive,
            [Description("sendsubrogation")]
            sendsubrogation,
            [Description("markAsRead")]
            markAsRead
        }
        public enum TablesToShow
        {
            [Description("Queue")]
            Queue,
            [Description("State")]
            State,
            [Description("Role")]
            Role,
            [Description("User")]
            User,
            [Description("CaseForm")]
            CaseForm,
            [Description("Case")]
            Case,
            [Description("Tenant")]
            Tenant
        }
        public enum EmailSettingFor
        {
            [Description("Password Reset")]
            PasswordReset,
            [Description("User Creation")]
            UserCreation,
            [Description("Email Confirmation")]
            EmailConfirmation,
            [Description("Payment Request")]
            PaymentRequest,
            [Description("Payment Request Success")]
            PaymentRequestSuccess

        }
        public enum Typesource
        {
            [Description("rest-api")]
            RestAPI,
            [Description("tenant-database")]
            TenantDatabase
        }

        public enum TabType
        {
            [Description("Normal")]
            Normal = 0,
            [Description("Case Timeline")]
            CaseTimeLine = 1
        }
    }
}
