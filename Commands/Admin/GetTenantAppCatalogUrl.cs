#if !SP2013
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPTenantAppCatalogUrl", SupportsShouldProcess = true)]
    [CmdletHelp(@"Retrieves the url of the tenant scoped app catalog",
        Category = CmdletHelpCategory.TenantAdmin,
        SupportedPlatform = CmdletSupportedPlatform.Online|CmdletSupportedPlatform.SP2016|CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTenantAppCatalogUrl", 
        Remarks = "Returns the url of the tenant scoped app catalog site collection", SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPTenantAppCatalogUrl", Remarks = "Returns the url of the tenant scoped app catalog site collection", SortOrder = 1)]
    public class GetTenantAppCatalogUrl : PnPCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var settings = TenantSettings.GetCurrent(ClientContext);
            settings.EnsureProperties(s => s.CorporateCatalogUrl);
            WriteObject(settings.CorporateCatalogUrl);
        }
    }
}
#endif