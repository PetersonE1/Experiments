using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace SDL_Test
{
    internal static class WindowSample
    {
        private static IntPtr window;
        private static IntPtr renderer;
        private static bool running = true;

        public static void Run()
        {
            Setup();

            while (running)
            {
                PollEvents();
                Render();
            }

            Cleanup();
        }

        private static void Setup()
        {
            if (SDL_Init(SDL_INIT_VIDEO) < 0)
                Console.WriteLine($"Error initializing SDL. {SDL_GetError()}");

            window = SDL_CreateWindow(
                "SDL .NET 7 Test",
                SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
                640, 480,
                SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if (window == IntPtr.Zero)
                Console.WriteLine($"Error creating the window. {SDL_GetError()}");

            renderer = SDL_CreateRenderer(window, -1,
                SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (renderer == IntPtr.Zero)
                Console.WriteLine($"Error creating the renderer. {SDL_GetError()}");
        }

        private static void PollEvents()
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false; break;
                }
            }
        }

        private static void Render()
        {
            SDL_SetRenderDrawColor(renderer, 135, 206, 235, 255);
            SDL_RenderClear(renderer);
            SDL_RenderPresent(renderer);
        }

        private static void Cleanup()
        {
            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }
    }
}
