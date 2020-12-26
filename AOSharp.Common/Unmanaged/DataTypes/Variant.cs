﻿using System;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.DataTypes
{ 
    public class Variant
    {
        public readonly IntPtr Pointer;

        public Variant(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public static Variant Create(int value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(0x10), value));
        }

        public static Variant Create(float value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(0x10), value));
        }

        public static Variant Create(bool value)
        {
            return new Variant(Variant_c.Constructor(MSVCR100.New(0x10), value));
        }

        public void Dispose() => Variant_c.Deconstructor(Pointer);

        public int AsInt32() => Variant_c.AsInt32(Pointer);

        public float AsFloat() => Variant_c.AsFloat(Pointer);

        public bool AsBool() => Variant_c.AsBool(Pointer);
    }
}
