# Color Assistant
## _Fast color solution for hyper casual games_

# Table of contents
1. [Features](#features)
2. [Theory](#theory)
3. [Tutorial](#tutorial)
4. [Custom Use Cases](#custom)
5. [Importing palettes from websites](#importcss)
6. [Modifying the final color](#modifyfinal)


## Features <a name="features"></a>

- Create color palettes with your own color setup
- Create color palettes online and copy the whole palette at once
- Apply color palette very easily
- Check color contrast 
- Modify the final HSV color value

## Theory <a name="theory"></a>

It is based around 3 data types.
- **Color Palette Settings:** It contains the definition of the color palette. It has a list of keys that defines what colors will be present in color palette. So for a simple level with a player and a floor there can be 2 keys in color palette settings. 'Player' and 'Floor'

- **Color Palette:** It contains a reference to it's 'Color Palette Settings'. It contains the color values of the keys defined in the color palette settings. So in the above example different color palettes can be created from the 'Color Palette Settings' which can contain different combinations of 'Player' and 'Floor' colors

- **Project Color Setup:** It contains the reference of the active 'Color Palette Settings' of the current project and the active 'Color Palette'

## Tutorial <a name="tutorial"></a>

### Step 1: Creating Color Palette Settings
- Create -> Color Assistant -> New Palette Settings
- Define palette keys. Each key will correspond to a color type. For example: 'Player' can be a key which will contain the color of the player and following the key different palettes will contain different player color

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283127/Color%20Assistant/ColorPaletteSettings.png)

### Step 2: Creating Color Palette
- Create -> Color Assistant -> New Palette
- Assign the Palette Settings you created earlier on 'Settings'
- The 'Main Color Palette' will be generated following the keys of the 'Settings'
- Assign different colors on the palette!

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283127/Color%20Assistant/ColorPalette.png)

### Step 3: Creating Project Color Setup
- Create -> Color Assistant -> Project Color Setup. Put it in 'Resources' folder. There should be only one instance of this per project. So create this once only
- Press Alt+C or (Rakib -> Select Color Assistant) to open up Project Color Settings Settings. Assign the 'Palette Settings' and a 'Color Palette' that follows that settings

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283128/Color%20Assistant/ProjectColorSetup.png)

### Step 4: Using
- Create any gameobject. Assign a new material
- Add Component -> Color Assistant -> Mesh Color Setter. Pick a Color Key. It will show you all the keys from your active 'Color Palette Settings' on the 'Project Color Setup'
- Now everytime you change color palette in Project Color Settings, the gameobject will change it's color following the key

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283127/Color%20Assistant/Use.png)

## Custom Use cases <a name="custom"></a>
At the moment, it can change colors on meshes by taking the Shader property and the material index. So basically it changes the color on the required materials. If you want to use the palette colors on your custom class then you can do that. You can even change the colors during runtime. Here you will see an example to change the color of the line renderer with your own custom class

1. Create your custom script. Name it 'LinePaletteSetter.cs'. You can use any name
2. Extend it from 'RendererPaletteBase'
3. Override the function 'SetPaletteColor'
4. You have access to the field 'colorKey' which you need to set from inspector
5. You have access to 'GetPaletteColor()' using which you can get the active palette color. Use the color that this function returns to set the color of whatever you want in the overriden function of Step 3
```
public class LinePaletteSetter : RendererPaletteBase
{
    public LineRenderer lineRenderer;
    public override void SetPaletteColor()
    {
        lineRenderer.startColor = GetPaletteColor();
        lineRenderer.endColor = GetPaletteColor();
    }
}
```
6. Add LineRenderer and make necessary adjustments on the inspector. Use 'Alt+C' to open up Project Color Setup and Reassign the color palette to make the changes take effect.

## Importing palettes from websites <a name="importcss"></a>
Import color palettes from any websites simply by creating a color palette and then copying it as CSS. Most popular website in this sector is Adobe Colors. You can create color palettes in different ways and save it in your Library. Then just copy the palette as CSS
![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619286665/Color%20Assistant/AdobeColor.png)
Open any color palette inspector and you will see the colors show up in the clipboard section
![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619286664/Color%20Assistant/ClipboardfromAbodeColors.png)
Now click on 'Pin the colors' and then copy and paste colors as you like into your main palette

## Modifying the final color <a name="modifyfinal"></a>
You can modify the final color by selecting the Project Color Setup and then checking values on Final Color Modifier section
1. Check Contrast: It turns the color to Grayscale. Use this to check the contrast between foreground and background

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619286985/Color%20Assistant/CheckContrast.png)

2. Final Color Modifier: Use this to change the brightness, saturation and contrast of the final color. 

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619286986/Color%20Assistant/FinalColor.png)

These are automatically disabled in builds as each take one extra draw calls
