using MarshalsExceptions;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Data;

namespace Bank
{
    /// <summary>
    /// Repository for DB using.
    /// </summary>
    public class RepositoryForDB
    {
        /// <summary>
        /// Saving the entity to the DB.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception"></exception>
        public E SaveEntityToDB<E>(E entity, string dbConnectionName) where E : class
        {
            try
            {
                E newEntity = PrepareAndExecuteQuery(entity, dbConnectionName, OperationType.Save)[0];
                return newEntity;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// The method for saving all entities data to the DB
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="scores"></param>
        /// <exception cref="Exception"></exception>
        public void SaveAllDataToDB<E>(List<E> entities, string dbConnectionName) where E : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    PrepareAndExecuteQuery<E>(entity, dbConnectionName, OperationType.Save);
                }
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// Update entity into the DB.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="entity"></param>
        /// <param name="dbConnectionName"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public E UpdateEntityIntoDB<E>(E entity, string dbConnectionName) where E : class
        {
            try
            {
                E newEntity = PrepareAndExecuteQuery(entity, dbConnectionName, OperationType.Update)[0];
                return newEntity;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// The method for updating all entities in the DB.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="scores"></param>
        /// <exception cref="DBException"></exception>
        public void UpdateEntitiesIntoDB<E>(List<E> entities, string dbConnectionName) where E : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    PrepareAndExecuteQuery(entity, dbConnectionName, OperationType.Update);
                }
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// Load entity by id from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public E FindEntityByIdFromDB<E>(long id, string dbConnectionName) where E : class
        {
            try
            {
                E newEntity = PrepareAndExecuteQuery<E>(null, dbConnectionName, OperationType.FindById, id)[0];
                return newEntity;
            }
            catch
            {
                throw new DBException("Find in DB exception");
            }
        }

        /// <summary>
        /// Find entities with concret type from db.
        /// </summary>
        /// <typeparam name="E">Type of entity</typeparam>
        /// <param name="dbConnectionName">Name connection</param>
        /// <returns>List entities</returns>
        /// <exception cref="DBException"></exception>
        public List<E> FindEntitiesFromDB<E>(string dbConnectionName) where E : class
        {
            try
            {
                List<E> entities = PrepareAndExecuteQuery<E>(null, dbConnectionName, OperationType.FindAll);
                return entities;
            }
            catch (Exception ex)
            {
                throw new DBException("Find in DB exception", ex);
            }
        }

        private List<E> PrepareAndExecuteQuery<E>(E entity, string dbConnectionName, OperationType typeQuery, long id = 0) where E : class
        {
            ConnectionToDb connection = new ConnectionToDb();

            connection.Connect(dbConnectionName);

            List<E> objects = new List<E>();

            if (connection.GetConnection() != null)
            {
                using (DbMyEntitiesContext dbContext = connection.GetConnection())
                {
                    switch (typeQuery)
                    {
                        case OperationType.Save:
                            dbContext.Set<E>().Add(entity);
                            dbContext.SaveChanges();
                            objects.Add(entity);
                            break;

                        case OperationType.Update:
                            UpdateEntity(dbContext, entity);
                            break;

                        case OperationType.FindAll:
                            objects.AddRange(FindAllEntities<E>(dbContext));
                            break;
                        case OperationType.FindById:
                            FindEntityById<E>(id, dbContext);
                            break;
                    }
                    dbContext.SaveChanges();
                }
            }

            return objects;
        }

        private E FindEntityById<E>(long id, DbMyEntitiesContext dbContext) where E : class
        {
            object result = null;

            if (typeof(E) == typeof(Client))
            {
                result = dbContext.Clients
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
            }
            else if (typeof(E) == typeof(Score))
            {
                result = dbContext.Scores
                     .Where(s => s.Id == id)
                     .FirstOrDefault();
            }

            return result as E;
        }

        private E UpdateEntity<E>(DbMyEntitiesContext dbContext, E entity) where E : class
        {
            if (typeof(E) == typeof(Client))
            {
                return UpdateClient(dbContext, entity as Client) as E;
            }
            else if (typeof(E) == typeof(Score))
            {
                return UpdateScore(dbContext, entity as Score) as E;
            }
            else
            {
                return null;
            }
        }

        private Client UpdateClient(DbMyEntitiesContext dbContext, Client client)
        {
            Client existingClient = dbContext.Clients.Find(client.Id);
            Client newClient = null;

            if (existingClient != null)
            {
                existingClient.FirstName = client.FirstName;
                existingClient.LastName = client.LastName;
                existingClient.History = client.History;
                existingClient.Prestige = client.Prestige;
                existingClient.Status = client.Status;

                dbContext.Clients.Update(existingClient);

                newClient = client;
            }
            else
            {
                dbContext.Clients.Add(client);

                //dbContext.SaveChanges();

                newClient = client;
            }
            return newClient;
        }

        private Score UpdateScore(DbMyEntitiesContext dbContext, Score score)
        {
            Score existingScore = dbContext.Scores.FirstOrDefault(s => s.Id == score.Id);
            Score newScore = null;

            if (existingScore != null)
            {
                existingScore.Balance = score.Balance;
                existingScore.Percent = score.Percent;
                existingScore.DateScore = score.DateScore;
                existingScore.IsCapitalization = score.IsCapitalization;
                existingScore.IsMoney = score.IsMoney;
                existingScore.Deadline = score.Deadline;
                existingScore.DateLastDividends = score.DateLastDividends;
                //existingScore.Client.Id = score.Id;
                existingScore.ScoreType = score.ScoreType;
                existingScore.IsActive = score.IsActive;

                dbContext.Scores.Update(existingScore);

                newScore = score;
            }
            else
            {
                dbContext.Scores.Add(score);

                newScore = score;

                //dbContext.SaveChanges();
            }
            return newScore;
        }

        private List<E> FindAllEntities<E>(DbMyEntitiesContext dbContext) where E : class
        {
            List<E> objects = new List<E>();
            if (typeof(E) == typeof(Client))
            {
                objects = dbContext.Clients.Include(c => c.Scores).OfType<E>().ToList();
            }
            else if (typeof(E) == typeof(Score))
            {
                objects = dbContext.Scores.OfType<E>().ToList();
            }

            return objects;
        }

        private enum OperationType
        {
            Save,
            Update,
            FindAll,
            FindById
        }

    }
}