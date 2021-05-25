/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Components;
using Grasshopper.Kernel.Types;
using Microsoft.VisualBasic;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class RenderItem
    {
		public Color Colour 
		{
			get => Shader.Diffuse;
			set => Shader.Diffuse = value;
		}

		public DisplayMaterial Shader
        {
			get => Material.Value;
			set => Material.Value = value;
        }

        public GH_Material Material { get; set; }

        public IGH_PreviewData Geometry { get; set; }

        public RenderItem(IGH_PreviewData geometry, GH_Material material)
        {
			Geometry = geometry;
			Material = material;
        }

		public virtual void DrawViewportWires(IGH_PreviewArgs args, bool selected)
        {
			Geometry.DrawViewportWires(new GH_PreviewWireArgs(args.Viewport, args.Display, selected ? args.WireColour_Selected : Colour, args.DefaultCurveThickness));
        }

		public virtual void DrawViewportMeshes(IGH_PreviewArgs args, bool selected)
        {
			Geometry.DrawViewportMeshes(new GH_PreviewMeshArgs(args.Viewport, args.Display, selected ? args.ShadeMaterial_Selected : Shader, args.MeshingParameters));
        }

		#region Render
		public void PushToRenderPipeline(GH_RenderArgs args)
		{
			IGH_RenderAwareData iGH_RenderAwareData = Geometry as IGH_RenderAwareData;
			if (iGH_RenderAwareData != null)
			{
				int hashCode = Material.GetHashCode();
				RenderMaterial renderMaterial = null;
				Dictionary<int, RenderMaterial> materialCache = args.MaterialCache;
				if (!Information.IsNothing(materialCache) && materialCache.ContainsKey(hashCode))
				{
					renderMaterial = materialCache[hashCode];
				}
				if (Information.IsNothing(renderMaterial))
				{
					renderMaterial = Material.MaterialBestGuess();
					renderMaterial.DocumentAssoc = args.Document;
					materialCache.Add(hashCode, renderMaterial);
					materialCache.Add(renderMaterial.GetHashCode(), renderMaterial);
				}
				iGH_RenderAwareData.AppendRenderGeometry(args, renderMaterial);
			}
		}

		public Guid PushToRhinoDocument(RhinoDoc doc, ObjectAttributes att)
		{
			IGH_BakeAwareData iGH_BakeAwareData = Geometry as IGH_BakeAwareData;
			if (iGH_BakeAwareData == null)
			{
				return Guid.Empty;
			}
			Guid obj_guid = Guid.Empty;
			if (!iGH_BakeAwareData.BakeGeometry(doc, att, out obj_guid))
			{
				obj_guid = Guid.Empty;
			}
			RhinoObject rhinoObject = doc.Objects.FindId(obj_guid);
			if (rhinoObject == null)
			{
				return Guid.Empty;
			}
			rhinoObject.RenderMaterial = AddOrUseMaterial(doc, Material);
			rhinoObject.CommitChanges();
			return obj_guid;
		}

		private static RenderMaterial AddOrUseMaterial(RhinoDoc doc, GH_Material material)
		{
			if (material.Type == GH_Material.MaterialType.RhinoMaterial)
			{
				foreach (RenderMaterial renderMaterial2 in doc.RenderMaterials)
				{
					if (renderMaterial2.Id == material.RdkMaterialId)
					{
						return renderMaterial2;
					}
				}
			}
			RenderMaterial renderMaterial = material.MaterialBestGuess();
			renderMaterial.DocumentAssoc = doc;
			uint renderHash = renderMaterial.RenderHash;
			foreach (RenderMaterial renderMaterial3 in doc.RenderMaterials)
			{
				if (renderHash == renderMaterial3.RenderHash)
				{
					return renderMaterial3;
				}
			}
			doc.RenderMaterials.Add(renderMaterial);
			return renderMaterial;
		}
	}
    #endregion
}
