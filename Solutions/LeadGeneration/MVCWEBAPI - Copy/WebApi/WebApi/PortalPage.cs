using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace CRM
{
    public class PortalPage : System.Web.UI.Page
    {
        #region Private Methods

        /// <summary>
        /// Gets web service connection information from the app.config file.
        /// If there is more than one available, the user is prompted to select
        /// the desired connection configuration by name.
        /// </summary>
        /// <returns>A String containing web service connection configuration information.</returns>
        public static String GetServiceConfiguration()
        {
            // Get available connection strings from app.config.
            int count = ConfigurationManager.ConnectionStrings.Count;

            // Create a filter list of connection strings so that we have a list of valid
            // connection strings for Microsoft Dynamics CRM only.
            List<KeyValuePair<String, String>> filteredConnectionStrings =
                new List<KeyValuePair<String, String>>();

            for (int a = 0; a < count; a++)
            {
                if (isValidConnectionString(ConfigurationManager.ConnectionStrings[a].ConnectionString))
                    filteredConnectionStrings.Add
                        (new KeyValuePair<String, String>
                            (ConfigurationManager.ConnectionStrings[a].Name,
                            ConfigurationManager.ConnectionStrings[a].ConnectionString));
            }

            // No valid connections strings found. Write out and error message.
            if (filteredConnectionStrings.Count == 0)
            {
                Console.WriteLine("An app.config file containing at least one valid Microsoft Dynamics CRM " +
                    "connection String configuration must exist in the run-time folder.");
                Console.WriteLine("\nThere are several commented out example connection strings in " +
                    "the provided app.config file. Uncomment one of them and modify the String according " +
                    "to your Microsoft Dynamics CRM installation. Then re-run the sample.");
                return null;
            }

            // If one valid connection String is found, use that.
            if (filteredConnectionStrings.Count == 1)
            {
                return filteredConnectionStrings[0].Value;
            }

            // If more than one valid connection String is found, let the user decide which to use.
            if (filteredConnectionStrings.Count > 1)
            {
                Console.WriteLine("The following connections are available:");
                Console.WriteLine("------------------------------------------------");

                for (int i = 0; i < filteredConnectionStrings.Count; i++)
                {
                    Console.Write("\n({0}) {1}\t",
                    i + 1, filteredConnectionStrings[i].Key);
                }

                Console.WriteLine();

                Console.Write("\nType the number of the connection to use (1-{0}) [{0}] : ",
                    filteredConnectionStrings.Count);
                String input = Console.ReadLine();
                int configNumber;
                if (input == String.Empty) input = filteredConnectionStrings.Count.ToString();
                if (!Int32.TryParse(input, out configNumber) || configNumber > count ||
                    configNumber == 0)
                {
                    Console.WriteLine("Option not valid.");
                    return null;
                }

                return filteredConnectionStrings[configNumber - 1].Value;

            }
            return null;

        }

        /// <summary>
        /// Verifies if a connection String is valid for Microsoft Dynamics CRM.
        /// </summary>
        /// <returns>True for a valid String, otherwise False.</returns>
        protected static Boolean isValidConnectionString(String connectionString)
        {
            // At a minimum, a connection String must contain one of these arguments.
            if (connectionString.Contains("Url=") ||
                connectionString.Contains("Server=") ||
                connectionString.Contains("ServiceUri="))
                return true;

            return false;
        }
        #endregion
    }
}