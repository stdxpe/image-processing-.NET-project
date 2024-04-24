using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_process_demo5
{
    public partial class Splash_screen : Form
    {
        public Splash_screen()
        {
            InitializeComponent();
        }

        int counter;
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter >= 250)
            {
                timer1.Enabled = false;
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
        }

        int move = 2;
        private void timer2_Tick(object sender, EventArgs e)
        {
            panel2.Width += 8;

            if (panel2.Width > 1500)
            {
                panel2.Width = 0;
            }

            if (panel2.Width < 0)
            {
                move = 2;
            }
        }
    }
}
