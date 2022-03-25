using UnityEngine;

public struct RenderIndex
{
    public int index;
    public Renderer renderer;

   public RenderIndex(int index, Renderer renderer)
    {
        this.index = index;
        this.renderer = renderer;
    }
   
}
