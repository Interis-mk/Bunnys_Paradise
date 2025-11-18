# Pixel Art Tile Assets

This folder contains tileable pixel art textures for the top-down view portion of Bunny's Paradise.

## Asset Specifications

- **Tile Size**: 32x32 pixels
- **Target Resolution**: 640x360 (upscaled to 1080p)
- **Format**: PNG with no transparency (RGB)
- **Unity Settings**: Configured as sprites with Point (no filter) filtering for crisp pixel art

## Available Tiles

### Stage 1: Happy/Normal Environment

1. **grass_floor_tile.png** - Bright green grass for outdoor areas
   - Color: Green (#50A050 base)
   - Use: Outdoor ground, garden areas

2. **wood_floor_tile.png** - Wooden floor planks for indoor areas
   - Color: Brown wood (#8B5A2B base)
   - Use: Indoor flooring in bunny's house

3. **wall_tile.png** - Clean beige walls
   - Color: Light beige (#C8B4A0 base)
   - Use: Interior walls

4. **carpet_tile.png** - Red carpet/rug texture
   - Color: Red (#B45050 base)
   - Use: Interior decoration, carpet areas

### Stage 2: Deteriorating Environment

5. **dark_grass_tile.png** - Darker, less vibrant grass
   - Color: Dark green/brown (#3C5032 base)
   - Use: Outdoor ground in deteriorating stage

6. **decaying_wall_tile.png** - Cracked and grimier walls
   - Color: Dark gray-brown (#78645A base)
   - Features: Visible cracks and dark patches
   - Use: Interior walls in deteriorating stage

### Stage 3: Dark/Final Environment

7. **dead_grass_tile.png** - Dead, brown grass
   - Color: Brown (#322D23 base)
   - Use: Outdoor ground in final dark stage

## Usage in Unity

### Setting up Tilemaps

1. Create a Tilemap GameObject in your scene
2. In the Project window, select a tile texture
3. Drag the texture into the Tile Palette to create tiles
4. Paint tiles onto your Tilemap using the Tile Palette

### Recommended Layer Structure

```
- Background
  - Floor Tilemap (grass/wood/carpet tiles)
- Midground
  - Wall Tilemap (wall tiles)
- Foreground
  - Objects and Props
- Character Layer
  - Player character
```

### Import Settings

The .meta files are configured with the following settings:
- **Filter Mode**: Point (no filter) - for crisp pixel art
- **Pixels Per Unit**: 32 - matches tile size
- **Compression**: None - preserves pixel-perfect quality
- **Texture Type**: Sprite (2D and UI)

## Design Notes

### Progression Through Stages

The tiles are designed to reflect the game's progression through three stages:

1. **Happy Stage**: Bright, saturated colors representing the initial cheerful environment
2. **Deteriorating Stage**: Desaturated, darker tones showing decay and neglect
3. **Dark Stage**: Very dark, grim colors for the final disturbing revelation

### Tiling

All textures are designed to tile seamlessly:
- Can be repeated horizontally and vertically without visible seams
- Random variations ensure tiles don't look too repetitive when placed together
- 32x32 size is optimal for a 640x360 resolution game

### Color Palette

The color scheme follows a progression:
- **Happy**: Bright greens (#50A050), warm browns (#8B5A2B), light beige (#C8B4A0)
- **Decay**: Muted greens (#3C5032), dark grays (#78645A)
- **Death**: Deep browns (#322D23), very dark tones

## Creating More Assets

To create additional tiles that match this style:
1. Use 32x32 pixel size
2. Follow the color palette for each stage
3. Add subtle variations to avoid repetitive patterns
4. Ensure edges tile seamlessly
5. Use Point filtering (no anti-aliasing) for pixel-perfect rendering

## Future Additions

Consider adding:
- Flower tiles (living and dead versions)
- Furniture sprites (laptop, fridge, etc.)
- Door and window tiles
- Path/road tiles
- Character sprites for the bunny protagonist

## Technical Details

- Created using Pillow (PIL) for Python
- Procedurally generated with controlled randomness for texture variation
- All assets use RGB color space (no alpha channel needed for floor/wall tiles)
