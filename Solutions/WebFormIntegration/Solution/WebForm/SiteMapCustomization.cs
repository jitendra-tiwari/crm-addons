using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoNumberGeneration
{
    public static class SiteMapCustomization
    {

        public static void CreateSiteMap(OrganizationServiceProxy _serviceProxy, string _customEntityName)
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = "sitemap";
            query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);

            EntityCollection col = _serviceProxy.RetrieveMultiple(query);

            Entity sitemap = null;
            if (col != null && col.Entities.Count > 0)
                sitemap = col.Entities[0];


            string sitemapcontent = sitemap["sitemapxml"].ToString();
            XDocument sitemapxml = XDocument.Parse(sitemapcontent);

           // create new area
                       sitemapxml.Element("SiteMap")
            .Elements("Area")
            .Where(x => (string)x.Attribute("Id") == "DotsquaresPack")
            .Remove();

            XElement root = new XElement("Area");
            root.Add(new XAttribute("Id", "DotsquaresPack"),
                new XAttribute("ShowGroups", "true"),
                new XAttribute("Title", "DotsquaresPack"));
            root.Add(new XElement("Group",
                new XAttribute("Id", "Group_SubDotsquaresWebForm"),
                new XAttribute("Title", "DotsquaresAutoNumber"),
                new XElement("SubArea", new XAttribute("Id", "SubArea_dots_autonumber"),
                new XAttribute("Entity", _customEntityName)
                )));


            sitemapxml.Element("SiteMap").Add(root);


            sitemap["sitemapxml"] = sitemapxml.ToString();
            _serviceProxy.Update(sitemap);

            PublishXmlRequest request = new PublishXmlRequest();
            request.ParameterXml = "<importexportxml><sitemaps><sitemap></sitemap></sitemaps></importexportxml>";
            _serviceProxy.Execute(request);
        }
    }

    
}
