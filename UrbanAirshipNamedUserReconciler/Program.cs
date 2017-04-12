using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UrbanAirshipNamedUserReconciler.Deserializers;
using UrbanAirshipNamedUserReconciler.Serializers;

namespace UrbanAirshipNamedUserReconciler
{
    class Program
    {
        static void Main(string[] args)
        {
            // List the named users
            IApiClient apiClient = new UrbanAirshipAPIClient();
            var result = GetAllNamedUsers(apiClient);

            // Get their channel ids 
            foreach (var user in result.NamedUsers)
            {
                // If it's not in Uppser Caese...
                if (!user.NamedUserId.All(char.IsLower))
                {
                    foreach (var channel in user.Channels)
                    {
                        // Associate the channel with the upper case version of this user
                        AssociateNamedUser(channel, user.NamedUserId, apiClient);
                    }
                }
            }
        }

        public static NamedUsersDeserializer GetAllNamedUsers(IApiClient urbanAirshipClient)
        {
            // Get a single named user by Id
            var requestObject = urbanAirshipClient.CreateRequest(() =>
            {
                var request = new RestRequest("api/named_users", Method.GET);
                return request;
            });

            var apiResult = urbanAirshipClient.RunAsync<NamedUsersDeserializer>(requestObject);

            if (apiResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return apiResult.Data;
            }
            else
            {
                return null;
            }
        }

        public static void AssociateNamedUser(Channel channel, string userId, IApiClient urbanAirshipClient)
        {
            var requestBodyObj = new AssociationSerializer
            {
                channel_id = channel.ChannelId,
                device_type = channel.DeviceType,
                named_user_id = userId.ToLower()
            };

            var requestBody = JsonConvert.SerializeObject(requestBodyObj);

            var requestObject = urbanAirshipClient.CreateRequest(() =>
            {
                var request = new RestRequest("api/named_users/associate", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
                return request;
            });

            var apiResult = urbanAirshipClient.RunAsync(requestObject);

            if (apiResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Successfully associated channel {0} with user {1}: ",requestBodyObj.channel_id, requestBodyObj.named_user_id);
            }
            else
            {
                Console.WriteLine("ERROR in associating channel {0} with user {1}: ", requestBodyObj.channel_id, requestBodyObj.named_user_id);
            }
        }
    }
}
