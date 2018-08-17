using System;
using System.Collections;
using System.Collections.Generic;

namespace Beads
{
    public class BeadsSequence: IEnumerable<BeadsValue>
    {
        private byte[] buffer;
        private int elementCount;
        private int flagIndex;
        private int cursor;

        public BeadsSequence(uint capacity = 64)
        {
            buffer = new byte[capacity];
        }
        
        public BeadsSequence(byte[] beadsBuffer)
        {
            if (beadsBuffer.Length < 2)
            {
                throw new Exception("Buffer needs to be bigger than 2");
            }
            if ((beadsBuffer.Length - 2) <= sbyte.MaxValue)
            {
                var size = BitConverter.IsLittleEndian ? beadsBuffer[0] : beadsBuffer[1];
                if ((size + 2) != beadsBuffer.Length)
                {
                    throw new Exception("Size is incorrect");
                }

                elementCount = BitConverter.IsLittleEndian ? beadsBuffer[1] : beadsBuffer[0];
                buffer = new byte[size];
                cursor = size;
                Buffer.BlockCopy(beadsBuffer, 2, buffer, 0, size);
                return;
            }
            else if ((beadsBuffer.Length - 4) <= short.MaxValue)
            {
                var size = BitConverter.ToInt16(beadsBuffer, BitConverter.IsLittleEndian ? 0 : 2);
                if ((size + 4) != beadsBuffer.Length)
                {
                    throw new Exception("Size is incorrect");
                }
                
                elementCount = BitConverter.ToInt16(beadsBuffer, BitConverter.IsLittleEndian ? 2 : 0);
                buffer = new byte[size];
                cursor = size;
                Buffer.BlockCopy(beadsBuffer, 4, buffer, 0, size);
                return;
            }
            else
            {
                var size = BitConverter.ToInt32(beadsBuffer, BitConverter.IsLittleEndian ? 0 : 4);
                if ((size + 8) != beadsBuffer.Length)
                {
                    throw new Exception("Size is incorrect");
                }
                
                elementCount = BitConverter.ToInt32(beadsBuffer, BitConverter.IsLittleEndian ? 4 : 0);
                buffer = new byte[size];
                cursor = size;
                Buffer.BlockCopy(beadsBuffer, 8, buffer, 0, size);
                return;
            }
        }

        public byte[] Bytes()
        {
            while ((cursor) == elementCount) // solving polindrom problem
            {
                AddTag(BeadType.Skip);
            }

            if (cursor <= sbyte.MaxValue) // worst case scenario cursor = 2 x elementCount
            {
                var result = new byte[cursor + 2];
                if (BitConverter.IsLittleEndian)
                {
                    result[0] = (byte) (cursor);
                    result[1] = (byte) elementCount;
                }
                else
                {
                    result[0] = (byte) elementCount;
                    result[1] = (byte) (cursor);
                }

                Buffer.BlockCopy(buffer, 0, result, 2, cursor);
                return result;
            } else if (cursor <= short.MaxValue)
            {
                var result = new byte[cursor + 4];
                var sizeArray = BitConverter.GetBytes((short) cursor);
                var elementCountArray = BitConverter.GetBytes((short) elementCount);
                if (BitConverter.IsLittleEndian)
                {
                    result[0] = sizeArray[0];
                    result[1] = sizeArray[1];
                    result[2] = elementCountArray[0];
                    result[3] = elementCountArray[1];
                }
                else
                {
                    result[0] = elementCountArray[0];
                    result[1] = elementCountArray[1];
                    result[2] = sizeArray[0];
                    result[3] = sizeArray[1];
                }
                
                Buffer.BlockCopy(buffer, 0, result, 4, cursor);
                return result;
            } else
            {
                var result = new byte[cursor + 8];
                var sizeArray = BitConverter.GetBytes(cursor);
                var elementCountArray = BitConverter.GetBytes(elementCount);
                if (BitConverter.IsLittleEndian)
                {
                    result[0] = sizeArray[0];
                    result[1] = sizeArray[1];
                    result[2] = sizeArray[2];
                    result[3] = sizeArray[3];
                    result[4] = elementCountArray[0];
                    result[5] = elementCountArray[1];
                    result[6] = elementCountArray[2];
                    result[7] = elementCountArray[3];
                }
                else
                {
                    result[0] = elementCountArray[0];
                    result[1] = elementCountArray[1];
                    result[2] = elementCountArray[2];
                    result[3] = elementCountArray[3];
                    result[4] = sizeArray[0];
                    result[5] = sizeArray[1];
                    result[6] = sizeArray[2];
                    result[7] = sizeArray[3];
                }
                
                Buffer.BlockCopy(buffer, 0, result, 8, cursor);
                return result;
            }
        }

