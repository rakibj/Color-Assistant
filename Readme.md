# Color Assistant
## _Fast color solution for hyper casual games_


## Features

- Create color palettes with your own color setup
- Create color palettes online and copy the whole palette at once
- Apply color palette very easily
- Check color contrast 
- Modify the final HSV color value

## Theory

It is based around 3 data types.
- **Color Palette Settings:** It contains the definition of the color palette. It has a list of keys that defines what colors will be present in color palette. So for a simple level with a player and a floor there can be 2 keys in color palette settings. 'Player' and 'Floor'

- **Color Palette:** It contains a reference to it's 'Color Palette Settings'. It contains the color values of the keys defined in the color palette settings. So in the above example different color palettes can be created from the 'Color Palette Settings' which can contain different combinations of 'Player' and 'Floor' colors

- **Project Color Setup:** It contains the reference of the active 'Color Palette Settings' of the current project and the active 'Color Palette'

## Tutorial

##### Step 1: Creating Color Palette Settings
- Create -> Color Assistant -> New Palette Settings
- Define palette keys. Each key will correspond to a color type. For example: 'Player' can be a key which will contain the color of the player and following the key different palettes will contain different player color

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283127/Color%20Assistant/ColorPaletteSettings.png)

##### Step 2: Creating Color Palette
- Create -> Color Assistant -> New Palette
- Assign the Palette Settings you created earlier on 'Settings'
- The 'Main Color Palette' will be generated following the keys of the 'Settings'
- Assign different colors on the palette!

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283127/Color%20Assistant/ColorPalette.png)

##### Step 3: Creating Project Color Setup
- Create -> Color Assistant -> Project Color Setup. Put it in 'Resources' folder. There should be only one instance of this per project. So create this once only
- Press Alt+C or (Rakib -> Select Color Assistant) to open up Project Color Settings Settings. Assign the 'Palette Settings' and a 'Color Palette' that follows that settings

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283128/Color%20Assistant/ProjectColorSetup.png)

##### Step 4: Using
- Create any gameobject. Assign a new material
- Add Component -> Color Assistant -> Mesh Color Setter. Pick a Color Key. It will show you all the keys from your active 'Color Palette Settings' on the 'Project Color Setup'
- Now everytime you change color palette in Project Color Settings, the gameobject will change it's color following the key

![N|Solid](https://res.cloudinary.com/rakib56/image/upload/v1619283127/Color%20Assistant/Use.png)
