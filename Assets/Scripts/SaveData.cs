using System;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace Project
{
    public static class SaveData
    {
        public static bool TryGetUserValue<T>(string key, out T value)
        {
            if (PlayerPrefs.HasKey(key))
            {
                if (typeof(T) == typeof(string))
                {
                    var result = PlayerPrefs.GetString(key);

                    value = Unsafe.As<string, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(int))
                {
                    var result = PlayerPrefs.GetInt(key);

                    value = Unsafe.As<int, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(float))
                {
                    var result = PlayerPrefs.GetFloat(key);

                    value = Unsafe.As<float, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(bool))
                {
                    var result = PlayerPrefs.GetInt(key) != 0;

                    value = Unsafe.As<bool, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(byte))
                {
                    var result = (byte)PlayerPrefs.GetInt(key);

                    value = Unsafe.As<byte, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(sbyte))
                {
                    var result = (sbyte)PlayerPrefs.GetInt(key);

                    value = Unsafe.As<sbyte, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(short))
                {
                    var result = (short)PlayerPrefs.GetInt(key);

                    value = Unsafe.As<short, T>(ref result);

                    return true;
                }

                if (typeof(T) == typeof(ushort))
                {
                    var result = (ushort)PlayerPrefs.GetInt(key);

                    value = Unsafe.As<ushort, T>(ref result);

                    return true;
                }

                throw new Exception($"Unable to get user value of type {typeof(T).Name}");
            }

            value = default;

            return false;
        }

        public static void SetUserValue<T>(string key, T value)
        {
            if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(key, Unsafe.As<T, string>(ref value));
            }
            else if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, Unsafe.As<T, int>(ref value));
            }
            else if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, Unsafe.As<T, float>(ref value));
            }
            else if (typeof(T) == typeof(bool))
            {
                PlayerPrefs.SetInt(key, Unsafe.As<T, bool>(ref value) ? 1 : 0);
            }
            else if (typeof(T) == typeof(byte))
            {
                PlayerPrefs.SetInt(key, Unsafe.As<T, byte>(ref value));
            }
            else if (typeof(T) == typeof(sbyte))
            {
                PlayerPrefs.SetInt(key, Unsafe.As<T, sbyte>(ref value));
            }
            else if (typeof(T) == typeof(short))
            {
                PlayerPrefs.SetInt(key, Unsafe.As<T, short>(ref value));
            }
            else if (typeof(T) == typeof(ushort))
            {
                PlayerPrefs.SetInt(key, Unsafe.As<T, ushort>(ref value));
            }
            else
            {
                throw new Exception($"Unable to set user value of type {typeof(T).Name}");
            }
        }

        public static void DeleteUserValue(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
