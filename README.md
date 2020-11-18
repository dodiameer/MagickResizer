# MagickResizer
A wrapper around ImageMagick to batch resize images

## Usage
```ImageMagick (-i | --input) <Input Directory> (-o | --output) <Output Directory> (-w | --width) 500 [(-h | --height) 250]```
Height defaults to the width value if omitted

For example:
```ImageMagick -i inputFiles -o outputFiles -w 800 -h 600```

## Output
Images are placed in the specified output directory in the format ```[Original Name]_[Width]x[Height].[Original file extention]>```

For example: 
```garden.png``` => ```garden_800x600.png```
