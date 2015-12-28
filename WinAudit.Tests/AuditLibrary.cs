﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using WinAudit.AuditLibrary;
namespace WinAudit.Tests
{
    public class AuditLibraryTests
    {
        protected Audit audit = new Audit("1.1");

        public AuditLibraryTests()
        {

        }
        [Fact]
        public void CanGetMSIPackages()
        {
            IEnumerable<OSSIndexQueryObject> packages = audit.GetMSIPackages();
            Assert.NotEmpty(packages);
            Assert.NotEmpty(packages.Where(p => !string.IsNullOrEmpty(p.Vendor) && p.Vendor.StartsWith("Microsoft")));
            Assert.NotEmpty(packages.Where(p => !string.IsNullOrEmpty(p.Vendor) && p.Name.StartsWith("Windows")));
        }

        [Fact]
        public void CanGetChocolateyPackages()
        {
            IEnumerable<OSSIndexQueryObject> packages = audit.GetChocolateyPackages();
            Assert.NotEmpty(packages);
        }

        [Fact]
        public void CanGetOneGetPackages()
        {
            IEnumerable<OSSIndexQueryObject> packages = audit.GetOneGetPackages();
            Assert.NotEmpty(packages);
            Assert.NotEmpty(packages.Where(p => p.PackageManager == "msi"));
        }

        [Fact]
        public void CanGetBowerPackages()
        {
            IEnumerable<OSSIndexQueryObject> packages = audit.GetBowerPackages();
            Assert.NotEmpty(packages);
        }

        [Fact]
        public async Task CanSearchOSSIndex()
        {
            OSSIndexQueryObject q1 = new OSSIndexQueryObject("msi", "Adobe Reader", "11.0.10", "");
            OSSIndexQueryObject q2 = new OSSIndexQueryObject("msi", "Adobe Reader", "10.1.1", "");

            IEnumerable<OSSIndexQueryResultObject> r1 = await audit.SearchOSSIndex("msi", q1);
            Assert.NotEmpty(r1);
            IEnumerable<OSSIndexQueryResultObject> r2 = await audit.SearchOSSIndex("msi", new List<OSSIndexQueryObject>() { q1, q2 });
            Assert.NotEmpty(r2);
        }

        [Fact]
        public async Task CanGetOSSIndexVulnerabilityForId()
        {
            IEnumerable<OSSIndexProjectVulnerability> v = await audit.GetOSSIndexVulnerabilitiesForId("284089289");
            Assert.NotEmpty(v);
        }

        [Fact]
        public async Task CanGetPackagesAsync()
        {
            Task<IEnumerable<OSSIndexQueryObject>>[] t = 
            {
              Task<IEnumerable<OSSIndexQueryObject>>.Factory.StartNew(() => audit.GetMSIPackages()),
              Task<IEnumerable<OSSIndexQueryObject>>.Factory.StartNew(() => audit.GetOneGetPackages()),
              Task<IEnumerable<OSSIndexQueryObject>>.Factory.StartNew(() => audit.GetChocolateyPackages())
            };
            IEnumerable<OSSIndexQueryObject>[] results = await Task.WhenAll(t);
            Assert.NotEmpty(results);
        }

        public async Task CanSearchPackagesAsync()
        {
            List<Exception> errors = new List<Exception>();
            Task<IEnumerable<OSSIndexProjectVulnerability>>[] t =
            {audit.GetOSSIndexVulnerabilitiesForId("284089289"), audit.GetOSSIndexVulnerabilitiesForId("2840892") };
            try
            {
                Task.WaitAll(t);
            }
            catch (AggregateException ae)
            {
                
            }

        }
    }
}