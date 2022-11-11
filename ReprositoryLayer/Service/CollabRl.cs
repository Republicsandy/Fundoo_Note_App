using CommonLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class CollabRl : ICollabRl
    {
        public readonly FundooContext fundooContext;

        public CollabRl(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public CollabEntity AddCollab(CollabModel collabModel)
        {
            try
            {
                var noteData = fundooContext.NotesTable.Where(x => x.NoteID == collabModel.NoteID).FirstOrDefault();
                var userData = fundooContext.UserTable.Where(x => x.Email == collabModel.CollabEmail).FirstOrDefault();
                if (noteData != null && userData != null)
                {
                    CollabEntity collabEntity = new CollabEntity()
                    {
                        CollabEmail = collabModel.CollabEmail,
                        NoteID = collabModel.NoteID,
                        UserId = userData.UserId
                    };
                    fundooContext.CollabTable.Add(collabEntity);
                    var result = fundooContext.SaveChanges();
                    return collabEntity;
                }

                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string RemoveCollab(long collabId, long UserId)
        {
            try
            {
                var collab = fundooContext.CollabTable.Where(X => X.CollabID == collabId).FirstOrDefault();
                if (collab != null)
                {
                    fundooContext.CollabTable.Remove(collab);
                    fundooContext.SaveChanges();
                    return "Removed Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CollabEntity> GetCollab(long noteId, long UserId)
        {
            try
            {
                var result = fundooContext.CollabTable.ToList().Where(x => x.NoteID == noteId);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
