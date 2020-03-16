namespace FPS_Kotikov_D
{
	public sealed class Wall : Environment, IViewObject
	{

        public string ViewObject()
        {
            return gameObject.name;
        }

    }
}