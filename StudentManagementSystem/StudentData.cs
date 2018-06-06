using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentManagementSystem
{
    public partial class StudentData : Form
    {
        string connString = @"Data Source=DESKTOP-OR90E4H\SQLEXPRESS;Initial Catalog=student;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlDataAdapter dataAdapter;// helps to build the connection between application and database
        DataTable table; // table to hold the information so we can fill the datagrid view
        SqlCommandBuilder commandBuilder;// declare new sql command builder object
        SqlConnection conn; // declare a variable to hold the sqlconnection
        string selectStatement = "select * studentInfo";
        SqlCommand command; // declare a new sql command object

        public StudentData()
        {
            InitializeComponent();
        }

        private void StudentData_Load(object sender, EventArgs e)
        {
            cbo.SelectedIndex = 0;// first item in combo box is selected when the form load.
            dataGridView1.DataSource = bindingSource1;// set the source to be displayed in the grid view

            // Get all the data from studentInfo table
            GetData(selectStatement);
        }

        private void GetData(string selectStatement)
        {
            try
            {
                dataAdapter = new SqlDataAdapter(selectStatement, connString);
                table = new DataTable();// make new data table object
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);// fill data table
                bindingSource1.DataSource = table; // set the datasource on the binding source to the table
                dataGridView1.Columns[0].ReadOnly = true;// Prevent editing Id column
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message); // show a useful message to the user
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // SqlCommand command; // declare a new sql command object

            // field names in the table
            string insert = @"insert into studentInfo(Name,Address,Dob,ContactNumber,Gpa) 
                            values(@Name,@Address,@Dob,@ContactNumber,@Gpa)"; // parameter names
            using(conn = new SqlConnection(connString)) // "using()" method will dispose low level resources automatically.
            {
                try
                {
                    conn.Open(); // open the connection
                    command = new SqlCommand(insert, conn);// create new sql command object
                    command.Parameters.AddWithValue(@"Name", textBoxName.Text);
                    command.Parameters.AddWithValue(@"Address", textBoxAddress.Text);
                    command.Parameters.AddWithValue(@"Dob", textBoxDob.Text);
                    command.Parameters.AddWithValue(@"ContactNumber", textBoxContactNumber.Text);
                    command.Parameters.AddWithValue(@"Gpa", textBoxGpa.Text);
                    command.ExecuteNonQuery();// push data into table
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                } 
            }

            GetData("Select * from studentInfo");
            dataGridView1.Update(); // update datagrid view

            ClearFields(); // clear the text boxes are adding the data
        }

        private void ClearFields()
        {
            textBoxName.Clear();
            textBoxAddress.Clear();
            textBoxDob.Clear();
            textBoxContactNumber.Clear();
            textBoxGpa.Clear();

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();//get the update command

            try
            {
                bindingSource1.EndEdit();// updates the table in memory in our program
                dataAdapter.Update(table);//actually update the table
                MessageBox.Show("Update Successful!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentCell.OwningRow;//grab a reference to the current row
            string value = row.Cells["Id"].Value.ToString();// grab the value from the Id field of the selected record
            string Name = row.Cells["Name"].Value.ToString();
            string Address = row.Cells["Address"].Value.ToString();
            string Dob = row.Cells["Dob"].Value.ToString();
            string ContactNumber = row.Cells["ContactNumber"].Value.ToString();
            string Gpa = row.Cells["Gpa"].Value.ToString();

            DialogResult result = MessageBox.Show("Do you really want to delete " + Name + " " + Address + " " + Dob + " " + ContactNumber + " " + Gpa + ", record " + value,"Message",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            string deleteStatement = @"Delete from studentInfo where Id= '"+value+"'";//sql command to delete the record from the database
            if(result==DialogResult.Yes)//Check if user really wants to delete the record
            {
                using (conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();// try to open connection
                        command = new SqlCommand(deleteStatement,conn);
                        command.ExecuteNonQuery();// delete the data from database.
                        GetData(selectStatement);
                        dataGridView1.Update();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
