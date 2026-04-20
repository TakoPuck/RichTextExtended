using Microsoft.Xna.Framework;

namespace RichTextExtended.Banks;

public class PaletteBank : Bank<Color[]>
{
    public PaletteBank()
    {
        Add("rainbow",
        [
            new(255, 0, 0),
            new(255, 135, 0),
            new(255, 211, 0),
            new(222, 255, 10),
            new(161, 255, 10),
            new(10, 255, 153),
            new(10, 239, 255),
            new(20, 125, 245),
            new(88, 10, 255),
            new(190, 10, 255)
        ]);
        Add("soft-candy",
        [
            new(245, 255, 198),
            new(180, 225, 255),
            new(171, 135, 255),
            new(255, 172, 228),
            new(193, 255, 155)
        ]);
        Add("retro",
        [
            new(249, 65, 68),
            new(243, 114, 44),
            new(248, 150, 30),
            new(249, 199, 79),
            new(144, 190, 109),
            new(67, 170, 139),
            new(87, 117, 144)
        ]);
        Add("elemental",
        [
            new(84, 71, 140),
            new(44, 105, 154),
            new(4, 139, 168),
            new(13, 179, 158),
            new(22, 219, 147),
            new(131, 227, 119),
            new(185, 231, 105),
            new(239, 234, 90),
            new(241, 196, 83),
            new(242, 158, 76)
        ]);
        Add("missing-white",
        [
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.Transparent,
            Color.White
        ]);
    }
}
