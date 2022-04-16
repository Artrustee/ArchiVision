using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ArchiVision
{
    public class ArchiVisionInfo : GH_AssemblyInfo
    {
        internal static readonly ArchiVisionConduit Conduit = new ArchiVisionConduit() { Enabled = true };

        public override string Name => "ArchiVision";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("E63A0373-C29D-4787-926D-CFB6B990518B");

        //Return a string identifying you or your company.
        public override string AuthorName => "秋水";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "1123993881@qq.com";
    }
}