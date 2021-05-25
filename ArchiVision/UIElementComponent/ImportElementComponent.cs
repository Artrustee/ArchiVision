/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class ImportElementAttributes<T> : GH_ComponentAttributes where T: IGH_DocumentObject
    {
        public RectangleF ButtonBounds { get; set; }
        private bool highLight = false;

        public ImportElementAttributes(ImportElementComponent<T> owner)
            :base(owner)
        {
        }

        protected override void Layout()
        {
            float height = 20;
            float padding = 2;

            base.Layout();
            RectangleF relay = new RectangleF(this.Bounds.X, this.Bounds.Y + this.Bounds.Height, this.Bounds.Width, height);
            relay.Inflate(-padding, -padding);
            this.ButtonBounds = relay;
            this.Bounds = new RectangleF(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height + height);
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            float extend = 5;
            if(channel == GH_CanvasChannel.Wires && highLight)
            {
                foreach (T obj in ((ImportElementComponent<T>)Owner).DocObjects)
                {
                    RectangleF bounds = obj.Attributes.Bounds;
                    bounds.Inflate(extend, extend);
                    GH_Capsule cap = GH_Capsule.CreateCapsule(bounds, GH_Palette.Blue, (int)extend, 0);
                    cap.Render(graphics, Color.FromArgb(255, 19, 34, 122));
                }
            }
            base.Render(canvas, graphics, channel); 
            if(channel == GH_CanvasChannel.Objects)
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                GH_Capsule cap = GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Black, "Select", 2, 0);
                cap.Render(graphics, Selected, Owner.Locked, Owner.Hidden);
            }
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            bool contain = ButtonBounds.Contains(e.CanvasLocation);
            if (contain && !highLight)
            {
                highLight = true;
                sender.Refresh();
                return GH_ObjectResponse.Capture;
            }
            else if (!contain && highLight)
            {
                highLight = false;
                sender.Refresh();
                return GH_ObjectResponse.Release;
            }
            return base.RespondToMouseMove(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (ButtonBounds.Contains(e.CanvasLocation))
            {
                ((ImportElementComponent<T>)Owner).DocObjects.Clear();
                foreach (var obj in Owner.OnPingDocument().SelectedObjects())
                {
                    if (!(obj is T)) continue;
                    ((ImportElementComponent<T>)Owner).DocObjects.Add((T)obj);
                }
                this.Owner.ExpireSolution(true);
                return GH_ObjectResponse.Release;
            }
            return base.RespondToMouseDown(sender, e);
        }
    }

    public abstract class ImportElementComponent<T> : ElementComponent where T: IGH_DocumentObject
    {
        #region Values

        public List<T> DocObjects { get; private set; } = new List<T>();
        public List<Guid> Guids { get; private set; } = new List<Guid>();

        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("56a7ba86-5dab-4045-9105-86b806cb89d0");


        #endregion
        #endregion

        public override void CreateAttributes()
        {
            m_attributes = new ImportElementAttributes<T>(this);
        }

        /// <summary>
        /// Initializes a new instance of the ImportElementComponent class.
        /// </summary>
        public ImportElementComponent(string name, string nickname, string description)
          : base(name, nickname, description, Subcategory.UI_Element)
        {

        }

        #region Calculate

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected sealed override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new UIElementParameter(), "UI Elements", "Es", "UI Elements", GH_ParamAccess.list);
        }

        protected override void BeforeSolveInstance()
        {
            while (Guids.Count > 0)
            {
                IGH_DocumentObject obj = this.OnPingDocument().FindObject(Guids[0], false);
                if (obj is T)
                {
                    this.DocObjects.Add((T)obj);
                }
                Guids.RemoveAt(0);
            }
            base.BeforeSolveInstance();
        }

        public override bool Read(GH_IReader reader)
        {
            int count = 0;
            if(reader.TryGetInt32("Count", ref count))
            {
                for (int i = 0; i < count; i++)
                {
                    Guid guid = reader.GetGuid("Guid" + i);
                    try
                    {
                        IGH_DocumentObject obj = this.OnPingDocument().FindObject(guid, false);
                        if (obj is T) this.DocObjects.Add((T)obj);
                    }
                    catch
                    {
                        Guids.Add(guid);
                    }

                }
            }
            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            if(DocObjects.Count > 0)
            {
                writer.SetInt32("Count", DocObjects.Count);
                for (int i = 0; i < DocObjects.Count; i++)
                {
                    writer.SetGuid("Guid" + i, this.DocObjects[i].InstanceGuid); 
                }
            }
            return base.Write(writer);
        }
        #endregion
    }
}