using AutoSMSGeneration;
using AutoSMSGeneration.Model;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DS.CRM
{

    public class InitSolutions
    {
        #region Class Level Members

        private OrganizationServiceProxy _serviceProxy;
        private readonly string _customizationPrefix = Constants.CustomizationPrefix;
        private readonly string _managedSolutionLocation = Constants.ManagedSolutionLocation;
        private readonly string _outputDir = Constants.OutputDirectory;
        // private readonly string _customEntityName = Constants.CustomEntityName;
        // private readonly string _customConfigurationEntityName = Constants.CustomConfigurationEntityName;

        private readonly string _customEntityName = AutoSMS._customEntityName;
        private readonly string _customConfigurationEntityName = AutoSMS._customConfigurationEntityName;
       
        private Guid _sitemapId;
        private Guid[] _webResourceIds = Constants.WebResourceIds;
        private Guid[] _webResourceIdForSolution = Constants.WebResourceIdForSolution;

        private readonly int _languageCode = Constants.LanguageCode;

        private Guid _solutionsSampleSolutionId = Guid.Empty;
        private Guid _crmSdkPublisherId = Guid.Empty;
               

        #endregion Class Level Members

        #region Public Funtions
        /// <summary>
        /// Shows how to perform the following tasks with solutions:
        /// - Create a Publisher
        /// - Retrieve the Default Publisher
        /// - Create a Solution
        /// - Retrieve a Solution
        /// - Add an existing Solution Component
        /// - Remove a Solution Component
        /// - Export or Package a Solution
        /// - Install or Upgrade a solution
        /// - Delete a Solution
        /// </summary>
        /// <param name="serverConfig">Contains server connection information.</param>
        /// <param name="promptForDelete">When True, the user will be prompted to delete all
        /// created entities.</param>
        public void Run(ServerConnection.Configuration serverConfig, bool promptForDelete)
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = new OrganizationServiceProxy(serverConfig.OrganizationUri, serverConfig.HomeRealmUri, serverConfig.Credentials, serverConfig.DeviceCredentials))
                {
                    // This statement is required to enable early-bound type support.
                      _serviceProxy.EnableProxyTypes();
                    CreateSiteMap();
                   // AutoNumberGeneration.AutoNumber.CreateWorkFlow(_serviceProxy, _languageCode);
                    AutoSMS.SetProxy(_serviceProxy, _languageCode);
                    // Call the method to create any data that this sample requires.
                    CreateRequiredRecords();
                }
            }

            // Catch any service fault exceptions that Microsoft Dynamics CRM throws.
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                // You can handle an exception here or pass it back to the calling method.
                throw;
            }
        }
        
        /// <summary>
        /// This method creates any entity records that this sample requires.
        /// </summary>
        public void CreateRequiredRecords()
        {
            // Create a managed solution for the Install or upgrade a solution sample

            Guid _tempPublisherId = new Guid();
            System.String _tempCustomizationPrefix = "ds";
            Guid _tempSolutionsSampleSolutionId = new Guid();
            Random rn = new Random();
            System.String _TempGlobalOptionSetName = "_TempSampleGlobalOptionSetName" + rn.Next();
            Boolean _publisherCreated = false;
            Boolean _solutionCreated = false;


            //Define a new publisher
            Publisher _crmSdkPublisher = new Publisher
            {
                UniqueName = Constants.PublisherUniqueName,
                FriendlyName = Constants.PublisherFriendlyName,
                SupportingWebsiteUrl = Constants.PublisherSupportingWebsiteUrl,
                CustomizationPrefix = Constants.PublisherCustomizationPrefix,
                EMailAddress = Constants.PublisherEmailAddress,
                Description = Constants.PublisherDescription
            };

            //Does publisher already exist?
            QueryExpression querySDKSamplePublisher = new QueryExpression
            {
                EntityName = Publisher.EntityLogicalName,
                ColumnSet = new ColumnSet("publisherid", "customizationprefix"),
                Criteria = new FilterExpression()
            };

            querySDKSamplePublisher.Criteria.AddCondition("uniquename", ConditionOperator.Equal, _crmSdkPublisher.UniqueName);
            EntityCollection querySDKSamplePublisherResults = _serviceProxy.RetrieveMultiple(querySDKSamplePublisher);
            Publisher SDKSamplePublisherResults = null;

            //If it already exists, use it
            if (querySDKSamplePublisherResults.Entities.Count > 0)
            {
                SDKSamplePublisherResults = (Publisher)querySDKSamplePublisherResults.Entities[0];
                _tempPublisherId = (Guid)SDKSamplePublisherResults.PublisherId;
                _tempCustomizationPrefix = SDKSamplePublisherResults.CustomizationPrefix;
            }
            //If it doesn't exist, create it
            if (SDKSamplePublisherResults == null)
            {
                _tempPublisherId = _serviceProxy.Create(_crmSdkPublisher);
                _tempCustomizationPrefix = _crmSdkPublisher.CustomizationPrefix;
                _publisherCreated = true;
            }

            //Upload only configuration page 
            UploadConfigurationPageForSolution();
            //SetWebResourceConfigurationForSolution();

            //Define a solution
            Solution solution = new Solution
            {
                UniqueName = Constants.SolutionUniqueName,
                FriendlyName = Constants.SolutionFriendlyName,
                PublisherId = new EntityReference(Publisher.EntityLogicalName, _tempPublisherId),
                Description = Constants.SolutionDescription,
                Version = Constants.SolutionVersion,
                ConfigurationPageId = new EntityReference(WebResource.EntityLogicalName, _webResourceIdForSolution[0])
            };

            //Check whether it already exists
            QueryExpression querySampleSolution = new QueryExpression
            {
                EntityName = Solution.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria = new FilterExpression()
            };
            querySampleSolution.Criteria.AddCondition("uniquename", ConditionOperator.Equal, solution.UniqueName);

            EntityCollection querySampleSolutionResults = _serviceProxy.RetrieveMultiple(querySampleSolution);
            Solution SampleSolutionResults = null;
            if (querySampleSolutionResults.Entities.Count > 0)
            {
                SampleSolutionResults = (Solution)querySampleSolutionResults.Entities[0];
                _tempSolutionsSampleSolutionId = (Guid)SampleSolutionResults.SolutionId;
            }
            
            if (SampleSolutionResults == null)
            {
                _tempSolutionsSampleSolutionId = _serviceProxy.Create(solution);
                _solutionCreated = true;
            }

            // Add a solution Component
            OptionSetMetadata optionSetMetadata = new OptionSetMetadata()
            {
                Name = _tempCustomizationPrefix + _TempGlobalOptionSetName,
                DisplayName = new Label("Example Option Set", _languageCode),
                IsGlobal = true,
                OptionSetType = OptionSetType.Picklist,
                Options =
                    {
                        new OptionMetadata(new Label("Option A", _languageCode), null),
                        new OptionMetadata(new Label("Option B", _languageCode), null )
                    }
            };
            CreateOptionSetRequest createOptionSetRequest = new CreateOptionSetRequest
            {
                OptionSet = optionSetMetadata,
                SolutionUniqueName = solution.UniqueName
            };
            
            _serviceProxy.Execute(createOptionSetRequest);

            //delete configuration entity
            if (IsEntityExist(_customEntityName) > 0)
            {
                DeleteEntityRequest customEntityNameFormField = new DeleteEntityRequest()
                {
                    LogicalName = _customEntityName,
                };
                _serviceProxy.Execute(customEntityNameFormField);
            }


            // Create the dots_autonumber entity.
            AutoSMS.DotsAutoNumberEntity();
            // CreateTab();


            //delete dots_configuration entity
            if (IsEntityExist(_customConfigurationEntityName) > 0)
            {
                DeleteEntityRequest customEntityNameFormField = new DeleteEntityRequest()
                {
                    LogicalName = _customConfigurationEntityName,
                };
                _serviceProxy.Execute(customEntityNameFormField);
            }


            //for create dots_configuration entity           
            AutoSMS.DotsAutoNumberConfigurationEntity();


            // assign dots_autonumber form entity to solution
            RetrieveEntityRequest retrievepowertEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = _customEntityName
            };


            RetrieveEntityResponse retrievepowerEntityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(retrievepowertEntityRequest);

            AddSolutionComponentRequest addReq = new AddSolutionComponentRequest()
            {
                ComponentType = 1,
                ComponentId = (Guid)retrievepowerEntityResponse.EntityMetadata.MetadataId,
                SolutionUniqueName = solution.UniqueName,
                AddRequiredComponents = true
            };
            _serviceProxy.Execute(addReq);


            //assign dots_configuration entity to solution
            RetrieveEntityRequest retrieveconfigurationtEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = _customConfigurationEntityName
            };


            RetrieveEntityResponse retrieveconfigEntityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(retrieveconfigurationtEntityRequest);

            AddSolutionComponentRequest addConfigReq = new AddSolutionComponentRequest()
            {
                ComponentType = 1,
                ComponentId = (Guid)retrieveconfigEntityResponse.EntityMetadata.MetadataId,
                SolutionUniqueName = solution.UniqueName,
                AddRequiredComponents = true
            };
            _serviceProxy.Execute(addConfigReq);

            //assign web resource to slution
            CreateWebResource(solution.UniqueName);

            //assign configuration page above created to solution
            AssiginConfigurationPageToSolution(_webResourceIdForSolution[0], solution.UniqueName);

            ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
            exportSolutionRequest.Managed = false;
            exportSolutionRequest.SolutionName = solution.UniqueName;

            ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)_serviceProxy.Execute(exportSolutionRequest);

            byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
            System.IO.Directory.CreateDirectory(_outputDir);
            File.WriteAllBytes(_managedSolutionLocation, exportXml);

            // Delete the solution and the components so it can be installed.

            DeleteOptionSetRequest delOptSetReq = new DeleteOptionSetRequest { Name = (_tempCustomizationPrefix + _TempGlobalOptionSetName).ToLower() };
            _serviceProxy.Execute(delOptSetReq);

            DeleteEntityRequest delEntReq = new DeleteEntityRequest { LogicalName = (_customEntityName) };
            _serviceProxy.Execute(delEntReq);

            DeleteEntityRequest delEntReqConfig = new DeleteEntityRequest { LogicalName = (_customConfigurationEntityName) };
            _serviceProxy.Execute(delEntReqConfig);


            if (_solutionCreated)
            {
                _serviceProxy.Delete(Solution.EntityLogicalName, _tempSolutionsSampleSolutionId);

                //delete webresorce 
                foreach (var _id in _webResourceIds)
                {
                    _serviceProxy.Delete(WebResource.EntityLogicalName, _id);
                }
                //for configuration page above created delete
                _serviceProxy.Delete(WebResource.EntityLogicalName, _webResourceIdForSolution[0]);
            }

            if (_publisherCreated)
            {
                _serviceProxy.Delete(Publisher.EntityLogicalName, _tempPublisherId);
            }

            Console.WriteLine("Managed Solution created and copied to {0}", _managedSolutionLocation);
        }

        public static List<CRMEntityModel> GetEntitiesDisplayNameAndLogicalName(IOrganizationService organizationService)
        {
            //RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
            //{
            //    EntityFilters = EntityFilters.Entity,
            //    RetrieveAsIfPublished = true
            //};

            //// Retrieve the MetaData.
            //RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)organizationService.Execute(request);
            //foreach (EntityMetadata currentEntity in response.EntityMetadata)
            //{

            //    var s = currentEntity.LogicalName;
            //    }

            //Dictionary<string, string> EntityDisplayAndLogicalname = new Dictionary<string, string>();
            List<CRMEntityModel> EntityDisplayAndLogicalname = new List<CRMEntityModel>();
            RetrieveAllEntitiesRequest metaDataRequest = new RetrieveAllEntitiesRequest();
            RetrieveAllEntitiesResponse metaDataResponse = new RetrieveAllEntitiesResponse();
            metaDataRequest.EntityFilters = EntityFilters.Entity;

            // Execute the request.

            metaDataResponse = (RetrieveAllEntitiesResponse)organizationService.Execute(metaDataRequest);

            var entities = metaDataResponse.EntityMetadata.ToList();

            foreach (EntityMetadata AEntity in entities)
            {
                //if(Entity.DisplayName.LocalizedLabels[i].Label!=null )
                // s = Entity.DisplayName.LocalizedLabels[i].Label;

                EntityDisplayAndLogicalname.Add(new CRMEntityModel { DisplayName = AEntity.DisplayName.UserLocalizedLabel != null ? AEntity.DisplayName.UserLocalizedLabel.Label : "Not Found", LogicalName = AEntity.LogicalName });


            }
            EntityDisplayAndLogicalname = EntityDisplayAndLogicalname.OrderBy(o => o.DisplayName).ToList();
            return EntityDisplayAndLogicalname;


        }

        public void GetDisplayNameAndLogicalNameOfFields(string selectedEntityName)
        {
            // Create the request
            RetrieveEntityRequest entityRequest = new RetrieveEntityRequest();
            // Retrieve only the currently published changes, ignoring the changes that have
            // not been published.
            // entityRequest.RetrieveAsIfPublished = false;
            entityRequest.LogicalName = selectedEntityName;
            entityRequest.EntityFilters = EntityFilters.Entity;
            entityRequest.EntityFilters = EntityFilters.Attributes;


            // Execute the request
            RetrieveEntityResponse entityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(entityRequest);

            // Access the retrieved entity
            EntityMetadata retrievedEntityMetadata = entityResponse.EntityMetadata;
            List<EntitiyFieldNameModel> entityFieldList = new List<EntitiyFieldNameModel>();
            //get the display name

            foreach (var attribute in retrievedEntityMetadata.Attributes)
            {

                entityFieldList.Add(new EntitiyFieldNameModel { FieldName = attribute.DisplayName.UserLocalizedLabel != null ? attribute.DisplayName.UserLocalizedLabel.Label : "Field name not found", Value = attribute.LogicalName });

            }
        }

        public List<EntitiyFieldNameModel> GetAllFieldsOfEntityWithValue(string selectedEntityName)
        {
            List<EntitiyFieldNameModel> entityFieldList = new List<EntitiyFieldNameModel>();

            QueryExpression query = new QueryExpression();
            query.EntityName = selectedEntityName;
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);

            EntityCollection col = _serviceProxy.RetrieveMultiple(query);

            Entity EntityName = null;
            if (col != null && col.Entities.Count > 0)
                EntityName = col.Entities[0];

            foreach (KeyValuePair<String, Object> attribute in EntityName.Attributes)
            {
                //Console.WriteLine(attribute.Key + ": " + attribute.Value);

                entityFieldList.Add(new EntitiyFieldNameModel { FieldName = attribute.Key, Value = attribute.Value.ToString() });
            }

            return entityFieldList;
           

        }

        public void CreateSiteMap()
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = "sitemap";
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);

            EntityCollection col = _serviceProxy.RetrieveMultiple(query);

            Entity sitemap = null;
            if (col != null && col.Entities.Count > 0)
                sitemap = col.Entities[0];


            string sitemapcontent = sitemap["sitemapxml"].ToString();
            XDocument sitemapxml = XDocument.Parse(sitemapcontent);

           // create new area
 //           sitemapxml.Element("SiteMap")
 //.Elements("Area")
 //.Where(x => (string)x.Attribute("Id") == "DotsquaresPack")
 //.Remove();

            XElement root = new XElement("Area");
            root.Add(new XAttribute("Id", "DotsquaresPack"),
                new XAttribute("ShowGroups", "true"),
                new XAttribute("Title", "DotsquaresPack"));
            //root.Add(new XElement("Group",
            //   new XAttribute("Id", "Group_SubDotsquaresWebForm"),
            //   new XAttribute("Title", "DotsquaresAutoNumber"),
            //   new XElement("SubArea", new XAttribute("Id", "SubArea_dots_autonumber"),
            //   new XAttribute("Entity", "dots_autonumber")
            //   )));
            root.Add(new XElement("Group",
                new XAttribute("Id", "Group_SubDotsquaresAutoSMSWebForm"),
                new XAttribute("Title", "DotsquaresAutoSMS"),
                new XElement("SubArea", new XAttribute("Id", "SubArea_dots_autosms"),
                new XAttribute("Entity", _customEntityName)
                )));


            sitemapxml.Element("SiteMap").Add(root);           


            sitemap["sitemapxml"] = sitemapxml.ToString();
            _serviceProxy.Update(sitemap);

            PublishXmlRequest request = new PublishXmlRequest();
            request.ParameterXml = "<importexportxml><sitemaps><sitemap></sitemap></sitemaps></importexportxml>";
            _serviceProxy.Execute(request);
        }

        public void RetriveRecordByCondition()
        {
            RetrieveMultipleRequest rmr = new RetrieveMultipleRequest();
            RetrieveMultipleResponse resp = new RetrieveMultipleResponse();
            WebResource wb = new WebResource();

            QueryExpression query = new QueryExpression()
            {
                EntityName = "webresource",
                ColumnSet = new ColumnSet("content"),
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                    new ConditionExpression
                    {
                        AttributeName = "webresourceid",
                        Operator = ConditionOperator.Equal,
                        Values = { "572b5e08-289d-e411-80d5-000001010103" }
                    }
                            }
                }
            };

            rmr.Query = query;
            resp = (RetrieveMultipleResponse)_serviceProxy.Execute(rmr);
            wb = (WebResource)resp.EntityCollection.Entities[0];
            var s = wb.SolutionId;
            byte[] b = Convert.FromBase64String(wb.Content);

            //**************************************
            //The string below will contain your HTML
            //**************************************
            string strHTML = System.Text.Encoding.UTF8.GetString(b);
        }

        public void RetriveRecord()
        {
            RetrieveMultipleRequest rmr = new RetrieveMultipleRequest();
            RetrieveMultipleResponse resp = new RetrieveMultipleResponse();
            SiteMap wb = new SiteMap();

            QueryExpression query = new QueryExpression()
            {
                EntityName = "sitemap",
                ColumnSet = new ColumnSet(true),

            };

            rmr.Query = query;
            resp = (RetrieveMultipleResponse)_serviceProxy.Execute(rmr);
            wb = (SiteMap)resp.EntityCollection.Entities[0];
            _sitemapId = wb.SiteMapId.Value;
            // byte[] b = Convert.FromBase64String(wb.Content);

            //**************************************
            //The string below will contain your HTML
            //**************************************
            //string strHTML = System.Text.Encoding.UTF8.GetString(b);
        }

        public void AssignSiteMap(Guid SesId, string _ImportWebResourcesSolutionUniqueName)
        {
            SiteMap sMap = new SiteMap();

            sMap.Id = SesId;

            UpdateRequest updateSiteMapResourceItem = new UpdateRequest()
            {
                Target = sMap,
            };
            updateSiteMapResourceItem.Parameters.Add("SolutionUniqueName", _ImportWebResourcesSolutionUniqueName);
            _serviceProxy.Execute(updateSiteMapResourceItem);
        }

        public void AssiginConfigurationPageToSolution(Guid WesId, string _ImportWebResourcesSolutionUniqueName)
        {
            WebResource wes = new WebResource();
            wes.WebResourceId = WesId;

            UpdateRequest updateWebResourceItem = new UpdateRequest()
            {
                Target = wes,
            };
            updateWebResourceItem.Parameters.Add("SolutionUniqueName", _ImportWebResourcesSolutionUniqueName);
            _serviceProxy.Execute(updateWebResourceItem);
        }
        
        public void GetRibbon()
        {

            // assign entity to solution
            RetrieveEntityRequest retrievepowertEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = SiteMap.EntityLogicalName,
            };
            RetrieveEntityResponse retrievepowerEntityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(retrievepowertEntityRequest);
            
        }

        public void CreateTab()
        {

            List<TabParametersModel> tnt = new List<TabParametersModel>();
            List<EntityFields> ent = new List<EntityFields>();
            ent.Add(new EntityFields { ControlId = "new_name", DataFieldName = "new_name", FieldDisplayName = "Name" });
            ent.Add(new EntityFields { ControlId = "new_email", DataFieldName = "new_email", FieldDisplayName = "Email" });
            ent.Add(new EntityFields { ControlId = "new_message", DataFieldName = "new_message", FieldDisplayName = "Message" });

            tnt.Add(new TabParametersModel { TabName = "tab_custom", TabDisplayName = "Custom Tab", TabSectionName = "tab_section", TabSectionDisplayName = "New Section", EntityFields = ent });


            CopySetTabandFieldsData(_customEntityName, tnt);
        }

        public int IsEntityExist(string entityName)
        {
            try
            {
                RetrieveEntityRequest entityReq = new RetrieveEntityRequest();
                // entityReq.EntityFilters = EntityFilters.Entity;
                entityReq.LogicalName = entityName;
                RetrieveEntityResponse resp = (RetrieveEntityResponse)_serviceProxy.Execute(entityReq);

                return resp.Results.Count;
            }
            catch { return 0; }
        }

        public StringBuilder CopySetTabandFieldsData(string entityLogicalName, List<TabParametersModel> tabParm)
        {

            QueryExpression qe = new QueryExpression("systemform");
            qe.Criteria.AddCondition("type", ConditionOperator.Equal, 2); //main form
            qe.Criteria.AddCondition("objecttypecode", ConditionOperator.Equal, entityLogicalName); //for new_bankaccount entity
            qe.ColumnSet.AddColumn("formxml");
            //Retrieve the first main entity form for this entity
            SystemForm bankAccountMainForm = (SystemForm)_serviceProxy.RetrieveMultiple(qe).Entities[0];

            XDocument bankAccountFormXml = XDocument.Parse(bankAccountMainForm.FormXml);
            //Set the showImage attribute so the entity image will be displayed
            bankAccountFormXml.Root.SetAttributeValue("showImage", true);
            bankAccountFormXml.Element("form")
   .Element("tabs")
   .Elements("tab")
   .Where(x => (string)x.Attribute("verticallayout") == "true")
   .Remove();

            StringBuilder MyStringBuilder = new StringBuilder();
            int i = 1;
            foreach (var tab in tabParm)
            {

                i += 1;
                Guid tabId = Guid.NewGuid();
                Guid sectioId = Guid.NewGuid();
                if (i > 2)
                {
                    MyStringBuilder.AppendLine();
                    MyStringBuilder.Append("<tab name=\"" + tab.TabName + "\" id=\"{" + tabId + "}\" IsUserDefined=\"0\" locklevel=\"0\" showlabel=\"true\"  expanded=\"true\" >");
                }
                else
                    MyStringBuilder.Append("<tab name=\"" + tab.TabName + "\" id=\"{" + tabId + "}\" IsUserDefined=\"0\" locklevel=\"0\" showlabel=\"true\"  expanded=\"true\" >");

                MyStringBuilder.Append("<labels><label description=\"" + tab.TabDisplayName + "\" languagecode=\"1033\" /></labels>");
                MyStringBuilder.Append("<columns><column width=\"100%\"><sections><section name=\"" + tab.TabSectionName + "\" showlabel=\"false\"  showbar=\"false\" locklevel=\"0\" id=\"{" + sectioId + "}\" IsUserDefined=\"0\" layout=\"varwidth\" columns=\"1\" labelwidth=\"115\" celllabelalignment=\"Left\" celllabelposition=\"Left\" ><labels><label description=\"" + tab.TabSectionDisplayName + "\" languagecode=\"1033\" /></labels>");
                MyStringBuilder.Append("<rows>");
                foreach (var field in tab.EntityFields)
                {
                    Guid cellId = Guid.NewGuid();
                    Guid classId = Guid.NewGuid();
                    if (field.IsGrid)
                        MyStringBuilder.Append("<row><cell id=\"{" + cellId + "}\" showlabel=\"true\" locklevel=\"0\" > <labels><label description=\"" + field.FieldDisplayName + "\" languagecode=\"1033\" /> </labels><control id=\"" + field.ControlId + "\" classid=\"{" + classId + "}\"  disabled=\"false\"><parameters><ViewId>" + field.ViewId + "</ViewId><IsUserView>false</IsUserView><RelationshipName>New_Parent_powerformId</RelationshipName><TargetEntityType>new_powerformfieldsid</TargetEntityType><AutoExpand>Fixed</AutoExpand><EnableQuickFind>false</EnableQuickFind><EnableViewPicker>false</EnableViewPicker><ViewIds>" + field.ViewId + "</ViewIds ><EnableJumpBar>false</EnableJumpBar><ChartGridMode>Grid</ChartGridMode><VisualizationId/><IsUserChart>false</IsUserChart><EnableChartPicker>false</EnableChartPicker><RecordsPerPage>10</RecordsPerPage></parameters></control></cell></row>");
                    else
                        MyStringBuilder.Append("<row><cell id=\"{" + cellId + "}\" showlabel=\"true\" locklevel=\"0\" > <labels><label description=\"" + field.FieldDisplayName + "\" languagecode=\"1033\" /> </labels><control id=\"" + field.ControlId + "\" classid=\"{" + classId + "}\" datafieldname=\"" + field.DataFieldName + "\" disabled=\"false\" /></cell></row>");
                }

                MyStringBuilder.Append("</rows>");
                MyStringBuilder.Append("</section></sections></column></columns>");
                MyStringBuilder.Append("</tab>");
            }          

            var sd = MyStringBuilder.ToString();

            if (!String.IsNullOrEmpty(MyStringBuilder.ToString()))
            {
                using (StringReader reader = new StringReader(sd))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {

                        XDocument formTabXml = XDocument.Parse(line.ToString());
                        //Adding this tab to the tabs element       
                        var s = formTabXml.Root;
                        bankAccountFormXml.Root.Element("tabs").Add(formTabXml.Root);


                    }
                    //Updateing the entity form definition
                    bankAccountMainForm.FormXml = bankAccountFormXml.ToString();
                    //saving the bank account form
                    _serviceProxy.Update(bankAccountMainForm);

                    // Customizations must be published after an entity is updated.
                    PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
                    _serviceProxy.Execute(publishRequest);
                    Console.WriteLine("\nCustomizations were published.");

                }
            }



            return MyStringBuilder;

        }

        public void PublishForm(Entity en)
        {
            var request = new PublishXmlRequest { ParameterXml = String.Format("<importexportxml><entities><entity>{0}</entity></entities></importexportxml>", en.GetAttributeValue<string>("objecttypecode")) };
            _serviceProxy.Execute(request);
        }

        public void CreateWebResource(string _ImportWebResourcesSolutionUniqueName)
        {
            //<snippetImportWebResources1>

            //Read the descriptive data from the XML file
            XDocument xmlDoc = XDocument.Load("../../ImportJob.xml");

            //Create a collection of anonymous type references to each of the Web Resources
            var webResources = from webResource in xmlDoc.Descendants("webResource")
                               select new
                               {
                                   path = webResource.Element("path").Value,
                                   displayName = webResource.Element("displayName").Value,
                                   description = webResource.Element("description").Value,
                                   name = webResource.Element("name").Value,
                                   type = webResource.Element("type").Value

                               };

            // Loop through the collection creating Web Resources
            int counter = 0;
            foreach (var webResource in webResources)
            {
                //<snippetImportWebResources2>
                //Set the Web Resource properties
                WebResource wr = new WebResource
                {
                    Content = GetEncodedFileContents(@"../../" + webResource.path),
                    DisplayName = webResource.displayName,
                    Description = webResource.description,
                    Name = _customizationPrefix + webResource.name,
                    LogicalName = WebResource.EntityLogicalName,
                    WebResourceType = new OptionSetValue(Int32.Parse(webResource.type)),
                    //IsCustomizable=new BooleanManagedProperty(false)
                };

                // Using CreateRequest because we want to add an optional parameter
                CreateRequest cr = new CreateRequest
                {
                    Target = wr
                };
                //Set the SolutionUniqueName optional parameter so the Web Resources will be
                // created in the context of a specific solution.
                cr.Parameters.Add("SolutionUniqueName", _ImportWebResourcesSolutionUniqueName);

                CreateResponse cresp = (CreateResponse)_serviceProxy.Execute(cr);
                //</snippetImportWebResources2>
                // Capture the id values for the Web Resources so the sample can delete them.
                _webResourceIds[counter] = cresp.id;
                counter++;
                Console.WriteLine("Created Web Resource: {0}", webResource.displayName);
            }

            //</snippetImportWebResources1>
        }

        public void UploadConfigurationPageForSolution()
        {
            //<snippetImportWebResources1>

            //Read the descriptive data from the XML file
            XDocument xmlDoc = XDocument.Load("../../ImportConfiguration.xml");

            //Create a collection of anonymous type references to each of the Web Resources
            var webResources = (from webResource in xmlDoc.Descendants("webResource")
                                select new
                                {
                                    path = webResource.Element("path").Value,
                                    displayName = webResource.Element("displayName").Value,
                                    description = webResource.Element("description").Value,
                                    name = webResource.Element("name").Value,
                                    type = webResource.Element("type").Value
                                }).FirstOrDefault();

            // Loop through the collection creating Web Resources
            int counter = 0;
            
            //Set the Web Resource properties
            WebResource wr = new WebResource
            {
                Content = GetEncodedFileContents(@"../../" + webResources.path),
                DisplayName = webResources.displayName,
                Description = webResources.description,
                Name = _customizationPrefix + webResources.name,
                LogicalName = WebResource.EntityLogicalName,
                WebResourceType = new OptionSetValue(Int32.Parse(webResources.type)),
                IsCustomizable = new BooleanManagedProperty(false)
            };

            // Using CreateRequest because we want to add an optional parameter
            CreateRequest cr = new CreateRequest
            {
                Target = wr
            };
            
            CreateResponse cresp = (CreateResponse)_serviceProxy.Execute(cr);
            
            // Capture the id values for the Web Resources so the sample can delete them.
            _webResourceIdForSolution[counter] = cresp.id;

            Console.WriteLine("Created Web Resource for solution configuration: {0}", webResources.displayName);


            //</snippetImportWebResources1>
        }
        
        static public string GetEncodedFileContents(String pathToFile)
        {
            FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);
            byte[] binaryData = new byte[fs.Length];
            long bytesRead = fs.Read(binaryData, 0, (int)fs.Length);
            fs.Close();
            return System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
        }

        /// <summary>
        /// Deletes any entity records that were created for this sample.
        /// <param name="prompt">Indicates whether to prompt the user to delete the records created in this sample.</param>
        /// </summary>
        public void DeleteRequiredRecords(bool prompt)
        {
            bool deleteRecords = true;

            if (prompt)
            {
                Console.WriteLine("\nDo you want these entity records deleted? (y/n)");
                String answer = Console.ReadLine();

                deleteRecords = (answer.StartsWith("y") || answer.StartsWith("Y"));
            }

            if (deleteRecords)
            {
                _serviceProxy.Delete(Solution.EntityLogicalName, _solutionsSampleSolutionId);
                _serviceProxy.Delete(Publisher.EntityLogicalName, _crmSdkPublisherId);
                // Remove the managed solution created by the create required fields code.
                File.Delete(_managedSolutionLocation);


                Console.WriteLine("Entity records have been deleted.");
            }
        }

        #endregion

        #region Main
        /// <summary>
        /// Standard Main() method used by most SDK samples.
        /// </summary>
        /// <param name="args"></param>
        static public void Main(string[] args)
        {
            try
            {
                // Obtain the target organization's Web address and client logon 
                // credentials from the user.
                ServerConnection serverConnect = new ServerConnection();
                ServerConnection.Configuration config = serverConnect.GetServerConfiguration();

                InitSolutions app = new InitSolutions();
                app.Run(config, true);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Message: {0}", ex.Message);
                Console.WriteLine("Stack Trace: {0}", ex.StackTrace);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine(ex.Message);

                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;
                    if (fe != null)
                    {
                        Console.WriteLine("Timestamp: {0}", fe.Detail.Timestamp);
                        Console.WriteLine("Code: {0}", fe.Detail.ErrorCode);
                        Console.WriteLine("Message: {0}", fe.Detail.Message);
                        Console.WriteLine("Plugin Trace: {0}", fe.Detail.TraceText);
                        Console.WriteLine("Inner Fault: {0}",
                            null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    }
                }
            }
            // Additional exceptions to catch: SecurityTokenValidationException, ExpiredSecurityTokenException,
            // SecurityAccessDeniedException, MessageSecurityException, and SecurityNegotiationException.
            finally
            {
                Console.WriteLine("Press <Enter> to exit.");
                Console.ReadLine();
            }
        }
        #endregion
    }  
}