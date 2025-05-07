using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoneySystemServer.Controllers
{

    [AllowAnonymous]
    public class FileController : GlobalController
    {
        private IDocumentService documentService;
        private IHeaderDataService HeaderDataService;
        public FileController(IDocumentService documentService, IHeaderDataService HeaderDataService)
        {
            this.documentService = documentService;
            this.HeaderDataService = HeaderDataService;
        }


        [HttpGet("{id}")]
        public ActionResult ShowFile(int id)
        {
            var file = documentService.GetFile(id);
            file.ContentType = GetContentType(file.FileName);
            return File(file.Content, file.ContentType);
        }

        [HttpGet("{id}")]
        public ActionResult ShowFileDesign(int id)
        {

            var file = HeaderDataService.GetFile(id);
            var contentType = GetContentType(file.FileName);
            return File(file.ContentLogo, contentType);

        }

        //לבדיקה
        //[HttpGet("{id}/{index}")]
        //public ActionResult ShowFile(int id, int index)
        //{
        //    string filename;
        //    byte[] content;
        //    string ContentType = "";
        //    if (index == 1)
        //    {
        //        var file = documentService.GetFile(id);
        //        filename = file.FileName;
        //        content = file.Content;
        //        file.ContentType = GetContentType(file.FileName);
        //        ContentType = file.ContentType;

        //        return File(content, ContentType);
        //    }
        //    else
        //    {
        //        var HeaderData = HeaderDataService.GetHeaderData(id);
        //        filename = HeaderData.FileName;
        //        content = HeaderData.ContentLogo;
        //        return File(content, filename);
        //    }
        //}

        private string GetContentType(string fileName)
        {
            string type = "application/pdf";
            var extention = Path.GetExtension(fileName);
            if (extention == ".png" || extention == ".PNG")
            {
                type = "image/png";
            }
            else if (extention == ".jpg" || extention == ".JPG")
            {
                type = "image/jpg";
            }
            return type;
        }
    }
}
