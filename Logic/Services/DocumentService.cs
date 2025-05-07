using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logic.DTO.FileFromClient;

namespace Logic.Services
{
    public interface IDocumentService
    {
        List<DocumentDTO> GetDocuments(int CurrentUserId, SearchDocDTO searchDoc);
        bool AddDocument(DocumentDTO document);
        bool DeleteDocument(int id, int CurrentUserId);
        bool UpdateDocument(int id,String description, int CurrentUserId);
        DocumentDTO GetFile(int id);
    }
    public class DocumentService : IDocumentService
    {
        IDBService dBService;
        public DocumentService(IDBService dBService)
        {
            this.dBService = dBService;
        }
        public List<DocumentDTO> GetDocuments(int currentUserId, SearchDocDTO searchDoc)
        {
            List<DocumentDTO> list = new List<DocumentDTO>();
            var query = dBService.entities.Documents.Where(x=>x.UserId==currentUserId);

            if (searchDoc != null)
            {
                if (searchDoc.Description != "")
                {
                    var l = query.Where(x => x.Description.Contains(searchDoc.Description));
                    query = l;
                }

                if (searchDoc.FileName != "")
                {
                    var jj = query.Where(x => x.FileName.Contains(searchDoc.FileName));
                    query = jj;
                }

            }


            list = query.Select(x => new DocumentDTO()
            {
                Description = x.Description,
                FileName = x.FileName,
                Id = x.Id,
                Src = "File/ShowFile/" + x.Id
            }).ToList();

            return list;
        }
        public bool AddDocument(DocumentDTO document)
        {
            if (dBService.entities.Documents.Any(l => l.UserId == document.UserId && l.FileName == document.FileName))
            {
                return false;
            }
            dBService.entities.Documents.Add(new Document()
            {
                Content = document.Content,
                Description = document.Description,
                FileName = document.FileName,
                UserId = document.UserId

            });
            dBService.Save();
            return true;
        }
        public bool DeleteDocument(int id, int CurrentUserId)
        {
            var DocumentToDelete = dBService.entities.Documents.FirstOrDefault(x => x.Id == id);
            if (DocumentToDelete != null)
            {
                dBService.entities.Documents.Remove(dBService.entities.Documents.FirstOrDefault(x => x.Id == id));
                dBService.Save();
                return true;
            }
            return false;
        }

        public DocumentDTO GetFile(int id)
        {
            var doc = new DocumentDTO();
            var dbFile = dBService.entities.Documents.FirstOrDefault(x => x.Id == id);
            if (dbFile != null)
            {
                doc.Content = dbFile.Content;
                doc.FileName = dbFile.FileName;
            }
            return doc;
        }

        public bool UpdateDocument(int id,String description, int CurrentUserId)
        {
            if (dBService.entities.Documents.Any(x => x.UserId == CurrentUserId && x.Id != id && x.Description == description))
            {
                return false;
            }
            var dbDocuments = dBService.entities.Documents.FirstOrDefault(x => x.Id == id);
            
            if (dbDocuments != null)
            {
               
                dbDocuments.Description = description;

                dBService.Save();
                return true;
            }
            return false;
        }
    }
}
