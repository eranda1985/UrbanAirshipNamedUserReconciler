using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace UrbanAirshipNamedUserReconciler
{
    public class UrbanAirshipAPIClient : IApiClient
    {
        public string Name { get { return "UrbanAirshipAPIClient"; } }
        private RestClient _client;
        private string path;
        
        public string UriPath
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }

        public UrbanAirshipAPIClient()
        {
            if (_client == null)
            {
                // TODO: Going ahead the following hard coded stuff needs to be taken out. 
                _client = new RestClient("https://go.urbanairship.com");
                _client.Authenticator = new HttpBasicAuthenticator("bsVQR0IGS9SErIHeAddL4A", "kFq55DFIQVuc82shxv8xgQ");
            }
        }

        public IRestRequest CreateRequest(Func<IRestRequest> t)
        {
            return t();
        }

        // TODO: Try to make this async 
        IRestResponse<T> IApiClient.RunAsync<T>(IRestRequest request)
        {
            if (request == null)
                throw new Exception("Uri request cannot be empty");

            var taskCompletionSource = new TaskCompletionSource<T>();

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/vnd.urbanairship+json; version=3");

            return _client.Execute<T>(request);

        }

        public IRestResponse RunAsync(IRestRequest request)
        {
            if (request == null)
                throw new Exception("Uri request cannot be empty");

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/vnd.urbanairship+json; version=3");

            return _client.Execute(request);
        }
    }
}
