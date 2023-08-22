using System.Runtime.InteropServices;

namespace HunterPie.Core.System.Linux;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public unsafe partial struct SecretSchema
{
    [NativeTypeName("const gchar *")]
    [MarshalAs(UnmanagedType.LPStr)]
    public nint name;

    public SecretSchemaFlags flags;

    [NativeTypeName("SecretSchemaAttribute[32]")]
    public _attributes_e__FixedBuffer attributes;

    [NativeTypeName("gint")]
    public int reserved;

    [NativeTypeName("gpointer")]
    public void* reserved1;

    [NativeTypeName("gpointer")]
    public void* reserved2;

    [NativeTypeName("gpointer")]
    public void* reserved3;

    [NativeTypeName("gpointer")]
    public void* reserved4;

    [NativeTypeName("gpointer")]
    public void* reserved5;

    [NativeTypeName("gpointer")]
    public void* reserved6;

    [NativeTypeName("gpointer")]
    public void* reserved7;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct _attributes_e__FixedBuffer
    {
        public SecretSchemaAttribute e0;
        public SecretSchemaAttribute e1;
        public SecretSchemaAttribute e2;
        public SecretSchemaAttribute e3;
        public SecretSchemaAttribute e4;
        public SecretSchemaAttribute e5;
        public SecretSchemaAttribute e6;
        public SecretSchemaAttribute e7;
        public SecretSchemaAttribute e8;
        public SecretSchemaAttribute e9;
        public SecretSchemaAttribute e10;
        public SecretSchemaAttribute e11;
        public SecretSchemaAttribute e12;
        public SecretSchemaAttribute e13;
        public SecretSchemaAttribute e14;
        public SecretSchemaAttribute e15;
        public SecretSchemaAttribute e16;
        public SecretSchemaAttribute e17;
        public SecretSchemaAttribute e18;
        public SecretSchemaAttribute e19;
        public SecretSchemaAttribute e20;
        public SecretSchemaAttribute e21;
        public SecretSchemaAttribute e22;
        public SecretSchemaAttribute e23;
        public SecretSchemaAttribute e24;
        public SecretSchemaAttribute e25;
        public SecretSchemaAttribute e26;
        public SecretSchemaAttribute e27;
        public SecretSchemaAttribute e28;
        public SecretSchemaAttribute e29;
        public SecretSchemaAttribute e30;
        public SecretSchemaAttribute e31;

        // [UnscopedRef]
        // public ref SecretSchemaAttribute this[int index]
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get
        //     {
        //         return ref AsSpan()[index];
        //     }
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // [UnscopedRef]
        // public Span<SecretSchemaAttribute> AsSpan() => MemoryMarshal.CreateSpan(ref e0, 32);
    }
}
