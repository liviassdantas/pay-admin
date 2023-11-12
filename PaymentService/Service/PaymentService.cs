using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pay_admin.DTO;
using pay_admin.Interfaces;
using pay_admin.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pay_admin.Service
{
    public class PaymentService : IPaymentService
    {
        public PaymentService() { }

        private readonly IMongoCollection<PaymentTransaction> _paymentTransactionCollection;
        private static HttpClient client = new();
        

        public PaymentService(IOptions<PaymentsDatabaseSettings> paymentDatabaseSettings)
        {
            var mongoClient = new MongoClient(paymentDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(paymentDatabaseSettings.Value.DatabaseName);
            _paymentTransactionCollection = mongoDatabase.GetCollection<PaymentTransaction>(paymentDatabaseSettings.Value.CollectionName);
        }

        public async Task<HttpResponseMessage> CreateNewBilling(HttpContext context, PaymentTransactionDTO billing)
        {
            try
            {
                var returnStatus = new HttpStatusCode();
                var loggedUser = GetLoggedUser(context);
                billing.UserID = loggedUser.FirstOrDefault().Key;

                var createBillingApi = await client.PostAsync("https://eoc56jqea5ysq7e.m.pipedream.net", JsonContent.Create(new { value = 10.8 }));
                
                if(createBillingApi.IsSuccessStatusCode && createBillingApi.Content != null)
                {
                    var resultJson = await createBillingApi.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ResponseFromBillingApiDTO>(resultJson);

                    if (!String.IsNullOrEmpty(result?.ID))
                    {
                        billing.PaymentStatus = result.status;
                        billing.TransactionID = result.ID;
                        billing.QrCode = result.qrCode;
                        billing.PaymentStatus = result.status;

                        var paymentTransaction = new PaymentTransaction
                        {
                            TransactionID = billing.TransactionID,
                            UserID = billing.UserID,
                            PaymentStatus = billing.PaymentStatus,
                            QrCode = billing.QrCode,
                        };

                        await _paymentTransactionCollection.InsertOneAsync(paymentTransaction);
                        returnStatus = HttpStatusCode.Created;
                    }
                    else
                    {
                        returnStatus = HttpStatusCode.BadRequest;
                    }
                    
                }
                return new HttpResponseMessage(returnStatus);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private static Dictionary<string, string> GetLoggedUser(HttpContext context)
        {
            var loggedUser = new Dictionary<string, string>(); 
            if (context.User.Identities.FirstOrDefault(x => String.IsNullOrEmpty(x.NameClaimType)) != null)
            {
                var userIdentity = new
                {
                    Email = context.User?.Claims?.FirstOrDefault(x => x?.Type == ClaimsIdentity.DefaultNameClaimType)?.ValueType ?? "",
                    Role = context.User?.Claims?.FirstOrDefault(x => x?.Type == ClaimsIdentity.DefaultRoleClaimType)?.ValueType ?? ""
                };
                loggedUser.Add(userIdentity.Email, userIdentity.Role);
                
                return loggedUser;
            }
            else
            {
                throw new WarningException("Seems you are not logged. Try to do the login to use the system.");
            }
        }

    }
}

