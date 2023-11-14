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

        public async Task<HttpResponseMessage> CreateNewBilling(HttpContext context)
        {
            try
            {
                var returnStatus = new HttpStatusCode();
                var loggedUser = GetLoggedUser(context);

                var createBillingApi = await client.PostAsync("https://eoc56jqea5ysq7e.m.pipedream.net", JsonContent.Create(new { value = 10.8 }));
                
                if(createBillingApi.IsSuccessStatusCode && createBillingApi.Content != null)
                {
                    var resultJson = await createBillingApi.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ResponseFromBillingApiDTO>(resultJson);

                    if (!String.IsNullOrEmpty(result?.qrCode))
                    {
                        var paymentTransaction = new PaymentTransaction
                        {
                            TransactionID = result.Id,
                            UserID = loggedUser.UserID,
                            PaymentStatus = result.status,
                            QrCode = result.qrCode,
                        };

                        var saveAsync = _paymentTransactionCollection.InsertOneAsync(paymentTransaction);
                        saveAsync.Wait();

                        if(saveAsync.IsCompletedSuccessfully)
                        {
                            returnStatus = HttpStatusCode.Created;
                        }
                        else
                        {
                            returnStatus = HttpStatusCode.InternalServerError;
                        }
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

        private static PaymentTransactionDTO GetLoggedUser(HttpContext context)
        {
            var loggedUser = new PaymentTransactionDTO(); 
            if (!String.IsNullOrEmpty(context?.User?.Identity?.Name))
            {
                var userIdentity = new
                {
                    Email = context.User?.Claims?.FirstOrDefault(x => x?.Type == ClaimTypes.Name)?.Value ?? "",
                    Role = context.User?.Claims?.FirstOrDefault(x => x?.Type == ClaimTypes.Role)?.Value ?? ""
                };
                loggedUser.UserID = userIdentity.Email;
                
                return loggedUser;
            }
            else
            {
                throw new WarningException("Seems you are not logged. Try to do the login to use the system.");
            }
        }

    }
}

