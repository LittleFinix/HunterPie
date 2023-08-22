namespace HunterPie.Core.System.Linux;

[NativeTypeName("unsigned int")]
public enum SecretSchemaFlags : uint
{
    SECRET_SCHEMA_NONE = 0,
    SECRET_SCHEMA_DONT_MATCH_NAME = 1 << 1,
}
