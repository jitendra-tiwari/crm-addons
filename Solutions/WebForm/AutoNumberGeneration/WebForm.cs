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
        public static string _customFieldEntityName = "dots_field";
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
                    SchemaName = "dots_name",
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
                    SchemaName = "dots_email",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Email,
                    DisplayName = new Label("Email", 1033),
                    Description = new Label("The Notify E-Mail.", 1033),
                }
            };

            _serviceProxy.Execute(createEmailAttributeRequest);


            CreateAttributeRequest createRelatedAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_relatedentity",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                    MaxLength = 100,
                    FormatName = StringFormatName.TextArea,
                    DisplayName = new Label("Related Entity", 1033),
                    Description = new Label("The Related Entity.", 1033),
                }
            };

            _serviceProxy.Execute(createRelatedAttributeRequest);

            // Create a boolean attribute like checkbox
            CreateAttributeRequest createBooleanAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new BooleanAttributeMetadata
                {
                    // Set base properties
                    SchemaName = "dots_labelposition",
                    DisplayName = new Label("Label Position", _languageCode),
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    Description = new Label("Boolean Label Position Attribute", _languageCode),
                    // Set extended properties
                    OptionSet = new BooleanOptionSetMetadata(
                    new OptionMetadata(new Label("True", _languageCode), 1),
                    new OptionMetadata(new Label("False", _languageCode), 0)
                    )
                }
            };

            _serviceProxy.Execute(createBooleanAttributeRequest);

            CreateAttributeRequest createMessageAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_submitmessage",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 500,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Submit Message", 1033),
                    Description = new Label("The Submit Message.", 1033),
                }
            };

            _serviceProxy.Execute(createMessageAttributeRequest);

            CreateAttributeRequest createSubmitButtonAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_submitbutton",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 500,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Submit Button Text", 1033),
                    Description = new Label("The Submit Button Text.", 1033),
                }
            };

            _serviceProxy.Execute(createSubmitButtonAttributeRequest);


            // Create a boolean attribute like checkbox
            CreateAttributeRequest createBooleanAttributeForCaptchaRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new BooleanAttributeMetadata
                {
                    // Set base properties
                    SchemaName = "dots_captcha",
                    DisplayName = new Label("Required Captcha", _languageCode),
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    Description = new Label("Boolean Captcha Attribute", _languageCode),
                    // Set extended properties
                    OptionSet = new BooleanOptionSetMetadata(
                    new OptionMetadata(new Label("True", _languageCode), 1),
                    new OptionMetadata(new Label("False", _languageCode), 0)
                    )
                }
            };

            _serviceProxy.Execute(createBooleanAttributeForCaptchaRequest);


            CreateAttributeRequest createRedirectUrlAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_redirecturl",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 500,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Redirect URL", 1033),
                    Description = new Label("The Redirect URL.", 1033),
                }
            };

            _serviceProxy.Execute(createRedirectUrlAttributeRequest);


            CreateAttributeRequest createRedirectModeAttributeRequest = new CreateAttributeRequest
            {

                EntityName = _customEntityName,
                Attribute = new PicklistAttributeMetadata
                {
                    SchemaName = "new_redirectmode",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    OptionSet = new OptionSetMetadata
                    {
                        IsGlobal = false,
                        OptionSetType = OptionSetType.Picklist,
                        Options =  {
                 new OptionMetadata(new Label("Auto",1033),1),
                 new OptionMetadata(new Label("Link",1033),2),
                  new OptionMetadata(new Label("Button",1033),3),

                        }

                    },
                    DisplayName = new Label("Redirect Mode", 1033),
                    Description = new Label("The Redirect Mode.", 1033),
                }
            };

            _serviceProxy.Execute(createRedirectModeAttributeRequest);


            CreateAttributeRequest createCSSAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_css",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 4000,
                    FormatName = StringFormatName.TextArea,
                    DisplayName = new Label("CSS", 1033),
                    Description = new Label("The CSS.", 1033),
                }
            };

            _serviceProxy.Execute(createCSSAttributeRequest);

        }

        public static void DotsWebFieldEntity()
        {
            CreateEntityRequest createwebfieldformInforequest = new CreateEntityRequest
            {

                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customFieldEntityName,
                    DisplayName = new Label("DS WebField", 1033),
                    DisplayCollectionName = new Label("DS Web Fields", 1033),
                    Description = new Label("An entity to store information about user webfields", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_fieldlabel",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Field Label", 1033),
                    Description = new Label("The primary attribute for the webfield entity.", 1033),

                }


            };
            _serviceProxy.Execute(createwebfieldformInforequest);


            // Add some attributes to the Power WebField entity
            CreateAttributeRequest createInitializeNumberRequest = new CreateAttributeRequest
            {
                EntityName = _customFieldEntityName,
                Attribute = new IntegerAttributeMetadata
                {
                    SchemaName = "dots_displayorder",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    // MaxLength = 100,
                    //FormatName = StringFormatName.wh,
                    DisplayName = new Label("Display Order", 1033),
                    Description = new Label("The Display Order.", 1033),

                }
            };

            _serviceProxy.Execute(createInitializeNumberRequest);

            CreateAttributeRequest createToolTipAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customFieldEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_fieldtooltip",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Field Tool Tip", 1033),
                    Description = new Label("The Field Tool Tip.", 1033),
                }
            };
            _serviceProxy.Execute(createToolTipAttributeRequest);


            // OptionSet  to the  entity
            CreateAttributeRequest createFieldTypeAttributeRequest = new CreateAttributeRequest
            {

                EntityName = _customFieldEntityName,
                Attribute = new PicklistAttributeMetadata
                {
                    SchemaName = "dots_fieldtype",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    OptionSet = new OptionSetMetadata
                    {
                        IsGlobal = false,
                        OptionSetType = OptionSetType.Picklist,
                        Options =  {
                 new OptionMetadata(new Label("Text",1033),1),
                new OptionMetadata(new Label("MultiLineText",1033),2),
                //new OptionMetadata(new Label("Drop Down List",1033),3),
                //new OptionMetadata(new Label("Radio Button",1033),4),
                new OptionMetadata(new Label("CheckBox",1033),3),
                new OptionMetadata(new Label("Date",1033),4),
                new OptionMetadata(new Label("EmailAddress",1033),5),
                 new OptionMetadata(new Label("Hidden",1033),6),
                  new OptionMetadata(new Label("ZipCode",1033),7),
                   new OptionMetadata(new Label("PhoneNumber",1033),8),

                        }

                    },
                    DisplayName = new Label("Field Type", 1033),
                    Description = new Label("The Field Type like text ,radiobutton and dropdown etc.", 1033),
                }
            };

            _serviceProxy.Execute(createFieldTypeAttributeRequest);


            CreateAttributeRequest createDefaultValueAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customFieldEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_fielddefaultvalue",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Field Default Value", 1033),
                    Description = new Label("The Field Default Value.", 1033),
                }
            };
            _serviceProxy.Execute(createDefaultValueAttributeRequest);


            CreateAttributeRequest createFieldLengthAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customFieldEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_fieldlength",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength =4000,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Field Length(chars)", 1033),
                    Description = new Label("The Field Length in charcater .", 1033),
                }
            };
            _serviceProxy.Execute(createFieldLengthAttributeRequest);


            // Create a boolean attribute like checkbox
            CreateAttributeRequest createBooleanAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customFieldEntityName,
                Attribute = new BooleanAttributeMetadata
                {
                    // Set base properties
                    SchemaName = "dots_fieldrequired",
                    DisplayName = new Label("Field Required", _languageCode),
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    Description = new Label("Boolean Field Required Attribute", _languageCode),
                    // Set extended properties
                    OptionSet = new BooleanOptionSetMetadata(
                    new OptionMetadata(new Label("True", _languageCode), 1),
                    new OptionMetadata(new Label("False", _languageCode), 0)
                    )
                }
            };

            _serviceProxy.Execute(createBooleanAttributeRequest);


            CreateAttributeRequest createMapFieldAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customFieldEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_mapfield",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Map Field", 1033),
                    Description = new Label("The Map Field.", 1033),
                }
            };
            _serviceProxy.Execute(createMapFieldAttributeRequest);

        }

        public static void CreateRelationShip()
        {
            //show subgrid on webform of fields
            CreateOneToManyRequest req = new CreateOneToManyRequest()
            {
                Lookup = new LookupAttributeMetadata()
                {
                    Description = new Label("The referral (" + _customFieldEntityName + ") from the " + _customEntityName + " table", 1033),
                    DisplayName = new Label("Web Form", 1033),
                    //LogicalName = "dots_parent_twitterpostid",
                    //SchemaName = "dots_Parent_twitterpostId",
                    LogicalName = "dots_parent_webformid",
                    SchemaName = "dots_Parent_webformId",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.Recommended)
                },
                OneToManyRelationship = new OneToManyRelationshipMetadata()
                {
                    AssociatedMenuConfiguration = new AssociatedMenuConfiguration()
                    {
                        Behavior = AssociatedMenuBehavior.UseCollectionName,
                        Group = AssociatedMenuGroup.Details,
                        Label = new Label("WebForm Accounts", 1033),
                        Order = 10000
                    },
                    CascadeConfiguration = new CascadeConfiguration()
                    {
                        Assign = CascadeType.Cascade,
                        Delete = CascadeType.Cascade,
                        Merge = CascadeType.Cascade,
                        Reparent = CascadeType.Cascade,
                        Share = CascadeType.Cascade,
                        Unshare = CascadeType.Cascade
                    },
                   

                    //ReferencedEntity = _customFieldEntityName,
                    //ReferencedAttribute = "dots_fieldid",
                    //ReferencingEntity = _customEntityName,
                    //SchemaName = "dots_field_dots_webform"

                    ReferencedEntity = _customEntityName,
                    ReferencedAttribute = "dots_webformid",
                    ReferencingEntity = _customFieldEntityName,
                    SchemaName = "dots_webform_dots_field"
                }
            };
            _serviceProxy.Execute(req);

            

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
                    SchemaName = "dots_serverurl",
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
                    SchemaName = "dots_registerid",
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
                    SchemaName = "dots_orguniquename",
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
                    SchemaName = "dots_username",
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
                    SchemaName = "dots_password",
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

