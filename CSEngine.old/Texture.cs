using System.Drawing;

using OpenGL;

namespace TheNoah.CSEngine{
  public class Texture{
    public uint Id{get; private set;}

    public Texture(Bitmap bitmap){
      Id = Gl.GenTexture();
      Gl.BindTexture(TextureTarget.Texture2d, Id);

      int value = Gl.REPEAT;
      Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, value);
      Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, value);

      value = Gl.NEAREST;
      Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, value);
      Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, value);

      byte[] data = TextureLoader.SetImageBuffer(bitmap);
      Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

      Gl.GenerateMipmap(TextureTarget.Texture2d);

      Gl.BindTexture(TextureTarget.Texture2d, 0);
    }

    public void Cleanup(){
      Gl.DeleteTextures(Id);
    }

    public void Bind(){
      Gl.BindTexture(TextureTarget.Texture2d, Id);
    }

    public static void Unbind(){
      Gl.BindTexture(TextureTarget.Texture2d, 0);
    }
  }
}
