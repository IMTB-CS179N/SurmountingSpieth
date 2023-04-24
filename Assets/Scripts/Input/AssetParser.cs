﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityEngine;

namespace Project.Input
{
    public class AssetParser
    {
        public static T[] ParseFromCSV<T>(string file) where T : IDisposable
        {
            var type = typeof(T);

            var properties = AssetParser.GetProperties(type);

            using (var stream = new StreamReader(File.OpenRead(file)))
            {
                var instances = new List<T>();

                var reader = new CsvReader(stream);

                int index = 1;

                while (reader.Read())
                {
                    if (reader.FieldsCount != properties.Length)
                    {
                        throw new Exception($"Unable to parse CSV file at line {index}, expected {properties.Length} number of arguments, got {reader.FieldsCount}");
                    }

                    var instance = (T)Activator.CreateInstance(type);

                    for (int k = 0; k < reader.FieldsCount; ++k)
                    {
                        AssetParser.PopulateProperty(properties[k], instance, reader[k]);
                    }

                    index++;

                    instances.Add(instance);
                }

                return instances.ToArray();
            }
        }

        private static PropertyInfo[] GetProperties(Type type)
        {
            var properties = new List<(PropertyInfo info, int order)>();

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attribute = property.GetCustomAttribute<OrderAttribute>();

                if (attribute is not null && property.GetSetMethod(false) is not null)
                {
                    properties.Add((property, attribute.Order));
                }
            }

            properties.Sort((x, y) => x.order.CompareTo(y.order));

            if (properties.Count == 0)
            {
                return Array.Empty<PropertyInfo>();
            }
            else
            {
                var result = new PropertyInfo[properties.Count];

                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = properties[i].info;
                }

                return result;
            }
        }

        private static void PopulateProperty(PropertyInfo property, object target, string value)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(target, value);
            }
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(target, Boolean.Parse(value));
            }
            else if (property.PropertyType == typeof(sbyte))
            {
                property.SetValue(target, SByte.Parse(value));
            }
            else if (property.PropertyType == typeof(byte))
            {
                property.SetValue(target, Byte.Parse(value));
            }
            else if (property.PropertyType == typeof(short))
            {
                property.SetValue(target, Int16.Parse(value));
            }
            else if (property.PropertyType == typeof(ushort))
            {
                property.SetValue(target, UInt16.Parse(value));
            }
            else if (property.PropertyType == typeof(int))
            {
                property.SetValue(target, Int32.Parse(value));
            }
            else if (property.PropertyType == typeof(uint))
            {
                property.SetValue(target, UInt32.Parse(value));
            }
            else if (property.PropertyType == typeof(long))
            {
                property.SetValue(target, Int64.Parse(value));
            }
            else if (property.PropertyType == typeof(ulong))
            {
                property.SetValue(target, UInt64.Parse(value));
            }
            else if (property.PropertyType == typeof(float))
            {
                property.SetValue(target, Single.Parse(value));
            }
            else if (property.PropertyType == typeof(double))
            {
                property.SetValue(target, Double.Parse(value));
            }
            else if (property.PropertyType == typeof(Sprite))
            {
                property.SetValue(target, Resources.Load<Sprite>(value));
            }
            else if (property.PropertyType == typeof(Texture2D))
            {
                property.SetValue(target, Resources.Load<Texture2D>(value));
            }
            else
            {
                throw new Exception($"Unable to parse the property {property.Name} since it has unsupported parsing type");
            }
        }
    }
}
