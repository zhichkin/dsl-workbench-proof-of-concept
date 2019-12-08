﻿using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace OneCSharp.MVVM
{
    public sealed class CompositeCollectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var list = new CompositeCollection();

            foreach (var item in values)
            {
                if (item is IEnumerable)
                {
                    list.Add(new CollectionContainer()
                    {
                        Collection = (IEnumerable)item
                    });
                }
                else
                {
                    list.Add(item);
                }
            }

            return list;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
