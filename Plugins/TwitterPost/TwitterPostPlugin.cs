//<snippetAssignAutoNumberPlugin>
using System;
using System.Linq;
// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using System.Net;
using System.Text;

namespace Dotsquares.DCRM.Plugins
{
    public class TwitterPostPlugin : IPlugin
    {
        internal IOrganizationService OrganizationService
        {
            get;
            private set;
        }

        enum CrmPluginStepDeployment
        {
            ServerOnly = 0,
            OfflineOnly = 1,
            Both = 2
        }

        enum CrmPluginStepMode
        {
            Asynchronous = 1,
            Synchronous = 0
        }

        enum CrmPluginStepStage
        {
            PreValidation = 10,
            PreOperation = 20,
            PostOperation = 40,
        }

        enum SdkMessageName
        {
            Create,
            Update,
            Delete,
            Retrieve,
            Assign,
            GrantAccess,
            ModifyAccess,
            RetrieveMultiple,
            RetrievePrincipalAccess,
            RetrieveSharedPrincipalsAndAccess,
            RevokeAccess,
            SetState,
            SetStateDynamicEntity,
        }

        string _assemblyName = "TwitterPost";
        string _pluginTypeName = "Dotsquares.DCRM.Plugins.TwitterPostPlugin";

        /// <summary>
        /// A plug-in that auto generates a number for provided entity when a record
        /// is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and pre-operation stage.
        /// </remarks>
        //<snippetAssignAutoNumberPlugin2>
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.
            Microsoft.Xrm.Sdk.IPluginExecutionContext context = (Microsoft.Xrm.Sdk.IPluginExecutionContext)
                serviceProvider.GetService(typeof(Microsoft.Xrm.Sdk.IPluginExecutionContext));

            IOrganizationServiceFactory service = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));



            OrganizationService = service.CreateOrganizationService(context.UserId);

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];
                //</snippetAssignAutoNumberPlugin2>      

                if (entity.LogicalName == "dots_twitterpost")
                {
                    // String x = entity.FormattedValues["dots_parent_twitterpublisherid"].ToString();


                    var lookup = entity.GetAttributeValue<EntityReference>("dots_parent_twitterpublisherid");
                    //var entityName = lookup.LogicalName;
                    var entityId = lookup.Id;
                    //var instanceName = lookup.Name;

                    var message = entity.Attributes["dots_message"].ToString();
                // var publisherId=   entity.Attributes["dots_parent_twitterpublisherid"].ToString();
                      string result = PostMessage(message, "{"+entityId.ToString()+"}");
                         if (result != "Success")
                        throw new InvalidPluginExecutionException(result);
               

                }

        
            }
        }

        

        public string PostMessage(String text_Message, String row_Id)
        {
            //WebClient Client = new WebClient();
            //String RequestURL, RequestData;

            //RequestURL = "https://crmwebapi.24livehost.com/Home/Tweet";

            ////RequestData = "AccountId=" + AccountID
            ////    + "&Email=" + System.Uri.EscapeDataString(Email)
            ////    + "&Password=" + System.Uri.EscapeDataString(Password)
            ////    + "&Recipient=" + System.Uri.EscapeDataString(Recipient)
            ////    + "&Message=" + System.Uri.EscapeDataString(Message);

            //RequestData = "?text=" + System.Uri.EscapeDataString(text_Message) + "&rowId=" + System.Uri.EscapeDataString(row_Id);


            //byte[] PostData = Encoding.ASCII.GetBytes(RequestData);
            //byte[] Response = Client.UploadData(RequestURL, PostData);

            //String Result = Encoding.ASCII.GetString(Response);
            //int ResultCode = System.Convert.ToInt32(Result.Substring(0, 4));
            //if (ResultCode != 0)
            //    return Result;
            //else
            //    return "Success";
            


            using (WebClient client = new WebClient())
            {
                var webAddress = "https://crmwebapi.24livehost.com/Home/Tweet?text=" + text_Message + "&rowId=" + row_Id;
                byte[] responseBytes = client.DownloadData(webAddress);
                string response = Encoding.UTF8.GetString(responseBytes);

                if (response != null)
                    return "Success";
                else
                    return "Error";
               
                // tracingService.Trace(response);

                // For demonstration purposes, throw an exception so that the response
                // is shown in the trace dialog of the Microsoft Dynamics CRM user interface.
                // throw new InvalidPluginExecutionException("WebClientPlugin completed successfully.");
            }
        }
    }
}
//</snippetAssignAutoNumberPlugin>
