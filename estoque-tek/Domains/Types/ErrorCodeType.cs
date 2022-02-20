using System.ComponentModel;

namespace estoque_tek.Domains.Types
{
    public enum ErrorCodeType
    {
        // Contractors
        [Description("Contractor does not exists")]
        ContractorNotExisting = 01,
    }
}
