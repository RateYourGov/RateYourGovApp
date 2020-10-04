## Solution Images Shared Item Folder

This folder contains _images_ that could be shared (linked to) by any project in the solution.  
**NB: Only add images that are licensed as "In the Public Domain", free to use without restriction and without the need for attribution.**
**Take care not to create items here that violate Separation of Concerns policy**

### Image File Naming Convention
Images should be named with PascalCase (the first letter of every word in the identifier is upper case) directly followed by an underscore and the image Height x Width, or just one of the two if they are the same, and finally the image file type description.

_For Example:_
- BannerLogo_32x200.png ($"{@ImageName}_{@ImageHeighInPixels}x{@ImageWidthInPixels}.{@ImageFileExt}") = A .png Image with a Height of 32px and Width of 200px
- LogoImage_48.png ($"{@ImageName}_{@ImageHeighOrWidthInPixels}.{@ImageFileExt}") = A .png Image with an equal Height of 48px and Width of 48px

### Image Source Information 
Images should include a Markdown text file referencing the source of the image.  
The file should share the exact same name as the image file (without the image size attributes) followed by ".source.md"  

_For Example:_
- BannerLogo.png.source.md ($"{@ImageName}.{@ImageFileExt}.source.md") 
- LogoImage.source.md ($"{@ImageName}.{@ImageFileExt}.source.md") 