        public void Append(sbyte? value)
        {
            PrepareToAppend(2);

            if (value == null)
            {
                AddTag(BeadType.Nil);
            }
            else
            {
                AddTag(BeadType.I8);
                buffer[cursor] = (byte) value;
                cursor++;
            }
        }

        public void Append(byte? value)
        {
            PrepareToAppend(2);

            if (value == null)
            {
                AddTag(BeadType.Nil);
            }
            else
            {
                AddTag(BeadType.U8);
                buffer[cursor] = (byte) value;
                cursor++;
            }
        }
        
        public void Append(uint? value)
        {
            PrepareToAppend(5);
            if (value == null)
            {
                AddTag(BeadType.Nil);
                return;
            }
            
            if (value == (uint) ((byte) value))
            {
                AddTag(BeadType.U8);
                buffer[cursor] = (byte) value;
                cursor += 1;
            } else if (value == (uint) ((ushort) value))
            {
                AddTag(BeadType.U16);
                Put(cursor, (ushort)value);
                cursor += 2;
            }
            else
            {
                AddTag(BeadType.U32);
                Put(cursor, (uint)value);
                cursor += 4;
            }
        }

        public void Append(int? value)
        {
            PrepareToAppend(5);
            if (value == null)
            {
                AddTag(BeadType.Nil);
                return;
            }

            if (value >= 0)
            {
                if (value == (int) ((byte) value))
                {
                    AddTag(BeadType.U8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (int) ((ushort) value))
                {
                    AddTag(BeadType.U16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                }
                else
                {
                    AddTag(BeadType.I32);
                    Put(cursor, (int)value);
                    cursor += 4;
                }    
            }
            else
            {
                if (value == (int) ((sbyte) value))
                {
                    AddTag(BeadType.I8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (int) ((short) value))
                {
                    AddTag(BeadType.I16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                }
                else
                {
                    AddTag(BeadType.I32);
                    Put(cursor, (int)value);
                    cursor += 4;
                }
            }
            
        }
        
        public void Append(ulong? value)
        {
            PrepareToAppend(9);
            if (value == null)
            {
                AddTag(BeadType.Nil);
                return;
            }
            
            if (value == (ulong) ((byte) value))
            {
                AddTag(BeadType.U8);
                buffer[cursor] = (byte) value;
                cursor += 1;
            } else if (value == (ulong) ((ushort) value))
            {
                AddTag(BeadType.U16);
                Put(cursor, (ushort)value);
                cursor += 2;
            } else if (value == (ulong) ((uint) value))
            {
                AddTag(BeadType.U32);
                Put(cursor, (uint)value);
                cursor += 4;
            }
            else
            {
                AddTag(BeadType.U64);
                Put(cursor, (ulong)value);
                cursor += 8;
            }
        }

        public void Append(long? value)
        {
            PrepareToAppend(9);
            if (value == null)
            {
                AddTag(BeadType.Nil);
                return;
            }

            if (value >= 0)
            {
                if (value == (long) ((byte) value))
                {
                    AddTag(BeadType.U8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (long) ((ushort) value))
                {
                    AddTag(BeadType.U16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                } else if (value == (long) ((uint) value))
                {
                    AddTag(BeadType.U32);
                    Put(cursor, (uint)value);
                    cursor += 4;
                }
                else
                {
                    AddTag(BeadType.I64);
                    Put(cursor, (long)value);
                    cursor += 8;
                }
            }
            else
            {
                if (value == (long) ((sbyte) value))
                {
                    AddTag(BeadType.I8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (long) ((short) value))
                {
                    AddTag(BeadType.I16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                } else if (value == (long) ((int) value))
                {
                    AddTag(BeadType.I32);
                    Put(cursor, (uint)value);
                    cursor += 4;
                }
                else
                {
                    AddTag(BeadType.I64);
                    Put(cursor, (long)value);
                    cursor += 8;
                }
            }
        }

        public void Append(float? value, float delta = 0)
        {
            PrepareToAppend(5);
            if (value == null)
            {
                AddTag(BeadType.Nil);
                return;
            }

            if (float.IsNaN(value.Value) || float.IsInfinity(value.Value))
            {
                AddTag(BeadType.F16);
                Put(cursor, ToHalf(value.Value));
                cursor += 2;
                return;
            }
            
            if (value >= 0)
            {
                if (value == (float) ((byte) value))
                {
                    AddTag(BeadType.U8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (float) ((ushort) value))
                {
                    AddTag(BeadType.U16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                } else if (delta != 0f)
                {
                    var half = ToHalf(value.Value);
                    if (Math.Abs(FromHalf(half) - value.Value) <= delta)
                    {
                        AddTag(BeadType.F16);
                        Put(cursor, half);
                        cursor += 2;
                    }
                }
                else
                {
                    AddTag(BeadType.F32);
                    Put(cursor, (float)value);
                    cursor += 4;
                }    
            }
            else
            {
                if (value == (float) ((sbyte) value))
                {
                    AddTag(BeadType.I8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (float) ((short) value))
                {
                    AddTag(BeadType.I16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                } else if (delta != 0f)
                {
                    var half = ToHalf(value.Value);
                    if (Math.Abs(FromHalf(half) - value.Value) <= delta)
                    {
                        AddTag(BeadType.F16);
                        Put(cursor, half);
                        cursor += 2;
                    }
                }
                else
                {
                    AddTag(BeadType.F32);
                    Put(cursor, (float)value);
                    cursor += 4;
                }
            }
        }
        
        public void Append(double? value, double delta = 0)
        {
            PrepareToAppend(9);
            if (value == null)
            {
                AddTag(BeadType.Nil);
                return;
            }
            
            if (double.IsNaN(value.Value) || double.IsInfinity(value.Value))
            {
                AddTag(BeadType.F16);
                Put(cursor, ToHalf((float)value.Value));
                cursor += 2;
                return;
            }
            
            if (value >= 0)
            {
                if (value == (double) ((byte) value))
                {
                    AddTag(BeadType.U8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (double) ((ushort) value))
                {
                    AddTag(BeadType.U16);
                    Put(cursor, (ushort)value);
                    cursor += 2;
                } else if (value == (double) ((uint) value))
                {
                    AddTag(BeadType.U32);
                    Put(cursor, (uint)value);
                    cursor += 4;
                } else if (Math.Abs(value.Value - (double)((float) value)) <= delta)
                {
                    var floatValue = (float) value;
                    var added = false;
                    if (delta != 0)
                    {
                        var half = ToHalf(floatValue);
                        if (Math.Abs((double)FromHalf(half) - value.Value) <= delta)
                        {
                            AddTag(BeadType.F16);
                            Put(cursor, half);
                            cursor += 2;
                            added = true;
                        }
                    }

                    if (added == false)
                    {
                        AddTag(BeadType.F32);
                        Put(cursor, floatValue);
                        cursor += 4;    
                    }
                }
                else
                {
                    AddTag(BeadType.F64);
                    Put(cursor, (double)value);
                    cursor += 8;
                }    
            }
            else
            {
                if (value == (double) ((sbyte) value))
                {
                    AddTag(BeadType.I8);
                    buffer[cursor] = (byte) value;
                    cursor += 1;
                } else if (value == (double) ((short) value))
                {
                    AddTag(BeadType.I16);
                    Put(cursor, (short)value);
                    cursor += 2;
                } else if (value == (double) ((int) value))
                {
                    AddTag(BeadType.I32);
                    Put(cursor, (int)value);
                    cursor += 4;
                } else if (value - (double)((float) value) <= delta)
                {
                    var floatValue = (float) value;
                    var added = false;
                    if (delta != 0)
                    {
                        var half = ToHalf(floatValue);
                        if (Math.Abs((double)FromHalf(half) - value.Value) <= delta)
                        {
                            AddTag(BeadType.F16);
                            Put(cursor, half);
                            cursor += 2;
                            added = true;
                        }
                    }

                    if (added == false)
                    {
                        AddTag(BeadType.F32);
                        Put(cursor, floatValue);
                        cursor += 4;    
                    }
                }
                else
                {
                    AddTag(BeadType.F64);
                    Put(cursor, (double)value);
                    cursor += 8;
                }
            }
        }

        public void Append(byte[] value)
        {
          
            if (value == null)
            {
                PrepareToAppend(1);
                AddTag(BeadType.Nil);
                return;
            }
            PrepareToAppend(value.Length + 1);
            
            AddTag(BeadType.Data);
            Append(value.Length);
            Buffer.BlockCopy(value, 0, buffer, cursor, value.Length);
            cursor += value.Length;
        }

        public void AppendCompact(byte[] value)
        {
            if (value == null)
            {
                PrepareToAppend(1);
                AddTag(BeadType.Nil);
                return;
            }
            PrepareToAppend(value.Length + 1);
            
            var count = value.Length;
            var size = 0;
            // worst case comapacted data is 1/8 bigger
            var compactData = new byte[(int) ((double)count * 1.2)];
            var flagCursor = 0;

            for (int i = 0; i < count; i++)
            {
                if (i % 8 == 0)
                {
                    flagCursor = size;
                    compactData[size] = 0;
                    size += 1;
                }

                var b = value[i];

                if (b != 0)
                {
                    var bitMask = (byte) (1 << (i % 8));
                    compactData[flagCursor] |= bitMask;
                    compactData[size] = b;
                    size += 1;
                }
            }

            var max = Math.Max(count, size);

            if (max == (int) ((byte) max))
            {
                PrepareToAppend(size + 4);
                AddTag(BeadType.CompactData);
                AddTag(BeadType.U8);
                Put(cursor, (byte)size);
                cursor += 1;
                Put(cursor, (byte)count);
                cursor += 1;
            } else if (max == (int) ((ushort) max))
            {
                PrepareToAppend(size + 6);
                AddTag(BeadType.CompactData);
                AddTag(BeadType.U16);
                Put(cursor, (ushort)size);
                cursor += 1;
                Put(cursor, (ushort)count);
                cursor += 1;
            } else
            {
                PrepareToAppend(size + 10);
                AddTag(BeadType.CompactData);
                AddTag(BeadType.U32);
                Put(cursor, size);
                cursor += 1;
                Put(cursor, count);
                cursor += 1;
            }
            Buffer.BlockCopy(compactData, 0, buffer, cursor, size);
            cursor += size;
        }

        private void AddTag(BeadType type)
        {
            if (elementCount % 2 == 0)
            {
                flagIndex = cursor;
                buffer[flagIndex] = (byte) type;
                cursor++;
            }
            else
            {
                buffer[flagIndex] = (byte) (buffer[flagIndex] | ((byte) type) << 4);
            }

            elementCount++;
        }

        private void PrepareToAppend(int numberOfBytes)
        {
            if (cursor + numberOfBytes < buffer.Length)
            {
                return;
            }

            var newBuffer = new byte[buffer.Length << 1];
            Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
            this.buffer = newBuffer;
        }

        private void Put(int offset, short value)
        {
            Put(offset, (ushort) value);
        }
        
        private void Put(int offset, ushort value)
        {
            // TODO: make it unsafe, avoid array allocation
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
        }
        
        private void Put(int offset, int value)
        {
            Put(offset, (uint) value);
        }
        
        private void Put(int offset, uint value)
        {
            // TODO: make it unsafe, avoid array allocation
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
        }
        
        private void Put(int offset, long value)
        {
            Put(offset, (ulong) value);
        }
        
        private void Put(int offset, ulong value)
        {
            // TODO: make it unsafe, avoid array allocation
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
        }
        
        private void Put(int offset, float value)
        {
            // TODO: make it unsafe, avoid array allocation
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
        }
        
        private void Put(int offset, double value)
        {
            // TODO: make it unsafe, avoid array allocation
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
            BeadsValue b = new BeadsValue(buffer, BeadType.Data, 0);
        }

        internal static ushort ToHalf(float value)
        {
            if (float.IsNaN(value))
            {
                return 0b0_11111_0000000001;
            }
            if (float.IsPositiveInfinity(value))
            {
                return 0b0_11111_0000000000;
            }
            if (float.IsNegativeInfinity(value))
            {
                return 0b1_11111_0000000000;
            }
            var f = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            var sign = (ushort)((f>>16)&0x8000);
            var expo = (ushort) ((((f&0x7f800000)-0x38000000)>>13)&0x7c00);
            var mant = (ushort)((f>>13)&0x03ff);
            return (ushort) (sign | expo | mant);
        }

        internal static float FromHalf(ushort value)
        {
            if (0b0_11111_0000000001 == value)
            {
                return float.NaN;
            }
            if (0b0_11111_0000000000 == value)
            {
                return float.PositiveInfinity;
            }
            if (0b1_11111_0000000000 == value)
            {
                return float.NegativeInfinity;
            }
            
            var sign = (uint)((value & 0x8000) << 16);
            var expo = (uint)(((value & 0x7c00) + 0x1C000) << 13);
            var mant = (uint)((value & 0x03FF) << 13);
            var f = (uint) (sign | expo | mant);
            return BitConverter.ToSingle(BitConverter.GetBytes(f), 0);
        }

        #pragma - IEnumerable


        public IEnumerator<BeadsValue> GetEnumerator()
        {
            return new BedsEnumerator(buffer, cursor);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}