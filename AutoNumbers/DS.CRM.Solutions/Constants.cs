using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.CRM
{
    public static class Constants
    {
        public const string CustomizationPrefix = "dots001";
        public const string ManagedSolutionLocation = @"D:\temp\ManagedSolutionForImportExample.zip";
        public const string OutputDirectory = @"D:\temp\";
        public const string CustomEntityName = "dots_forminformation001";
        public const string CustomConfigurationEntityName = "dots_configuration001";

        public const int LanguageCode = 1033;

        public static Guid[] WebResourceIds = new Guid[10];
        public static Guid[] WebResourceIdForSolution = new Guid[1];
    }
}
