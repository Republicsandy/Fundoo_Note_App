using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollabRl
    {
        public CollabEntity AddCollab(CollabModel collabModel);
        public string RemoveCollab(long collabId, long userId);
        public IEnumerable<CollabEntity> GetCollab(long noteId, long userId);
    }
}
