using CRMTwitter.Model;
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


namespace CRMTwitter
{
    public static class CRMTwitterPost
    {
        public static OrganizationServiceProxy _serviceProxy;
        public static string _customEntityName = "dots_twitterpost";
        public static string _custom_PublisherEntityName = "dots_twitterpublisher";
        public static string _customConfigurationEntityName = "dots_twitterconfiguration";
        public static int _languageCode;
        public static void SetProxy(OrganizationServiceProxy _serProxy, int _langCode)
        {
            _serviceProxy = _serProxy;
            _languageCode = _langCode;
        }


        public static void DotsTwitterMessage()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customEntityName,
                    DisplayName = new Label("DS TwitterPost", 1033),
                    DisplayCollectionName = new Label("Twitter Post", 1033),
                    Description = new Label("An entity to store message for  Twitter Post entity.", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,


                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_message",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired),
                    MaxLength = 140,
                    FormatName = StringFormatName.TextArea,
                    DisplayName = new Label("Message", 1033),
                    Description = new Label("The primary attribute for the dots_twittepost entity.", 1033),


                }


            };

            _serviceProxy.Execute(createRequest);


            // Add dateTime attributes to  entity
            CreateAttributeRequest createPostDateAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new DateTimeAttributeMetadata
                {
                    SchemaName = "dots_postdate",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    Format = DateTimeFormat.DateAndTime,
                    ImeMode = ImeMode.Disabled,
                    DisplayName = new Label("Post Date", 1033),
                    Description = new Label("The post date.", 1033),

                }


            };

            _serviceProxy.Execute(createPostDateAttributeRequest);

            // Create a boolean attribute like checkbox
            CreateAttributeRequest createTargetEntityBooleanAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new BooleanAttributeMetadata
                {
                    // Set base properties
                    SchemaName = "dots_isapporved",
                    DisplayName = new Label("Approve", _languageCode),
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    Description = new Label("Boolean Approve Attribute", _languageCode),
                    // Set extended properties
                    OptionSet = new BooleanOptionSetMetadata(
                    new OptionMetadata(new Label("True", _languageCode), 1),
                    new OptionMetadata(new Label("False", _languageCode), 0)
                    )
                }
            };

            _serviceProxy.Execute(createTargetEntityBooleanAttributeRequest);


            // OptionSet  to the  entity
            CreateAttributeRequest createPostMessageTypeAttributeRequest = new CreateAttributeRequest
            {

                EntityName = _customEntityName,
                Attribute = new PicklistAttributeMetadata
                {
                    SchemaName = "dots_postmessagetype",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    OptionSet = new OptionSetMetadata
                    {
                        IsGlobal = false,
                        OptionSetType = OptionSetType.Picklist,
                        Options =  {
                 new OptionMetadata(new Label("Message",1033),1),
                  //new OptionMetadata(new Label("Twitter",1033),1),

                                   }

                    },
                    DisplayName = new Label("Post Message Type", 1033),
                    Description = new Label("The Post Message Type like messsage and direct message.", 1033),
                }
            };

            _serviceProxy.Execute(createPostMessageTypeAttributeRequest);

            CreateAttributeRequest createNotesRequest = new CreateAttributeRequest
            {
                EntityName = _customEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_notes",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 500,
                    FormatName = StringFormatName.TextArea,
                    DisplayName = new Label("Notes", 1033),
                    Description = new Label("The Notes.", 1033),

                }
            };

            _serviceProxy.Execute(createNotesRequest);

        }

        public static void DotsTwitterPublisher()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _custom_PublisherEntityName,
                    DisplayName = new Label("DS Twitter Publisher", 1033),
                    DisplayCollectionName = new Label("Twitter Publisher", 1033),
                    Description = new Label("An entity to store information about Publisher a entity.", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_alias",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.SystemRequired),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Alias", 1033),
                    Description = new Label("The primary attribute for the dots_twitterpublisher entity.", 1033),

                }


            };

            _serviceProxy.Execute(createRequest);

            // Add some attributes to the  entity
            CreateAttributeRequest createMediaAttributeRequest = new CreateAttributeRequest
            {

                EntityName = _custom_PublisherEntityName,
                Attribute = new PicklistAttributeMetadata
                {
                    SchemaName = "dots_media",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.SystemRequired),
                    OptionSet = new OptionSetMetadata
                    {
                        IsGlobal = false,
                        OptionSetType = OptionSetType.Picklist,
                        Options =  {
                 new OptionMetadata(new Label("Twitter",1033),1),

                                   }

                    },
                    DisplayName = new Label("Media", 1033),
                    Description = new Label("The Media type like twitter.", 1033),
                }
            };

            _serviceProxy.Execute(createMediaAttributeRequest);





        }

        public static void DotsTwitterConfiguration()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customConfigurationEntityName,
                    DisplayName = new Label("DS Twitter Configuration", 1033),
                    DisplayCollectionName = new Label("Twitter Configuration", 1033),
                    Description = new Label("An entity to store information about dots_twitterconfiguration for  entity.", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_type",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Type", 1033),
                    Description = new Label("The primary attribute for the dots_twitterconfiguration  entity.", 1033),

                }


            };

            _serviceProxy.Execute(createRequest);

            // Add some attributes to the dots_autonumber entity
            CreateAttributeRequest createPlaceHolderAttributeRequest = new CreateAttributeRequest
            {
                EntityName = _customConfigurationEntityName,
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_value",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 500,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Value", 1033),
                    Description = new Label("The Value for Security", 1033),
                }
            };

            _serviceProxy.Execute(createPlaceHolderAttributeRequest);



        }

        public static void CreateRelationShip()
        {
            CreateOneToManyRequest req = new CreateOneToManyRequest()
            {
                Lookup = new LookupAttributeMetadata()
                {
                    Description = new Label("The referral (" + _customEntityName + ") from the " + _custom_PublisherEntityName + " table", 1033),
                    DisplayName = new Label("Publisher", 1033),
                    //LogicalName = "dots_parent_twitterpostid",
                    //SchemaName = "dots_Parent_twitterpostId",
                    LogicalName = "dots_parent_twitterpublisherid",
                    SchemaName = "dots_Parent_twitterpublisherId",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.Recommended)
                },
                OneToManyRelationship = new OneToManyRelationshipMetadata()
                {
                    AssociatedMenuConfiguration = new AssociatedMenuConfiguration()
                    {
                        Behavior = AssociatedMenuBehavior.UseCollectionName,
                        Group = AssociatedMenuGroup.Details,
                        Label = new Label("Twitter Accounts", 1033),
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
                    //ReferencedEntity = _customEntityName,
                    //ReferencedAttribute = "dots_twitterpostid",
                    //ReferencingEntity = _custom_PublisherEntityName,
                    //SchemaName = "dots_twitterpost_dots_twitterpublisher"
                    ReferencedEntity = _custom_PublisherEntityName,
                    ReferencedAttribute = "dots_twitterpublisherid",
                    ReferencingEntity = _customEntityName,
                    SchemaName = "dots_twitterpublisher_dots_twitterpost"
                }
            };
            _serviceProxy.Execute(req);


        }
    }
}


