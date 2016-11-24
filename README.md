# Awesome Tiles

Awesome tiles has two classes:

- Tile: Represents one tile with a zoom level and x-y coordinate.
- TileRange: Represents a 'rectangular' collection of tiles.

#### Usage:

Just include the classes in your code if you need them.

#### Example:

```csharp
// create a tile at a given location.
var tile = Tile.CreateAroundLocation(lat, lon, zoom);

// get info about tile.
var tileTop = tile.Top;

// and many more awesome stuff, that's why it's called awesome tiles and not just tiles!

```