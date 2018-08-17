using System;

namespace Beads
{
    public struct BeadsValue
    {
        readonly byte[] array;
        public readonly BeadType Type;
        readonly int cursor;
        readonly ulong size;
        readonly ulong count;

        public BeadsValue(byte[] array, BeadType type, int cursor, ulong size = 0, ulong count = 0)
        {
            this.array = array;
            this.Type = type;
            this.cursor = cursor;
            this.size = size;
            this.count = count;
        }

        public bool isNil => Type == BeadType.Nil;

        public int? Int
        {
            get
            {
                if (Type == BeadType.I8)
                {
                    return (int)(sbyte)array[cursor];
                }
                if (Type == BeadType.U8)
                {
                    return (int)array[cursor];
                }
                if (Type == BeadType.I16)
                {
                    return BitConverter.ToInt16(array, cursor);
                }
                if (Type == BeadType.U16)
                {
                    return BitConverter.ToUInt16(array, cursor);
                }
                if (Type == BeadType.I32)
                {
                    return BitConverter.ToInt32(array, cursor);
                }
            
                if (Type == BeadType.F32)
                {
                    var v = BitConverter.ToSingle(array, cursor);
                    return (v == (float)(int)v) ? (int?)v : null;
                }
                if (Type == BeadType.F64)
                {
                    var v = BitConverter.ToDouble(array, cursor);
                    return (v == (double)(int)v) ? (int?)v : null;
                }

                return null;
            }
        }
    
        public long? Long
        {
            get
            {
                if (Type == BeadType.I8)
                {
                    return (int)(sbyte)array[cursor];
                }
                if (Type == BeadType.U8)
                {
                    return (int)array[cursor];
                }
                if (Type == BeadType.I16)
                {
                    return BitConverter.ToInt16(array, cursor);
                }
                if (Type == BeadType.U16)
                {
                    return BitConverter.ToUInt16(array, cursor);
                }
                if (Type == BeadType.I32)
                {
                    return BitConverter.ToInt32(array, cursor);
                }
                if (Type == BeadType.U32)
                {
                    return BitConverter.ToUInt32(array, cursor);
                }
                if (Type == BeadType.F32)
                {
                    var v = BitConverter.ToSingle(array, cursor);
                    return (v == (float)(long)v) ? (long?)v : null;
                }
                if (Type == BeadType.I64)
                {
                    return BitConverter.ToInt64(array, cursor);
                }
                if (Type == BeadType.F64)
                {
                    var v = BitConverter.ToDouble(array, cursor);
                    return (v == (double)(long)v) ? (long?)v : null;
                }

                return null;
            }
        }
    
        public ulong? ULong
        {
            get
            {
                if (Type == BeadType.I8)
                {
                    var v = (sbyte)array[cursor];
                    return v > 0 ? (ulong?)v : null;
                }
                if (Type == BeadType.U8)
                {
                    return array[cursor];
                }
                if (Type == BeadType.I16)
                {
                    var v = BitConverter.ToInt16(array, cursor);
                    return v > 0 ? (ulong?)v : null;
                }
                if (Type == BeadType.U16)
                {
                    return BitConverter.ToUInt16(array, cursor);
                }
                if (Type == BeadType.I32)
                {
                    var v = BitConverter.ToInt32(array, cursor);
                    return v > 0 ? (ulong?)v : null;
                }
                if (Type == BeadType.U32)
                {
                    return BitConverter.ToUInt32(array, cursor);
                }
                if (Type == BeadType.F32)
                {
                    var v = BitConverter.ToSingle(array, cursor);
                    return v > 0 && (v == (float)(ulong)v) ? (ulong?)v : null;
                }
                if (Type == BeadType.I64)
                {
                    var v = BitConverter.ToInt64(array, cursor);
                    return v > 0 ? (ulong?)v : null;
                }
                if (Type == BeadType.U64)
                {
                    return BitConverter.ToUInt64(array, cursor);
                }
                if (Type == BeadType.F64)
                {
                    var v = BitConverter.ToDouble(array, cursor);
                    return v > 0 && (v == (double)(ulong)v) ? (ulong?)v : null;
                }

                return null;
            }
        }
    
        public double? Double
        {
            get
            {
                if (Type == BeadType.I8)
                {
                    return (double)(sbyte)array[cursor];
                }
                if (Type == BeadType.U8)
                {
                    return (double)array[cursor];
                }
                if (Type == BeadType.I16)
                {
                    return BitConverter.ToInt16(array, cursor);
                }
                if (Type == BeadType.U16)
                {
                    return BitConverter.ToUInt16(array, cursor);
                }
                if (Type == BeadType.F16)
                {
                    return BeadsSequence.FromHalf(BitConverter.ToUInt16(array, cursor));
                }
                if (Type == BeadType.I32)
                {
                    return BitConverter.ToInt32(array, cursor);
                }
                if (Type == BeadType.U32)
                {
                    return BitConverter.ToUInt32(array, cursor);
                }
                if (Type == BeadType.F32)
                {
                    return BitConverter.ToSingle(array, cursor);
                }
                if (Type == BeadType.I64)
                {
                    return BitConverter.ToInt64(array, cursor);
                }
                if (Type == BeadType.F64)
                {
                    return BitConverter.ToDouble(array, cursor);
                }

                return null;
            }
        }
    
        public float? Float
        {
            get
            {
                if (Type == BeadType.I8)
                {
                    return (float)(sbyte)array[cursor];
                }
                if (Type == BeadType.U8)
                {
                    return (float)array[cursor];
                }
                if (Type == BeadType.I16)
                {
                    return BitConverter.ToInt16(array, cursor);
                }
                if (Type == BeadType.U16)
                {
                    return BitConverter.ToUInt16(array, cursor);
                }
                if (Type == BeadType.F16)
                {
                    return BeadsSequence.FromHalf(BitConverter.ToUInt16(array, cursor));
                }
                if (Type == BeadType.I32)
                {
                    return BitConverter.ToInt32(array, cursor);
                }
                if (Type == BeadType.U32)
                {
                    return BitConverter.ToUInt32(array, cursor);
                }
                if (Type == BeadType.F32)
                {
                    return BitConverter.ToSingle(array, cursor);
                }
                if (Type == BeadType.I64)
                {
                    return BitConverter.ToInt64(array, cursor);
                }

                return null;
            }
        }

        public byte[] Data
        {
            get
            {
                if (Type == BeadType.Data)
                {
                    var result = new byte[size];
                    Buffer.BlockCopy(array, cursor, result, 0, result.Length);
                    return result;
                }

                if (Type == BeadType.CompactData)
                {
                    var result = new byte[count];
                    var elementIndex = 0UL;
                    var cursor = this.cursor;
                    while (elementIndex < count && cursor < array.Length)
                    {
                        var tag = array[cursor];
                        cursor++;
                        for (var bitIndex = 0; bitIndex < 8; bitIndex++)
                        {
                            var bitMask = 1 << bitIndex;
                            if (elementIndex >= count)
                            {
                                break;
                            }

                            if ((tag & bitMask) == 0)
                            {
                                result[elementIndex] = 0;
                            }
                            else
                            {
                                result[elementIndex] = array[cursor];
                                cursor++;
                            }

                            elementIndex++;
                        }
                    }

                    return result;
                }
                return null;
            }
        }
    }
}