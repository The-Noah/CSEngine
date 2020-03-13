using System;
using System.IO;
using System.Text;

using OpenGL;

namespace TheNoah.CSEngine{
  public class Shader{
    public uint Id{get; private set;}

    public Shader(string name){
      Console.WriteLine($"loading shader {name}");

      uint vertexShader = Compile(ReadFile($"{name}.vs"), ShaderType.VertexShader);
      uint fragmentShader = Compile(ReadFile($"{name}.fs"), ShaderType.FragmentShader);

      Id = CreateProgram(vertexShader, fragmentShader);

      Unbind();
    }

    public void Cleanup(){
      Gl.DeleteProgram(Id);
    }

    public void Bind(){
      Gl.UseProgram(Id);
    }

    private string[] ReadFile(string name){
      string path = Path.Combine("shaders", name);

      if(!File.Exists(path)){
        throw new FileNotFoundException($"shader {name} not found");
      }

      return new string[]{File.ReadAllText(path)};
    }

    #region Uniform Setters

    public void SetBool(string name, bool value){
      Bind();
      SetInt(name, value ? 1 : 0);
      Unbind();
    }

    public void SetInt(string name, int value){
      Bind();
      Gl.Uniform1i(Gl.GetUniformLocation(Id, name), 1, value);
      Unbind();
    }

    public void SetFloat(string name, float value){
      Bind();
      Gl.Uniform1f(Gl.GetUniformLocation(Id, name), 1, value);
      Unbind();
    }
    public void SetFloat(int location, float value){
      Bind();
      Gl.Uniform1f(location, 1, value);
      Unbind();
    }

    public void SetMatrix4f(string name, Matrix4x4f value){
      Bind();
      Gl.UniformMatrix4f(Gl.GetUniformLocation(Id, name), 1, false, value);
      Unbind();
    }

    #endregion

    #region Static Methods

    public static void Unbind(){
      Gl.UseProgram(0);
    }

    public static uint Compile(string[] source, ShaderType type){
      uint shader = Gl.CreateShader(type);
      Gl.ShaderSource(shader, source);
      Gl.CompileShader(shader);

      int compiled;
      Gl.GetShader(shader, ShaderParameterName.CompileStatus, out compiled);
      if(compiled != 0){
        return shader;
      }

      Gl.DeleteShader(shader);

      const int logMaxLength = 1024;
      StringBuilder infoLog = new StringBuilder(logMaxLength);
      int infoLogLength;

      Gl.GetShaderInfoLog(shader, logMaxLength, out infoLogLength, infoLog);

      throw new InvalidOperationException($"unable to compile shader: {infoLog}");
    }

    private static uint CreateProgram(uint vertexShader, uint fragmentShader){
      uint program = Gl.CreateProgram();

      Gl.AttachShader(program, vertexShader);
      Gl.AttachShader(program, fragmentShader);
      Gl.LinkProgram(program);

      int linked;
      Gl.GetProgram(program, ProgramProperty.LinkStatus, out linked);
      if(linked == 0){
        const int logMaxLength = 1024;
        StringBuilder infoLog = new StringBuilder(logMaxLength);
        int infoLogLength;

        Gl.GetProgramInfoLog(program, logMaxLength, out infoLogLength, infoLog);
        throw new InvalidOperationException($"unable to link shader program: {infoLog}");
      }

      Gl.DeleteShader(vertexShader);
      Gl.DeleteShader(fragmentShader);

      return program;
    }

    #endregion
  }
}
