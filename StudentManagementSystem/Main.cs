using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void addDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentData studentDataForm = new StudentData(); // make new student data form
            studentDataForm.MdiParent = this; // make main form as parent 
            studentDataForm.Show(); // Display form
        }
    }
}
