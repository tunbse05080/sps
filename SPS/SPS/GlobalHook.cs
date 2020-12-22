using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPS
{
     class GlobalHook
    {
        private IKeyboardMouseEvents m_GlobalHook;
        //khoa click chuot
        int sub = 0;
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            //Unsubscribe();
            sub = 1;
            m_GlobalHook = Hook.AppEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            //m_GlobalHook.KeyPress += GlobalHookKeyPress;
              m_GlobalHook.KeyDown += GlobalHookKeyDown;
            //  m_GlobalHook.KeyUp += GlobalHookKeyUp;
        }

        //private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        //{
        //    Console.WriteLine("KeyPress: \t{0}", e.KeyChar);

        //}
        private void GlobalHookKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde || e.KeyCode == Keys.Tab || e.KeyCode == Keys.OemOpenBrackets || e.KeyCode == Keys.OemPipe || e.KeyCode == Keys.OemCloseBrackets || e.KeyCode == Keys.OemSemicolon || e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.OemQuotes || e.KeyCode == Keys.PrintScreen || e.KeyCode == Keys.Print)
            {
                e.Handled = true;
            }

        }
        //private void GlobalHookKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Oemtilde || e.KeyCode == Keys.Alt || e.KeyCode == Keys.OemOpenBrackets || e.KeyCode == Keys.OemPipe || e.KeyCode == Keys.OemCloseBrackets || e.KeyCode == Keys.OemSemicolon || e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.OemQuotes || e.KeyCode == Keys.PrintScreen || e.KeyCode == Keys.Print)
        //    {
        //        e.Handled = true;
        //    }

        //}

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            // Console.WriteLine("MouseDown: \t{0}; \t System Timestamp: \t{1}", e.Button, e.Timestamp);

            // uncommenting the following line will suppress the middle mouse button click
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) { e.Handled = true; }
        }

        public void Unsubscribe()
        {
            if (sub == 1)
            {
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
                // m_GlobalHook.KeyPress -= GlobalHookKeyPress;
                m_GlobalHook.KeyDown -= GlobalHookKeyDown;
                //It is recommened to dispose it
                m_GlobalHook.Dispose();
                sub = 0;
            }

        }
    }
}
