using System.Reflection;
using ICities;

namespace RainFirefighting
{
    public class RainPlusMod : IUserMod
    {
        public string Name => "Rain Firefighting";

        public string Description
        {
            get
            {
                return "The modification allows the rain to put out fires."
#if DEBUG
                                                    + " v." + Assembly.GetExecutingAssembly().GetName().Version
#endif
                ;
            }
        }
    }
}
