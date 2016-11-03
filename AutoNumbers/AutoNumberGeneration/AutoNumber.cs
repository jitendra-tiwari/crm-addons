using AutoNumberGeneration.Model;
using Microsoft.Crm.Sdk.Messages;
//using Microsoft.Crm.Sdk.Samples;
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
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AutoNumberGeneration
{
    public  class AutoNumber
    {
        OrganizationServiceProxy _serviceProxy;
        private System.String _customizationPrefix = "dots001";
        System.String ManagedSolutionLocation = @"D:\temp\ManagedSolutionForImportExample.zip";
        String outputDir = @"D:\temp\";
        private string _customEntityName = "dots_forminformation001";
        private string _customConfigurationEntityName = "dots_configuration001";
        private Guid SitemapId;

        private System.Guid[] _webResourceIds = new System.Guid[9];
        private System.Guid[] _webResourceIdForSolution = new System.Guid[1];

        // Specify which language code to use in the sample. If you are using a language
        // other than US English, you will need to modify this value accordingly.
        // See http://msdn.microsoft.com/en-us/library/0h88fahh.aspx
        private const int _languageCode = 1033;
        private Guid _solutionsSampleSolutionId;
        private Guid _crmSdkPublisherId;

        public  void CreateRequiredRecords(OrganizationServiceProxy _newserviceProxy)
        {
            _newserviceProxy.EnableProxyTypes();

            // Create a managed solution for the Install or upgrade a solution sample

            Guid _tempPublisherId = new Guid();
            System.String _tempCustomizationPrefix = "new";
            Guid _tempSolutionsSampleSolutionId = new Guid();
            Random rn = new Random();
            System.String _TempGlobalOptionSetName = "_TempSampleGlobalOptionSetName" + rn.Next();
            Boolean _publisherCreated = false;
            Boolean _solutionCreated = false;


            //Define a new publisher
            Publisher _crmSdkPublisher = new Publisher
            {
                UniqueName = "sdksamples",
                FriendlyName = "Microsoft CRM SDK Samples",
                SupportingWebsiteUrl = "http://msdn.microsoft.com/en-us/dynamics/crm/default.aspx",
                CustomizationPrefix = "sample",
                EMailAddress = "someone@microsoft.com",
                Description = "This publisher was created with samples from the Microsoft Dynamics CRM SDK"

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


            //Create a Solution
            //Define a solution
            Solution solution = new Solution
            {
                UniqueName = "samplesolutionforImport",
                FriendlyName = "Sample Solution for Import",
                PublisherId = new EntityReference(Publisher.EntityLogicalName, _tempPublisherId),
                Description = "This solution was created by the WorkWithSolutions sample code in the Microsoft Dynamics CRM SDK samples.",
                Version = "1.0",
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

            //<snippetWorkWithSolutions5>
            // Add an existing Solution Component
            //Add the custom entity to the solution
            // Create the custom entity.

            CreateEntityRequest createformInforequest = new CreateEntityRequest
            {

                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customEntityName,
                    DisplayName = new Label("Sample WebForm", 1033),
                    DisplayCollectionName = new Label("Sample WebForms", 1033),
                    Description = new Label("An entity to store information about user webform", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "new_name",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Name", 1033),
                    Description = new Label("The primary attribute for the Power WebForm entity.", 1033),

                }


            };
            var _new_powerformEntity = _serviceProxy.Execute(createformInforequest);

            // Add some attributes to the Power WebForm entity
            CreateAttributeRequest createEmailAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_email",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Email,
                    DisplayName = new Label("Email", 1033),
                    Description = new Label("The Notify E-Mail.", 1033),
                }
            };

            _serviceProxy.Execute(createEmailAttributeRequest);

            CreateAttributeRequest createMessageAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_message",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.TextArea,
                    DisplayName = new Label("Message", 1033),
                    Description = new Label("The Message.", 1033),
                }
            };

            _serviceProxy.Execute(createMessageAttributeRequest);
            // CreateTab();


            //delete configuration entity
            if (IsEntityExist(_customConfigurationEntityName) > 0)
            {
                DeleteEntityRequest customEntityNameFormField = new DeleteEntityRequest()
                {
                    LogicalName = _customConfigurationEntityName,
                };
                _serviceProxy.Execute(customEntityNameFormField);
            }


            //for create configuration entity           
            CreateEntityRequest createrequest = new CreateEntityRequest
            {

                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customConfigurationEntityName,
                    DisplayName = new Label("Sample Configuration", 1033),
                    DisplayCollectionName = new Label("Sample Configuration", 1033),
                    Description = new Label("An entity to store information about user details", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "new_serverurl",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                    MaxLength = 200,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("ServerUrl", 1033),
                    Description = new Label("The primary attribute for the configuration entity.", 1033),

                }


            };
            _serviceProxy.Execute(createrequest);

            // Add some attributes to the Configuration entity
            CreateAttributeRequest createRegisterIdAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customConfigurationEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_registerid",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("RegisterId", 1033),
                    Description = new Label("The RegisterId.", 1033),
                }
            };
            _serviceProxy.Execute(createRegisterIdAttributeRequest);


            CreateAttributeRequest createorguniquenameAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customConfigurationEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_orguniquename",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("OrgUniqueName", 1033),
                    Description = new Label("The OrgUniqueName.", 1033),
                }
            };

            _serviceProxy.Execute(createorguniquenameAttributeRequest);

            CreateAttributeRequest createusernameAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customConfigurationEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_username",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("UserName", 1033),
                    Description = new Label("The UserName value.", 1033),
                }
            };

            _serviceProxy.Execute(createusernameAttributeRequest);


            CreateAttributeRequest createpasswordAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customConfigurationEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_password",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Password", 1033),
                    Description = new Label("The Password value.", 1033),
                }
            };

            _serviceProxy.Execute(createpasswordAttributeRequest);


            // assign entity sample form entity to solution
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


            //assign configuration entity to solution
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
            createWebResource(solution.UniqueName);

            //assign configuration page above creted to solution
            AssiginConfigurationPageToSolution(_webResourceIdForSolution[0], solution.UniqueName);

            ////assign sitemap to solution
            //RetrieveEntityRequest retrieveSiteEntityRequest = new RetrieveEntityRequest
            //{
            //    EntityFilters = EntityFilters.Entity,
            //    LogicalName = SiteMap.EntityLogicalName,

            //};
            //RetrieveEntityResponse retrieveSiteEntityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(retrieveSiteEntityRequest);

            //AddSolutionComponentRequest addReq2 = new AddSolutionComponentRequest()
            //{
            //    ComponentType = 1,
            //    ComponentId = (Guid)retrieveSiteEntityResponse.EntityMetadata.MetadataId,
            //    SolutionUniqueName = solution.UniqueName,
            //    AddRequiredComponents = true
            //};
            //_serviceProxy.Execute(addReq2);

            //Export an a solution





            ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
            exportSolutionRequest.Managed = false;
            exportSolutionRequest.SolutionName = solution.UniqueName;

            ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)_serviceProxy.Execute(exportSolutionRequest);

            byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
            System.IO.Directory.CreateDirectory(outputDir);
            File.WriteAllBytes(ManagedSolutionLocation, exportXml);

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


            Console.WriteLine("Managed Solution created and copied to {0}", ManagedSolutionLocation);



        }


        public  List<getAllEntitiesModel> GetEntitiesDisplayNameAndLogicalName(IOrganizationService organizationService)
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
            List<getAllEntitiesModel> EntityDisplayAndLogicalname = new List<getAllEntitiesModel>();
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

                EntityDisplayAndLogicalname.Add(new getAllEntitiesModel { DisplayName = AEntity.DisplayName.UserLocalizedLabel != null ? AEntity.DisplayName.UserLocalizedLabel.Label : "Not Found", LogicalName = AEntity.LogicalName });


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
            List<getEntitiyFieldNames> entityFieldList = new List<getEntitiyFieldNames>();
            //get the display name

            foreach (var attribute in retrievedEntityMetadata.Attributes)
            {

                entityFieldList.Add(new getEntitiyFieldNames { FieldName = attribute.DisplayName.UserLocalizedLabel != null ? attribute.DisplayName.UserLocalizedLabel.Label : "Field name not found", Value = attribute.LogicalName });

            }
        }

        public List<getEntitiyFieldNames> GetAllFieldsOfEntityWithValue(string selectedEntityName)
        {
            List<getEntitiyFieldNames> entityFieldList = new List<getEntitiyFieldNames>();

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

                entityFieldList.Add(new getEntitiyFieldNames { FieldName = attribute.Key, Value = attribute.Value.ToString() });
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

            //create new area

            //           sitemapxml.Element("SiteMap")
            //.Elements("Area")
            //.Where(x => (string)x.Attribute("Id") == "WebPowerPack")
            //.Remove();

            XElement root = new XElement("Area");
            root.Add(new XAttribute("Id", "WebPowerPack"),
                new XAttribute("ShowGroups", "true"),
                new XAttribute("Title", "WebPowerPack"));
            root.Add(new XElement("Group",
                new XAttribute("Id", "Group_SubWebPowerWebForm"),
                new XAttribute("Title", "SubWebPowerWebForm"),
                new XElement("SubArea", new XAttribute("Id", "SubArea_dots_forminformation"),
                new XAttribute("Entity", _customEntityName)
                )));


            sitemapxml.Element("SiteMap").Add(root);


            //XElement root = new XElement("Area");
            //root.Add(new XAttribute("Id", "WebPowerPack"), new XAttribute("ShowGroups", "true"), new XAttribute("Title", "WebPowerPack"));
            //root.Add(new XElement("Group", new XAttribute("Id", "Group_SubWebPowerWebForm"), new XAttribute("Title", "SubWebPowerWebForm"),
            //    new XElement("SubArea", new XAttribute("Id", "SubArea_dots_forminformation"), new XAttribute("Entity", "dots_forminformation")
            //    )));


            //sitemapxml.Element("SiteMap").Add(root);


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
            SitemapId = wb.SiteMapId.Value;
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



        public void getRibbon()
        {

            // assign entity to solution
            RetrieveEntityRequest retrievepowertEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = SiteMap.EntityLogicalName,
            };
            RetrieveEntityResponse retrievepowerEntityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(retrievepowertEntityRequest);
            //RetrieveEntityRibbonRequest entRibReq = new RetrieveEntityRibbonRequest() { RibbonLocationFilter = RibbonLocationFilters.All };
            ////Check for custom entities
            //RetrieveAllEntitiesRequest raer = new RetrieveAllEntitiesRequest() { EntityFilters = EntityFilters.Entity };

            //RetrieveAllEntitiesResponse resp = (RetrieveAllEntitiesResponse)_serviceProxy.Execute(raer);

            //foreach (EntityMetadata em in resp.EntityMetadata)
            //{
            //    if (em.IsCustomEntity == true )
            //    {
            //        if (em.LogicalName == _customEntityName)
            //        {
            //            entRibReq.EntityName = em.LogicalName;
            //            RetrieveEntityRibbonResponse entRibResp = (RetrieveEntityRibbonResponse)_serviceProxy.Execute(entRibReq);
            //        }


            //    }
            //}
        }

        public void CreateTab()
        {

            List<tabParameters> tnt = new List<tabParameters>();
            List<entityFields> ent = new List<entityFields>();
            ent.Add(new entityFields { controlId = "new_name", dataFieldName = "new_name", fieldDisplayName = "Name" });
            ent.Add(new entityFields { controlId = "new_email", dataFieldName = "new_email", fieldDisplayName = "Email" });
            ent.Add(new entityFields { controlId = "new_message", dataFieldName = "new_message", fieldDisplayName = "Message" });

            tnt.Add(new tabParameters { tabName = "tab_custom", tabDisplayName = "Custom Tab", tabSectionName = "tab_section", tabSectionDisplayName = "New Section", entityFields = ent });


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
            catch (Exception ex)
            {
                return 0;

            }
        }
        public StringBuilder CopySetTabandFieldsData(string entityLogicalName, List<tabParameters> tabParm)
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
                    MyStringBuilder.Append("<tab name=\"" + tab.tabName + "\" id=\"{" + tabId + "}\" IsUserDefined=\"0\" locklevel=\"0\" showlabel=\"true\"  expanded=\"true\" >");
                }
                else
                    MyStringBuilder.Append("<tab name=\"" + tab.tabName + "\" id=\"{" + tabId + "}\" IsUserDefined=\"0\" locklevel=\"0\" showlabel=\"true\"  expanded=\"true\" >");

                MyStringBuilder.Append("<labels><label description=\"" + tab.tabDisplayName + "\" languagecode=\"1033\" /></labels>");
                MyStringBuilder.Append("<columns><column width=\"100%\"><sections><section name=\"" + tab.tabSectionName + "\" showlabel=\"false\"  showbar=\"false\" locklevel=\"0\" id=\"{" + sectioId + "}\" IsUserDefined=\"0\" layout=\"varwidth\" columns=\"1\" labelwidth=\"115\" celllabelalignment=\"Left\" celllabelposition=\"Left\" ><labels><label description=\"" + tab.tabSectionDisplayName + "\" languagecode=\"1033\" /></labels>");
                MyStringBuilder.Append("<rows>");
                foreach (var field in tab.entityFields)
                {
                    Guid cellId = Guid.NewGuid();
                    Guid classId = Guid.NewGuid();
                    if (field.IsGrid)
                        MyStringBuilder.Append("<row><cell id=\"{" + cellId + "}\" showlabel=\"true\" locklevel=\"0\" > <labels><label description=\"" + field.fieldDisplayName + "\" languagecode=\"1033\" /> </labels><control id=\"" + field.controlId + "\" classid=\"{" + classId + "}\"  disabled=\"false\"><parameters><ViewId>" + field.viewId + "</ViewId><IsUserView>false</IsUserView><RelationshipName>New_Parent_powerformId</RelationshipName><TargetEntityType>new_powerformfieldsid</TargetEntityType><AutoExpand>Fixed</AutoExpand><EnableQuickFind>false</EnableQuickFind><EnableViewPicker>false</EnableViewPicker><ViewIds>" + field.viewId + "</ViewIds ><EnableJumpBar>false</EnableJumpBar><ChartGridMode>Grid</ChartGridMode><VisualizationId/><IsUserChart>false</IsUserChart><EnableChartPicker>false</EnableChartPicker><RecordsPerPage>10</RecordsPerPage></parameters></control></cell></row>");
                    else
                        MyStringBuilder.Append("<row><cell id=\"{" + cellId + "}\" showlabel=\"true\" locklevel=\"0\" > <labels><label description=\"" + field.fieldDisplayName + "\" languagecode=\"1033\" /> </labels><control id=\"" + field.controlId + "\" classid=\"{" + classId + "}\" datafieldname=\"" + field.dataFieldName + "\" disabled=\"false\" /></cell></row>");
                }

                MyStringBuilder.Append("</rows>");
                MyStringBuilder.Append("</section></sections></column></columns>");
                MyStringBuilder.Append("</tab>");
            }

            //QueryExpression qe = new QueryExpression();
            //qe.EntityName = "account";
            //qe.ColumnSet = new ColumnSet { AllColumns = true };


            // qe.LinkEntities.Add(new LinkEntity("account", "contact", "primarycontactid", "contactid", JoinOperator.Inner));
            // qe.LinkEntities[0].Columns.AddColumns("firstname", "lastname");
            // qe.LinkEntities[0].EntityAlias = "primarycontact";

            //EntityCollection ec = _serviceProxy.RetrieveMultiple(qe);

            //RetrieveEntityRequest entityReq = new RetrieveEntityRequest();
            //// entityReq.EntityFilters = EntityFilters.Entity;
            //entityReq.LogicalName = _customEntityName;
            //RetrieveEntityResponse resp = (RetrieveEntityResponse)_serviceProxy.Execute(entityReq);

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
        public void createWebResource(string _ImportWebResourcesSolutionUniqueName)
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
                    Content = getEncodedFileContents(@"../../" + webResource.path),
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
            XDocument xmlDoc = XDocument.Load("../../ImportJob2.xml");

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

            //<snippetImportWebResources2>
            //Set the Web Resource properties
            WebResource wr = new WebResource
            {
                Content = getEncodedFileContents(@"../../" + webResources.path),
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
            //Set the SolutionUniqueName optional parameter so the Web Resources will be
            // created in the context of a specific solution.
            //cr.Parameters.Add("SolutionUniqueName", _ImportWebResourcesSolutionUniqueName);

            CreateResponse cresp = (CreateResponse)_serviceProxy.Execute(cr);
            //</snippetImportWebResources2>
            // Capture the id values for the Web Resources so the sample can delete them.
            _webResourceIdForSolution[counter] = cresp.id;

            Console.WriteLine("Created Web Resource for solution configuration: {0}", webResources.displayName);


            //</snippetImportWebResources1>
        }
        //<snippetImportWebResources3>
        //Encodes the Web Resource File
        static public string getEncodedFileContents(String pathToFile)
        {
            FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);
            byte[] binaryData = new byte[fs.Length];
            long bytesRead = fs.Read(binaryData, 0, (int)fs.Length);
            fs.Close();
            return System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
        }
        //</snippetImportWebResources3>


    }
}
