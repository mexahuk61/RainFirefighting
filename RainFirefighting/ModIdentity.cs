using System.Reflection;
using ICities;

namespace RainFirefighting
{
    public class ModIdentity : IUserMod
    {
        public string Name => "Rain Firefighting";

        public string Description
        {
            get
            {
                return "The rain is able to extinguish the fire over a period of time."
#if DEBUG
                                                    + " v." + Assembly.GetExecutingAssembly().GetName().Version
#endif
                ;
            }
        }
    }
}
