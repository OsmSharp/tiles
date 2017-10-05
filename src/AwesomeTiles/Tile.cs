// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace AwesomeTiles
{
    /// <summary>
    /// Represents a tile.
    /// </summary>
    public class Tile
    {
        private readonly ulong _id;

        /// <summary>
        /// Creates a new tile from a given id.
        /// </summary>
        /// <param name="id"></param>
        public Tile(ulong id)
        {
            _id = id;

            var tile = Tile.CalculateTile(id);
            this.X = tile.X;
            this.Y = tile.Y;
            this.Zoom = tile.Zoom;
            this.CalculateBounds();
        }

        /// <summary>
        /// Creates a new tile.
        /// </summary>
        public Tile(int x, int y, int zoom)
        {
            this.X = x;
            this.Y = y;
            this.Zoom = zoom;

            _id = CalculateTileId(zoom, x, y);
            this.CalculateBounds();
        }

        private void CalculateBounds()
        {
            var n = Math.PI - ((2.0 * Math.PI * this.Y) / Math.Pow(2.0, this.Zoom));
            this.Left = (float)((this.X / Math.Pow(2.0, this.Zoom) * 360.0) - 180.0);
            this.Top = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            n = Math.PI - ((2.0 * Math.PI * (this.Y + 1)) / Math.Pow(2.0, this.Zoom));
            this.Right = (float)(((this.X + 1) / Math.Pow(2.0, this.Zoom) * 360.0) - 180.0);
            this.Bottom = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            this.CenterLat = (float)((this.Top + this.Bottom) / 2.0);
            this.CenterLon = (float)((this.Left + this.Right) / 2.0);
        }

        /// <summary>
        /// The X position of the tile.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The Y position of the tile.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The zoom level for this tile.
        /// </summary>
        public int Zoom { get; private set; }

        /// <summary>
        /// Gets the top.
        /// </summary>
        public float Top { get; private set; }

        /// <summary>
        /// Get the bottom.
        /// </summary>
        public float Bottom { get; private set; }

        /// <summary>
        /// Get the left.
        /// </summary>
        public float Left { get; private set; }

        /// <summary>
        /// Gets the right.
        /// </summary>
        public float Right { get; private set; }

        /// <summary>
        /// Gets the center lat.
        /// </summary>
        public float CenterLat { get; private set; }

        /// <summary>
        /// Gets the center lon.
        /// </summary>
        public float CenterLon { get; private set; }

        /// <summary>
        /// Gets the parent tile.
        /// </summary>
        public Tile Parent => new Tile(this.X / 2, this.Y / 2, this.Zoom - 1);

        /// <summary>
        /// Returns a hashcode for this tile position.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^
                   this.Y.GetHashCode() ^
                   this.Zoom.GetHashCode();
        }

        /// <summary>
        /// Returns true if the given object represents the same tile.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = (obj as Tile);
            if (other != null)
            {
                return other.X == this.X &&
                    other.Y == this.Y &&
                    other.Zoom == this.Zoom;
            }
            return false;
        }

        /// <summary>
        /// Returns a description for this tile.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}x-{1}y@{2}z", this.X, this.Y, this.Zoom);
        }

        /// <summary>
        /// Calculates the tile id of the tile at position (0, 0) for the given zoom.
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        private static ulong CalculateTileId(int zoom)
        {
            switch (zoom)
            {
                case 0: // zoom level 0: {0}.
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 5;
                case 3:
                    return 21;
                case 4:
                    return 85;
                case 5:
                    return 341;
                case 6:
                    return 1365;
                case 7:
                    return 5461;
                case 8:
                    return 21845;
                case 9:
                    return 87381;
                case 10:
                    return 349525;
                case 11:
                    return 1398101;
                case 12:
                    return 5592405;
                case 13:
                    return 22369621;
                case 14:
                    return 89478485;
                case 15:
                    return 357913941;
                case 16:
                    return 1431655765;
                case 17:
                    return 5726623061;
                case 18:
                    return 22906492245;
            }

            var size = (ulong)Math.Pow(2, 2 * (zoom - 1));
            var tileId = Tile.CalculateTileId(zoom - 1) + size;
            return tileId;
        }

        /// <summary>
        /// Calculates the tile id of the tile at position (x, y) for the given zoom.
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static ulong CalculateTileId(int zoom, int x, int y)
        {
            var id = Tile.CalculateTileId(zoom);
            var width = (long)System.Math.Pow(2, zoom);
            return id + (ulong)x + (ulong)(y * width);
        }

        /// <summary>
        /// Calculate the tile given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Tile CalculateTile(ulong id)
        {
            // find out the zoom level first.
            var zoom = 0;
            if (id > 0)
            { // only if the id is at least at zoom level 1.
                while (id >= Tile.CalculateTileId(zoom))
                {
                    // move to the next zoom level and keep searching.
                    zoom++;
                }
                zoom--;
            }

            // calculate the x-y.
            var local = id - Tile.CalculateTileId(zoom);
            var width = (ulong)System.Math.Pow(2, zoom);
            var x = (int)(local % width);
            var y = (int)(local / width);

            return new Tile(x, y, zoom);
        }

        /// <summary>
        /// Returns the id of this tile.
        /// </summary>
        public ulong Id => _id;

        /// <summary>
        /// Returns true if this tile is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (this.X >= 0 &&
                    this.Y >= 0 &&
                    this.Zoom >= 0)
                { // some are negative.
                    var size = System.Math.Pow(2, this.Zoom);
                    return this.X < size && this.Y < size;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns the tile at the given location at the given zoom.
        /// </summary>
        public static Tile CreateAroundLocation(double lat, double lon, int zoom)
        {
            int x, y;
            if (!Tile.CreateAroundLocation(lat, lon, zoom, out x, out y))
            {
                return null;
            }
            return new Tile(x, y, zoom);
        }

        /// <summary>
        /// Returns the tile at the given location at the given zoom.
        /// </summary>
        public static ulong CreateAroundLocationId(double lat, double lon, int zoom)
        {
            int x, y;
            if (!Tile.CreateAroundLocation(lat, lon, zoom, out x, out y))
            {
                return ulong.MaxValue;
            }
            return Tile.CalculateTileId(zoom, x, y);
        }

        /// <summary>
        /// A fast method of calculating x-y without creating a tile object.
        /// </summary>
        public static bool CreateAroundLocation(double lat, double lon, int zoom, out int x, out int y)
        {
            if (lon == 180)
            {
                lon = lon - 0.000001;
            }
            if (lat > 85.0511 || lat < -85.0511)
            {
                x = 0;
                y = 0;
                return false;
            }

            var n = (int)System.Math.Floor(System.Math.Pow(2, zoom));

            x = (int)((lon + 180.0) / 360.0 * (1 << zoom));
            var latRad = lat * Math.PI / 180.0;
            y = (int)((1.0 - Math.Log(Math.Tan(latRad) +
                1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom));
            return true;
        }

        /// <summary>
        /// Gets the tile id the given lat/lon belongs to one zoom level lower.
        /// </summary>
        public ulong GetSubTileIdFor(double lat, double lon)
        {
            var factor = 2;
            var zoom = this.Zoom + 1;
            int x = 0, y = 0;
            if (lat >= this.CenterLat && lon < this.CenterLon)
            {
                x = this.X * factor;
                y = this.Y * factor;
            }
            else if(lat >= this.CenterLat && lon >= this.CenterLon)
            {
                x = this.X * factor + factor - 1;
                y = this.Y * factor;
            }
            else if (lat < this.CenterLat && lon < this.CenterLon)
            {
                x = this.X * factor;
                y = this.Y * factor + factor - 1;
            }
            else if (lat < this.CenterLat && lon >= this.CenterLon)
            {
                x = this.X * factor + factor - 1;
                y = this.Y * factor + factor - 1;
            }

            return Tile.CalculateTileId(zoom, x, y);
        }

        /// <summary>
        /// Returns the subtiles of this tile at the given zoom.
        /// </summary>
        public TileRange GetSubTiles(int zoom)
        {
            if (this.Zoom > zoom) { throw new ArgumentOutOfRangeException("zoom", "Subtiles can only be calculated for higher zooms."); }

            if (this.Zoom == zoom)
            { // just return a range of one tile.
                return new TileRange(this.X, this.Y, this.X, this.Y, this.Zoom);
            }

            var factor = 1 << (zoom - this.Zoom);

            return new TileRange(
                this.X * factor,
                this.Y * factor,
                this.X * factor + factor - 1,
                this.Y * factor + factor - 1,
                zoom);
        }
        
        /// <summary>
        /// Inverts the X-coordinate.
        /// </summary>
        /// <returns></returns>
        public Tile InvertX()
        {
            var n = (int)System.Math.Floor(System.Math.Pow(2, this.Zoom));

            return new Tile(n - this.X - 1, this.Y, this.Zoom);
        }

        /// <summary>
        /// Inverts the Y-coordinate.
        /// </summary>
        /// <returns></returns>
        public Tile InvertY()
        {
            var n = (int)System.Math.Floor(System.Math.Pow(2, this.Zoom));

            return new Tile(this.X, n - this.Y - 1, this.Zoom);
        }
    }
}
