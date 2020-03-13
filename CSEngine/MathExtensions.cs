using System;

namespace TheNoah.CSEngine{
  public static class MathExtensions{
    public static float ToRadians(this float degree){
      return degree * (float)Math.PI / 180f;
    }

    public static double ToRadians(this double degree){
      return degree * Math.PI / 180.0;
    }
  }
}
