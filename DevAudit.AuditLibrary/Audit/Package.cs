﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DevAudit.AuditLibrary
{
    [Serializable]
    public class Package : IPackage
    {
        [JsonProperty("pm")]
        public string PackageManager { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        [JsonIgnore]
        public string Architecture { get; set; }

        public Package(string package_manager, string application_name, string version, string vendor = null, string group = null, string architecture = null)
        {
            this.PackageManager = package_manager;
            this.Name = application_name;
            this.Version = version;
            if (!string.IsNullOrEmpty(vendor)) this.Vendor = vendor;
            if (!string.IsNullOrEmpty(group)) this.Group = group;
            if (!string.IsNullOrEmpty(architecture)) this.Architecture = architecture;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Package);
        }

        public bool Equals(Package other)
        {
            if (Name != other.Name)
                return false;

            if (PackageManager != other.PackageManager)
                return false;

            if (Version != other.Version)
                return false;

            /*
            if (Group != other.Group)
                return false;

            if (Vendor != other.Vendor)
                return false;

            if (Architecture != other.Architecture)
                return false;
            */
            return true;
        } 

        public string getPurl()
        {
            if (Group != null)
            {
                return "pkg:" + PackageManager + "/" + Group + "/" + Name + "@" + Version;
            }
            else
            {
                return "pkg:" + PackageManager + "/" + Name + "@" + Version;
            }
        }
    }
}


