using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INotesBl
    {
        public NotesEntity CreateNote(long UserId, NotesModel notesModel);
        public NotesEntity DeleteNotes(long noteId);
        public NotesEntity UpdateNote(NotesModel notesModel, long noteId);
        public IEnumerable<NotesEntity> GetAllNotes(long userId);
        public List<NotesEntity> GetNote(long noteId);
        public NotesEntity ArchiveNote(long NoteId, long userId);
        public NotesEntity PinNote(long NoteId, long userId);
        public NotesEntity TrashNote(long NotesId, long userId);
        public NotesEntity NoteColor(long NoteId, string addcolor);
        public NotesEntity AddImage(string filePath, long userId, long noteId);

    }
}
