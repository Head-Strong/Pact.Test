using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

namespace Test.Pact.App.MockService
{
    public class ConsumerMyApiPact
    {
        const string ProviderName = "Userservice";
        const string ClientName = "Userclient";

        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 1234; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}/api/", MockServerPort); } }

        public ConsumerMyApiPact()
        {
            PactBuilder = new PactBuilder(new PactConfig { PactDir = @"D:\Pact", LogDir = @"D:\Pact" }); ;

            PactBuilder
                .ServiceConsumer(ClientName)
                .HasPactWith(ProviderName); // Provider Name

            MockProviderService = PactBuilder.MockService(MockServerPort);
            //Configure the http mock server

            MockProviderService = PactBuilder.MockService(MockServerPort, false);
            // By passing true as the second param, you can enabled SSL. 
            // This will however require a valid SSL certificate installed and bound 
            // with netsh (netsh http add sslcert ipport=0.0.0.0:port certhash=thumbprint appid={app-guid}) 
            // on the machine running the test. See https://groups.google.com/forum/#!topic/nancy-web-framework/t75dKyfgzpg
            //or


            MockProviderService = PactBuilder.MockService(MockServerPort, bindOnAllAdapters: false);
            //By passing true as the bindOnAllAdapters parameter the http mock server will be 
            // able to accept external network requests, but will require admin privileges in order to run

            MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings());
            //You can also change the default Json serialization settings using this overload		
        }

        public void Dispose()
        {
            PactBuilder.Build();
            //NOTE: Will save the pact file once finished
        }
    }
}
