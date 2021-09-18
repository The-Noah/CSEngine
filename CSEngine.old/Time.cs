using glfw3;

namespace TheNoah.CSEngine{
  public static class Time{
    public static float DeltaTime{get; private set;}
    private static float lastFrame;

    private static double lastTime = Glfw.GetTime();
    private static int frames;

    public static void Update(){
      double currentTime = Glfw.GetTime();
      DeltaTime = (float)currentTime - lastFrame;
      lastFrame = (float)currentTime;

      frames++;

      if(currentTime - lastTime >= 1.0){
        Debug.Log($"{frames} fps");
        frames = 0;
        lastTime += 1.0;
      }
    }
  }
}
