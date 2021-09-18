using System;

using GLFW;
using OpenGL;

namespace TheNoah.CSEngine{
  public sealed class MyWindow{
    public static MyWindow Singleton{get; private set;}

    public delegate void MyKeyCallback(Keys key, int scancode, InputState action, ModifierKeys mods);
    public MyKeyCallback keyCallback;

    private bool _fullscreen;
    public bool Fullscreen{
      get{return _fullscreen;}
      set{
        if(value == _fullscreen){
          return;
        }

        _fullscreen = value;

        if(_fullscreen){
          Glfw.GetWindowPosition(window, out _winInfo[0], out _winInfo[1]);
          Glfw.GetWindowSize(window, out _winInfo[2], out _winInfo[3]);

          Monitor monitor = Glfw.PrimaryMonitor;
          VideoMode videoMode = Glfw.GetVideoMode(monitor);
          Glfw.SetWindowMonitor(window, monitor, 0, 0, videoMode.Width, videoMode.Height, 0);
          UpdateViewportSize();
        } else{
          Glfw.SetWindowMonitor(window, Monitor.None, _winInfo[0], _winInfo[1], _winInfo[2], _winInfo[3], 0);
          UpdateViewportSize();
        }
      }
    }

    private double cursorLastX;
    private double cursorLastY;

    public bool ShouldClose{
      get{return Glfw.WindowShouldClose(window);}
      set{Glfw.SetWindowShouldClose(window, value);}
    }

    private Window window;
    private int[] _winInfo = new int[4];

    public MyWindow(int width, int height, string title){
      if(Glfw.Init()){
        Console.WriteLine("unable to initialize glfw");
        Environment.Exit(-1);
      }

      Glfw.WindowHint(Hint.ContextVersionMajor, 3);
      Glfw.WindowHint(Hint.ContextVersionMinor, 3);

      window = Glfw.CreateWindow(width, height, title, Monitor.None, Window.None);
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

      Glfw.SetInputMode(window, InputMode.Cursor, (int)CursorMode.Disabled);

      Singleton = this;
    }

    public bool GetKey(Keys key){
      return Glfw.GetKey(window, key) == InputState.Press;
    }

    public void UpdateViewportSize(){
      int width = 0;
      int height = 0;
      Glfw.GetFramebufferSize(window, out width, out height);

      Gl.Viewport(0, 0, width, height);
    }

    public void Close(){
      Glfw.DestroyWindow(window);
      Glfw.Terminate();
    }

    private void KeyPress(IntPtr _window, Keys key, int scancode, InputState action, ModifierKeys mods){
      keyCallback(key, scancode, action, mods);
    }

    public void SwapBuffers(){
      Glfw.SwapBuffers(window);
    }

    public void GetSize(ref int width, ref int height){
      Glfw.GetWindowSize(window, out width, out height);
    }

    public void GetCursorDelta(ref double x, ref double y){
      double xPos = 0;
      double yPos = 0;

      Glfw.GetCursorPosition(window, out xPos, out yPos);

      x = xPos - cursorLastX;
      y = cursorLastY - yPos;

      cursorLastX = xPos;
      cursorLastY = yPos;
    }
  }
}
