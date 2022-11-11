using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Service;

namespace BusinessLayer.Service
{
    public class NotesBl : INotesBl
    {
        private readonly INotesRl notesRl;

        public NotesBl(INotesRl notesRl)
        {
            this.notesRl = notesRl;
        }

        public NotesEntity CreateNote(long UserId, NotesModel notesModel)
        {
            try
            {
                return notesRl.CreateNote(UserId, notesModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity DeleteNotes(long noteId)
        {
            try
            {
                return notesRl.DeleteNotes(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity UpdateNote(NotesModel notesModel, long noteId)
        {
            try
            {
                return notesRl.UpdateNote(notesModel, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<NotesEntity> GetAllNotes(long userId)
        {
            try
            {
                return notesRl.GetAllNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotesEntity> GetNote(long noteId)
        {
            try
            {
                return notesRl.GetNote(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity ArchiveNote(long NoteId, long userId)
        {
            try
            {
                return notesRl.ArchiveNote(NoteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity PinNote(long NoteId, long userId)
        {
            try
            {
                return notesRl.PinNote(NoteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity TrashNote(long NotesId, long userId)
        {
            try
            {
                return notesRl.TrashNote(NotesId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity NoteColor(long NoteId, string addcolor)
        {
            try
            {
                return notesRl.NoteColor(NoteId, addcolor);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity AddImage(string imagePath, long userId, long noteId)
        {
            try
            {
                return notesRl.AddImage(imagePath, userId, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
