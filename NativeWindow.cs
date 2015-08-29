using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pausify
{
    //This class is not required but makes managing the modifiers easier.
    public static class Constants
    {
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        public const int WM_HOTKEY_MSG_ID = 0x0312;
    }

    public sealed class HotkeyManager : NativeWindow, IDisposable
    {
        public HotkeyManager()
        {
            CreateHandle(new CreateParams());
        }

        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                if (m.WParam.ToInt32() == 123)
                {
                    MessageBox.Show("HotKey ID: 123 has been pressed");
                }

                if (m.WParam.ToInt32() == 234)
                {
                    MessageBox.Show("HotKey ID: 234 has been pressed");
                }
            }
            base.WndProc(ref m);
        }

        public void Dispose()
        {
            DestroyHandle();
        }


    }
}





    

