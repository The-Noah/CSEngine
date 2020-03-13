using System;

using glfw3;
using OpenGL;

namespace TheNoah.CSEngine{
  public sealed class Window{
    public static Window Singleton{get; private set;}

    public delegate void KeyCallback(Key key, int scancode, State action, KeyModifier mods);
    public KeyCallback keyCallback;

    private bool _fullscreen;
    public bool Fullscreen{
      get{return _fullscreen;}
      set{
        if(value == _fullscreen){
          return;
        }

        _fullscreen = value;

        if(_fullscreen){
          Glfw.GetWindowPos(window, ref _winInfo[0], ref _winInfo[1]);
          Glfw.GetWindowSize(window, ref _winInfo[2], ref _winInfo[3]);

          GLFWmonitor monitor = Glfw.GetPrimaryMonitor();
          GLFWvidmode videoMode = Glfw.GetVideoMode(monitor);
          Glfw.SetWindowMonitor(window, monitor, 0, 0, videoMode.Width, videoMode.Height, 0);
          UpdateViewportSize();
        } else{
          Glfw.SetWindowMonitor(window, null, _winInfo[0], _winInfo[1], _winInfo[2], _winInfo[3], 0);
          UpdateViewportSize();
        }
      }
    }

    private double cursorLastX;
    private double cursorLastY;

    public bool ShouldClose{
      get{return Glfw.WindowShouldClose(window) == (int)State.True;}
      set{Glfw.SetWindowShouldClose(window, value ? 1 : 0);}
    }

    private GLFWwindow window;
    private int[] _winInfo = new int[4];

    public Window(int width, int height, string title){
      if(Glfw.Init() < 0){
        Console.WriteLine("unable to initialize glfw");
        Environment.Exit(-1);
      }

      Glfw.WindowHint((int)State.ContextVersionMajor, 3);
      Glfw.WindowHint((int)State.ContextVersionMinor, 3);

      window = Glfw.CreateWindow(width, height, title, null, null);
      if(window == null){
        Console.WriteLine("unable to create window");
        Glfw.Terminate();
        Environment.Exit(-1);
      }

      Gl.Initialize();
      Glfw.MakeContextCurrent(window);

      Glfw.SwapInterval(1);

      Glfw.SetWindowSizeCallback(window, delegate{UpdateViewportSize();});
      Glfw.SetKeyCallback(window, KeyPress);

      Glfw.SetInputMode(window, (int)State.Cursor, (int)State.CursorDisabled);

      Singleton = this;
    }

    public bool GetKey(Key key){
      return Glfw.GetKey(window, (int)key) == (int)State.True;
    }

    public void UpdateViewportSize(){
      int width = 0;
      int height = 0;
      Glfw.GetFramebufferSize(window, ref width, ref height);

      Gl.Viewport(0, 0, width, height);
    }

    public void Close(){
      Glfw.DestroyWindow(window);
      Glfw.Terminate();
    }

    private void KeyPress(IntPtr _window, int key, int scancode, int action, int mods){
      keyCallback((Key)key, scancode, (State)action, (KeyModifier)mods);
    }

    public void SwapBuffers(){
      Glfw.SwapBuffers(window);
    }

    public void GetSize(ref int width, ref int height){
      Glfw.GetWindowSize(window, ref width, ref height);
    }

    public void GetCursorDelta(ref double x, ref double y){
      double xPos = 0;
      double yPos = 0;

      Glfw.GetCursorPos(window, ref xPos, ref yPos);

      x = xPos - cursorLastX;
      y = cursorLastY - yPos;

      cursorLastX = xPos;
      cursorLastY = yPos;
    }
  }
}
