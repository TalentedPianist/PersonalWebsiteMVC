using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Kiota.Authentication.Azure;
using MyGraphSdk;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Helpers;

public class GraphHelper
{
     public GraphClient Graph()
     {
          var scopes = new[] { "User.Read" };

          var options = new DeviceCodeCredentialOptions
          {
               ClientId = "",
               DeviceCodeCallback = (code, cancellation) => 
               {
                    Console.WriteLine(code.Message);
                    return Task.CompletedTask;
               }
          };

          var credential = new DeviceCodeCredential(options);
          var authProvider = new AzureIdentityAuthenticationProvider(credential, scopes);
          var adapter = new BaseGraphRequestAdapter(authProvider);
          var graphClient = new GraphClient(adapter);
          return graphClient;
     }
    
}