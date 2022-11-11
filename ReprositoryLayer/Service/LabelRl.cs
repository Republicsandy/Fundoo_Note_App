using CommonLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class LabelRl : ILabelRl
    {
        public readonly FundooContext fundooContext;
        public LabelRl(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public LabelEntity AddLabel(LabelModel labelModel)
        {
            try
            {
                var note = fundooContext.NotesTable.Where(x => x.NoteID == labelModel.NoteID).FirstOrDefault();
                if (note != null)
                {
                    LabelEntity label = new LabelEntity();
                    label.LabelName = labelModel.LabelName;
                    label.NoteID = note.NoteID;
                    label.UserId = note.UserId;

                    fundooContext.LabelTable.Add(label);
                    int result = fundooContext.SaveChanges();
                    return label;
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

        public IEnumerable<LabelEntity> GetAllLabel(long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.ToList().Where(x => x.UserId == userId);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LabelEntity> Getlabel(long NotesId, long userId)
        {
            try
            {
                var response = fundooContext.LabelTable.Where(x => x.NoteID == NotesId).ToList();
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public LabelEntity UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                var update = fundooContext.LabelTable.Where(X => X.LabelID == labelID).FirstOrDefault();
                if (update != null && update.LabelID == labelID)
                {
                    update.LabelName = labelModel.LabelName;
                    update.NoteID = labelModel.NoteID;

                    fundooContext.SaveChanges();
                    return update;
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

        public LabelEntity DeleteLabel(long labelID, long userId)
        {
            try
            {
                var deleteLabel = fundooContext.LabelTable.Where(X => X.LabelID == labelID).FirstOrDefault();
                if (deleteLabel != null)
                {
                    fundooContext.LabelTable.Remove(deleteLabel);
                    fundooContext.SaveChanges();
                    return deleteLabel;
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

    }
}
