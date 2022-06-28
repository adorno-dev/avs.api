using AVS.API.Models;
using AVS.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AVS.API.Repositories
{
    public class UserRepository
    {
        #region +Private Methods

        private readonly IMongoCollection<User> users;        

        #endregion

        #region +Constructors
        
        public UserRepository(IOptions<DatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            users = client
                .GetDatabase(options.Value.DatabaseName)
                .GetCollection<User>(options.Value.UsersCollectionName);
            
            this.LoadPeople();
        }

        #endregion

        #region +Private Methods

        private void LoadPeople()
        {
            if (!users.Find(_ => true).Any())
            {
                users.InsertMany(new List<User> {
                    new User("Aaron","aaron@twd.com","aaron"),
                    new User("Abraham Ford","abraham.ford@twd.com","abraham"),
                    new User("Alden","alden@twd.com","alden"),
                    new User("Alpha","alpha@twd.com","alpha"),
                    new User("Andrea","andrea@twd.com","andrea"),
                    new User("Jadis","jadis@twd.com","jadis"),
                    new User("Beta","beta@twd.com","beta"),
                    new User("Beth Greene","beth.greene@twd.com","beth"),
                    new User("Bob Stookey","bob.stookey@twd.com","bob"),
                    new User("Carl Grimes","carl.grimes@twd.com","carl"),
                    new User("Carol Peletier","carol.peletier@twd.com","carol"),
                    new User("Connie","connie@twd.com","connie"),
                    new User("Dale Horvath","dale.horvath@twd.com","dale"),
                    new User("Daryl Dixon","daryl.dixon@twd.com","daryl"),
                    new User("Deanna Monroe","deanna.monroe@twd.com","deanna"),
                    new User("Dwight","dwight@twd.com","dwight"),
                    new User("Enid","enid@twd.com","enid"),
                    new User("Eugene Porter","eugene.porter@twd.com","eugene"),
                    new User("Ezekiel","ezekiel@twd.com","ezekiel"),
                    new User("Gabriel Stokes","gabriel.stokes@twd.com","gabriel"),
                    new User("Gareth","gareth@twd.com","gareth"),
                    new User("Glenn Rhee","glenn.rhee@twd.com","glenn"),
                    new User("Gregory","gregory@twd.com","gregory"),
                    new User("Hershel Greene","hershel.greene@twd.com","hershel"),
                    new User("Jerry","jerry@twd.com","jerry"),
                    new User("Jessie Anderson","jessie.anderson@twd.com","jessie"),
                    new User("Juanita Sanchez","juanita.sanchez@twd.com","juanita"),
                    new User("Judith Grimes","judith.grimes@twd.com","judith"),
                    new User("Lance Hornsby","lance.hornsby@twd.com","lance"),
                    new User("Leah Shaw","leah.shaw@twd.com","leah"),
                    new User("Lori Grimes","lori.grimes@twd.com","lori"),
                    new User("Lydia","lydia@twd.com","lydia"),
                    new User("Maggie Greene","maggie.greene@twd.com","maggie"),
                    new User("Magna","magna@twd.com","magna"),
                    new User("Mercer","mercer@twd.com","mercer"),
                    new User("Merle Dixon","merle.dixon@twd.com","merle"),
                    new User("Michonne","michonne@twd.com","michonne"),
                    new User("Morgan Jones","morgan.jones@twd.com","morgan"),
                    new User("Negan Smith","negan.smith@twd.com","negan"),
                    new User("Paul Rovia","paul.rovia@twd.com","paul"),
                    new User("Rick Grimes","rick.grimes@twd.com","rick"),
                    new User("Rosita Espinosa","rosita.espinosa@twd.com","rosita"),
                    new User("Sasha Williams","sasha.williams@twd.com","sasha"),
                    new User("Shane Walsh","shane.walsh@twd.com","shane"),
                    new User("Siddiq","siddiq@twd.com","siddiq"),
                    new User("Simon","simon@twd.com","simon"),
                    new User("Spencer Monroe","spencer.monroe@twd.com","spencer"),
                    new User("Stephanie Vega","stephanie.vega@twd.com","stephanie"),
                    new User("Tara Chambler","tara.chambler@twd.com","tara"),
                    new User("Tyreese Williams","tyreese.williams@twd.com","tyreese"),
                    new User("Yumiko ","yumiko.@twd.com","yumiko ")
                });
            }
        }

        #endregion

        #region +Public Methods

        // public List<User>? GetAll() => users
        //     .Find(_ => true)
        //     .Project(s => new User(s.Username, s.Email, null) { Id = s.Id, Creation = null })
        //     .ToList();

        public List<MongoDB.Bson.BsonDocument>? GetAll() => users
            .Find(_ => true)
            .Project(s => new MongoDB.Bson.BsonDocument { {"Id", s.Id}, {"Username", s.Username}})
            .ToList();

        public User? Get(string email) => users.Find(x => x.Email == email).FirstOrDefault();

        public void Create(User user) => users.InsertOne(user);

        #endregion

        #region +Extension Methods

        public void Get(string email, out User? user) => user = Get(email);

        #endregion
    }
}