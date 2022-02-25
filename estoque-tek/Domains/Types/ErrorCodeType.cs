using System.ComponentModel;

namespace estoque_tek.Domains.Types
{
    public enum ErrorCodeType
    {
        // Contractors
        [Description("Contractor does not exists")]
        ContractorNotExisting = 01,

        [Description("Contractor not found")]
        ContractorNotFound = 02,

        // Users 
        [Description("User not found")]
        UserNotFound = 10,
    }
}
