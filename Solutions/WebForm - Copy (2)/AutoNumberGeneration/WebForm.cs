using WebForm.Model;
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


namespace WebForm
{
    public static class WebForm 
    {
        public  static  OrganizationServiceProxy _serviceProxy;
        public static string _customEntityName = "dots_webform";
        public static string _customConfigurationEntityName = "dots_webformconfiguration";
        public static int _languageCode;
        public static void SetProxy(OrganizationServiceProxy _serProxy, int _langCode)
        {
            _serviceProxy = _serProxy;
            _languageCode = _langCode;
        }


        public static void DotsWebFormEntity()
        {
            CreateEntityRequest createformInforequest = new CreateEntityRequest
            {

                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customEntityName,
                    DisplayName = new Label("DS WebForm", 1033),
                    DisplayCollectionName = new Label("DS Web Forms", 1033),
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
                       

        }

        public static void DotsWebFormConfigEntity()
        {
            //for create configuration entity           
            CreateEntityRequest createrequest = new CreateEntityRequest
            {

                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customConfigurationEntityName,
                    DisplayName = new Label("DS WebForm Configuration", 1033),
                    DisplayCollectionName = new Label("DS WebForm Configurations", 1033),
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

        }
    }
}

