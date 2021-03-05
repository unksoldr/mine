﻿using System;
using AOSharp.Common.GameData;
using AOSharp.Common.Helpers;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;

namespace AOSharp.Common.Unmanaged.Interfaces
{
    public class InventoryGUIModule
    {
        public static string GetBackpackName(Identity identity)
        {
            IntPtr pInvGUIModule = InventoryGUIModule_c.GetInstance();

            if (pInvGUIModule == IntPtr.Zero)
                return string.Empty;

            StdString name = StdString.Create();
            InventoryGUIModule_c.GetBackpackName(pInvGUIModule, name.Pointer, ref identity, true);
            return name.ToString();
        }
    }
}
