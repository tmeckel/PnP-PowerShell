using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Enums;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantDeletedSite", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieve site information for a deleted site.", "Use this cmdlet to retrieve information about a deleted site from your tenant administration.",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online,
        OutputType = typeof(Microsoft.Online.SharePoint.TenantAdministration.DeletedSiteProperties),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.online.sharepoint.tenantadministration.siteproperties.aspx")]
#if !ONPREMISES
    [CmdletExample(Code = @"PS:> Get-PnPTenantDeletedSite", Remarks = "Returns all site collections", SortOrder = 1)]
#endif
    [CmdletExample(Code = @"PS:> Get-PnPTenantDeletedSite -Url http://tenant.sharepoint.com/sites/projects", Remarks = "Returns information about the deleted project site", SortOrder = 1)]
    public class GetTenantDeletedSite : PnPAdminCmdlet
    {
#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "The URL of the site", Position = 0, ValueFromPipeline = true)]
#else
        [Parameter(Mandatory = true, HelpMessage = "The URL of the site", Position = 0, ValueFromPipeline = true)]
#endif
        [Alias("Identity")]
        public string Url;

        protected override void ExecuteCmdlet()
        {
            if (SPOnlineConnection.CurrentConnection.ConnectionType == ConnectionType.OnPrem)
            {
                WriteObject(ClientContext.Site);
            }
            else
            {
#if ONPREMISES
                var list = Tenant.GetDeletedSitePropertiesByUrl(Url);
                list.Context.Load(list);
                list.Context.ExecuteQueryRetry();
                WriteObject(list, true);
#else
                if (!string.IsNullOrEmpty(Url))
                {
                    var list = Tenant.GetSitePropertiesByUrl(Url, Detailed);
                    list.Context.Load(list);
                    list.Context.ExecuteQueryRetry();
                    WriteObject(list, true);
                }
                else
                {
                    throw new NotImplementedException();
                }
#endif
            }
        }
    }
}