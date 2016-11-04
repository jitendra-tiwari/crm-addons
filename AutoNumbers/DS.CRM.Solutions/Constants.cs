using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.CRM
{
    public static class Constants
    {
        public const string CustomizationPrefix = "ds";
        public const string ManagedSolutionLocation = @"D:\temp\ManagedSolutionForAutoNumber.zip";
        public const string OutputDirectory = @"D:\temp\";
        public const string CustomEntityName = "dots_forminformation001";
        public const string CustomConfigurationEntityName = "dots_configuration001";

        public const int LanguageCode = 1033;

        public static Guid[] WebResourceIds = new Guid[10];
        public static Guid[] WebResourceIdForSolution = new Guid[1];

        // Publisher Information
        public const string PublisherUniqueName = "dotsquares";
        public const string PublisherFriendlyName = "Dotsquares Ltd.";
        public const string PublisherSupportingWebsiteUrl = "http://msdn.microsoft.com/en-us/dynamics/crm/default.aspx";
        public const string PublisherCustomizationPrefix = "ds";
        public const string PublisherEmailAddress = "jitendra.tiwari@dotsquares.com";
        public const string PublisherDescription = "";

        // Solution Information
        public const string SolutionUniqueName = "dsautonumber";
        public const string SolutionFriendlyName = "Dotsquares Auto Number Solution";
        public const string SolutionDescription = "This solution was created by the Dotsquares in the Microsoft Dynamics CRM SDK samples.";
        public const string SolutionVersion = "1.0";
    }
}
