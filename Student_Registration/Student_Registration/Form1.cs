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

namespace Student_Registration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load();
        }

        SqlConnection con = new SqlConnection("Data Source=LAPTOP-SV555PUQ\\SQLEXPRESS; Initial Catalog=Student_Registration;User Id=sa;Password=Shahi@24");
        SqlCommand cmd;
        SqlDataReader read;
        SqlDataAdapter drr;
        string id;
        bool Mode = true;
        string sql;

        public object Id { get; private set; }

        public void Load()
        {
            try
            {
                sql = "select * from student";
                cmd = new SqlCommand(sql, con);
                con.Open();

                read = cmd.ExecuteReader();

                drr = new SqlDataAdapter(sql, con);
                dataGridView1.Rows.Clear();

                while(read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                con.Close();



            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);

            }
        }

        public void getID(string id)
            {
            sql = "select * from student where id = '" + id +"' ";

            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();

            while(read.Read())
            { 
               txtName.Text = read[1].ToString();
                txtCourse.Text = read[2].ToString();
                txtFee.Text = read[3].ToString();
            }
            con.Close();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string Course = txtCourse.Text; 
            string Fee = txtFee.Text;

            if(Mode == true)
            {
                sql = "insert into student(Studentname,Course,Fee) values(@Studentname,@Course,@Fee)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Studentname", name);
                cmd.Parameters.AddWithValue("@Course", Course);
                cmd.Parameters.AddWithValue("@Fee", Fee);
                MessageBox.Show("Record added");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCourse.Clear();
                txtFee.Clear();
                txtName.Focus();
            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update student set @Studentname =  @Studentname, Course = @Course, Fee = @Fee where id = @id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Studentname", name);
                cmd.Parameters.AddWithValue("@Course", Course);
                cmd.Parameters.AddWithValue("@Fee", Fee);
                cmd.Parameters.AddWithValue("@Id",  Id);
                MessageBox.Show("Record Updated");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCourse.Clear();  
                txtFee.Clear();
                txtName.Focus();
                button1.Text = "Save";
                Mode = true;

            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex== dataGridView1.Columns["Edit"].Index && e.RowIndex>=0) 
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button1.Text = "Edit";
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from student where id = @Id";
                con.Open();
                cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@id ", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted");
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            txtName.Clear();
            txtCourse.Clear();
            txtFee.Clear();
            txtName.Focus();
            button1.Text = "Save";
            Mode = true;
        }
    }
}
