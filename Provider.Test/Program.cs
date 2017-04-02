using PactNet;
using PactNet.Reporters.Outputters;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentAssertions;

namespace Provider.Test
{
    public class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        const string ProviderName = "Userservice";
        const string ClientName = "Userclient";

        public static void Main(string[] args)
        {
            var outputter = new CustomOutputter();
            var config = new PactVerifierConfig();
            config.ReportOutputters.Add(outputter);
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { }, config);

            pactVerifier.ProviderState("Get user with id '1'");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("http://localhost:61131/api/");

            //Act
            pactVerifier
                      .ServiceProvider(ProviderName, _httpClient)
                      .HonoursPactWith(ClientName)
                      .PactUri(string.Format("D:/pact/{0}-{1}.json",ClientName, ProviderName))
                      .Verify();


            // Assert
            outputter.Should().NotBeNull();
            outputter.Output.Should().NotBeNullOrWhiteSpace();
            outputter.Output.Should().Contain(string.Format("Verifying a Pact between {0} and {1}", ClientName, ProviderName));
            System.Console.ReadLine();
        }

        private class CustomOutputter : IReportOutputter
        {
            public string Output { get; private set; }

            public void Write(string report)
            {
                Output += report;
            }
        }
    }
}
