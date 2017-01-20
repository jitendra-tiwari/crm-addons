//<snippetAssignAutoNumberPlugin>
using System;
using System.Linq;
// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Dotsquares.DCRM.Plugins
{
    public class AssignAutoNumberPlugin : IPlugin
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

        string _assemblyName = "AssignAutoNumberPlugin";
        string _pluginTypeName = "Dotsquares.DCRM.Plugins.AssignAutoNumberPlugin";

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
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];
                //</snippetAssignAutoNumberPlugin2>   
                string messageName = context.MessageName;

                if (entity.LogicalName == "dots_autonumber")
                {
                    if (messageName == "Create")
                    {

                        //for check duplicate field
                        //if (IsFieldExist(entity.LogicalName, entity.Attributes["new_targetattributelogicalname"].ToString()))
                        //    throw new InvalidPluginExecutionException(string.Format("Field {0} already exist in Entity {1}!", entity.Attributes["new_targetattributelogicalname"].ToString(), entity.LogicalName));
                        if (IsFieldExist2(entity.Attributes["new_targetentitylogicalname"].ToString(), entity.Attributes["new_targetattributelogicalname"].ToString()))
                            throw new InvalidPluginExecutionException(string.Format("Field {0} already exist in Entity {1}!", entity.Attributes["new_targetattributelogicalname"].ToString(), entity.Attributes["new_targetentitylogicalname"].ToString()));

                        // allow one fields for one entity
                        if (IsEntityExist2(entity.LogicalName, entity.Attributes["new_targetentitylogicalname"].ToString()))
                            throw new InvalidPluginExecutionException(string.Format("Already created AutoNumber field for Entity {0}!", entity.Attributes["new_targetentitylogicalname"].ToString()));



                        SdkMessageProcessingStep step = new SdkMessageProcessingStep
                        {
                            AsyncAutoDelete = false,
                            Mode = new OptionSetValue((int)CrmPluginStepMode.Synchronous),
                            Name = _pluginTypeName + ": " + entity.Attributes["new_name"].ToString(),
                            EventHandler = new EntityReference("plugintype", GetPluginTypeId()),
                            Rank = 1,
                            SdkMessageId = new EntityReference("sdkmessage", GetMessageId()),
                            Stage = new OptionSetValue((int)CrmPluginStepStage.PreOperation),
                            SupportedDeployment = new OptionSetValue((int)CrmPluginStepDeployment.ServerOnly),
                            SdkMessageFilterId = new EntityReference("sdkmessagefilter", GetSdkMessageFilterId(entity.Attributes["new_targetentitylogicalname"].ToString())),
                            Description = entity.Attributes["new_targetattributelogicalname"].ToString()
                        };
                        var SdkMessageProcessingStepId = OrganizationService.Create(step);

                        entity.Attributes.Add("new_sdkmessageprocessingstepid", SdkMessageProcessingStepId.ToString());
                    }
                   

                }
                else
                {

                    var entityDetail = GetEntityByName(entity.LogicalName);
                    if (entity.Attributes.Contains(entityDetail.TargetAttributeLogicalName) == false)
                    {
                        var InitalValue = GetNewCode(entityDetail);
                        var setValue = (InitalValue == null || InitalValue == "") ? "0" : InitalValue;
                        string[] splitValues;
                        if (setValue.Contains("#"))
                        {
                            splitValues = setValue.Split('#');
                            entity.Attributes.Add(entityDetail.TargetAttributeLogicalName, splitValues[0] + "" + splitValues[1]);
                            int incrementValue = Convert.ToInt32(splitValues[1]);
                            IncreaseCurrentNumber(entity.LogicalName, incrementValue);
                        }
                        else
                        {

                            entity.Attributes.Add(entityDetail.TargetAttributeLogicalName, setValue);
                            int incrementValue = Convert.ToInt32(setValue);
                            IncreaseCurrentNumber(entity.LogicalName, incrementValue);
                        }
                    }
                }
            }
            else if (context.InputParameters["Target"] is EntityReference)
            {
                if (context.PrimaryEntityName == "dots_autonumber")
                {
                    if (context.MessageName == "Delete")
                    {

                        Guid id = ((EntityReference)context.InputParameters["Target"]).Id;
                        DSAutoNumber entity2 = OrganizationService.Retrieve("dots_autonumber", ((EntityReference)context.InputParameters["Target"]).Id, new ColumnSet(true)).ToEntity<DSAutoNumber>();
                        OrganizationService.Delete("sdkmessageprocessingstep", new Guid(entity2.SdkMessageProcessingStepId.ToString()));


                    }
                }
            }
        }

        private string GetNewCode(DSAutoNumber entityDetail)
        {
              string newCode=entityDetail.FieldFormat;
             string InitializeNumber = entityDetail.InitializeNumber.ToString();

            if (string.IsNullOrEmpty(entityDetail.FieldFormat) && string.IsNullOrEmpty(entityDetail.InitializeNumber.ToString()))
            {
                int currentNumber;
                currentNumber=(entityDetail.CurrentNumber == null) ? 1 : entityDetail.CurrentNumber.Value;
                return currentNumber.ToString();
            }
           else if (!string.IsNullOrEmpty(entityDetail.FieldFormat) && !string.IsNullOrEmpty(entityDetail.InitializeNumber.ToString()))
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
                if (entityDetail.FieldFormat.Contains("{0000}"))
                    newCode = newCode.Replace("{0000}", "0000");

              
                int currentNumber;
                string result = "";
                currentNumber = (entityDetail.CurrentNumber == null) ? entityDetail.InitializeNumber.Value : entityDetail.CurrentNumber.Value;
                if (entityDetail.FieldFormat == "{0000}")
                {
                    int newcodelength = newCode.Length;
                    int numberlength = currentNumber.ToString().Length;

                    if (newCode.Length != numberlength)
                    {
                        newCode = newCode.Remove(newCode.ToString().Length - numberlength);
                        result = newCode + "#" + currentNumber.ToString();
                    }
                    if (numberlength >= newcodelength)
                    {
                        result = currentNumber.ToString();
                    }
                }
                else
                    result = newCode + "#" + currentNumber.ToString();

                return result;
            }
            else if(!string.IsNullOrEmpty(entityDetail.FieldFormat) && string.IsNullOrEmpty(entityDetail.InitializeNumber.ToString()))
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
                if (entityDetail.FieldFormat.Contains("{0000}"))
                    newCode = newCode.Replace("{0000}", "0000");

                
                int currentNumber;
                string result = "";
                currentNumber = (entityDetail.InitializeNumber == null && entityDetail.CurrentNumber == null) ? 1 : entityDetail.CurrentNumber.Value;

                if (entityDetail.FieldFormat == "{0000}")
                {
                    int newcodelength = newCode.Length;
                    int numberlength = currentNumber.ToString().Length;

                    if (newCode.Length != numberlength)
                    {
                        newCode = newCode.Remove(newCode.ToString().Length - numberlength);
                        result = newCode + "#" + currentNumber.ToString();
                    }
                    if (numberlength >= newcodelength)
                    {
                        result = currentNumber.ToString();
                    }
                }
              else
                    result = newCode + "#" + currentNumber.ToString();

                return result;
            }
         
            else // if entityDetail.FieldFormat null and entityDetail.InitializeNumber not null
            {
                int currentNumber;
                currentNumber = (entityDetail.InitializeNumber!= null && entityDetail.CurrentNumber == null) ? entityDetail.InitializeNumber.Value : entityDetail.CurrentNumber.Value;
                return currentNumber.ToString();
                                
            }
           
        }
        private bool IsFieldExist(String entityName, String fieldName)
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = entityName;
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);

            EntityCollection col = OrganizationService.RetrieveMultiple(query);

            Entity EntityName = null;
            if (col != null && col.Entities.Count > 0)
            {
                EntityName = col.Entities[0];

                var result = EntityName.Attributes.Where(o => o.Value.ToString() == fieldName).ToList();
                
                if (result.Count() > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }

        private bool IsFieldExist2(String entityName, String fieldName)
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = "dots_autonumber";
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("new_targetentitylogicalname", "new_targetattributelogicalname");

            EntityCollection col = OrganizationService.RetrieveMultiple(query);

            // Entity EntityName = null;
            if (col != null && col.Entities.Count > 0)
            {
                // EntityName = col.Entities[0];

                //var result = EntityName.Attributes.Where(o => o.Value.ToString() == fieldName).ToList();
                int count = 0;
                foreach (var act in col.Entities)
                {
                    var entity = act["new_targetentitylogicalname"].ToString();
                    var attr = act["new_targetattributelogicalname"].ToString();
                    if (entity == entityName && attr == fieldName)
                        count = count + 1;
                }
                if (count > 0)
                    return true;
                else
                    return false;
                //if (result.Count() > 0)
                //    return true;
                //else
                //    return false;
            }
            return false;

        }
        private bool IsEntityExist(String mainEntityName, String AssignedentityName)
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = mainEntityName;
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);
            EntityCollection col = OrganizationService.RetrieveMultiple(query);

            Entity EntityName = null;
            if (col != null && col.Entities.Count > 0)
            {
                EntityName = col.Entities[0];

                var result = EntityName.Attributes.Where(o => o.Value.ToString() == AssignedentityName).ToList();
                if (result.Count() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }

        private bool IsEntityExist2(String mainEntityName, String AssignedentityName)
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = mainEntityName;
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);
            EntityCollection col = OrganizationService.RetrieveMultiple(query);

           // Entity EntityName = null;
            if (col != null && col.Entities.Count > 0)
            {
                //EntityName = col.Entities[0];

                //var result = EntityName.Attributes.Where(o => o.Value.ToString() == AssignedentityName).ToList();
                int count = 0;
                foreach (var act in col.Entities)
                {
                    var entity = act["new_targetentitylogicalname"].ToString();                   
                    if (entity == AssignedentityName)
                        count = count + 1;
                }
                if (count > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }
        private DSAutoNumber GetEntityByName(string entityName)
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var autoNumberDetail = context.dots_autonumberSet.FirstOrDefault(w => w.TargetEntityLogicalName == entityName);
                return autoNumberDetail;
            }
        }

        private void IncreaseCurrentNumber(string entityName,int InitalValue)
        {
            using (XrmServiceContext context = new XrmServiceContext(OrganizationService))
            {
                var autoNumberDetail = context.dots_autonumberSet.FirstOrDefault(w => w.TargetEntityLogicalName == entityName);
                //autoNumberDetail.CurrentNumber = (Convert.ToInt32(autoNumberDetail.CurrentNumber) + 1).ToString();
                int currentNumber = ((autoNumberDetail.CurrentNumber == null) ? InitalValue : autoNumberDetail.CurrentNumber.Value);
                autoNumberDetail.CurrentNumber = ((currentNumber) + 1);
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
    }
}
//</snippetAssignAutoNumberPlugin>
