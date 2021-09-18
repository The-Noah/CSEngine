using GLFW;

namespace TheNoah.CSEngine{
  public static class Input{
    public const Keys forward = Keys.W;
    public const Keys backward = Keys.S;
    public const Keys left = Keys.A;
    public const Keys right = Keys.D;

    public static bool GetKey(Keys key){
      return MyWindow.Singleton.GetKey(key);
    }
  }
}
