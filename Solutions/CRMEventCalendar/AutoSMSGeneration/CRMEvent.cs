
using CRMEventsModel.Model;
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


namespace EventsCalendar
{
    public static class CRMEvent 
    {
        public  static  OrganizationServiceProxy _serviceProxy;
        public static string _customEntityName = "dots_eventcalendar";
        public static string _customConfigurationEntityName = "dots_eventconfiguration";
        public static int _languageCode;
        public static void SetProxy(OrganizationServiceProxy _serProxy, int _langCode)
        {
            _serviceProxy = _serProxy;
            _languageCode = _langCode;
        }

        public static void DotsEventCalendarEntity()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customEntityName,
                    DisplayName = new Label("DS EventsCalendar", 1033),
                    DisplayCollectionName = new Label("Events Calendar", 1033),
                    Description = new Label("An entity to store information about EventsCalendar for particular entity.", 1033),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    CanCreateViews=new BooleanManagedProperty(false)
                     


                    //CanCreateForms = new BooleanManagedProperty(true),
                },

                // Define the primary attribute for the entity               
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "dots_name",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    FormatName = StringFormatName.Text,
                    DisplayName = new Label("Name", 1033),
                    Description = new Label("The primary attribute for the dots_eventscalendar entity.", 1033),


                }


            };

            _serviceProxy.Execute(createRequest);
        }

           

        public static void DotsEventCalendarConfigurationEntity()
        {
            // Create the custom entity.
            CreateEntityRequest createRequest = new CreateEntityRequest
            {
                //Define the entity
                Entity = new EntityMetadata
                {
                    SchemaName = _customConfigurationEntityName,
                    DisplayName = new Label("DS EventCalendar Configuration", 1033),
                    DisplayCollectionName = new Label("Events Calandar Configuration ", 1033),
                    Description = new Label("An entity to store information about EventCalendar configuration for particular entity.", 1033),
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
                    Description = new Label("The primary attribute for the dots_crmeventconfiguration configuration entity.", 1033),

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
    }
}

