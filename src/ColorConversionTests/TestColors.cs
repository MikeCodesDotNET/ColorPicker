using System;
using System.Collections.Generic;
using System.Text;

namespace ColorConversionTests
{
    abstract class TestColorBase 
    {
        public abstract string Hex { get; }

        public abstract int[] HSB { get; }

        public abstract int[] HSL { get; }

        public abstract byte[] RGB { get; }

        public abstract int[] CMYK { get; }

        public abstract int[] LAB { get; }

    }


    class Cerise : TestColorBase
    {
        public static Cerise Instance => new Cerise();

        public override string Hex => "F61067";

        public override int[] HSB => new int[] { 337, 93, 96 };

        public override int[] HSL => new int[] { 337, 93, 51 };

        public override byte[] RGB => new byte[] { 246, 16, 103 };

        public override int[] CMYK => new int[] { 0, 93, 58, 4 };

        public override int[] LAB => new int[] { 53, 80, 16 };
    }


    class Purple : TestColorBase
    {
        public static Purple Instance => new Purple();

        public override string Hex => "5E239D";

        public override int[] HSB => new int[] { 269, 78, 62 };

        public override int[] HSL => new int[] { 269,64,38 };

        public override byte[] RGB => new byte[] { 94,35,157 };

        public override int[] CMYK => new int[] {40, 78, 0,38 };

        public override int[] LAB => new int[] { 29,50,-55};
    }

}
