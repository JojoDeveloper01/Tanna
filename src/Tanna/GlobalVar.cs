﻿using System.Data.SQLite;
using System.Data;

namespace Tanna
{
    public static class GlobalVar
    {
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static int ID { get; set; }
        public static int Type { get; set; }
        public static string SelectedWorldName { get; set; }
        public static string SelectedFBName { get; set; }
        public static List<string> SelectedEnemiesName { get; set; } = new List<string>();
        public static List<int> SelectedEnemiesIds = new List<int>();
        public static void Logout(Home homeForm)
        {
            // Redefinir as variáveis globais para o estado de deslogado
            Username = null;
            Password = null;
            ID = 0;
            Type = 0;
        }
        public static void LoadData(string tableName, DataGridView dataGridView)
        {
            string sql;

            switch (tableName)
            {
                case "Game":
                    sql = $@"SELECT 
                g.name,
                w.name as World,
                f.name as 'FinalBoss'
            FROM 
                Game g
            JOIN 
                World w ON g.world_id = w.id
            JOIN 
                FinalBoss f ON g.finalBoss_id = f.id
            WHERE 
                g.player_id = {GlobalVar.ID}";
                    break;
                case "World":
                    sql = $"SELECT name, size, duration FROM World WHERE player_id = {GlobalVar.ID}";
                    break;
                case "FinalBoss":
                    sql = $"SELECT name, life, stamina, velocity, energy FROM FinalBoss WHERE player_id = {GlobalVar.ID}";
                    break;
                case "Enemies":
                    sql = $"SELECT name, amount, life FROM Enemies WHERE player_id = {GlobalVar.ID}";
                    break;
                case "SelectedGames":
                    ShowSelectedDataVertical(dataGridView);
                    return; // Retorna para evitar execução do restante do código
                default:
                    MessageBox.Show("Invalid table name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            try
            {
                using (var cmd = new SQLiteCommand(sql, Program.conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao carregar os dados da tabela {tableName}: " + ex.Message);
            }
        }

        public static void ShowSelectedDataVertical(DataGridView dataGridView)
        {
            // Limpa o DataGridView antes de adicionar novos itens
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            // Adiciona as colunas ao DataGridView
            dataGridView.Columns.Add("Attribute", "Attribute");
            dataGridView.Columns.Add("Value", "Value");


            dataGridView.Rows.Add("World", GlobalVar.SelectedWorldName);
            dataGridView.Rows.Add("Final Boss", GlobalVar.SelectedFBName);
            string selectedEnemies = string.Join(", ", GlobalVar.SelectedEnemiesName);
            dataGridView.Rows.Add("Enemies", selectedEnemies);

            // Ajusta a largura das colunas para caber no conteúdo
            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static bool Create(string tableName, Dictionary<string, string> columns, int playerId)
        {
            try
            {
                // Adiciona o campo player_id ao dicionário de colunas
                columns.Add("player_id", playerId.ToString());

                var columnNames = string.Join(", ", columns.Keys);
                var columnValues = string.Join(", ", columns.Values.Select(v => $"'{v}'"));
                string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({columnValues})";

                using (var cmd = new SQLiteCommand(query, Program.conn))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating {tableName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static int GetIdByName(string tableName, string name)
        {
            const string sqlTemplate = "SELECT id FROM {0} WHERE name = @name";
            string sql = string.Format(sqlTemplate, tableName);

            using (var cmd = new SQLiteCommand(sql, Program.conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static bool Delete(string name, string column)
        {
            try
            {
                // Verificar se o nome de usuário existe
                var sqlCheck = $"SELECT COUNT(*) FROM {column} WHERE name = @name";
                using (var cmdCheck = new SQLiteCommand(sqlCheck, Program.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@name", name);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Username does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                // Deletar o jogador
                var sql = $"DELETE FROM {column} WHERE name = @name";
                using (var cmd = new SQLiteCommand(sql, Program.conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("SQLite Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool IsNameAlreadyExists(string tableName, string name)
        {
            const string sqlTemplate = "SELECT COUNT(*) FROM {0} WHERE name = @name";
            string sql = string.Format(sqlTemplate, tableName);

            using (var cmd = new SQLiteCommand(sql, Program.conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public static int GetLastInsertId(string tableName)
        {
            const string sqlTemplate = "SELECT seq FROM sqlite_sequence WHERE name = @tableName";
            using (var cmd = new SQLiteCommand(sqlTemplate, Program.conn))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
    }
}
