﻿/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

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
using Rhino.Geometry;
using Rhino.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class MeshRenderItem : BaseRenderItem
    {
		public override Color Colour  => Shader.Diffuse;

		public DisplayMaterial Shader => Material.Value;

        public GH_Material Material { get; private set; }

		public CurveRenderAttribute OutLineAtt { get; private set; }

        public MeshRenderItem(GH_Mesh geometry, GH_Material material, CurveRenderAttribute nakedEdgeAtt, CurveRenderAttribute interiorEdgeAtt, CurveRenderAttribute outLineAtt, CurveRenderAttribute sharpEdgeAtt, double angle)
            :base(geometry)
        {
			Material = material;
			OutLineAtt = outLineAtt;

			SubRenderItem.Clear();

			if(nakedEdgeAtt != null || interiorEdgeAtt != null || sharpEdgeAtt != null)
            {
				List<GH_Curve> naked = new List<GH_Curve>();
				List<GH_Curve> interior = new List<GH_Curve>();
				List<GH_Curve> sharp = new List<GH_Curve>();
				checked
				{
                    geometry.Value.FaceNormals.ComputeFaceNormals();
                    for (int i = 0; i < geometry.Value.TopologyEdges.Count; i++)
					{
						if (!geometry.Value.TopologyEdges.IsNgonInterior(i))
						{
                            GH_Curve line = new GH_Curve(geometry.Value.TopologyEdges.EdgeLine(i).ToNurbsCurve());
                            int[] faces = geometry.Value.TopologyEdges.GetConnectedFaces(i);
                            switch (faces.Length)
                            {
                                case 1:
                                    naked.Add(line);
                                    break;
                                case 2:
                                    if(sharpEdgeAtt == null)
                                    {
                                        interior.Add(line);
                                        break;
                                    }
                                    Vector3d vec0 = geometry.Value.FaceNormals[faces[0]];
                                    Vector3d vec1 = geometry.Value.FaceNormals[faces[1]];
                                    if (Vector3d.VectorAngle(vec0, vec1) > angle)
                                        sharp.Add(line);
                                    else interior.Add(line);
                                    break;
                            }
                        }
                    }
				}

				if (nakedEdgeAtt != null)
					naked.ForEach((crv) => SubRenderItem.Add(new CurveRenderItem(crv, nakedEdgeAtt)));

				if (interiorEdgeAtt != null)
					interior.ForEach((crv) => SubRenderItem.Add(new CurveRenderItem(crv, interiorEdgeAtt)));

				if (sharpEdgeAtt != null)
					sharp.ForEach((crv) => SubRenderItem.Add(new CurveRenderItem(crv, sharpEdgeAtt)));
			}

			if (outLineAtt != null) SubRenderItem.Add(new MeshOutlineRenderItem(geometry, outLineAtt));
		}

        public override void DrawViewportMeshes(RhinoViewport Viewport, DisplayPipeline Display, Color WireColour_Selected, DisplayMaterial ShadeMaterial_Selected, bool selected)
        {
			DisplayMaterial mate = selected ? ShadeMaterial_Selected : Shader;
			Display.DrawMeshShaded(((GH_Mesh)Geometry).Value, mate);

			base.DrawViewportMeshes(Viewport, Display, WireColour_Selected, ShadeMaterial_Selected, selected);
        }


    }

}