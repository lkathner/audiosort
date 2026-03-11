using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AudioSort
{
    public class HockeyManager
    {
        public const int WM_HOTKEY = 0x312;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;

        [DllImport("user32")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static int _id = 0;

        private Form _wnd;

        // put modifiers on instance todo?

        public static event EventHandler<HotKeyEventArgs> HotKeyPressed;

        public HockeyManager(Form form)
        {
            _wnd = form;
        }

        public int RegisterHotKey(Keys key, KeyModifiers modifiers)
        {
            if (_wnd == null)
                throw new Exception("not attached to window");

            int id = System.Threading.Interlocked.Increment(ref _id);
            RegisterHotKey(_wnd.Handle, id, (uint)modifiers, (uint)key);
            return id;
        }

        /* todo remove after commit
        public static bool UnregisterHotKey(int? id = null)
        {
            if (_wnd == null)
                return false;

            int i = id.HasValue ? id.Value : _id;
            var b = UnregisterHotKey(_wnd.Handle, i);
            return b;
        }

        public static bool UnregisterHotKey(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return UnregisterHotKey();
            }

            var b = true;
            //foreach (var id in ids)
            //{
            //    b &= UnregisterHotKey(id);
            //}

            for (int j = ids.Count - 1; j >= 0; j--)
            {
                var id = ids[j];
                b &= UnregisterHotKey(id);
                ids.RemoveAt(j);
            }

            return b;
        }
        */

        public int UnregisterHotKeys(List<int> ids)
        {
            return UnregisterHotKeys(ids?.ToArray());
        }

        public int UnregisterHotKeys(params int[] ids)
        {
            int count = 0;

            if (_wnd == null)
                throw new Exception("not attached to window");

            if (ids == null)
                return count;

            for (int j = ids.Length - 1; j >= 0; j--)
            {
                if (UnregisterHotKey(_wnd.Handle, ids[j]))
                    count++;

                //ids.RemoveAt(j);
            }

            if (_id != 0 && !ids.Contains(_id))
            {
                if (UnregisterHotKey(_wnd.Handle, _id))
                    count++;

                throw new Exception($"unknown hotkey id {_id} was registered");
            }

            return count;
        }

        // todo non static event
        internal static void OnHotKeyPressed(HotKeyEventArgs e)
        {
            if (HockeyManager.HotKeyPressed != null)
            {
                HockeyManager.HotKeyPressed(null, e);
            }
        }
    }


    public class HotKeyEventArgs : EventArgs
    {
        public readonly Keys Key;
        public readonly KeyModifiers Modifiers;

        public HotKeyEventArgs(Keys key, KeyModifiers modifiers)
        {
            this.Key = key;
            this.Modifiers = modifiers;
        }

        public HotKeyEventArgs(IntPtr hotKeyParam)
        {
            uint param = (uint)hotKeyParam.ToInt64();
            Key = (Keys)((param & 0xffff0000) >> 16);
            Modifiers = (KeyModifiers)(param & 0x0000ffff);
        }
    }

    [Flags]
    public enum KeyModifiers
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
        NoRepeat = 0x4000
    }
}
