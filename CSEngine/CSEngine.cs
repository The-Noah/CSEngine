﻿// Vercidium is the best. Not possible without him!

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using GLFW;
using OpenGL;

namespace TheNoah.CSEngine{
  public class CSEngine{
    private MyWindow window;

    private bool wireframe;
    private uint ScreenshotNumber = 0;

    private Camera camera = new Camera();
    private Scene scene = new Scene();

    public CSEngine(){
      Console.WriteLine("Hello, World!");
      window = new MyWindow(800, 600, "CSEngine");
      window.keyCallback += KeyPress;

      Init();

      while(!window.ShouldClose){
        Time.Update();

        camera.Update();
        Render();

        window.SwapBuffers();
        Glfw.PollEvents();
      }

      window.Close();
      Debug.Log("\nclosed");
    }

    private void Init(){
      Debug.Log($"OpenGL version: {Gl.GetString(StringName.Version)}");
      Debug.Log($"OpenGL shading version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
      Debug.Log($"OpenGL vendor: {Gl.GetString(StringName.Vendor)}");
      Debug.Log($"OpenGL renderer: {Gl.GetString(StringName.Renderer)}");

      Gl.ReadBuffer(ReadBufferMode.Back);

      Gl.Enable(EnableCap.Texture2d);
      Gl.Enable(EnableCap.DepthTest);
      Gl.DepthFunc(DepthFunction.Less);

      //Gl.Enable(EnableCap.Blend);
      //Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

      float[] quadVertices = {
        // position         // tex coords
         0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
         0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f
      };

      uint[] quadIndices = {
        0, 1, 3,
        1, 2, 3
      };

      float[] cubeVertices = {
        // position         // tex coords
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
         0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
         0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f, 0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
         0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
         0.5f,  0.5f,  0.5f, 1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f, 1.0f, 0.0f
      };

      uint[] cubeIndices = {
        0, 1, 3, 3, 1, 2,
        1, 5, 2, 2, 5, 6,
        5, 4, 6, 6, 4, 7,
        4, 0, 7, 7, 0, 3,
        3, 2, 7, 7, 2, 6,
        4, 5, 0, 0, 5, 1
      };

      Texture texture = TextureLoader.GetTexture("wall.jpg");
      Models.Model cube = new Models.Model(cubeVertices, cubeIndices, texture);

      scene.AddModel(cube);
    }

    private void KeyPress(Keys key, int scancode, InputState action, ModifierKeys mods){
      if(action != InputState.Press){
        return;
      }

      switch(key){
        case Keys.Escape:
          window.ShouldClose = true; break;
        case Keys.F1:
          window.Fullscreen = !window.Fullscreen; break;
        case Keys.F2:
          TakeScreenshot(); break;
        case Keys.F3:
          wireframe = !wireframe;
          Gl.PolygonMode(MaterialFace.FrontAndBack, wireframe ? PolygonMode.Line : PolygonMode.Fill);
          break;
        case Keys.F4:
          camera.Reset();
          break;
      }
    }

    private void Render(){
      window.UpdateViewportSize();

      int width = 0;
      int height = 0;
      window.GetSize(ref width, ref height);

      Gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
      Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      Matrix4x4f proj = Matrix4x4f.Perspective(80f.ToRadians(), (float)width / height, .1f, 1000f);

      Matrix4x4f view = camera.GetViewMatrix();

      scene.Render(view * proj);
    }

    private void TakeScreenshot(){
      int width = 0;
      int height = 0;

      window.GetSize(ref width, ref height);

      Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
      BitmapData bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

      Gl.ReadPixels(0, 0, width, height, OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);

      Directory.CreateDirectory("screenshots");
      bitmap.Save($"screenshots/{String.Format("screenshot_{0:D3}.png", ScreenshotNumber)}");
    }
  }
}
