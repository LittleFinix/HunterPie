using HunterPie.Core.Vault;
using HunterPie.Core.Vault.Model;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace HunterPie.Core.System.Linux;

[SupportedOSPlatform("Linux")]
public class SecretService : ICredentialVault
{
    private static SecretSchema schema = new()
        {
            Name = "HunterPie",
            flags = SecretSchemaFlags.SECRET_SCHEMA_NONE,
            attributes =
            {
                e0 = {
                    Name = "username",
                    type = SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING
                },
                e1 = {
                    Name = "NULL",
                    type = SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING
                }
            }
        };
    
    private GCHandle Handle { get; } = GCHandle.Alloc(schema, GCHandleType.Pinned);
    
    private IntPtr GetSchema()
    {
        return Handle.AddrOfPinnedObject();
    }
    
    public void Create(string username, string password)
    {
        Secret.password_store_sync(
            GetSchema(),
            null,
            "HunterPi Logon",
            password,
            IntPtr.Zero, 
            IntPtr.Zero,
            __arglist(
                "username", username,
                null
            )
        );
    }

    public Credential? Get()
    {
        return null;
    }

    public void Delete()
    {
        // Secret.password_clear_sync()
    }
}