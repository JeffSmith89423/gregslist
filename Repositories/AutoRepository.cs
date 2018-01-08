using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using gregslist.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace gregslist.Repositories
{
    public class AutoRepository
    {
        private readonly IDbConnection _db;

        public AutoRepository(IDbConnection db)
        {
            _db = db;
        }

        // Find One Find Many add update delete
        public IEnumerable<Auto> GetAll()
        {
            return _db.Query<Auto>("SELECT * FROM Autos");
        }

        public Auto GetById(int id)
        {
            return _db.QueryFirstOrDefault<Auto>($"SELECT * FROM Autos WHERE id = @id", id);
        }

        public Auto Add(Auto auto)
        {

            int id = _db.ExecuteScalar<int>("INSERT INTO Autos (Make, Year, Price)"
                        + " VALUES(@Make, @Year, @Price); SELECT LAST_INSERT_ID()", new
                        {
                            auto.Make,
                            auto.Year,
                            auto.Price,
                        });
            auto.Id = id;
            return auto;
 
        }

        public Auto GetOneByIdAndUpdate(int id, Auto auto)
        {
            return _db.QueryFirstOrDefault<Auto>($@"
                UPDATE Autos SET  
                    Make = @Make,
                    Year = @Year,
                    Price = @Price,
                WHERE Id = {id};
                SELECT * FROM Autos WHERE id = {id};", auto);
        }

        public string FindByIdAndRemove(int id)
        {
            var success = _db.Execute(@"
                DELETE FROM Autos WHERE Id = @id
            ", id);
            return success > 0 ? "success" : "umm that didnt work";
        }
    }
}
