using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Windows.Forms;
using Pausify.Properties;
using System.Drawing;

namespace Pausify
{
    class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            
            item = new ToolStripMenuItem();
            item.Text = "Close";
            item.Click += new System.EventHandler(Exit_Click);
            //item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Program.processIcon.Dispose();
            Application.Exit();
        }
    }
}