# Ratran
GIF editor, personally used to hack together new Discord profile pictures.

<hr/>

As it stands, the config is defined in Ratran.Console's Program.cs, in a variable called project. 

(Should really be in a GUI or at least a JSON config later down the line.)

The structure is as follows:
* Project - defines the width and height of the gif, and a list of layers.
* Layer - A list of segments.
* Segment - A sequence of frames. Sequences are abstract, and can be whatever, as long as it outputs frames at the end of it. There are currently two segments implemented:
  * StillSegment - A single frame, that lasts a specified duration.
  * BlendAToBSegment - A animated transition between two frames (A and B).

Features:
* The currently implemented segments (above)
* Export to GIF
* When using multiple layers with segments that have different durations, the flattened layers/gif will use a minimum amount of extra frames needed to convey the changes.
  * I suck at explaining, hope this helps?
  * ![image](https://github.com/SquirrelKiev/Ratran/assets/41798511/a85e9a2b-ccce-469b-9f94-f1f3af2b0cad)
