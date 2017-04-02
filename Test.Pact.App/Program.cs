using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System.Collections.Generic;
using Test.Pact.App.Client;
using Test.Pact.App.MockService;
using FluentAssertions;

namespace Test.Pact.App
{
    class Program
    {
        private static IMockProviderService _mockProviderService;
        private static string _mockProviderServiceBaseUri;

        static void Main(string[] args)
        {
            var consumerPact = new ConsumerMyApiPact();
            _mockProviderService = consumerPact.MockProviderService;
            _mockProviderServiceBaseUri = consumerPact.MockProviderServiceBaseUri;
            consumerPact.MockProviderService.ClearInteractions();
            //NOTE: Clears any previously registered interactions before the test is run


            _mockProviderService
                .Given("Get user with id '1'")
                .UponReceiving("A GET request to retrieve the user")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/user/1",
                    Headers = new Dictionary<string, string>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                    {
                        id = 1,
                        firstName = "Aditya",
                        lastName = "Magotra"
                    }
                }); //NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new UserApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.GetUsers(1);

            //Assert
            result.Should().NotBeNull();
            _mockProviderService.VerifyInteractions();
            //NOTE: Verifies that interactions registered on the mock provider are called once and only once

            consumerPact.Dispose();
        }
    }
}
