using System;
using System.Numerics;

using OpenGL;

namespace TheNoah.CSEngine.Models{
  public class Model{
    public Shader shader;
    private Texture texture;

    public Vector3 position = Vector3.Zero;
    public Vector3 rotation = Vector3.Zero;

    private uint vao;
    private uint vbo;
    private uint ebo;

    private int vertexCount;

    // uniform locations
    private int projViewMatrixUniformLocation;
    private int modelMatrixUniformLocation;

    public Model(float[] vertices, uint[] indices, Texture texture, string shaderName = "texture"){
      // vertex array
      vao = Gl.GenVertexArray();
      Gl.BindVertexArray(vao);

      // vertex buffer
      vbo = Gl.GenBuffer();
      Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
      Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices, BufferUsage.StaticDraw);

      // element buffer
      ebo = Gl.GenBuffer();
      Gl.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
      Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)(sizeof(uint) * indices.Length), indices, BufferUsage.StaticDraw);

      // position
      Gl.VertexAttribPointer(0, 3, VertexAttribType.Float, false, 5 * sizeof(float), IntPtr.Zero);
      Gl.EnableVertexAttribArray(0);

      // texture coords
      Gl.VertexAttribPointer(1, 2, VertexAttribType.Float, false, 5 * sizeof(float), (IntPtr)(3 * sizeof(float)));
      Gl.EnableVertexAttribArray(1);

      // unbind
      Gl.BindVertexArray(0);
      Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);

      // shader
      shader = new Shader(shaderName);
      shader.Bind();
      shader.SetInt("textureCoords", 0);

      projViewMatrixUniformLocation = Gl.GetUniformLocation(shader.Id, "projViewMatrix");
      modelMatrixUniformLocation = Gl.GetUniformLocation(shader.Id, "modelMatrix");

      Shader.Unbind();

      this.texture = texture;

      vertexCount = indices.Length;
    }

    public void Cleanup(){
      Gl.DeleteVertexArrays(vao);
      Gl.DeleteBuffers(vbo);
      Gl.DeleteBuffers(ebo);

      texture.Cleanup();
      shader.Cleanup();
    }

    public void Render(Matrix4x4f projViewMatrix){
      Matrix4x4f modelMatrix = Matrix4x4f.Translated(position.X, position.Y, position.Z);
      modelMatrix.RotateX(rotation.X);
      modelMatrix.RotateY(rotation.Y);
      modelMatrix.RotateZ(rotation.Z);

      Gl.ActiveTexture(TextureUnit.Texture0);
      texture.Bind();

      shader.Bind();
      Gl.UniformMatrix4f(projViewMatrixUniformLocation, 1, false, projViewMatrix);
      Gl.UniformMatrix4f(modelMatrixUniformLocation, 1, false, modelMatrix);

      Gl.BindVertexArray(vao);
      Gl.DrawElements(PrimitiveType.Triangles, vertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

      Texture.Unbind();
      Shader.Unbind();
    }
  }
}
