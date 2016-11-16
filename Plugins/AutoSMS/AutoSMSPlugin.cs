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
    public class AutoSMSPlugin : IPlugin
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

        string _assemblyName = "AutoSMS";
        string _pluginTypeName = "Dotsquares.DCRM.Plugins.AutoSMSPlugin";

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

                if (entity.LogicalName == "dots_autosms")
                {
                    SdkMessageProcessingStep step = new SdkMessageProcessingStep
                    {
                        AsyncAutoDelete = false,
                        Mode = new OptionSetValue((int)CrmPluginStepMode.Synchronous),
                        Name = _pluginTypeName + ": " + entity.Attributes["dots_name"].ToString(),
                        EventHandler = new EntityReference("plugintype", GetPluginTypeId()),
                        Rank = 1,
                        SdkMessageId = new EntityReference("sdkmessage", GetMessageId()),
                        Stage = new OptionSetValue((int)CrmPluginStepStage.PreOperation),
                        SupportedDeployment = new OptionSetValue((int)CrmPluginStepDeployment.ServerOnly),
                        SdkMessageFilterId = new EntityReference("sdkmessagefilter", GetSdkMessageFilterId(entity.Attributes["dots_targetentitylogicalname"].ToString())),
                        Description = entity.Attributes["dots_targetattributelogicalname"].ToString()
                    };
                    OrganizationService.Create(step);
                }
                else
                {
                    var entityDetail = GetEntityByName(entity.LogicalName);
                    if (entity.Attributes.Contains(entityDetail.TargetAttributeLogicalName) == true)
                    {
                       
                        string contactNO = entity.Attributes[entityDetail.TargetAttributeLogicalName].ToString();                   
                        string i=    SendSMS("CI00184183", "akram.khan@dotsquares.com", "xRldS3d7", contactNO, "hii You have created record for "+entity.LogicalName+" Thanks!");
                        if (i != "Success")
                         throw new InvalidPluginExecutionException(i);
                        //entity.Attributes.Add(entityDetail.TargetAttributeLogicalName, GetNewCode(entityDetail));
                        //IncreaseCurrentNumber(entity.LogicalName);
                    }
                }
            }
        }

        private string GetNewCode(DSAutoSMS entityDetail)
        {
            string newCode=entityDetail.FieldFormat;
            if (string.IsNullOrEmpty(entityDetail.FieldFormat))
            {
                return entityDetail.CurrentNumber;
            }
            else
            {
                if (entityDetail.FieldFormat.Contains("{dd}"))
                    newCode = newCode.Replace("{dd}", DateTime.UtcNow.ToString("dd"));
                if (entityDetail.FieldFormat.Contains("{MM}"))
                    newCode = newCode.Replace("{MM}", DateTime.UtcNow.ToString("MM"));
                if (entityDetail.FieldFormat.Contains("{MMM}"))
                    newCode = newCode.Replace("{MMM}", DateTime.UtcNow.ToString("MMM"));
                if (entityDetail.FieldFormat.Contains("{yy}"))
                    newCode = newCode.Replace("{yy}", DateTime.UtcNow.ToString("yy"));
                if (entityDetail.FieldFormat.Contains("{yyyy}"))
                    newCode = newCode.Replace("{yyyy}", DateTime.UtcNow.ToString("yyyy"));
                if (entityDetail.FieldFormat.Contains("{0}"))
                    newCode = newCode.Replace("{0}", entityDetail.CurrentNumber);
            }
            return newCode;
        }

        private DSAutoSMS GetEntityByName(string entityName)
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var autoNumberDetail = context.dots_autosmsSet.FirstOrDefault(w => w.TargetEntityLogicalName == entityName);
                return autoNumberDetail;
            }
        }

        private void IncreaseCurrentNumber(string entityName)
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var autoNumberDetail = context.dots_autosmsSet.FirstOrDefault(w => w.TargetEntityLogicalName == entityName);
                autoNumberDetail.CurrentNumber = (Convert.ToInt32(autoNumberDetail.CurrentNumber) + 1).ToString();
                context.UpdateObject(autoNumberDetail);
                context.SaveChanges();
            }
        }

        private Guid GetSdkMessageFilterId(string entityName)
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var sdkMessageFilters = from s in context.SdkMessageFilterSet
                                        where s.PrimaryObjectTypeCode == entityName
                                        where s.SdkMessageId.Id == GetMessageId()
                                        select s;
                return sdkMessageFilters.First().Id;
            }
        }

        private Guid GetMessageId()
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var sdkMessages = from s in context.SdkMessageSet
                                  where s.Name == SdkMessageName.Create.ToString()
                                  select s;
                return sdkMessages.First().Id;
            }
        }

        private Guid GetPluginTypeId()
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var pluginType = context.PluginTypeSet
                    .FirstOrDefault(f => f.AssemblyName == _assemblyName && f.TypeName == _pluginTypeName);

                return pluginType.Id;
            }
        }

        public string SendSMS(String AccountID, String Email, String Password, String Recipient, String Message)
        {
            WebClient Client = new WebClient();
            String RequestURL, RequestData;
            
            RequestURL = "https://redoxygen.net/sms.dll?Action=SendSMS";

            RequestData = "AccountId=" + AccountID
                + "&Email=" + System.Uri.EscapeDataString(Email)
                + "&Password=" + System.Uri.EscapeDataString(Password)
                + "&Recipient=" + System.Uri.EscapeDataString(Recipient)
                + "&Message=" + System.Uri.EscapeDataString(Message);

            byte[] PostData = Encoding.ASCII.GetBytes(RequestData);
            byte[] Response = Client.UploadData(RequestURL, PostData);

            String Result = Encoding.ASCII.GetString(Response);
            int ResultCode = System.Convert.ToInt32(Result.Substring(0, 4));
            if (ResultCode != 0)
                return Result;
            else
                return "Success";
        }
    }
}
//</snippetAssignAutoNumberPlugin>
