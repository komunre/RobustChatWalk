using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.Log;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using Content.Shared.Items;

namespace Content.Server.Database
{
    class ServerDbSqlite
    {
        SqliteConnection conn;

        public void Initialize()
        {
            conn = new SqliteConnection("Data Source=gameData.db");
            conn.Open();
        }

        public SqliteCommand GetCommand(string sql)
        {
            var command = conn.CreateCommand();
            command.CommandText = sql;
            return command;
        }

        public void CreatePlayer(string name, string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                var hash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var cmd = GetCommand("INSERT INTO users (username, passw) VALUES($name, $pass)");
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$pass", hash);
                cmd.ExecuteNonQuery();

                var wealth = GetCommand("INSERT INTO wealth (username, counter) VALUES($name, 0)");
                wealth.Parameters.AddWithValue("$name", name);
                wealth.ExecuteNonQuery();

                var equipment = GetCommand("INSERT INTO euipment (username, equip) VALUES($name, 'magic_wand'");
                equipment.Parameters.AddWithValue("$name", name);
                equipment.ExecuteNonQuery();
            }
        }

        public bool RequirePlayer(string name, string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                var hash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var cmd = GetCommand("SELECT username FROM users WHERE username=$name AND passw=$pass");
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$pass", hash);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public void SaveWealth(string name, int amount)
        {
            var cmd = GetCommand("UPDATE wealth SET counter=$amount WHERE username=$name");
            cmd.Parameters.AddWithValue("$name", name);
            cmd.Parameters.AddWithValue("$amount", amount);

            cmd.ExecuteNonQuery();
        }

        public int GetWealth(string name)
        {
            var cmd = GetCommand("SELECT counter FROM wealth WHERE username=$name");
            cmd.Parameters.AddWithValue("$name", name);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                return 0;
            }
        }

        public string[] GetEquipment(string name)
        {
            var cmd = GetCommand("SELECT equip FROM equipment WHERE username=$name");
            cmd.Parameters.AddWithValue("$name", name);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetString(0).Split(",");
                }
                return null;
            }
        }

        public void SaveEquipment(List<EquipmentComponent> equipment)
        {
            var result = "";
            foreach(var eq in equipment)
            {
                result += eq.Owner.Prototype + ",";
            }

            var cmd = GetCommand("UPDATE equipment SET equip=$eq WHERE username=$name");
            cmd.Parameters.AddWithValue("$eq", result);
            cmd.ExecuteNonQuery();
        }
    }
}
