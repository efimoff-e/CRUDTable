using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDTable
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //инициализируем подключение
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-V5QB5R7\SQLEXPRESS;Initial Catalog=AppCRUD;Integrated Security=True");
        //объявляем ID сотрудника
        public int EmployeeID;
        private void Form1_Load(object sender, EventArgs e)
        {
            GetEmployeeRecord();
        }
        private void GetEmployeeRecord()
        {   // выбираем все записи из БД
            SqlCommand sqlCommand = new SqlCommand("Select * from EmployeeDb", connection);
            DataTable dataTable = new DataTable();

            connection.Open();

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            dataTable.Load(sqlDataReader);
            connection.Close();

            dgvInfo.DataSource = dataTable;
        }

        //вызываем событие по щелчку на Insert
        private void btnInsert_Click(object sender, EventArgs e)
        {   /* делаем запрос в БД  EmployeeDb:
             - вставить в поля таблицы данные из textbox'ов*/
            if (isValid())
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO EmployeeDb VALUES (@employeeName, @employeeSurname, @employeePatronimic, @dateOfBirth, @Adress, @Department, @About)", connection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@employeeName", tbName.Text);
                sqlCommand.Parameters.AddWithValue("@employeeSurname", tbSurname.Text);
                sqlCommand.Parameters.AddWithValue("@employeePatronimic", tbPatronymic.Text);
                sqlCommand.Parameters.AddWithValue("@dateOfBirth", dtBirth.Value);
                sqlCommand.Parameters.AddWithValue("@Adress", tbAdress.Text);
                sqlCommand.Parameters.AddWithValue("@Department", tbDepartment.Text);
                sqlCommand.Parameters.AddWithValue("@About", tbAbout.Text);
                // открываем БД, выполняем запрос, закрываем
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                //сообщение о том, что операция прошла успешно
                MessageBox.Show("New Employee has been added!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //живое обновление данными  gridbox'а
                GetEmployeeRecord();
                ResetFormControl();
            }
        }

        // инициализируем флаг "если true"
        private bool isValid()
        { // проверка на заполнение имени сотрудника
        if(tbName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //очищаем поля формы
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFormControl();
        }

        private void ResetFormControl()
        {
            EmployeeID = 0;
            tbName.Clear();
            tbSurname.Clear();
            tbPatronymic.Clear();
            tbAdress.Clear();
            tbDepartment.Clear();
            tbAbout.Clear();
            
            tbName.Focus();
        }

        private void dgvInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //подсветка данных при выборе строки из gridbox'а
            EmployeeID = Convert.ToInt32(dgvInfo.SelectedRows[0].Cells[0].Value);
            tbName.Text = dgvInfo.SelectedRows[0].Cells[1].Value.ToString();
            tbSurname.Text = dgvInfo.SelectedRows[0].Cells[2].Value.ToString();
            tbPatronymic.Text = dgvInfo.SelectedRows[0].Cells[3].Value.ToString();
            dtBirth.Text = Convert.ToString(dgvInfo.SelectedRows[0].Cells[4].Value);
            tbAdress.Text = dgvInfo.SelectedRows[0].Cells[5].Value.ToString();
            tbDepartment.Text = dgvInfo.SelectedRows[0].Cells[6].Value.ToString();
            tbAbout.Text = dgvInfo.SelectedRows[0].Cells[7].Value.ToString();
        }
        // обновляем данные сотрудника
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(EmployeeID > 0)
            {
                SqlCommand sqlCommand = new SqlCommand("UPDATE EmployeeDb SET employeeName = @employeeName, employeeSurname = @employeeSurname, employeePatronimic = @employeePatronimic, dateOfBirth = @dateOfBirth, Adress = @Adress, Department = @Department, About = @About WHERE EmployeeID = @id", connection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@employeeName", tbName.Text);
                sqlCommand.Parameters.AddWithValue("@employeeSurname", tbSurname.Text);
                sqlCommand.Parameters.AddWithValue("@employeePatronimic", tbPatronymic.Text);
                sqlCommand.Parameters.AddWithValue("@dateOfBirth", Convert.ToString(dtBirth.Value));
                sqlCommand.Parameters.AddWithValue("@Adress", tbAdress.Text);
                sqlCommand.Parameters.AddWithValue("@Department", tbDepartment.Text);
                sqlCommand.Parameters.AddWithValue("@About", tbAbout.Text);
                sqlCommand.Parameters.AddWithValue("@id", this.EmployeeID);

                // открываем БД, выполняем запрос, закрываем
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Employee info has been updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //живое обновление данными  gridbox'а
                GetEmployeeRecord();

                ResetFormControl();
            }
            else
            {
                MessageBox.Show("Select employee to update", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        // удаляем запись из БД
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(EmployeeID > 0)
            {
                SqlCommand sqlCommand = new SqlCommand("DELETE FROM EmployeeDb WHERE EmployeeID = @id", connection);
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@id", this.EmployeeID);

                // открываем БД, выполняем запрос, закрываем
                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Employee info has been deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //живое обновление данными  gridbox'а
                GetEmployeeRecord();

                ResetFormControl();
            }
            else
            {
                MessageBox.Show("Select employee to delete his information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
