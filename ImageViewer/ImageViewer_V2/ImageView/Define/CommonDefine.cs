using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageView.Define;

static public class CommonDefine
{
    public const double ScaleStep = 0.1;
    public const double ScaleMax = 40;
    public const double ScaleMin = 0.9;

    public const double MouseSensitivity = 0.5;

    public enum Coordinate {eImage2Control = 0 , eControl2Image };
}
