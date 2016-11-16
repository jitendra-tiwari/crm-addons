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
    public static class AutoNumber 
    {
        public  static  OrganizationServiceProxy _serviceProxy;
        public static string _customEntityName = "dots_autonumber";
        public static string _customConfigurationEntityName = "dots_autonumberconfiguration";
        public static int _languageCode;
        public static void SetProxy(OrganizationServiceProxy _serProxy, int _langCode)
        {
            _serviceProxy = _serProxy;
            _languageCode = _langCode;
        }
        public static void CreateWorkFlow(OrganizationServiceProxy _serProxy, int _langCode)
        {
            _serviceProxy = _serProxy;
            _languageCode = _langCode;
              Guid _workflowId; 
            {



                #region Create XAML

                // Define the workflow XAML.
                string xamlWF;

                xamlWF = @"<?xml version=""1.0"" encoding=""utf-16""?>
                        <Activity x:Class=""SampleWF"" 
                                  xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" 
                                  xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"" 
                                  xmlns:mxs=""clr-namespace:Microsoft.Xrm.Sdk;assembly=Microsoft.Xrm.Sdk, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"" 
                                  xmlns:mxswa=""clr-namespace:Microsoft.Xrm.Sdk.Workflow.Activities;assembly=Microsoft.Xrm.Sdk.Workflow, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"" 
                                  xmlns:s=""clr-namespace:System;assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" 
                                  xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" 
                                  xmlns:srs=""clr-namespace:System.Runtime.Serialization;assembly=System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""                                  
                                  xmlns:this=""clr-namespace:"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                            <x:Members>
                                <x:Property Name=""InputEntities"" Type=""InArgument(scg:IDictionary(x:String, mxs:Entity))"" />
                                <x:Property Name=""CreatedEntities"" Type=""InArgument(scg:IDictionary(x:String, mxs:Entity))"" />
                            </x:Members>
                            <this:SampleWF.InputEntities>
                                <InArgument x:TypeArguments=""scg:IDictionary(x:String, mxs:Entity)"" />
                            </this:SampleWF.InputEntities>
                            <this:SampleWF.CreatedEntities>
                              <InArgument x:TypeArguments=""scg:IDictionary(x:String, mxs:Entity)"" />
                           </this:SampleWF.CreatedEntities>
                            <mva:VisualBasic.Settings>Assembly references and imported namespaces for internal implementation</mva:VisualBasic.Settings>
                            <mxswa:Workflow>
                                <Sequence>
                                    <Sequence.Variables>
                                        <Variable x:TypeArguments=""x:Int32"" Default=""[40]"" Name=""probability_value"" />
                                        <Variable x:TypeArguments=""mxs:Entity"" Default=""[CreatedEntities(&quot;primaryEntity#Temp&quot;)]"" Name=""CreatedEntity"" />
                                    </Sequence.Variables>
                                    <Assign x:TypeArguments=""mxs:Entity"" To=""[CreatedEntity]"" Value=""[New Entity(&quot;opportunity&quot;)]"" />
                                    <Assign x:TypeArguments=""s:Guid"" To=""[CreatedEntity.Id]"" Value=""[InputEntities(&quot;primaryEntity&quot;).Id]"" />
                                    <mxswa:SetEntityProperty Attribute=""closeprobability"" Entity=""[CreatedEntity]"" 
                                        EntityName=""opportunity"" TargetType=""[Type.GetType(&quot;probability_value&quot;)]"" 
                                                       Value=""[probability_value]"">
                                    </mxswa:SetEntityProperty>
                                    <mxswa:UpdateEntity Entity=""[CreatedEntity]"" EntityName=""opportunity"" />
                                    <Assign x:TypeArguments=""mxs:Entity"" To=""[InputEntities(&quot;primaryEntity&quot;)]"" Value=""[CreatedEntity]"" />
                                    <Persist />
                                </Sequence>
                            </mxswa:Workflow>
                        </Activity>";

                #endregion Create XAML

                #region Create Workflow

                //<snippetCreateAWorkflow1>
                // Create an asynchronous workflow.
                // The workflow should execute after a new opportunity is created.
                Workflow workflow = new Workflow()
                {
                    // These properties map to the New Process form settings in the web application.
                    Name = "Set closeprobability on opportunity create (async)",
                    Type = new OptionSetValue((int)WorkflowType.Definition),
                    Category = new OptionSetValue((int)WorkflowCategory.Workflow),
                    PrimaryEntity = Opportunity.EntityLogicalName,
                    Mode = new OptionSetValue((int)WorkflowMode.Background),

                    // Additional settings from the second New Process form.
                    Description = @"When an opportunity is created, this workflow" +
                        " sets the closeprobability field of the opportunity record to 40%.",
                    OnDemand = false,
                    Subprocess = false,
                    Scope = new OptionSetValue((int)WorkflowScope.User),
                    TriggerOnCreate = true,
                    AsyncAutoDelete = true,
                    Xaml = xamlWF,

                    // Other properties not in the web forms.
                    LanguageCode = 1033,  // U.S. English                        
                };
                _workflowId = _serviceProxy.Create(workflow);
                //</snippetCreateAWorkflow1>

                Console.WriteLine("Created Workflow: " + workflow.Name);

                #endregion Create Workflow

                #region Activate Workflow

                // Activate the workflow.
                var activateRequest = new SetStateRequest
                {
                    EntityMoniker = new EntityReference
                        (Workflow.EntityLogicalName, _workflowId),
                    State = new OptionSetValue((int)WorkflowState.Activated),
                    Status = new OptionSetValue((int)workflow_statuscode.Activated)
                };
                _serviceProxy.Execute(activateRequest);
                Console.WriteLine("Activated Workflow: " + workflow.Name);

                #endregion Activate Workflow


                // Deactivate and delete workflow
                SetStateRequest deactivateRequest = new SetStateRequest
                {
                    EntityMoniker = new EntityReference(Workflow.EntityLogicalName, _workflowId),
                    State = new OptionSetValue((int)WorkflowState.Draft),
                    Status = new OptionSetValue((int)workflow_statuscode.Draft)
                };
                _serviceProxy.Execute(deactivateRequest);
                _serviceProxy.Delete(Workflow.EntityLogicalName, _workflowId);
                
            }
        }

        public static void DotsAutoNumberEntity()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customEntityName,
                    DisplayName = new Label("DS AutoNumber", 1033),
                    DisplayCollectionName = new Label("Auto Number", 1033),
                    Description = new Label("An entity to store information about autonumber for particular entity.", 1033),
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
                    Description = new Label("The primary attribute for the dots_autonumber entity.", 1033),
                     

                }


            };

            _serviceProxy.Execute(createRequest);

            // Add some attributes to the dots_autonumber entity
            CreateAttributeRequest createPlaceHolderAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_placeholder",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("PlaceHolder", 1033),
                    Description = new Label("The PlaceHolder.", 1033),
                     
                }
                 
                 
            };

            _serviceProxy.Execute(createPlaceHolderAttributeRequest);

            CreateAttributeRequest createTargetEntityNameAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_targetentityname",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Target Entity Name", 1033),
                    Description = new Label("The Target Entity Name.", 1033),
                }
            };

            _serviceProxy.Execute(createTargetEntityNameAttributeRequest);


            CreateAttributeRequest createTargetEntityLogicalNameAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_targetentitylogicalname",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Target Entity Logical Name", 1033),
                    Description = new Label("The Target Entity Logical Name.", 1033),
                   
                }
            };

            _serviceProxy.Execute(createTargetEntityLogicalNameAttributeRequest);


            CreateAttributeRequest createAttributeTypeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_attributename",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Attribute Name", 1033),
                    Description = new Label("The Attribute Name.", 1033),

                }
            };

            _serviceProxy.Execute(createAttributeTypeRequest);

            CreateAttributeRequest createMaxLengthRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_maxlength",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("MaxLength", 1033),
                    Description = new Label("The MaxLength.", 1033),

                }
            };

            _serviceProxy.Execute(createMaxLengthRequest);

           
           
            //CreateOptionSetRequest createTwoOptionSetRequest = new CreateOptionSetRequest
            //{
            //    // Create a global option set (OptionSetMetadata).
            //    OptionSet = new OptionSetMetadata
            //    {
            //        Name = "new_generateautonumber",
            //        DisplayName = new Label("Generate Autonumber", _languageCode),
            //        //IsGlobal = true,
            //        OptionSetType = OptionSetType.Picklist,
            //        Options =
            //            {
            //                new OptionMetadata(new Label("Yes", _languageCode),1),
            //                new OptionMetadata(new Label("No", _languageCode), 0),
                         
            //            }
            //    }
            //};
            //_serviceProxy.Execute(createTwoOptionSetRequest);

            CreateAttributeRequest createTagetAttributeNameRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_targetattributename",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Target Attribute Name", 1033),
                    Description = new Label("The Target Attribute Name.", 1033),

                }
            };

            _serviceProxy.Execute(createTagetAttributeNameRequest);

            CreateAttributeRequest createTagetAttributeLogicalNameRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_targetattributelogicalname",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Target Attribute Logical Name", 1033),
                    Description = new Label("The Target Attribute Logical Name.", 1033),

                }
            };

            _serviceProxy.Execute(createTagetAttributeLogicalNameRequest);

            CreateAttributeRequest createInitializeNumberRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new IntegerAttributeMetadata
                {
                    SchemaName = "new_initializenumber",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                   // MaxLength = 100,
                    //FormatName = StringFormatName.wh,
                    DisplayName = new Label("Initialize Number", 1033),
                    Description = new Label("The Initialize Number.", 1033),

                }
            };

            _serviceProxy.Execute(createInitializeNumberRequest);

            CreateAttributeRequest createCurrentNumberRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new IntegerAttributeMetadata
                {
                    SchemaName = "new_currentnumber",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                   //  MaxLength = 100,
                   // FormatName = StringFormatName.Text,
                    DisplayName = new Label("Current Number", 1033),
                    Description = new Label("The Current Number.", 1033),

                }
            };

            _serviceProxy.Execute(createCurrentNumberRequest);


            CreateAttributeRequest createFieldFormatRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_fieldformat",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 200,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Field Format", 1033),
                    Description = new Label("The Field Format.", 1033),

                }
            };

            _serviceProxy.Execute(createFieldFormatRequest);
        }

        public static void DotsAutoNumberConfigurationEntity()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customConfigurationEntityName,
                    DisplayName = new Label("DS AutoNumber Configuration", 1033),
                    DisplayCollectionName = new Label("Auto Number Configuration", 1033),
                    Description = new Label("An entity to store information about autonumber configuration for particular entity.", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "new_type",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Type", 1033),
                    Description = new Label("The primary attribute for the dots_autonumber configuration entity.", 1033),

                }


            };

            _serviceProxy.Execute(createRequest);

            // Add some attributes to the dots_autonumber entity
            CreateAttributeRequest createPlaceHolderAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customConfigurationEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_value",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 500,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Value", 1033),
                    Description = new Label("The Value for Security", 1033),
                }
            };

            _serviceProxy.Execute(createPlaceHolderAttributeRequest);
         

            
        }
    }
}

