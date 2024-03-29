using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace pay_admin.Model.Integration
{
    public class OnBillingCreatedIntegrationEventArgs : EventArgs
    {
        public string IdMessage { get; set; }
        public string IdUser { get; set; }
        public string MessageContent { get; set; }

    }
    public delegate void OnBillingCreatedIntegrationEventHandler(object sender, OnBillingCreatedIntegrationEventArgs eventArgs);
}
