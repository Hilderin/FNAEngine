using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Low level writer in a byte array
    /// </summary>
    public class BinWriter
    {
        private const byte TRUE_VALUE = (byte)1;
        private const byte FALSE_VALUE = (byte)0;

        private static readonly int _lenShort = sizeof(short);
        private static readonly int _lenInt32 = sizeof(Int32);
        private static readonly int _lenInt64 = sizeof(Int64);
        private static readonly int _lenChar = sizeof(char);
        private static readonly int _lenGuid;
        //private static readonly int _lenDouble = 8;
        //private static readonly int _lenDecimal = 16;
        //private static readonly int _lenSingle = 4;

        private static readonly Encoding _encoding = System.Text.Encoding.UTF8;

        /// <summary>
        /// Serializer
        /// </summary>
        private static JsonSerializer _serializer = new JsonSerializer();

        /// <summary>
        /// Current buffer
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// Current position in the buffer
        /// </summary>
        private int _position = 0;

        /// <summary>
        /// Position in the array
        /// </summary>
        public int Position { get { return _position; } set { _position = value; } }

        /// <summary>
        /// Constructor 
        /// </summary>
        static BinWriter()
        {
            //We need the guid array length only one time
            _lenGuid = Guid.Empty.ToByteArray().Length;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BinWriter(byte[] buffer)
        {
            _buffer = buffer;
        }

        /// <summary>
        /// Constreucteur
        /// </summary>
        public BinWriter(byte[] buffer, int offset)
        {
            _buffer = buffer;
            _position = offset;
        }


        /// <summary>
        /// Write a bool
        /// </summary>
        public void Write(bool value)
        {
            _buffer[_position++] = (value ? TRUE_VALUE : FALSE_VALUE);
        }

        /// <summary>
        /// Write a Byte
        /// </summary>
        public void Write(byte value)
        {
            _buffer[_position++] = value;
        }

        /// <summary>
        /// Write an array of bytes
        /// </summary>
        public void Write(byte[] value)
        {
            Write(value, 0, value.Length);
        }

        /// <summary>
        /// Write an array of bytes at an offset and for a length
        /// </summary>
        public void Write(byte[] value, int offset, int count)
        {
            Buffer.BlockCopy(value, offset, _buffer, _position, count);
            _position += count;
        }

        /// <summary>
        /// Write an Int16
        /// </summary>
        public unsafe void Write(Int16 value)
        {
            
            fixed (byte* pointer = &_buffer[_position])
            {
                *((short*)pointer) = value;
            }

            _position += _lenShort;
        }

        /// <summary>
        /// Write an UInt16
        /// </summary>
        public unsafe void Write(UInt16 value)
        {

            fixed (byte* pointer = &_buffer[_position])
            {
                *((ushort*)pointer) = value;
            }

            _position += _lenShort;
        }


        /// <summary>
        /// Write an Int32
        /// </summary>
        public unsafe void Write(Int32 value)
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                *((int*)pointer) = value;
            }

            _position += _lenInt32;
        }

        /// <summary>
        /// Write an UInt32
        /// </summary>
        public unsafe void Write(UInt32 value)
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                *((uint*)pointer) = value;
            }

            _position += _lenInt32;
        }

        /// <summary>
        /// Write an Int64
        /// </summary>
        public unsafe void Write(Int64 value)
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                *((long*)pointer) = value;
            }

            _position += _lenInt64;
        }

        /// <summary>
        /// Write an UInt64
        /// </summary>
        public unsafe void Write(UInt64 value)
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                *((ulong*)pointer) = value;
            }

            _position += _lenInt64;
        }


        /// <summary>
        /// Write a char
        /// </summary>
        public unsafe void Write(char value)
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                *((char*)pointer) = value;
            }

            _position += _lenChar;
        }

        /// <summary>
        /// Write a string
        /// </summary>
        public void Write(string value)
        {
            if (value == null)
            {
                //String null.....
                Write(Int32.MinValue);
            }
            else
            {
                byte[] dataString = _encoding.GetBytes(value);

                Write(dataString.Length);
                Write(dataString);
            }
        }

        /// <summary>
        /// Write an array of char
        /// </summary>
        public void Write(char[] value)
        {
            if (value == null)
            {
                //String null.....
                Write(Int32.MinValue);
            }
            else
            {
                byte[] dataString = _encoding.GetBytes(value);

                Write(dataString.Length);
                Write(dataString);
            }
        }

        /// <summary>
        /// Write a date
        /// </summary>
        public void Write(DateTime value)
        {
            Write(value.Ticks);
        }

        /// <summary>
        /// Write a decimal
        /// </summary>
        public void Write(Decimal value)
        {
            Int32[] bits = decimal.GetBits(value);
            Write(bits[0]);
            Write(bits[1]);
            Write(bits[2]);
            Write(bits[3]);
        }

        /// <summary>
        /// Write a Double
        /// </summary>
        public unsafe void Write(Double value)
        {
            ulong TmpValue = *(ulong*)&value;
            _buffer[_position++] = (byte)TmpValue;
            _buffer[_position++] = (byte)(TmpValue >> 8);
            _buffer[_position++] = (byte)(TmpValue >> 16);
            _buffer[_position++] = (byte)(TmpValue >> 24);
            _buffer[_position++] = (byte)(TmpValue >> 32);
            _buffer[_position++] = (byte)(TmpValue >> 40);
            _buffer[_position++] = (byte)(TmpValue >> 48);
            _buffer[_position++] = (byte)(TmpValue >> 56);

        }

        /// <summary>
        /// Write a float
        /// </summary>
        public unsafe void Write(Single value)
        {
            uint TmpValue = *(uint*)&value;
            _buffer[_position++] = (byte)TmpValue;
            _buffer[_position++] = (byte)(TmpValue >> 8);
            _buffer[_position++] = (byte)(TmpValue >> 16);
            _buffer[_position++] = (byte)(TmpValue >> 24);
        }

        /// <summary>
        /// Write a Guid
        /// </summary>
        public void Write(Guid value)
        {
            
           Buffer.BlockCopy(value.ToByteArray(), 0, _buffer, _position, _lenGuid);

            _position += _lenGuid;

        }

        /// <summary>
        /// Write a Nullable Guid
        /// </summary>
        public void Write(Guid? value)
        {

            if (value == null)
            {
                //null value
                Write(false);
            }
            else
            {
                //We have guid
                Write(true);
                Write(value.Value);
            }


        }


        /// <summary>
        /// Write a Vector2
        /// </summary>
        public void Write(Vector2 vector)
        {
            Write(vector.X);
            Write(vector.Y);
        }

        /// <summary>
        /// Write a Rectangle
        /// </summary>
        public void Write(Rectangle rectangle)
        {
            Write(rectangle.X);
            Write(rectangle.Y);
            Write(rectangle.Width);
            Write(rectangle.Height);
        }

        /// <summary>
        /// Write a Point
        /// </summary>
        public void Write(Point point)
        {
            Write(point.X);
            Write(point.Y);
        }

        /// <summary>
        /// Write a Color
        /// </summary>
        public void Write(Color color)
        {
            Write(color.PackedValue);
        }

        /// <summary>
        /// Write an object
        /// </summary>
        public unsafe void WriteObject(object obj)
        {
            if (obj == null)
            {
                Write(0);
                return;
            }

            int originPosition = _position;

            //Skip a place for the length of the json...
            _position += _lenInt32;

            using (MemoryStream ms = new MemoryStream(_buffer, _position, _buffer.Length - _position))
            {
                using (StreamWriter writer = new StreamWriter(ms))
                {

                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        _serializer.Serialize(jsonWriter, obj);

                        jsonWriter.Flush();

                        //Writing the length...
                        fixed (byte* pointer = &_buffer[originPosition])
                        {
                            *((int*)pointer) = ((int)ms.Position);
                        }

                        _position += ((int)ms.Position);
                    }

                }


            }
        }

    }
}
