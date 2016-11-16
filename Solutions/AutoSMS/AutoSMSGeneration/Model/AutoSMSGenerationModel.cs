using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSMSGeneration.Model
{
    public class TabParametersModel
    {

        public string TabName { get; set; }
        public string TabDisplayName { get; set; }
        public string TabSectionName { get; set; }
        public string TabSectionDisplayName { get; set; }

        public List<EntityFields> EntityFields { get; set; }
    }

    public class EntityFields
    {
        public string ControlId { get; set; }
        public string DataFieldName { get; set; }
        public string FieldDisplayName { get; set; }

        public string ViewId { get; set; }
        public bool IsGrid { get; set; }
    }

    public class CRMEntityModel
    {
        public string DisplayName { get; set; }
        public string LogicalName { get; set; }
    }

    public class EntitiyFieldNameModel
    {
        public string FieldName { get; set; }
        public string Value { get; set; }

    }
}
