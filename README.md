# SoftRender: A software renderer for C#

* Uses SDL2 to create a surface and manage windows and input, but everything else is done through writing directly to a C# array that represents the RAM

## Installation

### Windows

On Windows, nothing should be needed, the SDL.dll library is included.

### MacOS

Just need to install sdl2, using for example brew:

'''
brew install sdl2
'''

## Licenses

Engine code developed by [Diogo de Andrade][DAndrade] and is made available under the [Mozilla Public License 2.0][MPLv2].

[SDL2#][SDL2#] by [Ethan Lee][ELee]

All the text and documentation (i.e., non-code files) are made available under
the [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International
License][CC BY-NC-SA 4.0].
https://github.com/flibitijibibo/SDL2-CS

[MPLv2]:https://opensource.org/licenses/MPL-2.0
[CC BY-NC-SA 4.0]:https://creativecommons.org/licenses/by-nc-sa/4.0/
[SDL2#]:https://github.com/flibitijibibo/SDL2-CS/blob/master/LICENSE
[ELee]:https://github.com/flibitijibibo
[DAndrade]:https://github.com/DiogoDeAndrade
