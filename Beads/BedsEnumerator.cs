using System;
using System.Collections;
using System.Collections.Generic;

namespace Beads
{
    public class BedsEnumerator : IEnumerator<BeadsValue>
    {
        private readonly byte[] array;
        private readonly int size;
        private int cursor;
        private byte typeFlag;
        private int index;

        public BedsEnumerator(byte[] array, int size)
        {
            this.array = array;
            this.size = size;
        }

        public bool MoveNext()
        {
            if (cursor > size)
            {
                return false;
            }

            if (cursor == size && index % 2 == 0)
            {
                return false;
            }

            if (index % 2 == 0)
            {
                typeFlag = array[cursor];
                cursor += 1;
            }

            var beadType = index % 2 == 0 ? (BeadType) (typeFlag & 0b0000_1111) : (BeadType) (typeFlag >> 4);
        
            if (beadType == BeadType.Skip)
            {
                index++;
                return MoveNext();
            }

            if (beadType == BeadType.Nil)
            {
                Current = new BeadsValue(array, beadType, cursor);
                index++;
                return true;
            }

            if (beadType == BeadType.I8 || beadType == BeadType.U8)
            {
                Current = new BeadsValue(array, beadType, cursor);
                index++;
                cursor += 1;
                return true;
            }
        
            if (beadType == BeadType.I16 || beadType == BeadType.U16 || beadType == BeadType.F16)
            {
                Current = new BeadsValue(array, beadType, cursor);
                index++;
                cursor += 2;
                return true;
            }
        
            if (beadType == BeadType.I32 || beadType == BeadType.U32 || beadType == BeadType.F32)
            {
                Current = new BeadsValue(array, beadType, cursor);
                index++;
                cursor += 4;
                return true;
            }
        
            if (beadType == BeadType.I64 || beadType == BeadType.U64 || beadType == BeadType.F64)
            {
                Current = new BeadsValue(array, beadType, cursor);
                index++;
                cursor += 8;
                return true;
            }

            if (beadType == BeadType.Data)
            {
                index++;
                if (index % 2 == 0)
                {
                    typeFlag = array[cursor];
                    cursor += 1;
                }
                var sizeType = index % 2 == 0 ? (BeadType) (typeFlag & 0b0000_1111) : (BeadType) (typeFlag >> 4);
                var size = new BeadsValue(array, sizeType, cursor).ULong ?? 0;

                if (sizeType == BeadType.U8)
                {
                    cursor += 1;
                } else if (sizeType == BeadType.U16)
                {
                    cursor += 2;
                } else if (sizeType == BeadType.U32)
                {
                    cursor += 4;
                }  else if (sizeType == BeadType.U64)
                {
                    cursor += 8;
                }
                else
                {
                    throw new Exception("Unexpected type for size");
                }
            
                Current = new BeadsValue(array, beadType, cursor, size);

                index++;
                cursor += (int)size;
                return true;
            }
        
            if (beadType == BeadType.CompactData)
            {
                index++;
                if (index % 2 == 0)
                {
                    typeFlag = array[cursor];
                    cursor += 1;
                }
                var sizeType = index % 2 == 0 ? (BeadType) (typeFlag & 0b0000_1111) : (BeadType) (typeFlag >> 4);
                var size = new BeadsValue(array, sizeType, cursor).ULong ?? 0;
                var sizeAndCountNumberLength = 0;

                if (sizeType == BeadType.U8)
                {
                    sizeAndCountNumberLength = 1;
                } else if (sizeType == BeadType.U16)
                {
                    sizeAndCountNumberLength = 2;
                } else if (sizeType == BeadType.U32)
                {
                    sizeAndCountNumberLength = 4;
                }  else if (sizeType == BeadType.U64)
                {
                    sizeAndCountNumberLength = 8;
                }
                else
                {
                    throw new Exception("Unexpected type for size");
                }
            
                cursor += sizeAndCountNumberLength;
                var count = new BeadsValue(array, sizeType, cursor).ULong ?? 0;
                cursor += sizeAndCountNumberLength;
            
                Current = new BeadsValue(array, beadType, cursor, size, count);

                index++;
                cursor += (int)size;
                return true;
            }
        
            throw new Exception("Unexpected Bead Type");
        }

        public void Reset()
        {
            cursor = 0;
            index = 0;
            Current = new BeadsValue(array, BeadType.Skip, cursor);
        }

        public BeadsValue Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose(){}
    }
}