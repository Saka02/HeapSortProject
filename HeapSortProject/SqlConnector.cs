using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HeapSortProject
{
    internal class SqlConnector
    {

        private static OleDbConnection connection = new OleDbConnection();
        private OleDbCommand command;

        public static OleDbConnection Connection { get => connection; set => connection = value; }

        public SqlConnector(string path)
        {
            Connection.ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={path}Heap.accdb; Persist Security Info=False;";
            command = Connection.CreateCommand();
        }

        public void Write(string nodeKaoString, string arrayName, string Time)
        {
            try
            {
                Connection.Open();
                string sql = $"INSERT INTO Array (ArrayName, Nodes, [Time]) " + // Napravi tabelu Array koja ima ArrayName, Nodes i Time polja nek su ista ovakva imena <-------------------------------------
                    $"VALUES('{arrayName}', '{nodeKaoString}', '{Time}');";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                MessageBox.Show($"Array {arrayName} has been successfully Saved");

                Connection.Close();
            }
            catch (OleDbException ex)
            {
                if (Connection != null)
                    Connection.Close();
                MessageBox.Show($"Error Write()\n{ex}");
            }
        }

        public DataTable ReadArrays()
        {
            DataTable dt = new DataTable();
            bool i = Environment.Is64BitProcess; // If this is true it wont work, in configuration menager process must be set to x86
            try
            {
                Connection.Open();
                string sql = "SELECT ArrayName FROM Array";
                command.CommandText = sql;
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(dt);
                Connection.Close();
            }
            catch (Exception)
            {
                if (Connection != null)
                    Connection.Close();
                MessageBox.Show("Error ReadArrays()");
            }

            return dt;
        }

        public DataTable ReadArrayElements(string name)
        {
            DataTable dt = new DataTable();
            bool i = Environment.Is64BitProcess; // If this is true it wont work, in configuration menager process must be set to x86
            try
            {
                Connection.Open();
                string sql = $"SELECT * FROM Array WHERE ArrayName = '{name}'";
                command.CommandText = sql;
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(dt);
                Connection.Close();
            }
            catch (Exception)
            {
                if (Connection != null)
                    Connection.Close();
                MessageBox.Show("Error ReadArrayElements()");
            }

            return dt;
        }

        public void Delete(string arrayName)
        {
            try
            {
                Connection.Open();

                string sql = $"DELETE * " +
                    $"FROM Array WHERE ArrayName = '{arrayName}'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                Connection.Close();
            }
            catch (OleDbException ex)
            {
                if (Connection != null)
                    Connection.Close();
                MessageBox.Show($"Error Delete()\n{ex}");
            }
        }

        public void Edit(string nodeKaoString, string arrayName, string Time)
        {
            try
            {
                Connection.Open();
                string sql = $" UPDATE Array " +
                    $"SET [Time] = '{Time}', Nodes = '{nodeKaoString}' " +
                    $"WHERE(ArrayName = '{arrayName}')";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                MessageBox.Show($"Array {arrayName} has been successfully Updated");

                Connection.Close();
            }
            catch (OleDbException ex)
            {
                if (Connection != null)
                    Connection.Close();
                MessageBox.Show($"Error Edit()\n{ex}");
            }
        }

    }
}
