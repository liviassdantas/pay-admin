using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pay_admin.DTO;
using pay_admin.Interfaces;
using pay_admin.Model;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace pay_admin.Service
{
    public class PaymentService : IPaymentService
    {
        public PaymentService() { }

        private readonly IMongoCollection<PaymentTransaction> _paymentTransactionCollection;
        private static HttpClient client = new();
        private static Object loggedUser = new Object();

        public PaymentService(IOptions<PaymentsDatabaseSettings> paymentDatabaseSettings)
        {
            var mongoClient = new MongoClient(paymentDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(paymentDatabaseSettings.Value.DatabaseName);
            _paymentTransactionCollection = mongoDatabase.GetCollection<PaymentTransaction>(paymentDatabaseSettings.Value.CollectionName);
        }

        public async Task<HttpResponse> CreateNewBilling(HttpContext context, PaymentTransactionDTO billing)
        {

        }

        private static async void GetLoggedUser(HttpContext context)
        {
            if (context.User.Identity?.Name != null)
            {
                var userIdentity = new
                {
                    Email = context.User.Identity.Name,
                    Role = context.User?.Claims?.FirstOrDefault(x => x?.Type == ClaimTypes.Role)?.ValueType ?? ""
                };

                loggedUser = userIdentity;
            }
            else
            {
                throw new WarningException("Seems you are not logged. Try to do the login to use the system.");
            }
        }

    }
}

