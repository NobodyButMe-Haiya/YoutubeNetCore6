using MySql.Data.MySqlClient;
using YoutubeNetCore6.Models;

namespace YoutubeNetCore6.Repository
{
    public class PersonRepository
    {
        private string ConnectionString { get; set; }
        public PersonRepository()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            ConnectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }
        public int Create(string name, int age)
        {
            // okay next we create skeleton for the code
            var lastInsertId = 0;

            using MySqlConnection connection = new(ConnectionString);
            try
            {

                connection.Open();

                MySqlTransaction mySqlTransaction = connection.BeginTransaction();

                string SQL = "INSERT INTO person VALUES (null,@name,@age);";
                MySqlCommand mySqlCommand = new(SQL, connection);
                // we add some parameter
                mySqlCommand.Parameters.AddWithValue("@name", name);
                mySqlCommand.Parameters.AddWithValue("@age", age);
                mySqlCommand.ExecuteNonQuery();
                // we have error here .. 
                mySqlTransaction.Commit();

                lastInsertId = (int)mySqlCommand.LastInsertedId;

                // c;ear some memory 
                mySqlCommand.Dispose();

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                /// you may throw new exception here or use any log debugger to text file 
            }

            return lastInsertId;


        }
        public List<PersonModel> Read()
        {
            List<PersonModel> personModels = new();

            // for reading we don't need transaction ya .. 


            using (MySqlConnection connection = new(ConnectionString))
            {
                try
                {

                    connection.Open();
                    string SQL = "SELECT * FROM person ORDER BY personId DESC ";
                    MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);
                    using (var reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // it all depend , for ease we just send all data as string it all depend on web api to translate to "" or number only 
                            personModels.Add(new PersonModel()
                            {

                                Name = reader["name"].ToString(),
                                Age = Convert.ToInt32(reader["age"]),
                                PersonId = Convert.ToInt32(reader["personId"])
                            });
                        }
                    }


                    // c;ear some memory 
                    mySqlCommand.Dispose();

                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    /// you may throw new exception here or use any log debugger to text file 
                }
            }


            return personModels;
        }
        public List<PersonModel> Search(string search)
        {
            List<PersonModel> personModels = new();

            // for reading we don't need transaction ya .. 


            using (MySqlConnection connection = new(ConnectionString))
            {
                try
                {

                    connection.Open();
                    string SQL = "SELECT * FROM person  WHERE name like concat('%',@search,'%') or  age like concat('%',@search,'%'); ";
                    MySqlCommand mySqlCommand = new(SQL, connection);
                    mySqlCommand.Parameters.AddWithValue("@search", search);
                    using (var reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // it all depend , for ease we just send all data as string it all depend on web api to translate to "" or number only 
                            personModels.Add(new PersonModel()
                            {

                                Name = reader["name"].ToString(),
                                Age = Convert.ToInt32(reader["age"]),
                                PersonId = Convert.ToInt32(reader["personId"])
                            });
                        }
                    }


                    // c;ear some memory 
                    mySqlCommand.Dispose();

                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    /// you may throw new exception here or use any log debugger to text file 
                }
            }


            return personModels;
        }
        public void Update(string name, int age, int personId)
        {
            using MySqlConnection connection = new(ConnectionString);
            try
            {

                connection.Open();
                MySqlTransaction mySqlTransaction = connection.BeginTransaction();

                string SQL = "UPDATE person SET name = @name, age = @age WHERE personId = @personId ";
                MySqlCommand mySqlCommand = new(SQL, connection);
                // we add some parameter
                mySqlCommand.Parameters.AddWithValue("@name", name);
                mySqlCommand.Parameters.AddWithValue("@age", age);
                mySqlCommand.Parameters.AddWithValue("@personId", personId);

                mySqlCommand.ExecuteNonQuery();

                mySqlTransaction.Commit();
                // c;ear some memory 
                mySqlCommand.Dispose();

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                /// you may throw new exception here or use any log debugger to text file 
            }
        }
        public void Delete(int personId)
        {
            // the old using is using(xxx) { }  now it's weirder 
            using MySqlConnection connection = new MySqlConnection(ConnectionString);
            try
            {

                connection.Open();
                // sorry we tend to forget sometimes..  but with error like this you all can remember what's error if failure not all working  fine :P
                MySqlTransaction mySqlTransaction = connection.BeginTransaction();

                string SQL = "DELETE FROM person WHERE personId  = @personId;";
                MySqlCommand mySqlCommand = new(SQL, connection);
                // we add some parameter
                mySqlCommand.Parameters.AddWithValue("@personId", personId);
                mySqlCommand.ExecuteNonQuery();

                mySqlTransaction.Commit();
                // c;ear some memory 
                mySqlCommand.Dispose();

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                /// you may throw new exception here or use any log debugger to text file 
            }
        }
    }
}
