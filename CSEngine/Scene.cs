using System.Collections.Generic;

using OpenGL;

namespace TheNoah.CSEngine{
  public class Scene{
    private List<Models.Model> models = new List<Models.Model>();

    public void Render(Matrix4x4f projViewMatrix){
      foreach(Models.Model model in models){
        model.Render(projViewMatrix);
      }
    }

    public void Cleanup(){
      foreach(Models.Model model in models){
        model.Cleanup();
      }
    }

    public void AddModel(Models.Model model){
      models.Add(model);
    }
  }
}
