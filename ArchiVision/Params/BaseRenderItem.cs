/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public abstract class BaseRenderItem
    {
        public virtual Color Colour { get; protected set; }
        public IGH_PreviewData Geometry { get; protected set; }
        protected List<BaseRenderItem> SubRenderItem { get; } = new List<BaseRenderItem>();

        public bool IsTopMost { get; set; }

        public BaseRenderItem(IGH_PreviewData geometry, Color color = default(Color), bool topMost = false)
        {
            Geometry = geometry;
            Colour = color;
            IsTopMost = topMost;
        }

        public virtual void DrawViewportWires( RhinoViewport Viewport, DisplayPipeline Display,
                Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {

        }

        public virtual void DrawViewportMeshes( RhinoViewport Viewport, DisplayPipeline Display,
            Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {

        }

        public void DrawViewportWires(IGH_PreviewArgs args, bool selected) 
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportWires(args, selected));
            if (IsTopMost) return;
            DrawViewportWires(args.Viewport, args.Display, args.WireColour_Selected, args.ShadeMaterial_Selected, selected);

        }

        public void DrawViewportMeshes(IGH_PreviewArgs args, bool selected) 
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportMeshes(args, selected));
            if (IsTopMost) return;
            DrawViewportMeshes(args.Viewport, args.Display, args.WireColour_Selected, args.ShadeMaterial_Selected, selected);

        }

        public void DrawViewportWires(DrawEventArgs e)
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportWires(e));
            if (!IsTopMost) return;
            DrawViewportWires(e.Viewport, e.Display, Color.White, null, false);

        }

        public void DrawViewportMeshes(DrawEventArgs e)
        {
            SubRenderItem.ForEach((sub) => sub.DrawViewportMeshes(e));
            if (!IsTopMost) return;
            DrawViewportMeshes(e.Viewport, e.Display, Color.White, null, false);

        }


        //	#region Render
        //	public void PushToRenderPipeline(GH_RenderArgs args)
        //	{
        //		IGH_RenderAwareData iGH_RenderAwareData = Geometry as IGH_RenderAwareData;
        //		if (iGH_RenderAwareData != null)
        //		{
        //			int hashCode = Material.GetHashCode();
        //			RenderMaterial renderMaterial = null;
        //			Dictionary<int, RenderMaterial> materialCache = args.MaterialCache;
        //			if (!Information.IsNothing(materialCache) && materialCache.ContainsKey(hashCode))
        //			{
        //				renderMaterial = materialCache[hashCode];
        //			}
        //			if (Information.IsNothing(renderMaterial))
        //			{
        //				renderMaterial = Material.MaterialBestGuess();
        //				renderMaterial.DocumentAssoc = args.Document;
        //				materialCache.Add(hashCode, renderMaterial);
        //				materialCache.Add(renderMaterial.GetHashCode(), renderMaterial);
        //			}
        //			iGH_RenderAwareData.AppendRenderGeometry(args, renderMaterial);
        //		}
        //	}

        //	public Guid PushToRhinoDocument(RhinoDoc doc, ObjectAttributes att)
        //	{
        //		IGH_BakeAwareData iGH_BakeAwareData = Geometry as IGH_BakeAwareData;
        //		if (iGH_BakeAwareData == null)
        //		{
        //			return Guid.Empty;
        //		}
        //		Guid obj_guid = Guid.Empty;
        //		if (!iGH_BakeAwareData.BakeGeometry(doc, att, out obj_guid))
        //		{
        //			obj_guid = Guid.Empty;
        //		}
        //		RhinoObject rhinoObject = doc.Objects.FindId(obj_guid);
        //		if (rhinoObject == null)
        //		{
        //			return Guid.Empty;
        //		}
        //		rhinoObject.RenderMaterial = AddOrUseMaterial(doc, Material);
        //		rhinoObject.CommitChanges();
        //		return obj_guid;
        //	}

        //	private static RenderMaterial AddOrUseMaterial(RhinoDoc doc, GH_Material material)
        //	{
        //		if (material.Type == GH_Material.MaterialType.RhinoMaterial)
        //		{
        //			foreach (RenderMaterial renderMaterial2 in doc.RenderMaterials)
        //			{
        //				if (renderMaterial2.Id == material.RdkMaterialId)
        //				{
        //					return renderMaterial2;
        //				}
        //			}
        //		}
        //		RenderMaterial renderMaterial = material.MaterialBestGuess();
        //		renderMaterial.DocumentAssoc = doc;
        //		uint renderHash = renderMaterial.RenderHash;
        //		foreach (RenderMaterial renderMaterial3 in doc.RenderMaterials)
        //		{
        //			if (renderHash == renderMaterial3.RenderHash)
        //			{
        //				return renderMaterial3;
        //			}
        //		}
        //		doc.RenderMaterials.Add(renderMaterial);
        //		return renderMaterial;
        //	}
        //#endregion
    }
}
