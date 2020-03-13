using glfw3;

namespace TheNoah.CSEngine{
  public static class Input{
    public const Key forward = Key.W;
    public const Key backward = Key.S;
    public const Key left = Key.A;
    public const Key right = Key.D;

    public static bool GetKey(Key key){
      return Window.Singleton.GetKey(key);
    }
  }
}
