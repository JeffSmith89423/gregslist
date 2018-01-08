using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using gregslist.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace gregslist.Repositories
{
    public class AnimalRepository
    {
        private readonly IDbConnection _db;

        public AnimalRepository(IDbConnection db)
        {
            _db = db;
        }

        // Find One Find Many add update delete
        public IEnumerable<Animal> GetAll()
        {
            return _db.Query<Animal>("SELECT * FROM Animals");
        }

        public Animal GetById(int id)
        {
            return _db.QueryFirstOrDefault<Animal>($"SELECT * FROM Animals WHERE id = @id", id);
        }

        public Animal Add(Animal animal)
        {

            int id = _db.ExecuteScalar<int>("INSERT INTO Animals (Name, Description, Price)"
                        + " VALUES(@Name, @Description, @Price); SELECT LAST_INSERT_ID()", new
                        {
                            animal.Type,
                            animal.Age,
                            animal.Price,
                            animal.Color
                        });
            animal.Id = id;
            return animal;
 
        }

        public Animal GetOneByIdAndUpdate(int id, Animal animal)
        {
            return _db.QueryFirstOrDefault<Animal>($@"
                UPDATE Animals SET  
                    Type = @Type,
                    Age = @Age,
                    Price = @Price,
                    Color = @Color
                WHERE Id = {id};
                SELECT * FROM Animals WHERE id = {id};", animal);
        }

        public string FindByIdAndRemove(int id)
        {
            var success = _db.Execute(@"
                DELETE FROM Animals WHERE Id = @id
            ", id);
            return success > 0 ? "success" : "umm that didnt work";
        }
    }
}
