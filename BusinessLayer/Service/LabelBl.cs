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
    public class LabelBl : ILabelBl
    {
        private readonly ILabelRl labelRl;
        public LabelBl(ILabelRl labelRl)
        {
            this.labelRl = labelRl;
        }
        public LabelEntity AddLabel(LabelModel labelModel)
        {
            try
            {
                return labelRl.AddLabel(labelModel);
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
                return labelRl.GetAllLabel(userId);
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
                return labelRl.Getlabel(NotesId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public LabelEntity UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                return labelRl.UpdateLabel(labelModel, labelID);
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
                return labelRl.DeleteLabel(labelID, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
