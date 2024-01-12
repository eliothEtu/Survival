using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Survival.GameEngine
{
    internal class Artifact : Item
    {
        public Artifact(string name, string description, BitmapImage image, string type) : base(name, description, image, type)
        {

        }
    }
}
