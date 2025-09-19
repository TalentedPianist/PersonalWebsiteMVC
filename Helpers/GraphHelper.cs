using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Authentication.Azure;
using Microsoft.Graph.Users.Item;
using MyGraphSdk;

namespace PersonalWebsiteMVC.Helpers
{
     public class GraphHelper
     {
          // Settings object
          private static Settings? _settings;
          // User auth token credential
          private static DeviceCodeCredential? _deviceCodeCredential;
          // Client configured with user authentication
          private static GraphServiceClient? _userClient;


        
     }
}
