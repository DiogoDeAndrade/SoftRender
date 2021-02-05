using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.UnityApp
{
    public enum CursorLockMode { None, Locked, Confined };

    public static class Cursor
    {
        static CursorLockMode _lockMode;

        static public CursorLockMode lockState
        {
            get => _lockMode;
            set
            {
                _lockMode = value;
                switch (_lockMode)
                {
                    case CursorLockMode.None:
                        SDL2.SDL.SDL_SetRelativeMouseMode(SDL2.SDL.SDL_bool.SDL_FALSE);
                        SDL2.SDL.SDL_SetWindowGrab(Application.current.GetWindowPtr(), SDL2.SDL.SDL_bool.SDL_FALSE);
                        break;
                    case CursorLockMode.Locked:
                        SDL2.SDL.SDL_SetRelativeMouseMode(SDL2.SDL.SDL_bool.SDL_TRUE);
                        break;
                    case CursorLockMode.Confined:
                        SDL2.SDL.SDL_SetWindowGrab(Application.current.GetWindowPtr(), SDL2.SDL.SDL_bool.SDL_TRUE);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
