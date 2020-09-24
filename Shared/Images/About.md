## Solution Images Shared Item Folder

This folder contains _images_ that could be shared (linked to) by any project in the solution.  
**NB: Take care not to create items here that violate Separation of Concerns policy**

### Image File Naming Convention
Images should be named with PascalCase (the first letter of every word in the identifier is upper case) direcctly followed by the image Height x Width, or just one of the two if they are the same.

For Example:
- BannerLogo32x200.png ($"ImageName{@ImageHeighInPixels}x{ImageWidthInPixels}") = An Image with a Height of 32px and Width of 200px
- LogoImage48.png ($"ImageName{@ImageHeighOrWidthInPixels}") = An Image with an equal Height of 48px and Width of 48px

