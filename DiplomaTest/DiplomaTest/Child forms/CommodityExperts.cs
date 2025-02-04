﻿using DiplomaTest.Secondary_forms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

#pragma warning disable IDE1006 // Подавить проверку стилей именования

namespace DiplomaTest.Child_froms
{
    public partial class CommodityExperts : Form
    {
        #region Variables

        //DB variables
        private readonly string connectionString = @"Server=tcp:ubicakrovi.database.windows.net,1433;Initial Catalog=DiplomaTest;Persist Security Info=False;User ID=firetruck;Password=74Rjcnzy;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection connection;
        private SqlDataReader reader = null;
        private SqlCommand cmd;
        private string query;
        private readonly string defaultQuery = "Select * from Users where UserTypeID = 4";
        private string oldUsername, oldPassword;
        private int currentUserID = 0;

        private enum DataMode
        {
            Add,
            Edit,
            Delete,
            View
        }
        private DataMode dataMode = new DataMode();

        #endregion Variables


        //Constructor
        public CommodityExperts()
        {
            InitializeComponent();
        }

        #region Methods


        //Update dataGridView info
        public void RefreshCommodityExperts(string query)
        {
            string SqlText = query;
            SqlDataAdapter da = new SqlDataAdapter(SqlText, connectionString);
            DataSet ds = new DataSet();
            da.Fill(ds, "Users");
            dataGridViewCommodityExperts.DataSource = ds.Tables["Users"].DefaultView;
        }


        //dataGridView Cell Click
        private void dataGridViewCommodityExperts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowID = dataGridViewCommodityExperts.CurrentCell.RowIndex;
            SelectUser(rowID);
        }


        //Form load
        private void CommodityExperts_Load(object sender, EventArgs e)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                dataMode = DataMode.View;
                RefreshCommodityExperts(defaultQuery);
                SelectUser(0);
                connection.Close();
            }
            catch (Exception exception)
            {
                AlertForm alert = new AlertForm();
                alert.showAlert(exception.Message, AlertForm.alertType.Error);
            }
        }


        //Buttons
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditViewUserForm addEditForm = new AddEditViewUserForm(AddEditViewUserForm.DataMode.Add, "товароведа", 4, 0);
            addEditForm.Show();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddEditViewUserForm addEditForm = new AddEditViewUserForm(AddEditViewUserForm.DataMode.Edit, "товароведа", 4, currentUserID);
            addEditForm.Show();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteUser();
            SelectUser(0);
        }
        private void btnRefreshTable_Click(object sender, EventArgs e)
        {
            RefreshCommodityExperts(defaultQuery);
            SelectUser(0);
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            AddEditViewUserForm addEditForm = new AddEditViewUserForm(AddEditViewUserForm.DataMode.View, "товароведа", 4, currentUserID);
            addEditForm.Show();
        }


        //Independent methods 
        private void DeleteUser()
        {
            int count = 0;
            connection.Open();
            query = $"Delete from Users where";
            if (!string.IsNullOrEmpty(textBoxUsername.Text))
            {
                query += $" Users.Username = N'{textBoxUsername.Text}'";
                count++;
            }
            if (!string.IsNullOrEmpty(textBoxPassword.Text))
            {
                if (count > 0)
                {
                    query += $" and Users.Password = N'{textBoxPassword.Text}'";
                }
                else
                {
                    query += $" Users.Password = N'{textBoxPassword.Text}'";
                    count++;
                }
            }
            if (!string.IsNullOrEmpty(textBoxName.Text))
            {
                if (count > 0)
                {
                    query += $" and Users.Name = N'{textBoxName.Text}'";
                }
                else
                {
                    query += $" Users.Name = N'{textBoxName.Text}'";
                    count++;
                }
            }
            if (!string.IsNullOrEmpty(textBoxSurname.Text))
            {
                if (count > 0)
                {
                    query += $" and Users.Surname = N'{textBoxSurname.Text}'";
                }
                else
                {
                    query += $" Users.Surname = N'{textBoxSurname.Text}'";
                    count++;
                }
            }
            if (!string.IsNullOrEmpty(textBoxPatronymic.Text))
            {
                if (count > 0)
                {
                    query += $" and Users.Patronymic = N'{textBoxPatronymic.Text}'";
                }
                else
                {
                    query += $" Users.Patronymic = N'{textBoxPatronymic.Text}'";
                }
            }
            if (query != "Delete from Users where")
            {
                cmd = new SqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                reader.Close();
                RefreshCommodityExperts(defaultQuery);
            }
            connection.Close();
        }
        private void SelectUser(int rowID)
        {
            if (dataGridViewCommodityExperts.RowCount < 1)
            {
                return;
            }
            currentUserID = Convert.ToInt32(dataGridViewCommodityExperts.Rows[rowID].Cells[0].Value);
            textBoxName.Text = dataGridViewCommodityExperts.Rows[rowID].Cells[3].Value.ToString();
            textBoxSurname.Text = dataGridViewCommodityExperts.Rows[rowID].Cells[4].Value.ToString();
            textBoxPatronymic.Text = dataGridViewCommodityExperts.Rows[rowID].Cells[5].Value.ToString();
            textBoxUsername.Text = dataGridViewCommodityExperts.Rows[rowID].Cells[1].Value.ToString();
            textBoxPassword.Text = dataGridViewCommodityExperts.Rows[rowID].Cells[2].Value.ToString();
        }


        #endregion
    }
}
