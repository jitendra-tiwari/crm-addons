using WebForm.Model;
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
        private readonly string _publisherCustomizationPrefix = Constants.PublisherCustomizationPrefix;

        private readonly string _customEntityName = WebForm.WebForm._customEntityName;
        private readonly string _customFieldEntityName = WebForm.WebForm._customFieldEntityName;
        private readonly string _customConfigurationEntityName = WebForm.WebForm._customConfigurationEntityName;
        
        private List<Guid> _webResourceIds = new List<Guid>();
        private Guid _webResourceIdForSolution = Guid.Empty;
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
                    // SiteMapCustomization.CreateSiteMap(_serviceProxy, _customEntityName);
                    // AutoNumberGeneration.AutoNumber.CreateWorkFlow(_serviceProxy, _languageCode);
                    WebForm.WebForm.SetProxy(_serviceProxy, _languageCode);
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
        private void CreateRequiredRecords()
        {
            // Create a managed solution for the Install or upgrade a solution sample

            Guid _tempPublisherId = new Guid();
            String _tempCustomizationPrefix = _publisherCustomizationPrefix;
            Guid _tempSolutionsSampleSolutionId = new Guid();            
            
            Boolean _publisherCreated = false;
            Boolean _solutionCreated = false;


            //Define a new publisher
            Publisher _crmSdkPublisher = new Publisher
            {
                //UniqueName = "dsleadintegration",
                //FriendlyName = "Dotsquares Ltd.",

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
                        
            //Define a solution
            Solution solution = new Solution
            {                
                UniqueName = Constants.SolutionUniqueName,
                FriendlyName = Constants.SolutionFriendlyName,
                PublisherId = new EntityReference(Publisher.EntityLogicalName, _tempPublisherId),
                Description = Constants.SolutionDescription,
                Version = Constants.SolutionVersion,
                ConfigurationPageId = new EntityReference(WebResource.EntityLogicalName, _webResourceIdForSolution)

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

            //delete webform entity
            if (IsEntityExist(_customEntityName) > 0)
            {
                DeleteEntityRequest customEntityNameFormField = new DeleteEntityRequest()
                {
                    LogicalName = _customEntityName,
                };
                _serviceProxy.Execute(customEntityNameFormField);
            }
            // Create the webform entity.
            WebForm.WebForm.DotsWebFormEntity();


            //delete webfield entity
            if (IsEntityExist(_customFieldEntityName) > 0)
            {
                DeleteEntityRequest customFieldEntityNameFormField = new DeleteEntityRequest()
                {
                    LogicalName = _customFieldEntityName,
                };
                _serviceProxy.Execute(customFieldEntityNameFormField);
            }

            // Create the webfield entity.
            WebForm.WebForm.DotsWebFieldEntity();


            //create relationship with webform and field entity
            WebForm.WebForm.CreateRelationShip();


            //delete webformconfig entity
            if (IsEntityExist(_customConfigurationEntityName) > 0)
            {
                DeleteEntityRequest customEntityConfigNameFormField = new DeleteEntityRequest()
                {
                    LogicalName = _customConfigurationEntityName,
                };
                _serviceProxy.Execute(customEntityConfigNameFormField);
            }
            // Create the config entity.
            WebForm.WebForm.DotsWebFormConfigEntity();

            // assign entity sample form entity to solution
            //##################### Start Assign WebForm Entity #############################
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
            //##################### End Assign WebForm Entity #############################



            // assign entity webfield entity to solution
            //##################### Start Assign WebField Entity #############################
            RetrieveEntityRequest retrievewebfieldEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Entity,
                LogicalName = _customEntityName
            };

            RetrieveEntityResponse retrievefieldEntityResponse = (RetrieveEntityResponse)_serviceProxy.Execute(retrievewebfieldEntityRequest);

            AddSolutionComponentRequest addFieldEntityReq = new AddSolutionComponentRequest()
            {
                ComponentType = 1,
                ComponentId = (Guid)retrievefieldEntityResponse.EntityMetadata.MetadataId,
                SolutionUniqueName = solution.UniqueName,
                AddRequiredComponents = true
            };
            _serviceProxy.Execute(addFieldEntityReq);
            //##################### End Assign WebField Entity #############################



            //assign configuration entity to solution
            //##################### End Assign WebConfig Entity #############################
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

            //##################### End Assign WebConfig Entity #############################

            //assign web resource to slution
            CreateWebResource(solution.UniqueName);

            //assign configuration page above creted to solution
            AssiginConfigurationPageToSolution(_webResourceIdForSolution, solution.UniqueName);
            
            ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
            exportSolutionRequest.Managed = false;
            exportSolutionRequest.SolutionName = solution.UniqueName;

            ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)_serviceProxy.Execute(exportSolutionRequest);

            byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
            System.IO.Directory.CreateDirectory(_outputDir);
            File.WriteAllBytes(_managedSolutionLocation, exportXml);

            // Delete the solution and the components so it can be installed.           

            DeleteEntityRequest delEntReq = new DeleteEntityRequest { LogicalName = (_customEntityName) };
            _serviceProxy.Execute(delEntReq);

            DeleteEntityRequest delFieldEntReq = new DeleteEntityRequest { LogicalName = (_customFieldEntityName) };
            _serviceProxy.Execute(delFieldEntReq);

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
                _serviceProxy.Delete(WebResource.EntityLogicalName, _webResourceIdForSolution);
            }

            if (_publisherCreated)
            {
                _serviceProxy.Delete(Publisher.EntityLogicalName, _tempPublisherId);
            }


            Console.WriteLine("Managed Solution created and copied to {0}", _managedSolutionLocation);
            
        }

        private void AssiginConfigurationPageToSolution(Guid WesId, string _ImportWebResourcesSolutionUniqueName)
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

        private int IsEntityExist(string entityName)
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

        private void CreateWebResource(string _ImportWebResourcesSolutionUniqueName)
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
                _webResourceIds.Add(cresp.id);
                
                Console.WriteLine("Created Web Resource: {0}", webResource.displayName);
            }

            //</snippetImportWebResources1>
        }

        private void UploadConfigurationPageForSolution()
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
            _webResourceIdForSolution = cresp.id;

            Console.WriteLine("Created Web Resource for solution configuration: {0}", webResources.displayName);


            //</snippetImportWebResources1>
        }
        
        private static string GetEncodedFileContents(String pathToFile)
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
        private void DeleteRequiredRecords(bool prompt)
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