using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DTO
{

    public class HeaderDataDTO
    {
        public int Id { get; set; }

        public int ManagerId { get; set; }

        public string Title { get; set; }

        public string Slogan { get; set; }

        public string ColorBackroundHeader { get; set; }

        public string ColorFont { get; set; }

        public byte[]? ContentLogo { get; set; }

        public IFormFile File { get; set; }

        public string? FileName { get; set; }

        public string? Src { get; set; }

        public bool IsCustomDesign { get; set; } = false;



    }

}

