namespace Beads
{
    public enum BeadType : byte
    {
        Skip = 0,
        U8 = 1,
        I8 = 2,
        U16 = 3,
        I16 = 4,
        U32 = 5,
        I32 = 6,
        F32 = 7,
        U64 = 8,
        I64 = 9,
        F64 = 10,
        F16 = 11,

        CompactData = 13, // Do we really need this type?
        Data = 14,
        Nil = 15
    }
}