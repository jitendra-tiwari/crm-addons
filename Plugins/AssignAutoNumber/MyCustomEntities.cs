using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]
namespace Dotsquares.DCRM.Plugins
{
    [System.Runtime.Serialization.DataContractAttribute()]
    public enum AutoNumberState
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Active = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Inactive = 1,
    }

    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("dots_autonumber")]
    public partial class DSAutoNumber : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public DSAutoNumber() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "dots_autonumber";

        public const int EntityTypeCode = 10190;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
        
        /// <summary>
        /// Unique identifier of the account.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("dots_autonumberid")]
        public System.Nullable<System.Guid> AutoNumberId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("dots_autonumberid");
            }
            set
            {
                this.OnPropertyChanging("AutoNumberId");
                this.SetAttributeValue("dots_autonumberid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("AutoNumberId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("dots_autonumberid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.AutoNumberId = value;
            }
        }

        /// <summary>
        /// Type an ID number or code for the account to quickly search and identify the account in system views.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_placeholder")]
        public string PlaceHolder
        {
            get
            {
                return this.GetAttributeValue<string>("new_placeholder");
            }
            set
            {
                this.OnPropertyChanging("PlaceHolder");
                this.SetAttributeValue("new_placeholder", value);
                this.OnPropertyChanged("PlaceHolder");
            }
        }
        
        /// <summary>
        /// Type the country or region for the primary address.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_targetentityname")]
        public string TargetEntityName
        {
            get
            {
                return this.GetAttributeValue<string>("new_targetentityname");
            }
            set
            {
                this.OnPropertyChanging("TargetEntityName");
                this.SetAttributeValue("new_targetentityname", value);
                this.OnPropertyChanged("TargetEntityName");
            }
        }
        
        /// <summary>
        /// Type the county for the primary address.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_targetentitylogicalname")]
        public string TargetEntityLogicalName
        {
            get
            {
                return this.GetAttributeValue<string>("new_targetentitylogicalname");
            }
            set
            {
                this.OnPropertyChanging("TargetEntityLogicalName");
                this.SetAttributeValue("new_targetentitylogicalname", value);
                this.OnPropertyChanged("TargetEntityLogicalName");
            }
        }

        /// <summary>
        /// Type the fax number associated with the primary address.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_attributename")]
        public string AttributeName
        {
            get
            {
                return this.GetAttributeValue<string>("new_attributename");
            }
            set
            {
                this.OnPropertyChanging("AttributeName");
                this.SetAttributeValue("new_attributename", value);
                this.OnPropertyChanged("AttributeName");
            }
        }

        /// <summary>
        /// new_maxlength
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_maxlength")]
        public string MaxLength
        {
            get
            {
                return this.GetAttributeValue<string>("new_maxlength");
            }
            set
            {
                this.OnPropertyChanging("MaxLength");
                this.SetAttributeValue("new_maxlength", value);
                this.OnPropertyChanged("MaxLength");
            }
        }

        /// <summary>
        /// new_targetattributename
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_targetattributename")]
        public string TargetAttributeName
        {
            get
            {
                return this.GetAttributeValue<string>("new_targetattributename");
            }
            set
            {
                this.OnPropertyChanging("TargetAttributeName");
                this.SetAttributeValue("new_targetattributename", value);
                this.OnPropertyChanged("TargetAttributeName");
            }
        }

        /// <summary>
        /// new_targetattributelogicalname
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_targetattributelogicalname")]
        public string TargetAttributeLogicalName
        {
            get
            {
                return this.GetAttributeValue<string>("new_targetattributelogicalname");
            }
            set
            {
                this.OnPropertyChanging("TargetAttributeLogicalName");
                this.SetAttributeValue("new_targetattributelogicalname", value);
                this.OnPropertyChanged("TargetAttributeLogicalName");
            }
        }
        /// <summary>
        /// new_initializenumber
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_initializenumber")]
        public string InitializeNumber
        {
            get
            {
                return this.GetAttributeValue<string>("new_initializenumber");
            }
            set
            {
                this.OnPropertyChanging("InitializeNumber");
                this.SetAttributeValue("new_initializenumber", value);
                this.OnPropertyChanged("InitializeNumber");
            }
        }
        
        /// <summary>
        /// new_currentnumber
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_currentnumber")]
        public string CurrentNumber
        {
            get
            {
                return this.GetAttributeValue<string>("new_currentnumber");
            }
            set
            {
                this.OnPropertyChanging("CurrentNumber");
                this.SetAttributeValue("new_currentnumber", value);
                this.OnPropertyChanged("CurrentNumber");
            }
        }

        /// <summary>
        /// new_fieldformat
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_fieldformat")]
        public string FieldFormat
        {
            get
            {
                return this.GetAttributeValue<string>("new_fieldformat");
            }
            set
            {
                this.OnPropertyChanging("FieldFormat");
                this.SetAttributeValue("new_fieldformat", value);
                this.OnPropertyChanged("FieldFormat");
            }
        }

        /// <summary>
        /// Version number of the account.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Date and time that the record was migrated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overriddencreatedon")]
        public System.Nullable<System.DateTime> OverriddenCreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overriddencreatedon");
            }
            set
            {
                this.OnPropertyChanging("OverriddenCreatedOn");
                this.SetAttributeValue("overriddencreatedon", value);
                this.OnPropertyChanged("OverriddenCreatedOn");
            }
        }

        /// <summary>
        /// Unique identifier of the data import or data migration that created this record.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("importsequencenumber")]
        public System.Nullable<int> ImportSequenceNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("importsequencenumber");
            }
            set
            {
                this.OnPropertyChanging("ImportSequenceNumber");
                this.SetAttributeValue("importsequencenumber", value);
                this.OnPropertyChanged("ImportSequenceNumber");
            }
        }
        
        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleversionnumber")]
        public System.Nullable<int> TimeZoneRuleVersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("timezoneruleversionnumber");
            }
            set
            {
                this.OnPropertyChanging("TimeZoneRuleVersionNumber");
                this.SetAttributeValue("timezoneruleversionnumber", value);
                this.OnPropertyChanged("TimeZoneRuleVersionNumber");
            }
        }

        /// <summary>
        /// Date and time when the system job was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Date and time when the attribute map was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }
        
        /// <summary>
        /// Time zone code that was in use when the record was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("utcconversiontimezonecode")]
        public System.Nullable<int> UTCConversionTimeZoneCode
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("utcconversiontimezonecode");
            }
            set
            {
                this.OnPropertyChanging("UTCConversionTimeZoneCode");
                this.SetAttributeValue("utcconversiontimezonecode", value);
                this.OnPropertyChanged("UTCConversionTimeZoneCode");
            }
        }

        /// <summary>
        /// Status of the AutoNumberState
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
        public System.Nullable<AutoNumberState> StateCode
        {
            get
            {
                Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
                if ((optionSet != null))
                {
                    return ((AutoNumberState)(System.Enum.ToObject(typeof(AutoNumberState), optionSet.Value)));
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.OnPropertyChanging("StateCode");
                if ((value == null))
                {
                    this.SetAttributeValue("statecode", null);
                }
                else
                {
                    this.SetAttributeValue("statecode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
                }
                this.OnPropertyChanged("StateCode");
            }
        }
        
        /// <summary>
        /// Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ownerid")]
        public Microsoft.Xrm.Sdk.EntityReference OwnerId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("ownerid");
            }
            set
            {
                this.OnPropertyChanging("OwnerId");
                this.SetAttributeValue("ownerid", value);
                this.OnPropertyChanged("OwnerId");
            }
        }
        /// <summary>
        /// Shows who created the record on behalf of another user.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Select the  status.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
        public Microsoft.Xrm.Sdk.OptionSetValue StatusCode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
            }
            set
            {
                this.OnPropertyChanging("StatusCode");
                this.SetAttributeValue("statuscode", value);
                this.OnPropertyChanged("StatusCode");
            }
        }


        /// <summary>
        /// Unique identifier of the user who created the activity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Shows who created the record on behalf of another user.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Shows who last updated the record.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }
    }

    [System.Runtime.Serialization.DataContractAttribute()]
    public enum au_counterconfigurationState
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Active = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Inactive = 1,
    }

    /// <summary>
    /// Type that inherits from the IPlugin interface and is contained within a plug-in assembly.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("plugintype")]
    public partial class PluginType : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PluginType() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "plugintype";

        public const int EntityTypeCode = 4602;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.PluginTypeId = value;
            }
        }

        /// <summary>
        /// Full path name of the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("assemblyname")]
        public string AssemblyName
        {
            get
            {
                return this.GetAttributeValue<string>("assemblyname");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the plug-in type was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the plugintype.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Culture code for the plug-in assembly.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("culture")]
        public string Culture
        {
            get
            {
                return this.GetAttributeValue<string>("culture");
            }
        }

        /// <summary>
        /// Customization level of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Serialized Custom Activity Type information, including required arguments. For more information, see SandboxCustomActivityInfo.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customworkflowactivityinfo")]
        public string CustomWorkflowActivityInfo
        {
            get
            {
                return this.GetAttributeValue<string>("customworkflowactivityinfo");
            }
        }

        /// <summary>
        /// Description of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// User friendly name for the plug-in.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("friendlyname")]
        public string FriendlyName
        {
            get
            {
                return this.GetAttributeValue<string>("friendlyname");
            }
            set
            {
                this.OnPropertyChanging("FriendlyName");
                this.SetAttributeValue("friendlyname", value);
                this.OnPropertyChanged("FriendlyName");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Indicates if the plug-in is a custom activity for workflows.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isworkflowactivity")]
        public System.Nullable<bool> IsWorkflowActivity
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isworkflowactivity");
            }
        }

        /// <summary>
        /// Major of the version number of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("major")]
        public System.Nullable<int> Major
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("major");
            }
        }

        /// <summary>
        /// Minor of the version number of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("minor")]
        public System.Nullable<int> Minor
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("minor");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the plug-in type was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the plugintype.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the plug-in type is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in assembly that contains this plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pluginassemblyid")]
        public Microsoft.Xrm.Sdk.EntityReference PluginAssemblyId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("pluginassemblyid");
            }
            set
            {
                this.OnPropertyChanging("PluginAssemblyId");
                this.SetAttributeValue("pluginassemblyid", value);
                this.OnPropertyChanged("PluginAssemblyId");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        public System.Nullable<System.Guid> PluginTypeId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("plugintypeid");
            }
            set
            {
                this.OnPropertyChanging("PluginTypeId");
                this.SetAttributeValue("plugintypeid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("PluginTypeId");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeidunique")]
        public System.Nullable<System.Guid> PluginTypeIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("plugintypeidunique");
            }
        }

        /// <summary>
        /// Public key token of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("publickeytoken")]
        public string PublicKeyToken
        {
            get
            {
                return this.GetAttributeValue<string>("publickeytoken");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("supportingsolutionid")]
        public System.Nullable<System.Guid> SupportingSolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("supportingsolutionid");
            }
        }

        /// <summary>
        /// Fully qualified type name of the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("typename")]
        public string TypeName
        {
            get
            {
                return this.GetAttributeValue<string>("typename");
            }
            set
            {
                this.OnPropertyChanging("TypeName");
                this.SetAttributeValue("typename", value);
                this.OnPropertyChanged("TypeName");
            }
        }

        /// <summary>
        /// Version number of the assembly for the plug-in type.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("version")]
        public string Version
        {
            get
            {
                return this.GetAttributeValue<string>("version");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// Group name of workflow custom activity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("workflowactivitygroupname")]
        public string WorkflowActivityGroupName
        {
            get
            {
                return this.GetAttributeValue<string>("workflowactivitygroupname");
            }
            set
            {
                this.OnPropertyChanging("WorkflowActivityGroupName");
                this.SetAttributeValue("workflowactivitygroupname", value);
                this.OnPropertyChanged("WorkflowActivityGroupName");
            }
        }

        /// <summary>
        /// 1:N plugintype_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintype_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> plugintype_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("plugintype_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintype_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("plugintype_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintype_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// 1:N plugintypeid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintypeid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> plugintypeid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("plugintypeid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintypeid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("plugintypeid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintypeid_sdkmessageprocessingstep");
            }
        }
    }

    /// <summary>
    /// Message that is supported by the SDK.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessage")]
    public partial class SdkMessage : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessage() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessage";

        public const int EntityTypeCode = 4606;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageId = value;
            }
        }

        /// <summary>
        /// Information about whether the SDK message is automatically transacted.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("autotransact")]
        public System.Nullable<bool> AutoTransact
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("autotransact");
            }
            set
            {
                this.OnPropertyChanging("AutoTransact");
                this.SetAttributeValue("autotransact", value);
                this.OnPropertyChanged("AutoTransact");
            }
        }

        /// <summary>
        /// Identifies where a method will be exposed. 0 - Server, 1 - Client, 2 - both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("availability")]
        public System.Nullable<int> Availability
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("availability");
            }
            set
            {
                this.OnPropertyChanging("Availability");
                this.SetAttributeValue("availability", value);
                this.OnPropertyChanged("Availability");
            }
        }

        /// <summary>
        /// If this is a categorized method, this is the name, otherwise None.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("categoryname")]
        public string CategoryName
        {
            get
            {
                return this.GetAttributeValue<string>("categoryname");
            }
            set
            {
                this.OnPropertyChanging("CategoryName");
                this.SetAttributeValue("categoryname", value);
                this.OnPropertyChanged("CategoryName");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Indicates whether the SDK message should have its requests expanded per primary entity defined in its filters.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("expand")]
        public System.Nullable<bool> Expand
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("expand");
            }
            set
            {
                this.OnPropertyChanging("Expand");
                this.SetAttributeValue("expand", value);
                this.OnPropertyChanged("Expand");
            }
        }

        /// <summary>
        /// Information about whether the SDK message is active.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isactive")]
        public System.Nullable<bool> IsActive
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isactive");
            }
            set
            {
                this.OnPropertyChanging("IsActive");
                this.SetAttributeValue("isactive", value);
                this.OnPropertyChanged("IsActive");
            }
        }

        /// <summary>
        /// Indicates whether the SDK message is private.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isprivate")]
        public System.Nullable<bool> IsPrivate
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isprivate");
            }
            set
            {
                this.OnPropertyChanging("IsPrivate");
                this.SetAttributeValue("isprivate", value);
                this.OnPropertyChanged("IsPrivate");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isvalidforexecuteasync")]
        public System.Nullable<bool> IsValidForExecuteAsync
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isvalidforexecuteasync");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public System.Nullable<System.Guid> SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageId");
                this.SetAttributeValue("sdkmessageid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageidunique")]
        public System.Nullable<System.Guid> SdkMessageIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageidunique");
            }
        }

        /// <summary>
        /// Indicates whether the SDK message is a template.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("template")]
        public System.Nullable<bool> Template
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("template");
            }
            set
            {
                this.OnPropertyChanging("Template");
                this.SetAttributeValue("template", value);
                this.OnPropertyChanged("Template");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("throttlesettings")]
        public string ThrottleSettings
        {
            get
            {
                return this.GetAttributeValue<string>("throttlesettings");
            }
        }

        /// <summary>
        /// Number that identifies a specific revision of the SDK message. 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// 1:N sdkmessageid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> sdkmessageid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("sdkmessageid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("sdkmessageid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// 1:N sdkmessageid_sdkmessagefilter
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessagefilter")]
        public System.Collections.Generic.IEnumerable<SdkMessageFilter> sdkmessageid_sdkmessagefilter
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageFilter>("sdkmessageid_sdkmessagefilter", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessagefilter");
                this.SetRelatedEntities<SdkMessageFilter>("sdkmessageid_sdkmessagefilter", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessagefilter");
            }
        }
    }

    /// <summary>
    /// Filter that defines which SDK messages are valid for each type of entity.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessagefilter")]
    public partial class SdkMessageFilter : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessageFilter() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessagefilter";

        public const int EntityTypeCode = 4607;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageFilterId = value;
            }
        }

        /// <summary>
        /// Identifies where a method will be exposed. 0 - Server, 1 - Client, 2 - both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("availability")]
        public System.Nullable<int> Availability
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("availability");
            }
            set
            {
                this.OnPropertyChanging("Availability");
                this.SetAttributeValue("availability", value);
                this.OnPropertyChanged("Availability");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message filter was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessagefilter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Indicates whether a custom SDK message processing step is allowed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomprocessingstepallowed")]
        public System.Nullable<bool> IsCustomProcessingStepAllowed
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("iscustomprocessingstepallowed");
            }
            set
            {
                this.OnPropertyChanging("IsCustomProcessingStepAllowed");
                this.SetAttributeValue("iscustomprocessingstepallowed", value);
                this.OnPropertyChanged("IsCustomProcessingStepAllowed");
            }
        }

        /// <summary>
        /// Indicates whether the filter should be visible.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isvisible")]
        public System.Nullable<bool> IsVisible
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isvisible");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message filter was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessagefilter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message filter is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Type of entity with which the SDK message filter is primarily associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("primaryobjecttypecode")]
        public string PrimaryObjectTypeCode
        {
            get
            {
                return this.GetAttributeValue<string>("primaryobjecttypecode");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message filter entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        public System.Nullable<System.Guid> SdkMessageFilterId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessagefilterid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageFilterId");
                this.SetAttributeValue("sdkmessagefilterid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageFilterId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilteridunique")]
        public System.Nullable<System.Guid> SdkMessageFilterIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessagefilteridunique");
            }
        }

        /// <summary>
        /// Unique identifier of the related SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageId");
                this.SetAttributeValue("sdkmessageid", value);
                this.OnPropertyChanged("SdkMessageId");
            }
        }

        /// <summary>
        /// Type of entity with which the SDK message filter is secondarily associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("secondaryobjecttypecode")]
        public string SecondaryObjectTypeCode
        {
            get
            {
                return this.GetAttributeValue<string>("secondaryobjecttypecode");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// 1:N sdkmessagefilterid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessagefilterid_sdkmessageprocessingstep")]
        public System.Collections.Generic.IEnumerable<SdkMessageProcessingStep> sdkmessagefilterid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntities<SdkMessageProcessingStep>("sdkmessagefilterid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessagefilterid_sdkmessageprocessingstep");
                this.SetRelatedEntities<SdkMessageProcessingStep>("sdkmessagefilterid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessagefilterid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 sdkmessageid_sdkmessagefilter
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessagefilter")]
        public SdkMessage sdkmessageid_sdkmessagefilter
        {
            get
            {
                return this.GetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessagefilter", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessagefilter");
                this.SetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessagefilter", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessagefilter");
            }
        }
    }

    [System.Runtime.Serialization.DataContractAttribute()]
    public enum SdkMessageProcessingStepState
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Enabled = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Disabled = 1,
    }

    /// <summary>
    /// Stage in the execution pipeline that a plug-in is to execute.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessageprocessingstep")]
    public partial class SdkMessageProcessingStep : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SdkMessageProcessingStep() :
            base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "sdkmessageprocessingstep";

        public const int EntityTypeCode = 4608;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
        public override System.Guid Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                this.SdkMessageProcessingStepId = value;
            }
        }

        /// <summary>
        /// Indicates whether the asynchronous system job is automatically deleted on completion.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("asyncautodelete")]
        public System.Nullable<bool> AsyncAutoDelete
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("asyncautodelete");
            }
            set
            {
                this.OnPropertyChanging("AsyncAutoDelete");
                this.SetAttributeValue("asyncautodelete", value);
                this.OnPropertyChanged("AsyncAutoDelete");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Step-specific configuration for the plug-in type. Passed to the plug-in constructor at run time.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("configuration")]
        public string Configuration
        {
            get
            {
                return this.GetAttributeValue<string>("configuration");
            }
            set
            {
                this.OnPropertyChanging("Configuration");
                this.SetAttributeValue("configuration", value);
                this.OnPropertyChanged("Configuration");
            }
        }

        /// <summary>
        /// Unique identifier of the user who created the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was created.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who created the sdkmessageprocessingstep.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
        }

        /// <summary>
        /// Customization level of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
        public System.Nullable<int> CustomizationLevel
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
            }
        }

        /// <summary>
        /// Description of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Unique identifier of the associated event handler.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
        public Microsoft.Xrm.Sdk.EntityReference EventHandler
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("eventhandler");
            }
            set
            {
                this.OnPropertyChanging("EventHandler");
                this.SetAttributeValue("eventhandler", value);
                this.OnPropertyChanged("EventHandler");
            }
        }

        /// <summary>
        /// Comma-separated list of attributes. If at least one of these attributes is modified, the plug-in should execute.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filteringattributes")]
        public string FilteringAttributes
        {
            get
            {
                return this.GetAttributeValue<string>("filteringattributes");
            }
            set
            {
                this.OnPropertyChanging("FilteringAttributes");
                this.SetAttributeValue("filteringattributes", value);
                this.OnPropertyChanged("FilteringAttributes");
            }
        }

        /// <summary>
        /// Unique identifier of the user to impersonate context when step is executed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("impersonatinguserid")]
        public Microsoft.Xrm.Sdk.EntityReference ImpersonatingUserId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("impersonatinguserid");
            }
            set
            {
                this.OnPropertyChanging("ImpersonatingUserId");
                this.SetAttributeValue("impersonatinguserid", value);
                this.OnPropertyChanged("ImpersonatingUserId");
            }
        }

        /// <summary>
        /// Version in which the form is introduced.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Identifies if a plug-in should be executed from a parent pipeline, a child pipeline, or both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invocationsource")]
        [System.ObsoleteAttribute()]
        public Microsoft.Xrm.Sdk.OptionSetValue InvocationSource
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invocationsource");
            }
            set
            {
                this.OnPropertyChanging("InvocationSource");
                this.SetAttributeValue("invocationsource", value);
                this.OnPropertyChanged("InvocationSource");
            }
        }

        /// <summary>
        /// Information that specifies whether this component can be customized.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
            }
            set
            {
                this.OnPropertyChanging("IsCustomizable");
                this.SetAttributeValue("iscustomizable", value);
                this.OnPropertyChanged("IsCustomizable");
            }
        }

        /// <summary>
        /// Information that specifies whether this component should be hidden.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ishidden")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsHidden
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("ishidden");
            }
            set
            {
                this.OnPropertyChanging("IsHidden");
                this.SetAttributeValue("ishidden", value);
                this.OnPropertyChanged("IsHidden");
            }
        }

        /// <summary>
        /// Information that specifies whether this component is managed.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Run-time mode of execution, for example, synchronous or asynchronous.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mode")]
        public Microsoft.Xrm.Sdk.OptionSetValue Mode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("mode");
            }
            set
            {
                this.OnPropertyChanging("Mode");
                this.SetAttributeValue("mode", value);
                this.OnPropertyChanged("Mode");
            }
        }

        /// <summary>
        /// Unique identifier of the user who last modified the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Date and time when the SDK message processing step was last modified.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Unique identifier of the delegate user who last modified the sdkmessageprocessingstep.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
        }

        /// <summary>
        /// Name of SdkMessage processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Unique identifier of the organization with which the SDK message processing step is associated.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Unique identifier of the plug-in type associated with the step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        [System.ObsoleteAttribute()]
        public Microsoft.Xrm.Sdk.EntityReference PluginTypeId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("plugintypeid");
            }
            set
            {
                this.OnPropertyChanging("PluginTypeId");
                this.SetAttributeValue("plugintypeid", value);
                this.OnPropertyChanged("PluginTypeId");
            }
        }

        /// <summary>
        /// Processing order within the stage.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("rank")]
        public System.Nullable<int> Rank
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("rank");
            }
            set
            {
                this.OnPropertyChanging("Rank");
                this.SetAttributeValue("rank", value);
                this.OnPropertyChanged("Rank");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message filter.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageFilterId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessagefilterid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageFilterId");
                this.SetAttributeValue("sdkmessagefilterid", value);
                this.OnPropertyChanged("SdkMessageFilterId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageId");
                this.SetAttributeValue("sdkmessageid", value);
                this.OnPropertyChanged("SdkMessageId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step entity.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepId");
                this.SetAttributeValue("sdkmessageprocessingstepid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("SdkMessageProcessingStepId");
            }
        }

        /// <summary>
        /// Unique identifier of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepidunique")]
        public System.Nullable<System.Guid> SdkMessageProcessingStepIdUnique
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepidunique");
            }
        }

        /// <summary>
        /// Unique identifier of the Sdk message processing step secure configuration.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
        public Microsoft.Xrm.Sdk.EntityReference SdkMessageProcessingStepSecureConfigId
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageprocessingstepsecureconfigid");
            }
            set
            {
                this.OnPropertyChanging("SdkMessageProcessingStepSecureConfigId");
                this.SetAttributeValue("sdkmessageprocessingstepsecureconfigid", value);
                this.OnPropertyChanged("SdkMessageProcessingStepSecureConfigId");
            }
        }

        /// <summary>
        /// Unique identifier of the associated solution.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Stage in the execution pipeline that the SDK message processing step is in.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stage")]
        public Microsoft.Xrm.Sdk.OptionSetValue Stage
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("stage");
            }
            set
            {
                this.OnPropertyChanging("Stage");
                this.SetAttributeValue("stage", value);
                this.OnPropertyChanged("Stage");
            }
        }

        /// <summary>
        /// Status of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
        public System.Nullable<SdkMessageProcessingStepState> StateCode
        {
            get
            {
                Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
                if ((optionSet != null))
                {
                    return ((SdkMessageProcessingStepState)(System.Enum.ToObject(typeof(SdkMessageProcessingStepState), optionSet.Value)));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Reason for the status of the SDK message processing step.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
        public Microsoft.Xrm.Sdk.OptionSetValue StatusCode
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
            }
            set
            {
                this.OnPropertyChanging("StatusCode");
                this.SetAttributeValue("statuscode", value);
                this.OnPropertyChanged("StatusCode");
            }
        }

        /// <summary>
        /// Deployment that the SDK message processing step should be executed on; server, client, or both.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("supporteddeployment")]
        public Microsoft.Xrm.Sdk.OptionSetValue SupportedDeployment
        {
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("supporteddeployment");
            }
            set
            {
                this.OnPropertyChanging("SupportedDeployment");
                this.SetAttributeValue("supporteddeployment", value);
                this.OnPropertyChanged("SupportedDeployment");
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("supportingsolutionid")]
        public System.Nullable<System.Guid> SupportingSolutionId
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("supportingsolutionid");
            }
        }

        /// <summary>
        /// Number that identifies a specific revision of the SDK message processing step. 
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// N:1 plugintype_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintype_sdkmessageprocessingstep")]
        public PluginType plugintype_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<PluginType>("plugintype_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintype_sdkmessageprocessingstep");
                this.SetRelatedEntity<PluginType>("plugintype_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintype_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 plugintypeid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintypeid_sdkmessageprocessingstep")]
        public PluginType plugintypeid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<PluginType>("plugintypeid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("plugintypeid_sdkmessageprocessingstep");
                this.SetRelatedEntity<PluginType>("plugintypeid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("plugintypeid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 sdkmessagefilterid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessagefilterid_sdkmessageprocessingstep")]
        public SdkMessageFilter sdkmessagefilterid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<SdkMessageFilter>("sdkmessagefilterid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessagefilterid_sdkmessageprocessingstep");
                this.SetRelatedEntity<SdkMessageFilter>("sdkmessagefilterid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessagefilterid_sdkmessageprocessingstep");
            }
        }

        /// <summary>
        /// N:1 sdkmessageid_sdkmessageprocessingstep
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessageprocessingstep")]
        public SdkMessage sdkmessageid_sdkmessageprocessingstep
        {
            get
            {
                return this.GetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessageprocessingstep", null);
            }
            set
            {
                this.OnPropertyChanging("sdkmessageid_sdkmessageprocessingstep");
                this.SetRelatedEntity<SdkMessage>("sdkmessageid_sdkmessageprocessingstep", null, value);
                this.OnPropertyChanged("sdkmessageid_sdkmessageprocessingstep");
            }
        }
    }

    /// <summary>
    /// Represents a source of entities bound to a CRM service. It tracks and manages changes made to the retrieved entities.
    /// </summary>
    public partial class XrmServiceContext : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public XrmServiceContext(Microsoft.Xrm.Sdk.IOrganizationService service) :
            base(service)
        {
        }

        /// <summary>
        /// Gets a binding to the set of all <see cref="au_counterconfiguration"/> entities.
        /// </summary>
        public System.Linq.IQueryable<DSAutoNumber> dots_autonumberSet
        {
            get
            {
                return this.CreateQuery<DSAutoNumber>();
            }
        }
        /// <summary>
        /// Gets a binding to the set of all <see cref="PluginType"/> entities.
        /// </summary>
        public System.Linq.IQueryable<PluginType> PluginTypeSet
        {
            get
            {
                return this.CreateQuery<PluginType>();
            }
        }
        /// <summary>
        /// Gets a binding to the set of all <see cref="SdkMessage"/> entities.
        /// </summary>
        public System.Linq.IQueryable<SdkMessage> SdkMessageSet
        {
            get
            {
                return this.CreateQuery<SdkMessage>();
            }
        }
        /// <summary>
        /// Gets a binding to the set of all <see cref="SdkMessageFilter"/> entities.
        /// </summary>
        public System.Linq.IQueryable<SdkMessageFilter> SdkMessageFilterSet
        {
            get
            {
                return this.CreateQuery<SdkMessageFilter>();
            }
        }
        /// <summary>
        /// Gets a binding to the set of all <see cref="SdkMessageProcessingStep"/> entities.
        /// </summary>
        public System.Linq.IQueryable<SdkMessageProcessingStep> SdkMessageProcessingStepSet
        {
            get
            {
                return this.CreateQuery<SdkMessageProcessingStep>();
            }
        }
    }
}
