using CommonLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace RepositoryLayer.Service
{
    public class NotesRl : INotesRl
    {
        private readonly FundooContext fundooContext;

        public NotesRl(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public NotesEntity CreateNote(long UserId, NotesModel notesModel)
        {
            try
            {
                NotesEntity notesEntity = new NotesEntity();
                notesEntity.Title = notesModel.Title;
                notesEntity.Description = notesModel.Description;
                notesEntity.Reminder = notesModel.Reminder;
                notesEntity.Color = notesModel.Color;
                notesEntity.Image = notesModel.Image;
                notesEntity.Archive = notesModel.Archive;
                notesEntity.Pin = notesModel.Pin;
                notesEntity.Trash = notesModel.Trash;
                notesEntity.Created = notesModel.Created;
                notesEntity.Edited = notesModel.Edited;
                notesEntity.UserId = UserId;

                fundooContext.NotesTable.Add(notesEntity);
                int result = fundooContext.SaveChanges();

                if (result != 0)
                {
                    return notesEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        public NotesEntity DeleteNotes(long noteId)
        {
            try
            {
                var deleteNote = fundooContext.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
                if (deleteNote != null)
                {
                    fundooContext.NotesTable.Remove(deleteNote);
                    fundooContext.SaveChanges();
                    return deleteNote;
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

        public NotesEntity UpdateNote(NotesModel notesModel, long noteId)
        {
            try
            {
                var update = fundooContext.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
                if (update != null)
                {
                    update.Title = notesModel.Title;
                    update.Description = notesModel.Description;
                    update.Reminder = notesModel.Reminder;
                    update.Color = notesModel.Color;
                    update.Image = notesModel.Image;
                    fundooContext.NotesTable.Update(update);
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
        public IEnumerable<NotesEntity> GetAllNotes(long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.ToList().Where(x => x.UserId == userId);
                return result;
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
                var Note = fundooContext.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
                if (Note != null)
                {
                    return fundooContext.NotesTable.Where(list => list.NoteID == noteId).ToList();
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

        public NotesEntity ArchiveNote(long NoteId, long userId)
        {
            try
            {
                var data = fundooContext.NotesTable.Where(A => A.NoteID == NoteId && A.UserId == userId).FirstOrDefault();
                if (data != null)
                {
                    if (data.Archive == false)
                    {
                        data.Archive = true;
                    }
                    else
                    {
                        data.Archive = false;
                    }
                    fundooContext.SaveChanges();
                    return data;
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

        public NotesEntity PinNote(long NoteId, long userId)
        {
            var pin = fundooContext.NotesTable.Where(p => p.NoteID == NoteId && p.UserId == userId).FirstOrDefault();
            if (pin != null)
            {
                if (pin.Pin == false)
                {
                    pin.Pin = true;
                }
                else
                {
                    pin.Pin = false;
                }
                fundooContext.SaveChanges();
                return pin;
            }
            else
            {
                return null;
            }
        }

        public NotesEntity TrashNote(long NotesId, long userId)
        {
            var trashed = fundooContext.NotesTable.Where(t => t.NoteID == NotesId && t.UserId == userId).FirstOrDefault();
            if (trashed != null)
            {
                if (trashed.Trash == false)
                {
                    trashed.Trash = true;
                }
                else
                {
                    trashed.Trash = false;
                }
                fundooContext.SaveChanges();
                return trashed;

            }
            else
            {
                return null;
            }
        }

        public NotesEntity NoteColor(long NoteId, string addcolor)
        {
            var note = fundooContext.NotesTable.Where(c => c.NoteID == NoteId).FirstOrDefault();
            if (note != null)
            {
                if (addcolor != null)
                {
                    note.Color = addcolor;
                    fundooContext.NotesTable.Update(note);
                    fundooContext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public NotesEntity AddImage(string filePath, long userId, long noteId)
        {
            try
            {
                var note = fundooContext.NotesTable.Where(n => n.UserId == userId && n.NoteID == noteId).FirstOrDefault();

                if (note != null)
                {
                    Account account = new Account("dds31bhfh", "351562673489543", "-HhPKusJnvofP42Bq_Pd5DBQRXM");
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams();
                    uploadParams.File = new FileDescription(filePath);
                    uploadParams.PublicId = userId + "_" + noteId + "_" + DateTime.Now.ToShortDateString();
                    ImageUploadResult uploadDetails = cloudinary.Upload(uploadParams);
                    note.Image = uploadDetails.Url.ToString();
                    fundooContext.SaveChanges();

                    return note;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        ///https://www.youtube.com/watch?v=FcdXA6EYWyM
        ////https://csharp.hotexamples.com/examples/CloudinaryDotNet/Cloudinary/Upload/php-cloudinary-upload-method-examples.html


    }
}
