using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;

namespace Hotel_PIS.Services
{
    public class ClientRepository : IClientRepository
    {
        public bool Delete(int id)
        {
            using (var db = new HotelContext())
            {
                try
                {
                    var client = db.Clients.FirstOrDefault(x => x.Id == id);
                    db.Entry(client).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public Client Get(int id)
        {
            using (var db = new HotelContext())
            {
                var client = db.Clients.Where(x => x.Id == id).FirstOrDefault();
                if (client == null)
                    throw new Exception($"Client with id:'{id}' was not found in database.");

                return client;
            }

        }

        public List<Client> GetAll()
        {
            using (var db = new HotelContext())
            {
                return db.Clients.ToList();
            }
        }

        public Client Save(int id, Client obj)
        {
            throw new NotImplementedException();
            Client savedClient;

            if (id == 0) // Create
            {
                savedClient = CreateNewClient(obj);
            }
            else
            {
                savedClient = UpdateClient(obj);
            }



        }
        private Client CreateNewClient(Client client)
        {
            using (var db = new HotelContext())
            {
                db.Clients.Add(client);
                db.SaveChanges();

                return client;
            }
        }

        private Client UpdateClient(Client client)
        {
            using (var db = new HotelContext())
            {
                var dbClient = db.Clients.Where(x => x.Id == client.Id).FirstOrDefault();
                if (dbClient == null)
                    throw new Exception($"Client with id:'{client.Id}' was not found in database.");

                if (dbClient.Equals(client))
                    return client;


                db.SaveChanges();

                return client;
            }
        }
    }
}
