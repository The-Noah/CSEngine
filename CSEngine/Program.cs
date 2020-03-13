using System;

namespace TheNoah.CSEngine{
  static class Program{
    [STAThread()]
    private static void Main(string[] args){
      new CSEngine();

      Console.ReadLine();
    }
  }
}
