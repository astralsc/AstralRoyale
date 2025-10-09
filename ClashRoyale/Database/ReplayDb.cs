using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClashRoyale.Core;
using ClashRoyale.Logic;
using ClashRoyale.Logic.Sessions;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SharpRaven.Data;

namespace ClashRoyale.Database
{
    public class ReplayDb
    {
        private static string _connectionString;
        private static long _replaySeed;

        public static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None
        };

        public ReplayDb()
        {
            _connectionString = new MySqlConnectionStringBuilder
            {
                Server = Resources.Configuration.MySqlServer,
                Database = Resources.Configuration.MySqlDatabase,
                UserID = Resources.Configuration.MySqlUserId,
                Password = Resources.Configuration.MySqlPassword,
                SslMode = MySqlSslMode.None,
                MinimumPoolSize = 4,
                MaximumPoolSize = 20
            }.ToString();

            _replaySeed = MaxReplayId();
        }

        public static async Task<long> CountAsync()
        {
            #region CountAsync

            try
            {
                long seed;

                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM replay", connection))
                    {
                        seed = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                    }

                    await connection.CloseAsync();
                }

                return seed;
            }
            catch (Exception exception)
            {
                Logger.Log(exception, null);

                return 0;
            }

            #endregion
        }

        public static async Task ExecuteAsync(MySqlCommand cmd)
        {
            try
            {
                cmd.Connection = new MySqlConnection(_connectionString);
                await cmd.Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (MySqlException exception)
            {
                Logger.Log(exception, null);
            }
            finally
            {
                cmd.Connection?.Close();
            }
        }

        public static long MaxReplayId()
        {
            #region MaxReplayId

            try
            {
                long seed;

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand("SELECT coalesce(MAX(Id), 0) FROM replay", connection))
                    {
                        seed = Convert.ToInt64(cmd.ExecuteScalar());
                    }

                    connection.Close();
                }

                return seed;
            }
            catch (Exception exception)
            {
                Logger.Log(exception, null);

                return -1;
            }

            #endregion
        }

        public static async Task<string> GetAsync(long id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string data = null;

                    using (var cmd = new MySqlCommand($"SELECT * FROM replay WHERE Id = '{id}'", connection))
                    {
                        var reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                            data = reader["Data"].ToString();
                    }

                    await connection.CloseAsync();

                    return data;
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception, null);

                return null;
            }
        }

        public static async Task<string> GetRandom()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string data = null;

                    using (var cmd = new MySqlCommand("SELECT * FROM replay ORDER BY RAND() LIMIT 1", connection))
                    {
                        var reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                            data = reader["Data"].ToString();
                    }

                    await connection.CloseAsync();

                    return data;
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception, null);

                return null;
            }
        }

        public static async Task<long> SaveAsync(string battle)
        {
            try
            {
                var id = _replaySeed++;
                if (id <= -1)
                    return -1;

                using (var cmd = new MySqlCommand(
                    $"INSERT INTO replay (`Id`, `Data`) VALUES ({id + 1}, @data)")
                )
                {
                    cmd.Parameters.AddWithValue("@data", battle);

                    await ExecuteAsync(cmd);

                    return id + 1;
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception, null);
                return -1;
            }
        }

        public static async Task DeleteAsync(long id)
        {
            try
            {
                using (var cmd = new MySqlCommand(
                    $"DELETE FROM replay WHERE Id = '{id}'")
                )
                {
                    await ExecuteAsync(cmd);
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception, null);
            }
        }
    }
}