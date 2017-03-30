using System;
using System.Data;
using System.Data.SqlClient; 
using System.Windows.Forms;

namespace AirlinesCMPT354
{
    public partial class FlightInfo : Form
    {
        public static DataSet ds;
        public static SqlDataAdapter adapter;
        public static SqlConnection connection;
        public static string PassengerId;
        public string flightCode;
        public FlightInfo()
        {
            InitializeComponent();
        }
        private void activateConnectionForPassenger()
        {
            //CSIL Lab//
            //connection = new SqlConnection("data source = cypress.csil.sfu.ca; user id = s_hlotey; password = QdJ326P6tH4yjf3f ; initial catalog = hlotey354;");
            //Local server
            connection = new SqlConnection("Data Source = DESKTOP-VK3SP4R\\SQLEXPRESS; Initial Catalog = CMPT354_Bank; Integrated Security = true");
            //adapter = new SqlDataAdapter("Select distinct P.passenger_id, P.first_name, P.last_name, P.miles, fly.flight_code, fly.departs, fl.arrival_iata, fl.departure_iata, fl.distance from Passenger P, Flies Fly, Flight_Instance f_Inst, Flight Fl where p.passenger_id = Fly.passenger_id and P.passenger_id = @p_id and fly.flight_code = f_Inst.flight_code and f_Inst.flight_code = fl.flight_code", connection);
            adapter = new SqlDataAdapter("Select distinct P.passenger_id, P.first_name, P.last_name, P.miles, fly.flight_code, fly.departs, Arr.airport_name as ArrAirport, Dep.airport_name as DepAirport, fl.distance from Passenger P, Flies Fly, Flight_Instance f_Inst, Flight Fl, Airport Arr, Airport Dep where p.passenger_id = Fly.passenger_id and P.passenger_id = @p_id and fly.flight_code = f_Inst.flight_code and f_Inst.flight_code = fl.flight_code and arr.iata = Fl.arrival_iata and dep.iata = Fl.departure_iata", connection);
            adapter.SelectCommand.Parameters.Add("@p_id", SqlDbType.Int);
            adapter.SelectCommand.Parameters["@p_id"].Value = Convert.ToInt32(label11.Text);

            SqlCommandBuilder commandBuild = new SqlCommandBuilder(adapter);
            ds = new DataSet();
            adapter.Fill(ds);

            label12.Text = ds.Tables[0].Rows[0].Field<string>("first_name").ToString();
            label13.Text = ds.Tables[0].Rows[0].Field<string>("last_name").ToString();
            label14.Text = ds.Tables[0].Rows[0].Field<int>("miles").ToString();
            label15.Text = ds.Tables[0].Rows[0].Field<string>("flight_code").ToString();
            label16.Text = ds.Tables[0].Rows[0].Field<string>("DepAirport").ToString();     
            label17.Text = ds.Tables[0].Rows[0].Field<DateTime>("departs").ToString();
            label18.Text = ds.Tables[0].Rows[0].Field<string>("ArrAirport").ToString();     
            label19.Text = ds.Tables[0].Rows[0].Field<int>("distance").ToString();
        }
        
        public string getPID;
        private void FlightInfo_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            activateConnectionForPassenger();            
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.ReadOnly = true;
            listBox1.DataSource =  ds.Tables[0];
            listBox1.DisplayMember = "flight_code";
            listBox1.Focus();
            listBox1.Text = flightCode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                label15.Text = ds.Tables[0].Rows[listBox1.SelectedIndex].Field<string>("flight_code").ToString();
                label16.Text = ds.Tables[0].Rows[listBox1.SelectedIndex].Field<string>("DepAirport").ToString();
                label17.Text = ds.Tables[0].Rows[listBox1.SelectedIndex].Field<DateTime>("departs").ToString();
                label18.Text = ds.Tables[0].Rows[listBox1.SelectedIndex].Field<string>("ArrAirport").ToString();
                label19.Text = ds.Tables[0].Rows[listBox1.SelectedIndex].Field<int>("distance").ToString();
            }
            catch { }
            finally { connection.Close(); }
            }
        }
    }

