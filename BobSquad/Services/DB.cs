using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobSquad.Services
{
    static class DB
    {
        private static SqlConnection _connection = null;

        public static SqlConnection GetConnection()
        {
            try
            {
                if (_connection != null)
                    return _connection;

                _connection =
                    new SqlConnection(
                        "Server=tcp:bobsquad.database.windows.net,1433;Initial Catalog=BobSquad;Persist Security Info=False;User ID=bobsquad;Password=AdminAdmin1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                _connection.Open();

                return _connection;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataTable RunSelectCommand(string command, List<SqlParameter> parameters = null)
        {
            GetConnection();
            if (command == "")
                return null;

            if (_connection == null)
                return null;

            SqlCommand _command = new SqlCommand(command, _connection);

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    _command.Parameters.Add(parameter);
                }
            }

            DataTable _table = new DataTable();

            try
            {
                SqlDataReader _reader = _command.ExecuteReader();
                _table.Load(_reader);
                return _table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void RunCommand(string command, List<SqlParameter> parameters = null)
        {
            GetConnection();
            if (command == "")
                return;

            if (_connection == null)
                return;

            SqlCommand _command = new SqlCommand(command, _connection);

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    _command.Parameters.Add(parameter);
                }
            }

            try
            {
                _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
