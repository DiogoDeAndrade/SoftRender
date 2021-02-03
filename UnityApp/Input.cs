using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public enum KeyCode
    {
        UNKNOWN = 0,
        BACKSPACE = 8,
        TAB = 9,
        RETURN = 13,
        ESCAPE = 27,
        SPACE = 32,
        EXCLAIM = 33,
        QUOTEDBL = 34,
        HASH = 35,
        DOLLAR = 36,
        PERCENT = 37,
        AMPERSAND = 38,
        QUOTE = 39,
        LEFTPAREN = 40,
        RIGHTPAREN = 41,
        ASTERISK = 42,
        PLUS = 43,
        COMMA = 44,
        MINUS = 45, 
        PERIOD = 46,
        SLASH = 47,
        Alpha0 = 48,
        Alpha1 = 49,
        Alpha2 = 50,
        Alpha3 = 51,
        Alpha4 = 52,
        Alpha5 = 53,
        Alpha6 = 54,
        Alpha7 = 55,
        Alpha8 = 56,
        Alpha9 = 57,
        COLON = 58,
        SEMICOLON = 59,
        LESS = 60,
        EQUALS = 61,
        GREATER = 62,
        QUESTION = 63,
        AT = 64,
        LEFTBRACKET = 91,
        BACKSLASH = 92,
        RIGHTBRACKET = 93,
        CARET = 94,
        UNDERSCORE = 95,
        BACKQUOTE = 96,
        A = 97,
        b = 98,
        c = 99,
        D = 100,
        E = 101,
        f = 102,
        g = 103,
        h = 104,
        i = 105,
        j = 106,
        k = 107,
        l = 108,
        m = 109,
        n = 110,
        o = 111,
        p = 112,
        Q = 113,
        r = 114,
        S = 115,
        t = 116,
        u = 117,
        v = 118,
        W = 119,
        x = 120,
        y = 121,
        z = 122,
        DELETE = 127,
        LSHIFT = 1,
        RSHIFT = 2,
        SHIFT = 3,
        LCTRL = 64,
        RCTRL = 128,
        CTRL = 192,
        LALT = 256,
        RALT = 512,
        ALT = 768,
        LGUI = 1024,
        RGUI = 2048,
    };

    public static class Input
    {
        static bool[] keyPress;
        static bool[] keyRelease;
        static bool[] key;

        static public bool GetKeyDown(KeyCode keyCode)
        {
            return keyPress[(int)keyCode];
        }

        static public bool GetKey(KeyCode keyCode)
        {
            return key[(int)keyCode];
        }
        static public bool GetKeyUp(KeyCode keyCode)
        {
            return keyRelease[(int)keyCode];
        }

        unsafe static public void PollKeyboard()
        {
            if (keyPress == null)
            {
                keyPress = new bool[4096];
                keyRelease= new bool[4096];
                key = new bool[4096];
            }

            int numKeys;
            var keys = SDL2.SDL.SDL_GetKeyboardState(out numKeys);

            int xpto = (int)SDL2.SDL.SDL_GetScancodeFromKey(SDL2.SDL.SDL_Keycode.SDLK_w);

            bool* srcData = (bool*)keys;

            for (int j = 0; j < numKeys; j++)
            {
                int i = (int)SDL2.SDL.SDL_GetKeyFromScancode((SDL2.SDL.SDL_Scancode)j);
                if (i > 4096) continue;

                keyRelease[i] = false;
                keyPress[i] = false;

                if (!srcData[j])
                {
                    if (key[i]) keyRelease[i] = true;
                }
                else
                {
                    if (!key[i]) keyPress[i] = true;
                }

                key[i] = srcData[j];
            }
        }
    }
}
