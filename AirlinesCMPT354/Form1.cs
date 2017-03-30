using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AirlinesCMPT354
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            //SqlConnection connection = new SqlConnection("data source = cypress.csil.sfu.ca; user id = s_hlotey; password = QdJ326P6tH4yjf3f ; initial catalog = hlotey354;");
            SqlConnection connection = new SqlConnection("Data Source = DESKTOP-VK3SP4R\\SQLEXPRESS; Initial Catalog = CMPT354_bank;" + "Integrated Security= true");
            connection.Open();
            SqlCommand sqlCom1 = new SqlCommand("Select * from Passenger", connection);
            SqlCommand sqlCom2 = new SqlCommand("Select F.flight_code from Flight F", connection);
            SqlDataReader readData = sqlCom1.ExecuteReader();
            
            while (readData.Read())
            {
                listBox1.Items.Add(readData[0]);
            }
            readData.Close();
            SqlDataReader readData2 = sqlCom2.ExecuteReader();
            while (readData2.Read())
            {
                listBox2.Items.Add(readData2[0]);
            }
            readData2.Close();
            connection.Close();
            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;
            listBox3.SelectedIndex = 0;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            //SqlConnection connection = new SqlConnection("data source = cypress.csil.sfu.ca; user id = s_hlotey; password = QdJ326P6tH4yjf3f ; initial catalog = hlotey354;");
            SqlConnection connection = new SqlConnection("Data Source = DESKTOP-VK3SP4R\\SQLEXPRESS; Initial Catalog = CMPT354_Bank;" + "Integrated Security = true");
            connection.Open();
            SqlCommand command = new SqlCommand("Select departs from Flight_Instance where flight_code = @code " , connection);
            command.Parameters.Add("@code", SqlDbType.VarChar);
            command.Parameters["@code"].Value = listBox2.Text;
            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                listBox3.Items.Add(read[0]);
            }
            connection.Close();
         }
        //Book Flight
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox2.SelectedIndex == -1 || listBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Please ensure all the fields are selected before you book a flight.", "Warning");
                listBox1.SelectedIndex = 0;
                listBox2.SelectedIndex = 0;
                richTextBox1.Focus();
            }
            else if (richTextBox1.Text.Length != 5)
            {
                MessageBox.Show("Passenger ID doesn't seem to be valid. Please enter valid value.", "Warning");
            }
            else {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox1.Items[i].ToString().Contains(richTextBox1.Text))
                    {
                        SqlConnection connection = new SqlConnection("Data source = DESKTOP-VK3SP4R\\SQLEXPRESS; Initial Catalog = CMPT354_Bank; Integrated Security = true");
                        //SqlConnection connection = new SqlConnection("data source = cypress.csil.sfu.ca; user id = s_hlotey; password = QdJ326P6tH4yjf3f ; initial catalog = hlotey354;");
                        using (SqlCommand cmdInsertData = connection.CreateCommand())
                        {
                            cmdInsertData.CommandText = "INSERT INTO FLIES VALUES(@fCode, @depart, @PID)";
                            cmdInsertData.Parameters.AddWithValue("@fCode", listBox2.Text);
                            cmdInsertData.Parameters.AddWithValue("@depart", Convert.ToDateTime(listBox3.Text));
                            cmdInsertData.Parameters.AddWithValue("@PID", Convert.ToInt32(richTextBox1.Text));
                            try
                            {
                                connection.Open();
                                if (cmdInsertData.ExecuteNonQuery() == 2) // 2 rows upated. 1 with Insert, second with the trigger.
                                {
                                    FlightInfo objectFInfo = new FlightInfo();
                                    objectFInfo.flightCode = listBox2.Text;
                                    listBox2.ClearSelected();
                                    objectFInfo.label11.Text = richTextBox1.Text;
                                    objectFInfo.Show();
                                    Hide();
                                }
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: Flight not Booked. Likely a duplicate flight or insufficient parameters. Please try again. \nServer Message :\n " + ex.Message);
                                richTextBox1.Clear();
                                listBox1.SelectedIndex = 0;
                                listBox2.SelectedIndex = 0;
                                listBox3.SelectedIndex = 0;
                            }
                            finally
                            {
                                errorProvider1.Clear();
                                listBox2.SelectedIndex = 0;
                                listBox3.SelectedIndex = 0;
                                connection.Close();
                                //this.Hide();
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (i == listBox1.Items.Count - 1)
                        {
                            MessageBox.Show("Passenger doesn't exist. Please enter a valid ID.");
                            richTextBox1.Clear();
                            break;
                        }
                    }
                }
            }
            //SqlConnection connection = new SqlConnection("data source = cypress.csil.sfu.ca; user id = s_hlotey; password = QdJ326P6tH4yjf3f ; initial catalog = hlotey354;");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = listBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Exit
            this.Close();
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length != 5)
            {
                errorProvider1.SetError(richTextBox1, "Invalid ID.");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length != 5)
            {
                errorProvider1.SetError(richTextBox1, "Invalid ID.");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
    }
}
