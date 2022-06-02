using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsingADO.NET
{
    public partial class QLSinhVienForm : Form
    {
        private string connectionString;
        private const string DefaultSeachText = "Nhập tên để tìm kiếm";
        private List<Class> ClassList;
        private List<Student> StudentList;

        public QLSinhVienForm()
        {
            InitializeComponent();

        }


        #region Method

        private void GetAllClass()
        {
            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "select * from lop";
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable("Lop");
            conn.Open();
            adapter.Fill(table);
            conn.Close();

            foreach (DataRow row in table.Rows)
            {
                ClassList.Add(new Class(row));
            }

            cbbClass.DisplayMember = "TenLop";
            cbbClass.ValueMember = "ID";
            cbbClass.DataSource = ClassList;
        }

        private void GetAllStudent()
        {
            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "select * from SinhVien";
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable("SinhVien");
            conn.Open();
            adapter.Fill(table);
            conn.Close();

            foreach (DataRow row in table.Rows)
            {
                StudentList.Add(new Student(row));
            }
        }

        private void LoadListView(List<Student> list)
        {
            lvStudentList.Items.Clear();
            foreach (Student student in list)
            {
                AddStudentToLV(student);
            }
        }

        private void AddStudentToLV(Student std)
        {
            string[] row = { std.ID.ToString(), std.HoTen, std.MaLop.ToString() };
            var item = new ListViewItem(row);
            lvStudentList.Items.Add(item);
        }


        private Student GetStudent()
        {
            var std = new Student();
            if (!string.IsNullOrWhiteSpace(tbFullName.Text))
            {
                std.ID = string.IsNullOrWhiteSpace(tbID.Text) ? -1 : int.Parse(tbID.Text);
                std.HoTen = tbFullName.Text;
                std.MaLop = Convert.ToInt32(cbbClass.SelectedValue);
            }
            return std;
        }

        private void SetInfomation(Student std)
        {
            tbID.Text = std.ID.ToString();
            tbFullName.Text = std.HoTen;
            cbbClass.SelectedValue = std.MaLop;
        }

        #endregion Method





        #region Event
        private void QLSinhVienForm_Load(object sender, EventArgs e)
        {
            ClassList = new List<Class>();
            StudentList = new List<Student>();
            connectionString = ConfigurationManager.ConnectionStrings["QLSinhVien"].ConnectionString;
            GetAllClass();
            GetAllStudent();
            LoadListView(StudentList);
           
        }


        private void lvStudentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var count = lvStudentList.SelectedItems.Count;
            if(count >0)
            {
                var id = int.Parse(lvStudentList.SelectedItems[0].Text);
                var student = StudentList.FirstOrDefault(std => std.ID == id);
                SetInfomation(student);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            StudentList.Clear(); //Xóa list sinh viên trước khi load lại fix lỗi danh sách bị lặp
            GetAllStudent();
            LoadListView(StudentList);
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            cbbClass.Text = "";
            tbID.Text = "";
            tbFullName.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var std = GetStudent();
            if(std.HoTen == null)
            {
                MessageBox.Show("Chưa nhập đủ thông tin", "Thông báo");
                return;
            }

            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();

            if (std.ID < 0)
                command.CommandText = "exec InsertStudent @HoTen, @MaLop";
            else
            {
                command.CommandText = "Update SinhVien set HoTen = @hoten, MaLop = @malop where ID = @id";
            }

            command.Parameters.AddWithValue("@HoTen", std.HoTen);
            command.Parameters.AddWithValue("@MaLop", std.MaLop);
            command.Parameters.AddWithValue("@ID", std.ID);

            conn.Open();
            var num = command.ExecuteNonQuery();
            if(num>0)
            {
                btnReload.PerformClick();
            }
            conn.Close();
        }


        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            var ten = tbSearch.Text;
            var list = StudentList.Where(d => d.HoTen.ToLower().Contains(ten)).ToList();
            LoadListView(list);
        }

        //private void SetSeachText()
        //{
        //    tbSearch.Text = DefaultSeachText;
        //    tbSearch.GotFocus += TbSearch_GotFocus;
        //    tbSearch.LostFocus += TbSearch_LostFocus;
        //}

        //private void TbSearch_LostFocus(object sender, EventArgs e)
        //{
        //}

        //private void TbSearch_GotFocus(object sender, EventArgs e)
        //{
        //}

        private void tbSearch_MouseDown(object sender, MouseEventArgs e)
        {
            tbSearch.Text = "";
        }


        private void QLSinhVienForm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                tbSearch.Text = DefaultSeachText;
                btnReload.PerformClick();
            }
        }
        #endregion Event


    }
}
