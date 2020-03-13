using System;
using System.Drawing;

namespace TheNoah.CSEngine{
  public static class TextureLoader{
    public static Texture GetTexture(string name){
      Console.WriteLine($"loading texture {name}");

      return new Texture(new Bitmap($"textures/{name}"));
    }

    public static byte[] SetImageBuffer(Bitmap image){
      image.RotateFlip(RotateFlipType.Rotate270FlipY);

      byte[] data = new byte[image.Width * image.Height * 4];

      uint i = 0;
      for(int x = 0; x < image.Width; x++){
        for(int y = 0; y < image.Height; y++){
          Color pixel = image.GetPixel(x, y);

          byte red = pixel.R;
          byte green = pixel.G;
          byte blue = pixel.B;
          byte alpha = pixel.A;

          data[i] = red;
          i++;

          data[i] = green;
          i++;

          data[i] = blue;
          i++;

          data[i] = alpha;
          i++;
        }
      }

      return data;
    }
  }
}
