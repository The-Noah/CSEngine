using System;
using System.Numerics;

using glfw3;
using OpenGL;

namespace TheNoah.CSEngine{
  public class Camera{
    public float speed = 3f;

    private Vector3 position = new Vector3(0f, 0f, 10f);
    private double pitch;
    private double yaw;

    private Vector3 forward = new Vector3(0f, 0f, -1f);
    private Vector3 right = new Vector3(1f, 0f, 0f);
    private Vector3 up = new Vector3(0f, 1f, 0f);

    public void Reset(){
      position = Vector3.Zero;
      pitch = 0;
      yaw = 0;
    }

    public void Update(){
      float velocity = speed * Time.DeltaTime;

      if(Input.GetKey(Input.forward)){
        position += forward * velocity;
      }else if(Input.GetKey(Input.backward)){
        position -= forward * velocity;
      }
      if(Input.GetKey(Input.left)){
        position -= right * velocity;
      }else if(Input.GetKey(Input.right)){
        position += right * velocity;
      }
      if(Window.Singleton.GetKey(Key.Space)){
        position -= up * velocity;
      }else if(Window.Singleton.GetKey(Key.LeftShift)){
        position += up * velocity;
      }

      double x = 0;
      double y = 0;
      Window.Singleton.GetCursorDelta(ref x, ref y);

      float sensitivity = .05f;
      yaw += x * sensitivity;
      pitch += y * sensitivity;

      if(pitch > 89f){
        pitch = 89f;
      }else if(pitch < -89f){
        pitch = -89f;
      }

      Vector3 front;
      front.X = (float)Math.Cos(yaw.ToRadians()) * (float)Math.Cos(pitch.ToRadians());
      front.Y = (float)Math.Sin(pitch.ToRadians());
      front.Z = (float)Math.Sin(yaw.ToRadians()) * (float)Math.Cos(pitch.ToRadians());

      forward = Vector3.Normalize(front);
      right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
      up = Vector3.Normalize(Vector3.Cross(right, forward));
    }

    public Matrix4x4f GetViewMatrix(){
      return Matrix4x4f.LookAt(new Vertex3f(position.X, position.Y, position.Z), new Vertex3f(position.X, position.Y, position.Z) + new Vertex3f(forward.X, forward.Y, forward.Z), new Vertex3f(up.X, up.Y, up.Z));
    }

    // Pitch = positive up
    // Yaw = positive right
    private static Matrix4x4f CreateFPSView(Vector3 pos, double pitch, double yaw){
      Vector3 eye = pos;
      
      eye.X = -eye.X;
      eye.Y = -eye.Y;

      float cosPitch = (float)Math.Cos(pitch);
      float sinPitch = (float)Math.Sin(pitch);
      float cosYaw = (float)Math.Cos(-yaw);
      float sinYaw = (float)Math.Sin(-yaw);

      var xaxis = new Vector3(cosYaw, 0f, -sinYaw);
      var yaxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
      var zaxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

      // Create a 4x4 view matrix from the right, up, forward and eye position vectors
      return new Matrix4x4f(
          xaxis.X, yaxis.X, zaxis.X, 0,
          xaxis.Y, yaxis.Y, zaxis.Y, 0,
          -xaxis.Z, -yaxis.Z, -zaxis.Z, 0,
          Vector3.Dot(xaxis, eye), Vector3.Dot(yaxis, eye), Vector3.Dot(zaxis, eye), 1);
    }

    private static Vector3 FromPitchYaw(double pitch, double yaw){
      return new Vector3((float)Math.Cos(pitch) * (float)Math.Sin(yaw), (float)Math.Sin(pitch), (float)Math.Cos(pitch) * (float)Math.Cos(yaw));
    }
  }
}
