/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class MergedMeshRenderItem: BaseRenderItem
    {
        public MergedMeshRenderItem(List<MeshRenderItem> meshRenderItems, CurveRenderAttribute outlineAttribute, CurveRenderAttribute intersectAttribute)
            : base(false)
        {
            SubRenderItem.Clear();

            double tolerance = RhinoDoc.ActiveDoc?.ModelAbsoluteTolerance ?? 0.001;
            tolerance *= Intersection.MeshIntersectionsTolerancesCoefficient;
            Mesh mesh = new Mesh();
            List<Curve> allInter = new List<Curve>();
            meshRenderItems.ForEach((meshItem) =>
            {
                meshItem.RemoveOutLineRenderItem();
                SubRenderItem.Add(meshItem);

                Mesh relayMesh = ((GH_Mesh)meshItem.Geometry).Value;

                var polys = Intersection.MeshMeshAccurate(mesh, relayMesh, tolerance);
                if(polys != null)
                {
                    List<Curve> intersect = new List<Curve>();
                    polys.ToList().ForEach((poly) => intersect.Add(poly.ToNurbsCurve()));
                    allInter.AddRange(Curve.JoinCurves(intersect));
                }
                mesh.Append(relayMesh);
            });

            if (outlineAttribute != null)
                SubRenderItem.Add(new MeshOutlineRenderItem(new GH_Mesh(mesh), outlineAttribute));

            if (intersectAttribute != null)
                allInter.ForEach((crv) => SubRenderItem.Add(new CurveRenderItem(new GH_Curve(crv), intersectAttribute)));
        }
    }
}
