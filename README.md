# Text2Rgb
## About this project
A tool to convert text into an RGB (color) image. I pretty much made this for fun so I can't immediately think of practical uses for this.
The first idea was to use it as an compression method, but that didn't really work out.

## How does it work?
In an RGB bitmap each color has 3 int values (0,255), 4 if you take alpha into account. It gets the ASCII value of the characters, uses this to create a RGB color and sets the pixel at the corresponding location.

All you have to do is define the width so it can auto-calculate the height.
