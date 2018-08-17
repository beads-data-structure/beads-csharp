using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beads;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BeadsTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void BeadsAppendByte()
        {
            var sequence = new BeadsSequence();
            sequence.Append((byte)1);
            sequence.Append((byte)2);
            sequence.Append((byte?)null);
            sequence.Append((byte)3);

            var expected = new byte[]{5, 4, 17, 1, 2, 31, 3};
            Assert.AreEqual(expected, sequence.Bytes());

            var list = new List<int?>();
            foreach (var value in sequence)
            {
                list.Add(value.Int);
            }

            var expectedList = new List<int?>(new int?[]{1, 2, null, 3});
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void BeadsAppendSignedByte()
        {
            var sequence = new BeadsSequence();
            sequence.Append((sbyte)-1);
            sequence.Append((sbyte)-2);
            sequence.Append((sbyte?)null);
            sequence.Append((sbyte)-3);

            var expected = new byte[]{5, 4, 34, 255, 254, 47, 253};
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<int?>();
            foreach (var value in sequence)
            {
                list.Add(value.Int);
            }

            var expectedList = new List<int?>(new int?[]{-1, -2, null, -3});
            Assert.AreEqual(expectedList, list);
        }

        [Test]
        public void PolindromFix()
        {
            var sequence = new BeadsSequence();
            sequence.Append((byte?)1);
            sequence.Append((byte?)null);
            var expected = new byte[]{3, 4, 241, 1, 0};
            Assert.AreEqual(expected, sequence.Bytes());
        }
        
        [Test]
        public void AppendUInt()
        {
            var sequence = new BeadsSequence();
            sequence.Append((uint)1);
            sequence.Append((uint?)250);
            sequence.Append((uint?)short.MaxValue);
            sequence.Append((uint?)ushort.MaxValue);
            sequence.Append((uint?)int.MaxValue);
            sequence.Append((uint?)uint.MaxValue);
            sequence.Append((uint?)null);
            var expected = new byte[]
            {
                18, 7, 
                17, 1, 250,
                51, 255, 127, 255, 255,
                85, 255, 255, 255, 127, 255, 255, 255, 255, 
                15
            };
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<ulong?>();
            foreach (var value in sequence)
            {
                list.Add(value.ULong);
            }

            var expectedList = new List<ulong?>(new ulong?[]
            {
                1, 250, (ulong?)short.MaxValue, ushort.MaxValue, 
                int.MaxValue, uint.MaxValue, null
            });
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void FromBeadsData()
        {
            var beadsBuffer = new byte[]
            {
                18, 7, 
                17, 1, 250,
                51, 255, 127, 255, 255,
                85, 255, 255, 255, 127, 255, 255, 255, 255, 
                15
            };
            var sequence = new BeadsSequence(beadsBuffer);
            
            var list = new List<ulong?>();
            foreach (var value in sequence)
            {
                list.Add(value.ULong);
            }

            var expectedList = new List<ulong?>(new ulong?[]
            {
                1, 250, (ulong?)short.MaxValue, ushort.MaxValue, 
                int.MaxValue, uint.MaxValue, null
            });
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void AppendInt()
        {
            var sequence = new BeadsSequence();
            sequence.Append((int?) 1);
            sequence.Append(250);
            sequence.Append(-45);
            sequence.Append((int?)short.MaxValue);
            sequence.Append((int?)ushort.MaxValue);
            sequence.Append((int?)short.MinValue);
            sequence.Append((int?)int.MaxValue);
            sequence.Append((int?)int.MinValue);
            sequence.Append((int?)null);
            
            var expected = new byte[]
            {
                22, 9, 
                17, 1, 250,
                50, 211, 255, 127,
                67, 255, 255, 0, 128,
                102, 255, 255, 255, 127, 0, 0, 0, 128,
                0b0000_1111
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<int?>();
            foreach (var value in sequence)
            {
                list.Add(value.Int);
            }

            var expectedList = new List<int?>(new int?[]
            {
                1, 250, -45, short.MaxValue, ushort.MaxValue, short.MinValue, 
                int.MaxValue, int.MinValue, null
            });
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void AppendULong()
        {
            var sequence = new BeadsSequence();
            sequence.Append((ulong?) 1);
            sequence.Append((ulong?)250);
            sequence.Append((ulong?)short.MaxValue);
            sequence.Append((ulong?)ushort.MaxValue);
            sequence.Append((ulong?)int.MaxValue);
            sequence.Append((ulong?)uint.MaxValue);
            sequence.Append(ulong.MaxValue);
            sequence.Append((int?)null);
            
            var expected = new byte[]
            {
                26, 8, 
                17, 1, 250,
                51, 255, 127, 255, 255,
                85, 255, 255, 255, 127, 255, 255, 255, 255,
                248, 255, 255, 255, 255, 255, 255, 255, 255
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<ulong?>();
            foreach (var value in sequence)
            {
                list.Add(value.ULong);
            }

            var expectedList = new List<ulong?>(new ulong?[]
            {
                1, 250, (ulong?)short.MaxValue, ushort.MaxValue, 
                int.MaxValue, uint.MaxValue, ulong.MaxValue, null
            });
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void AppendLong()
        {
            var sequence = new BeadsSequence();
            sequence.Append((long?) 1);
            sequence.Append((long?)250);
            sequence.Append((long?)-5);
            sequence.Append((long?)short.MaxValue);
            sequence.Append((long?)ushort.MaxValue);
            sequence.Append((long?)short.MinValue);
            sequence.Append((long?)int.MaxValue);
            sequence.Append((long?)uint.MaxValue);
            sequence.Append((long?)int.MinValue);
            sequence.Append((long?)long.MaxValue);
            sequence.Append(long.MinValue);
            sequence.Append((long?)null);
            
            var expected = new byte[]
            {
                43, 12, 
                17, 1, 250,
                50, 251, 255, 127,
                67, 255, 255, 0, 128,
                85, 255, 255, 255, 127, 255, 255, 255, 255,
                150, 0, 0, 0, 128, 255, 255, 255, 255, 255, 255, 255, 127, 
                249, 0, 0, 0, 0, 0, 0, 0, 128
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<long?>();
            foreach (var value in sequence)
            {
                list.Add(value.Long);
            }

            var expectedList = new List<long?>(new long?[]
            {
                1, 250, -5, short.MaxValue, ushort.MaxValue, short.MinValue, 
                int.MaxValue, uint.MaxValue, int.MinValue, long.MaxValue, long.MinValue, null
            });
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void AppendFloat()
        {
            var sequence = new BeadsSequence();
            sequence.Append((float?) 1);
            sequence.Append((float?)250);
            sequence.Append((float?)-5);
            sequence.Append((float?)short.MaxValue);
            sequence.Append((float?)ushort.MaxValue);
            sequence.Append((float?)short.MinValue);
            sequence.Append((float?)int.MaxValue);
            sequence.Append((float?)uint.MaxValue);
            sequence.Append((float?)int.MinValue);
            sequence.Append((float?)long.MaxValue);
            sequence.Append((float?)long.MinValue);
            sequence.Append((float?)ulong.MaxValue);
            sequence.Append(-1.2f);
            sequence.Append(1.5f);
            sequence.Append(1.1f);
            sequence.Append((float?)null);
            
            var expected = new byte[]
            {
                53, 16, 
                17, 1, 250,
                50, 251, 255, 127,
                67, 255, 255, 0, 128,
                119, 0, 0, 0, 79, 0, 0, 128, 79,
                119, 0, 0, 0, 207, 0, 0, 0, 95,
                119, 0, 0, 0, 223, 0, 0, 128, 95, 
                119, 154, 153, 153, 191, 0, 0, 192, 63, 
                247, 205, 204, 140, 63
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<float?>();
            foreach (var value in sequence)
            {
                list.Add(value.Float);
            }

            var expectedList = new List<float?>(new float?[]
            {
                1, 250, -5, short.MaxValue, ushort.MaxValue, short.MinValue, 
                int.MaxValue, uint.MaxValue, int.MinValue, 
                long.MaxValue, long.MinValue, ulong.MaxValue, 
                -1.2f, 1.5f, 1.1f,
                null
            });
            Assert.AreEqual(expectedList, list);
        }

        [Test]
        public void AppendHalfFloat()
        {
            var sequence = new BeadsSequence();
            sequence.Append(float.NaN);
            sequence.Append(1.5f);
            sequence.Append(1.5f, 0.0001f);
            sequence.Append(-1.25f, 0.0001f);
            sequence.Append(float.NegativeInfinity);
            sequence.Append(float.PositiveInfinity);
            sequence.Append(-0f);
            sequence.Append(0f);
            
            printBytes(sequence);
            
            var expected = new byte[]
            {
                20, 8, 
                123, 1, 124, 0, 0, 192, 63, 
                187, 0, 62, 0, 189, 
                187, 0, 252, 0, 124, 
                17, 0, 0
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<float?>();
            foreach (var value in sequence)
            {
                list.Add(value.Float);
            }

            var expectedList = new List<float?>(new float?[]
            {
                float.NaN, 1.5f, 1.5f, -1.25f, float.NegativeInfinity,
                float.PositiveInfinity, -0f, 0f
            });
            Assert.AreEqual(expectedList, list);
            
            var list2 = new List<double?>();
            foreach (var value in sequence)
            {
                list2.Add(value.Double);
            }

            var expectedList2 = new List<double?>(new double?[]
            {
                double.NaN, 1.5, 1.5, -1.25, double.NegativeInfinity,
                double.PositiveInfinity, -0.0, 0.0
            });
            Assert.AreEqual(expectedList2, list2);
        }

        [Test]
        public void AppendDouble()
        {
            var sequence = new BeadsSequence();
            sequence.Append((double?) 1);
            sequence.Append((double?)250);
            sequence.Append((double?)-5);
            sequence.Append((double?)short.MaxValue);
            sequence.Append((double?)ushort.MaxValue);
            sequence.Append((double?)short.MinValue);
            sequence.Append((double?)int.MaxValue);
            sequence.Append((double?)uint.MaxValue);
            sequence.Append((double?)int.MinValue);
            sequence.Append((double?)long.MaxValue);
            sequence.Append((double?)long.MinValue);
            sequence.Append((double?)ulong.MaxValue);
            sequence.Append(-1.2);
            sequence.Append(1.5);
            sequence.Append(-1.5);
            sequence.Append(1.1);
            sequence.Append((double?)null);
            
            printBytes(sequence);
            
            var expected = new byte[]
            {
                66, 17, 
                17, 1, 250,
                50, 251, 255, 127,
                67, 255, 255, 0, 128,
                85, 255, 255, 255, 127, 255, 255, 255, 255, 
                118, 0, 0, 0, 128, 0, 0, 0, 95, 
                119, 0, 0, 0, 223, 0, 0, 128, 95, 
                122, 51, 51, 51, 51, 51, 51, 243, 191, 0, 0, 192, 63, 
                167, 0, 0, 192, 191, 154, 153, 153, 153, 153, 153, 241, 63, 
                15
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<double?>();
            foreach (var value in sequence)
            {
                list.Add(value.Double);
            }

            var expectedList = new List<double?>(new double?[]
            {
                1, 250, -5, short.MaxValue, ushort.MaxValue, short.MinValue, 
                int.MaxValue, uint.MaxValue, int.MinValue, 
                long.MaxValue, long.MinValue, ulong.MaxValue, 
                -1.2, 1.5, -1.5, 1.1,
                null
            });
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void AppendHalfDouble()
        {
            var sequence = new BeadsSequence();
            sequence.Append(double.NaN);
            sequence.Append(1.5);
            sequence.Append(1.5, 0.0001);
            sequence.Append(-1.25, 0.0001);
            sequence.Append(double.NegativeInfinity);
            sequence.Append(double.PositiveInfinity);
            sequence.Append(-0.0);
            sequence.Append(0.0);
            
            printBytes(sequence);
            
            var expected = new byte[]
            {
                20, 8, 
                123, 1, 124, 0, 0, 192, 63, 
                187, 0, 62, 0, 189, 
                187, 0, 252, 0, 124, 
                17, 0, 0
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var list = new List<float?>();
            foreach (var value in sequence)
            {
                list.Add(value.Float);
            }

            var expectedList = new List<float?>(new float?[]
            {
                float.NaN, 1.5f, 1.5f, -1.25f, float.NegativeInfinity,
                float.PositiveInfinity, -0f, 0f
            });
            Assert.AreEqual(expectedList, list);
            
            var list2 = new List<double?>();
            foreach (var value in sequence)
            {
                list2.Add(value.Double);
            }

            var expectedList2 = new List<double?>(new double?[]
            {
                double.NaN, 1.5, 1.5, -1.25, double.NegativeInfinity,
                double.PositiveInfinity, -0.0, 0.0
            });
            Assert.AreEqual(expectedList2, list2);
        }

        [Test]
        public void AppendData()
        {
            var sequence = new BeadsSequence();
            sequence.Append(Encoding.UTF8.GetBytes("Maxim"));
            sequence.Append(Encoding.UTF8.GetBytes("hello 😱"));
            sequence.Append(Encoding.Unicode.GetBytes("hello 😱"));
            
            var expected = new byte[]
            {
                37, 6, 
                30, 5, 77, 97, 120, 105, 109, 
                30, 10, 104, 101, 108, 108, 111, 32, 240, 159, 152, 177, 
                30, 16, 104, 0, 101, 0, 108, 0, 108, 0, 111, 0, 32, 0, 61, 216, 49, 222
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var byteList = new List<byte[]>();

            foreach (var value in sequence)
            {
                byteList.Add(value.Data);
            }

            var list = new List<string>
            {
                Encoding.UTF8.GetString(byteList[0]),
                Encoding.UTF8.GetString(byteList[1]),
                Encoding.Unicode.GetString(byteList[2])
            };


            var expectedList = new List<string>(new string[]
            {
                "Maxim", "hello 😱", "hello 😱"
            });
            
            Assert.AreEqual(expectedList, list);
        }
        
        [Test]
        public void AppendCompactData()
        {
            var sequence = new BeadsSequence();
            sequence.AppendCompact(Encoding.UTF8.GetBytes("Maxim"));
            sequence.AppendCompact(Encoding.UTF8.GetBytes("hello 😱"));
            sequence.AppendCompact(Encoding.Unicode.GetBytes("hello 😱"));
            
            var expected = new byte[]
            {
                39, 6, 
                29, 6, 5, 31, 77, 97, 120, 105, 109, 
                29, 12, 10, 255, 104, 101, 108, 108, 111, 32, 240, 159, 3, 152, 177, 
                29, 12, 16, 85, 104, 101, 108, 108, 245, 111, 32, 61, 216, 49, 222
            };
            
            Assert.AreEqual(expected, sequence.Bytes());
            
            var byteList = new List<byte[]>();

            foreach (var value in sequence)
            {
                byteList.Add(value.Data);
            }

            var list = new List<string>
            {
                Encoding.UTF8.GetString(byteList[0]),
                Encoding.UTF8.GetString(byteList[1]),
                Encoding.Unicode.GetString(byteList[2])
            };


            var expectedList = new List<string>(new string[]
            {
                "Maxim", "hello 😱", "hello 😱"
            });
            
            Assert.AreEqual(expectedList, list);
        }

        [Test]
        public void Append200Values()
        {
            var sequence = new BeadsSequence();
            for (int i = 0; i < 200; i++)
            {
                sequence.Append(i);
            }

            var data = sequence.Bytes();
            
            var sequenceFromData = new BeadsSequence(data);
            var index = 0;
            foreach (var value in sequenceFromData)
            {
                Assert.AreEqual(value.Int, index);
                index++;
            }
        }
        
        [Test]
        public void Append40KValues()
        {
            var sequence = new BeadsSequence();
            for (int i = 0; i < 40_000; i++)
            {
                sequence.Append(i);
            }

            var data = sequence.Bytes();
            
            var sequenceFromData = new BeadsSequence(data);
            var index = 0;
            foreach (var value in sequenceFromData)
            {
                Assert.AreEqual(value.Int, index);
                index++;
            }
        }

        private void printBytes(BeadsSequence sequence)
        {
            foreach (var b in sequence.Bytes())
            {
                Console.Write(b + ", ");    
            } 
        }
    }
}