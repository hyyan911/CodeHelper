using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class ClassHelper
    {
        /// <summary>
        /// C#获取一个类在其所在的程序集中的所有实例子类
        /// </summary>
        /// <param name="parentType">给定的类型</param>
        /// <returns>所有子类的名称</returns>
        public static List<Type> GetSubClassTypes(Type parentType)
        {
            var subTypeList = new List<Type>();
            var assembly = parentType.Assembly;//获取当前父类所在的程序集``
            var assemblyAllTypes = assembly.GetTypes();//获取该程序集中的所有类型
            foreach (var itemType in assemblyAllTypes)//遍历所有类型进行查找
            {
                if (itemType.IsAbstract) continue;
                var baseType = itemType.BaseType;//获取元素类型的基类
                while (baseType != null && baseType.FullName != parentType.FullName)//如果有基类
                {
                    baseType = baseType.BaseType;
                }
                if (baseType != null)
                {
                    subTypeList.Add(itemType);//加入子类表中
                }
            }
            return subTypeList;//获取所有子类类型的名称
        }
}
}
