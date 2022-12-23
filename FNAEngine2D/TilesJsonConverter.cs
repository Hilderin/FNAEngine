using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// TileSet JsonConverter to take less space in the json file
    /// </summary>
    internal class TilesJsonConverter : JsonConverter
    {
        /// <summary>
        /// Write the json
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Tile[][] tiles = (Tile[][])value;
            writer.WriteStartArray();
            writer.WriteValue(tiles.Length);

            for (int x = 0; x < tiles.Length; x++)
            {
                Tile[] column = tiles[x];

                if (column != null && column.Any(t => t != null))
                {
                    writer.WriteValue(x);
                    writer.WriteValue(column.Length);

                    for (int y = 0; y < column.Length; y++)
                    {
                        if (column[y] != null)
                        {
                            writer.WriteValue(y);
                            writer.WriteValue(column[y].Col);
                            writer.WriteValue(column[y].Row);
                        }
                    }

                    //End!
                    writer.WriteValue(-1);
                }
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// Read the json
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int? nbCol = reader.ReadAsInt32();
            if (nbCol == null || nbCol.Value < 0)
                return null;


            Tile[][] tiles = new Tile[nbCol.Value][];

            while (true)
            {
                int? x = reader.ReadAsInt32();
                if (x == null || x.Value < 0)
                    break;

                int? nbRows = reader.ReadAsInt32();
                if (nbRows == null || nbRows.Value < 0)
                    break;

                Tile[] rowTiles = new Tile[nbRows.Value];
                tiles[x.Value] = rowTiles;

                while (true)
                {
                    int? y = reader.ReadAsInt32();
                    if (y == null || y.Value < 0)
                        break;

                    int? col = reader.ReadAsInt32();
                    if (col == null || col.Value < 0)
                        break;

                    int? row = reader.ReadAsInt32();
                    if (row == null || row.Value < 0)
                        break;

                    rowTiles[y.Value] = new Tile(col.Value, row.Value);
                }
            }

            return tiles;
        }

        /// <summary>
        /// Can convert?
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Tile[][]);
        }
    }
}
