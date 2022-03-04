using System;
using System.Collections.Generic;
using System.Text;

namespace IntuneFunctions
{
    class GraphDump
    {
    }


    public class Rootobject
    {
        public string odatacontext { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string odataid { get; set; }
        public string id { get; set; }
        public object deletedDateTime { get; set; }
        public bool accountEnabled { get; set; }
        public DateTime approximateLastSignInDateTime { get; set; }
        public object complianceExpirationDateTime { get; set; }
        public DateTime createdDateTime { get; set; }
        public object deviceCategory { get; set; }
        public string deviceId { get; set; }
        public object deviceMetadata { get; set; }
        public string deviceOwnership { get; set; }
        public int? deviceVersion { get; set; }
        public string displayName { get; set; }
        public object domainName { get; set; }
        public object enrollmentProfileName { get; set; }
        public string enrollmentType { get; set; }
        public object externalSourceName { get; set; }
        public bool? isCompliant { get; set; }
        public bool? isManaged { get; set; }
        public object isManagementRestricted { get; set; }
        public bool? isRooted { get; set; }
        public string managementType { get; set; }
        public string manufacturer { get; set; }
        public string mdmAppId { get; set; }
        public string model { get; set; }
        public DateTime? onPremisesLastSyncDateTime { get; set; }
        public bool? onPremisesSyncEnabled { get; set; }
        public string operatingSystem { get; set; }
        public string operatingSystemVersion { get; set; }
        public object[] hostnames { get; set; }
        public string[] physicalIds { get; set; }
        public string profileType { get; set; }
        public DateTime registrationDateTime { get; set; }
        public object sourceType { get; set; }
        public object[] systemLabels { get; set; }
        public string trustType { get; set; }
        public Alternativesecurityid[] alternativeSecurityIds { get; set; }
        public Extensionattributes extensionAttributes { get; set; }
    }

    public class Extensionattributes
    {
        public object extensionAttribute1 { get; set; }
        public object extensionAttribute2 { get; set; }
        public object extensionAttribute3 { get; set; }
        public object extensionAttribute4 { get; set; }
        public object extensionAttribute5 { get; set; }
        public object extensionAttribute6 { get; set; }
        public object extensionAttribute7 { get; set; }
        public object extensionAttribute8 { get; set; }
        public object extensionAttribute9 { get; set; }
        public object extensionAttribute10 { get; set; }
        public object extensionAttribute11 { get; set; }
        public object extensionAttribute12 { get; set; }
        public object extensionAttribute13 { get; set; }
        public object extensionAttribute14 { get; set; }
        public object extensionAttribute15 { get; set; }
    }

    public class Alternativesecurityid
    {
        public int type { get; set; }
        public object identityProvider { get; set; }
        public string key { get; set; }
    }

}
