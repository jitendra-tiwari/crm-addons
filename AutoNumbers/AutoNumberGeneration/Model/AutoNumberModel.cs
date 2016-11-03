using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNumberGeneration.Model
{
    public class tabParameters
    {

        public string tabName { get; set; }
        public string tabDisplayName { get; set; }
        public string tabSectionName { get; set; }
        public string tabSectionDisplayName { get; set; }

        public List<entityFields> entityFields { get; set; }
    }

    public class entityFields
    {
        public string controlId { get; set; }
        public string dataFieldName { get; set; }
        public string fieldDisplayName { get; set; }

        public string viewId { get; set; }
        public bool IsGrid { get; set; }
    }

    public class getAllEntitiesModel
    {
        public string DisplayName { get; set; }
        public string LogicalName { get; set; }
    }

    public class getEntitiyFieldNames
    {
        public string FieldName { get; set; }
        public string Value { get; set; }

    }
}
