using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class CollabBl : ICollabBl
    {
        private readonly ICollabRl collabRl;
        public CollabBl(ICollabRl collabRl)
        {
            this.collabRl = collabRl;
        }
        public CollabEntity AddCollab(CollabModel collabModel)
        {
            try
            {
                return collabRl.AddCollab(collabModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string RemoveCollab(long collabId, long userId)
        {
            try
            {
                return collabRl.RemoveCollab(collabId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CollabEntity> GetCollab(long noteId, long userId)
        {
            try
            {
                return collabRl.GetCollab(noteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
