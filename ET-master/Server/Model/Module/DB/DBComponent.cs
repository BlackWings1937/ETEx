using MongoDB.Driver;

namespace ETModel
{
	[ObjectSystem]
	public class DbComponentSystem : AwakeSystem<DBComponent>
	{
		public override void Awake(DBComponent self)
		{
			self.Awake();
		}
	}

	/// <summary>
	/// 连接mongodb
	/// </summary>
	public class DBComponent : Component
	{
		public MongoClient mongoClient;
		public IMongoDatabase database;

		public void Awake()
		{ 
            /*
             * 连接mongoDb
             * StartConfig connectionString
             * DBName databasename
             */
			//DBConfig config = Game.Scene.GetComponent<StartConfigComponent>().StartConfig.GetComponent<DBConfig>();

			//string connectionString = config.ConnectionString;
			//mongoClient = new MongoClient(connectionString);

			//this.database = this.mongoClient.GetDatabase(config.DBName);
		}

		public IMongoCollection<ComponentWithId> GetCollection(string name)
		{
			return this.database.GetCollection<ComponentWithId>(name);
		}
	}
}