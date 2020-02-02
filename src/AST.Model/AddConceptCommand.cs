using System;
using System.Collections;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    public sealed class CreateConceptCommand
    {
        /// <summary>
        /// propertyName parameter must be of type List<<T>> !
        /// </summary>
        /// <param name="concept"></param>
        /// <param name="propertyName"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public ISyntaxNode Create(ISyntaxNode concept, string propertyName, Type child)
        {
            ISyntaxNode item = (ISyntaxNode)Activator.CreateInstance(child);
            item.Parent = concept;

            IList list;
            PropertyInfo property = concept.GetType().GetProperty(propertyName);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(concept);
                list = (IList)optional.Value;
                if (list == null)
                {
                    Type listType = property.PropertyType.GetProperty("Value").PropertyType;
                    list = (IList)Activator.CreateInstance(listType);
                    optional.Value = list;
                }
            }
            else
            {
                list = (IList)property.GetValue(concept);
                if (list == null)
                {
                    Type listType = property.PropertyType;
                    list = (IList)Activator.CreateInstance(listType);
                    property.SetValue(concept, list);
                }
            }

            list.Add(item);
            return item;
        }
    }
}