# Quick Start Guide: Using Pixel Art Tiles in Unity

This guide will help you quickly set up and use the pixel art tiles in your Bunny's Paradise game.

## Step 1: Verify Assets in Unity

1. Open your Unity project
2. In the Project window, navigate to `Assets/Art/Sprites/Tiles`
3. You should see 7 tile textures:
   - grass_floor_tile.png
   - wood_floor_tile.png
   - wall_tile.png
   - carpet_tile.png
   - dark_grass_tile.png
   - decaying_wall_tile.png
   - dead_grass_tile.png

## Step 2: Set Up Your Scene

### Create the Tilemap Hierarchy

1. In your scene, create a new GameObject: `GameObject → 2D Object → Tilemap → Rectangular`
2. This creates a Grid parent with a Tilemap child
3. Rename the Tilemap to "Floor"
4. Create additional Tilemaps for different layers:
   - Right-click Grid → Create → 2D Object → Tilemap → Rectangular
   - Rename to "Walls"

Your hierarchy should look like:
```
Grid
  └─ Floor (Tilemap)
  └─ Walls (Tilemap)
```

## Step 3: Create Tile Assets

1. Open the Tile Palette window: `Window → 2D → Tile Palette`
2. Create a new palette: Click "Create New Palette"
   - Name it "Environment_Tiles"
   - Click "Create"
3. Select where to save the palette (recommend: `Assets/Art/Tiles/Palettes`)

## Step 4: Add Tiles to Palette

1. In the Project window, select `Assets/Art/Sprites/Tiles`
2. Select all PNG files (hold Ctrl/Cmd and click each)
3. Drag them into the Tile Palette window
4. Unity will prompt you to select a location to save the tile assets
5. Save them in `Assets/Art/Tiles/` (create this folder if needed)

## Step 5: Paint Your Level

1. In the Tile Palette window, select the "Active Tilemap" dropdown
2. Choose "Floor" tilemap
3. Select the Brush tool in the Tile Palette
4. Click on a tile (e.g., grass_floor_tile or wood_floor_tile)
5. Paint on your Scene view to create your level

### Tips for Painting:

- **Brush Tool**: Click individual tiles
- **Rectangle Tool**: Drag to fill rectangular areas
- **Fill Tool**: Fill enclosed areas
- **Eraser**: Remove tiles

## Step 6: Set Up Camera for Pixel-Perfect Rendering

For crisp pixel art at 640x360 resolution:

1. Add a Pixel Perfect Camera component to your Main Camera:
   - Select Main Camera
   - Add Component → Rendering → Pixel Perfect Camera
   - Set "Assets Pixels Per Unit" to 32
   - Set "Reference Resolution" to 640x360
   - Check "Upscale Render Texture"

## Step 7: Configure Tilemap Rendering Order

Set the Sorting Layers for proper layering:

1. Open Tag Manager: `Edit → Project Settings → Tags and Layers`
2. Add Sorting Layers:
   - Background
   - Floor
   - Walls
   - Props
   - Character

3. Assign to your Tilemaps:
   - Floor Tilemap → Tilemap Renderer → Sorting Layer: "Floor"
   - Walls Tilemap → Tilemap Renderer → Sorting Layer: "Walls"

## Example Level Layout

### Stage 1: Happy Bunny's Home

**Outdoor Area:**
- Floor: grass_floor_tile.png
- Consider adding paths, flowers later

**Indoor Area:**
- Floor: wood_floor_tile.png or carpet_tile.png
- Walls: wall_tile.png

### Stage 2: Deteriorating Home

- Outdoor: dark_grass_tile.png
- Walls: decaying_wall_tile.png

### Stage 3: Dark/Final Stage

- Outdoor: dead_grass_tile.png
- Walls: decaying_wall_tile.png (or reuse for consistency)

## Next Steps

Consider creating additional assets:
1. **Props**: Laptop, fridge, furniture sprites
2. **Decorations**: Flowers (living and dead versions)
3. **Doors and Windows**
4. **Character Sprite**: The bunny protagonist
5. **UI Elements**: For the clicker game portion

## Troubleshooting

**Tiles look blurry:**
- Check that Filter Mode is set to "Point (no filter)" in the texture import settings
- Verify Pixel Perfect Camera is configured correctly

**Tiles don't tile smoothly:**
- These tiles are designed to tile seamlessly
- Make sure you're not accidentally stretching or rotating them

**Can't see tiles in palette:**
- Make sure you've created tile assets (not just dragged the sprites)
- Check that the Tile Palette is pointing to the correct palette asset

## Resources

- Full documentation: See `Assets/Art/Sprites/Tiles/README.md`
- Unity Tilemap documentation: https://docs.unity3d.com/Manual/Tilemap.html
- Unity 2D Pixel Perfect: https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest
