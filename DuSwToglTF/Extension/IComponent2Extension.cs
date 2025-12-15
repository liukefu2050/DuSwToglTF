using SharpGLTF.Materials;
using SolidWorks.Interop.sldworks;
using System;

namespace DuSwToglTF.Extension
{
    public static class IComponent2Extension
    {
        public static MaterialBuilder GetMaterialBuilder(this IComponent2 comp)
        {
            try
            {
                var materialValue = comp.GetModelMaterialPropertyValues("") as double[];
                return MaterialUtility.MaterialValueToMaterialBuilder(materialValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetMaterialBuilder：" + ex.StackTrace);
                return null;
            }


        }
    }
}
