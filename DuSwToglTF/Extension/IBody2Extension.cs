using SharpGLTF.Geometry;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace DuSwToglTF.Extension
{
    using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPosition;

    public static class IBody2Extension
    {
        public static MeshBuilder<VERTEX> GetBodyMeshBuilder(this IBody2 swBody2,MaterialBuilder docMaterial, string displayName,bool improvedQuality = true)
        {
            var mesh = new MeshBuilder<VERTEX>();
            mesh.Name = displayName;
            var bodyMat = swBody2.GetMaterialBuilder() ?? docMaterial;

            //网格化
            var swTessellation = (Tessellation)swBody2.GetTessellation(null);
            {
                swTessellation.NeedFaceFacetMap = true;
                swTessellation.NeedVertexParams = true;
                swTessellation.NeedVertexNormal = true;
                swTessellation.ImprovedQuality = improvedQuality;
                // How to handle matches across common edges
                swTessellation.MatchType = (int)swTesselationMatchType_e.swTesselationMatchFacetTopology;
                // Do it
                bool bResult = swTessellation.Tessellate();
            }
            //int loop = 1;
            int counts = swBody2.GetFaceCount();
            Console.WriteLine("    "+counts + "个Face," + displayName);
            var face = (Face2)swBody2.GetFirstFace();

            //Stopwatch sw = new Stopwatch();

            while ((face != null))
            {
                try
                {
                    //sw.Restart();
                    var faceMaterial = face.GetMaterialBuilder();
                    var prim = mesh.UsePrimitive(faceMaterial ?? bodyMat);

                    int[] vFacetId = (int[])swTessellation.GetFaceFacets(face);
                    if (vFacetId != null && vFacetId.Length > 0)
                    {
                        //Console.WriteLine("    第" + loop + "face，lenth：" + vFacetId.Length + "," + DateTime.Now.ToString());
                        //Should always be three fins per facet
                        for (int i = 0; i < vFacetId.Length; i++)
                        {
                            int[] vFinId = (int[])swTessellation.GetFacetFins(vFacetId[i]);
                            List<VERTEX> points = new List<VERTEX>();
                            for (int j = 0; j < 3; j++)
                            {
                                int[] vVertexId = (int[])swTessellation.GetFinVertices(vFinId[j]);
                                //Should always be two vertices per fin
                                double[] vVertex1 = (double[])swTessellation.GetVertexPoint(vVertexId[0]);
                                double[] vVertex2 = (double[])swTessellation.GetVertexPoint(vVertexId[1]);
                                //double[] vVertex1 = { 2.2,5,56 };
                                //double[] vVertex2 = { 23.2, 45, 33 };
                                points.Add(new VERTEX(
                                    (float)vVertex1[0], (float)vVertex1[1], (float)vVertex1[2]
                                    ));
                                points.Add(new VERTEX(
                                     (float)vVertex2[0], (float)vVertex2[1], (float)vVertex2[2]
                                     ));
                            }
                            prim.AddTriangle(points[0], points[2], points[4]);

                        }
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                }
                
                //Console.WriteLine($"{loop},总耗时: {sw.ElapsedMilliseconds} ms" + "," + DateTime.Now.ToString());
                face = (Face2)face.GetNextFace();

                //loop++;
            }
            //sw.Stop();

            return mesh;
        }

        public static MaterialBuilder GetMaterialBuilder(this IBody2 body)
        {
            var materialValue = body.MaterialPropertyValues2 as double[];
            return MaterialUtility.MaterialValueToMaterialBuilder(materialValue);
        }


    }
}
