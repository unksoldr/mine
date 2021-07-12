﻿using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Imports;
using System;
using System.Reflection;

namespace AOSharp.Core.UI
{
    public class View
    {
        public readonly string Name;
        public readonly int Handle;
        public object Tag;

        protected readonly IntPtr _pointer;

        public IntPtr Pointer
        {
            get
            {
                return _pointer;
            }
        }

        internal unsafe View(IntPtr pointer, bool register = true)
        {
            _pointer = pointer;
            Handle = *(int*)(_pointer + 0x44);
            Name = "idk";
            //Name = StdString.FromPointer(*(IntPtr*)(_pointer + 0x20)).ToString();

            if (register)
                UIController.RegisterView(this);
        }

        public static View Create(Rect rect, string name, int unk1, int unk2)
        {
            IntPtr pView = View_c.Create(rect, name, unk1, unk2);

            if (pView == IntPtr.Zero)
                return null;

            return new View(pView);
        }
        
        public static View CreateFromXml(string filePath)
        {
            IntPtr pFilePath = StdString.Create(filePath).Pointer;
            IntPtr pUnknown = StdString.Create().Pointer;

            return GUIUnk.LoadViewFromXml(out var pView, pFilePath, pUnknown) ? new View(pView, false) : null;
        }

        public virtual void Dispose()
        {
            View_c.Deconstructor(_pointer);
        }

        public virtual void Update()
        {

        }

        private string GetName()
        {
            return "";
        }

        public void AddChild(View view, bool assignTabOrder)
        {
            View_c.AddChild(_pointer, view.Pointer, assignTabOrder);
        }

        public void RemoveChild(View view)
        {
            View_c.RemoveChild(_pointer, view.Pointer);
        }

        public bool FindChild<T>(string name, out T view, bool recursive = true) where T : View
        {
            view = null;
            IntPtr pView = View_c.FindChild(Pointer, name, recursive);

            if (pView == IntPtr.Zero)
                return false;

            //Try to return the view from cache.
            if (UIController.FindViewByPointer(pView, out view))
                return true;

            view = Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { pView, true }, null) as T;
            return true;
        }

        public void FitToContents(bool unk = false)
        {
            LimitMaxSize(CalculatePreferredSize(unk));
        }

        public Vector2 CalculatePreferredSize(bool unk = false)
        {
            Vector2 preferredSize = new Vector2();
            View_c.CalculatePreferredSize(_pointer, ref preferredSize, unk);
            return preferredSize;
        }

        public Vector2 GetPreferredSize(bool unk = false)
        {
            Vector2 preferredSize = new Vector2();
            View_c.GetPreferredSize(_pointer, ref preferredSize, unk);
            return preferredSize;
        }

        public void ResizeTo(Vector2 size)
        {
            View_c.ResizeTo(_pointer, ref size);
        }

        public void LimitMaxSize(Vector2 size)
        {
            View_c.LimitMaxSize(_pointer, ref size);
        }

        public void SetBorders(float x1, float y1, float x2, float y2)
        {
            View_c.SetBorders(_pointer, x1, y1, x2, y2);
        }

        public unsafe void SetFrame(Rect rect, bool unk)
        {
            View_c.SetFrame(_pointer, &rect, unk);
        }

        public void SetLayoutNode(LayoutNode layoutNode)
        {
            View_c.SetLayoutNode(_pointer, layoutNode.Pointer);
        }

        public void SetAlpha(float value)
        {
            View_c.SetAlpha(_pointer, value);
        }

        public void Show(bool visible, bool unk)
        {
            View_c.Show(_pointer, visible, unk);
        }
    }
}
