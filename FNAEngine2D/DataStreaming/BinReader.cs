using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace FNAEngine2D
{
    /// <summary>
    /// Low level reader in a byte array
    /// </summary>
    public class BinReader
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
        /// Buffer to serialize guid if needed
        /// </summary>
        private byte[] _bufferGuid;
        

        /// <summary>
        /// Position in the array
        /// </summary>
        public int Position { get { return _position; } set { _position = value; } }
        

        /// <summary>
        /// Static constructor 
        /// </summary>
        static BinReader()
        {
            //We need the guid array length only one time
            _lenGuid = Guid.Empty.ToByteArray().Length;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BinReader(byte[] buffer)
        {
            _buffer = buffer;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BinReader(byte[] buffer, int offset)
        {
            _buffer = buffer;
            _position = offset;
        }

        /// <summary>
        /// Read a bool
        /// </summary>
        public bool ReadBoolean()
        {
            return _buffer[_position++] == TRUE_VALUE;
        }

        /// <summary>
        /// Read a byte
        /// </summary>
        public byte ReadByte()
        {
            return _buffer[_position++];

        }

        /// <summary>
        /// Read an array of bytes
        /// </summary>
        public byte[] ReadBytes(int len)
        {
            if (len > 0)
            {
                byte[] data = new byte[len];

                ReadBytes(data, 0, len);

                return data;
            }
            else
                return new byte[0];
        }

        /// <summary>
        /// Read an array of bytes at an offset and for a length
        /// </summary>
        public void ReadBytes(byte[] data, int offset, int len)
        {
            if (len > 0)
            {
                Buffer.BlockCopy(_buffer, _position, data, offset, len);
                _position += len;
            }
        }

        /// <summary>
        /// Read an Int16
        /// </summary>
        public unsafe Int16 ReadInt16()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenShort;
                return *((short*)pointer);
            }
        }

        /// <summary>
        /// Read an UInt16
        /// </summary>
        public unsafe UInt16 ReadUInt16()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenShort;
                return *((ushort*)pointer);
            }
        }

        /// <summary>
        /// Read an Int32
        /// </summary>
        public unsafe Int32 ReadInt32()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenInt32;
                return *((int*)pointer);
            }

        }

        /// <summary>
        /// Read an UInt32
        /// </summary>
        public unsafe UInt32 ReadUInt32()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenInt32;
                return *((uint*)pointer);
            }

        }

        /// <summary>
        /// Read an Int64
        /// </summary>
        public unsafe Int64 ReadInt64()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenInt64;
                return *((long*)pointer);
            }

        }

        /// <summary>
        /// Read an UInt64
        /// </summary>
        public unsafe UInt64 ReadUInt64()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenInt64;
                return *((ulong*)pointer);
            }

        }

        /// <summary>
        /// Read a Decimal
        /// </summary>
        public Decimal ReadDecimal()
        {
            Int32[] bits = new Int32[4];
            bits[0] = ReadInt32();
            bits[1] = ReadInt32();
            bits[2] = ReadInt32();
            bits[3] = ReadInt32();

            return new decimal(bits);
        }

        /// <summary>
        /// Read a Double
        /// </summary>
        public unsafe Double ReadDouble()
        {
            uint lo = (uint)(_buffer[_position++] | _buffer[_position++] << 8 |
                _buffer[_position++] << 16 | _buffer[_position++] << 24);
            uint hi = (uint)(_buffer[_position++] | _buffer[_position++] << 8 |
                _buffer[_position++] << 16 | _buffer[_position++] << 24);

            ulong tmpBuffer = ((ulong)hi) << 32 | lo;
            return *((double*)&tmpBuffer);
        }

        /// <summary>
        /// Read a float
        /// </summary>
        public unsafe Single ReadSingle()
        {
            uint tmpBuffer = (uint)(_buffer[_position++] | _buffer[_position++] << 8 | _buffer[_position++] << 16 | _buffer[_position++] << 24);
            return *((float*)&tmpBuffer);
        }

        /// <summary>
        /// Read a char
        /// </summary>
        public unsafe char ReadChar()
        {
            fixed (byte* pointer = &_buffer[_position])
            {
                _position += _lenChar;
                return *((char*)pointer);
            }
        }

        /// <summary>
        /// Read an array of chars
        /// </summary>
        public char[] ReadChars()
        {
            int len = ReadInt32();

            if (len == Int32.MinValue)
                //Empty
                return null;

            if (len > 0)
            {
                byte[] value = ReadBytes(len);
                return _encoding.GetChars(value);
            }
            else
                return new char[0];


        }

        /// <summary>
        /// Permet de lire une string
        /// </summary>
        public string ReadString()
        {
            int len = ReadInt32();

            if (len == Int32.MinValue)
                //Ok, c'était null!
                return null;

            if (len > 0)
            {
                string value = _encoding.GetString(_buffer, _position, len);
                _position += len;
                return value;
            }
            else
                return String.Empty;


        }

        /// <summary>
        /// Read a date
        /// </summary>
        public DateTime ReadDateTime()
        {
            long ticks = ReadInt64();

            return new DateTime(ticks);
        }

        /// <summary>
        /// Read a Guid
        /// </summary>
        public unsafe Guid ReadGuid()
        {
            if (_bufferGuid == null)
                _bufferGuid = new byte[_lenGuid];

            //On lit les données du guid...
            Buffer.BlockCopy(_buffer, _position, _bufferGuid, 0, _lenGuid);

            _position += _lenGuid;

            return new Guid(_bufferGuid);
        }

        /// <summary>
        /// Read a Nullable Guid
        /// </summary>
        public unsafe Guid? ReadNullableGuid()
        {
            if (ReadBoolean())
                return ReadGuid();
            else
                return null;
        }

        /// <summary>
        /// Read a Vector2
        /// </summary>
        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Read a Rectangle
        /// </summary>
        public Rectangle ReadRectangle()
        {
            return new Rectangle(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
        }

        /// <summary>
        /// Read a Point
        /// </summary>
        public Point ReadPoint()
        {
            return new Point(ReadInt32(), ReadInt32());
        }

        /// <summary>
        /// Read a Color
        /// </summary>
        public Color ReadColor()
        {
            uint packedValue = ReadUInt32();
            return new Color((byte)packedValue, (byte)(packedValue >> 8), (byte)(packedValue >> 16), (byte)(packedValue >> 24));
        }

        /// <summary>
        /// Read an object
        /// </summary>
        public T ReadObject<T>()
        {
            int length = ReadInt32();
            if (length == 0)
                return default(T);

            using (MemoryStream ms = new MemoryStream(_buffer, _position, length))
            {
                using (StreamReader reader = new StreamReader(ms))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        T obj = _serializer.Deserialize<T>(jsonReader);
                        _position += length;
                        return obj;
                    }
                }
            }
        }



    }
}
