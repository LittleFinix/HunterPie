using HunterPie.Core.System.Linux;
using HunterPie.Core.System.Windows.Vault;
using HunterPie.Core.Vault;
using HunterPie.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;
internal class CredentialVaultInitializer : IInitializer
{
    public Task Init()
    {
        ICredentialVault credentialVault;
        
        if (OperatingSystem.IsWindows())
            credentialVault = new WindowsCredentialVault();
        else if (OperatingSystem.IsLinux())
            credentialVault = new SecretService();
        else
            throw new PlatformNotSupportedException();
        
        _ = new CredentialVaultService(credentialVault);

        return Task.CompletedTask;
    }
}
