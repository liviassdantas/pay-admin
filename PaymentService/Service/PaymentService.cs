﻿using Confluent.Kafka;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using pay_admin.DTO;
using pay_admin.Interfaces;
using pay_admin.Model;
using pay_admin.Model.ValueObjects.Enums;
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
        private static string bootstrapServer = "localhost:29092";
        private readonly KafkaProducerService _kafkaProducerService = new KafkaProducerService(bootstrapServer);
        private static HttpClient client = new();

        public PaymentService(IOptions<PaymentsDatabaseSettings> paymentDatabaseSettings)
        {
            var mongoClient = new MongoClient(paymentDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(paymentDatabaseSettings.Value.DatabaseName);
            _paymentTransactionCollection = mongoDatabase.GetCollection<PaymentTransaction>(paymentDatabaseSettings.Value.CollectionName);
        }

        public async Task<HttpResponseMessage> CancelABilling(HttpContext context)
        {
            try
            {
                var returnStatus = new HttpStatusCode();
                var loggedUser = GetLoggedUser(context);

                if (loggedUser.IsUserAdmin)
                {
                    var paymentTransaction = new PaymentTransaction
                    {
                        UserID = loggedUser.UserID,
                        PaymentStatus = EPaymentStatus.Canceled,
                        UpdatedAt = DateTime.UtcNow
                    };
                    var getBillingFromDatabase = await _paymentTransactionCollection.Find(bill => bill.UserID == loggedUser.UserID && bill.PaymentStatus == EPaymentStatus.Pending).FirstOrDefaultAsync();
                    var filters = Builders<PaymentTransaction>.Filter.Eq(payment => payment, getBillingFromDatabase);
                    var update = Builders<PaymentTransaction>.Update.Set(updt => updt, paymentTransaction);

                    var updateAsync = _paymentTransactionCollection.UpdateOneAsync(filters, update);
                    updateAsync.Wait();

                    if (updateAsync.IsCompletedSuccessfully)
                    {
                        var message = string.Format("user: {0}, transactionID: {1}, paymentStatus: {2}",
                            loggedUser.UserID, getBillingFromDatabase.TransactionID, EPaymentStatus.Canceled);
                        await _kafkaProducerService.ProduceMessageAsync("CancelABilling", new Message<string, string>
                        {
                            Key = Guid.NewGuid().ToString(),
                            Value = JsonSerializer.Serialize(message)
                        });
                        returnStatus = HttpStatusCode.OK;
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

                return new HttpResponseMessage(returnStatus);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<HttpResponseMessage> CreateNewBilling(HttpContext context)
        {
            try
            {
                var returnStatus = new HttpStatusCode();
                var loggedUser = GetLoggedUser(context);

                var paymentTransaction = new PaymentTransaction
                {
                    TransactionID = Guid.NewGuid().ToString(),
                    UserID = loggedUser.UserID,
                    PaymentStatus = EPaymentStatus.Pending,
                    CreatedAt = DateTime.Now,
                };

                var saveAsync = _paymentTransactionCollection.InsertOneAsync(paymentTransaction);
                saveAsync.Wait();

                if (saveAsync.IsCompletedSuccessfully)
                {
                    var message = string.Format("user: {0}, transactionID: {1}, paymentStatus: {2}",
                           loggedUser.UserID, paymentTransaction.TransactionID, paymentTransaction.PaymentStatus);
                    await _kafkaProducerService.ProduceMessageAsync("CreateABilling", new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(),
                        Value = JsonSerializer.Serialize(message)
                    });
                    returnStatus = HttpStatusCode.Created;
                }
                else
                {
                    returnStatus = HttpStatusCode.InternalServerError;
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
                loggedUser.IsUserAdmin = userIdentity.Role == "Admin" ? true : false;

                return loggedUser;
            }
            else
            {
                throw new WarningException("Seems you are not logged. Try to do the login to use the system.");
            }
        }


    }
}

