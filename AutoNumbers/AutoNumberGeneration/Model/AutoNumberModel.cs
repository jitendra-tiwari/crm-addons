using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNumberGeneration.Model
{
    public class TabParameters
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

    public class GetAllEntitiesModel
    {
        public string DisplayName { get; set; }
        public string LogicalName { get; set; }
    }

    public class GetEntitiyFieldNames
    {
        public string FieldName { get; set; }
        public string Value { get; set; }

    }
}
