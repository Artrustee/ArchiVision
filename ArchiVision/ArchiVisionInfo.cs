siusing Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ArchiVision
{
    public class ArchiVisionInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "ArchiVision";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("4affbeb7-9b1d-422e-8721-76da2c4e9ac9");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
