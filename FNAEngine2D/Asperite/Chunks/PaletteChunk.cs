using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace FNAEngine2D.Aseprite.Chunks
{
    public class PaletteEntry
    {
        public ushort EntryFlags { get; private set; }

        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }
        public byte Alpha { get; private set; }

        public string Name { get; private set; }

        public PaletteEntry(BinaryReader reader)
        {
            EntryFlags = reader.ReadUInt16();

            Red = reader.ReadByte();
            Green = reader.ReadByte();
            Blue = reader.ReadByte();
            Alpha = reader.ReadByte();

            if ((EntryFlags & 1) != 0)
            {
                Name = reader.ReadString();
            }
        }
    }


    public class PaletteChunk : Chunk
    {
        public uint PaletteSize { get; private set; }
        public uint FirstColorIndex { get; private set; }
        public uint LastColorIndex { get; private set; }

        // Future (8) bytes

        public List<PaletteEntry> Entries { get; private set; }


        public PaletteChunk(uint length, BinaryReader reader) : base(length, ChunkType.Palette)
        {
            PaletteSize = reader.ReadUInt32();
            FirstColorIndex = reader.ReadUInt32();
            LastColorIndex = reader.ReadUInt32();

            reader.ReadBytes(8); // For Future

            Entries = new List<PaletteEntry>();

            for (int i = 0; i < PaletteSize; i++)
            {
                Entries.Add(new PaletteEntry(reader));
            }
        }


        public Color GetColor(byte index)
        {
            if (index >= FirstColorIndex && index <= LastColorIndex)
            {
                PaletteEntry entry = Entries[index];

                return new Color(entry.Red, entry.Green, entry.Blue, entry.Alpha);
            }
            else
            {
                return Color.Magenta;
            }
        }
    }
}
