using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi
{
    public class CRMHelper
    {
        public static string getOptionSetText(string entityName, string attributeName, int optionsetValue, OrganizationService _service)
        {
            string optionsetText = string.Empty;
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest();
            retrieveAttributeRequest.EntityLogicalName = entityName;
            retrieveAttributeRequest.LogicalName = attributeName;
            retrieveAttributeRequest.RetrieveAsIfPublished = true;

            RetrieveAttributeResponse retrieveAttributeResponse =
              //(RetrieveAttributeResponse)OrganizationService.Execute(retrieveAttributeRequest);
              (RetrieveAttributeResponse)_service.Execute(retrieveAttributeRequest);
            PicklistAttributeMetadata picklistAttributeMetadata =
              (PicklistAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

            OptionSetMetadata optionsetMetadata = picklistAttributeMetadata.OptionSet;

            foreach (OptionMetadata optionMetadata in optionsetMetadata.Options)
            {
                if (optionMetadata.Value == optionsetValue)
                {
                    optionsetText = optionMetadata.Label.UserLocalizedLabel.Label;
                    return optionsetText;
                }

            }
            return optionsetText;
        }

        public static int getOptionSetValue(string entityName, string attributeName, string optionsetText, OrganizationService _service)
        {
            int optionSetValue = 0;
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest();
            retrieveAttributeRequest.EntityLogicalName = entityName;
            retrieveAttributeRequest.LogicalName = attributeName;
            retrieveAttributeRequest.RetrieveAsIfPublished = true;

            RetrieveAttributeResponse retrieveAttributeResponse =
              (RetrieveAttributeResponse)_service.Execute(retrieveAttributeRequest);
            PicklistAttributeMetadata picklistAttributeMetadata =
              (PicklistAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

            OptionSetMetadata optionsetMetadata = picklistAttributeMetadata.OptionSet;

            foreach (OptionMetadata optionMetadata in optionsetMetadata.Options)
            {
                if (optionMetadata.Label.UserLocalizedLabel.Label.ToLower() == optionsetText.ToLower())
                {
                    optionSetValue = optionMetadata.Value.Value;
                    return optionSetValue;
                }

            }
            return optionSetValue;
        }
    }
}