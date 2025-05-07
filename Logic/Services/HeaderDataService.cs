 using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{


    public interface IHeaderDataService
    {
        HeaderDataDTO GetHeaderData(int ManagerId, int CurrentUserId);
        bool AddHeaderData(HeaderDataDTO HeaderData, int CurrentUserId);
        bool UpdateHeaderData(HeaderDataDTO HeaderData, int CurrentUserId);

        HeaderDataDTO GetFile(int id);




    }
    public class HeaderDataService : IHeaderDataService
    {

        IDBService dBService;
        public HeaderDataService(IDBService dBService)
        {
            this.dBService = dBService;
        }

        public HeaderDataDTO GetHeaderDataDefault()
        {
            var HeaderData = new HeaderDataDTO();
            var dbSystemManager = dBService.entities.Users.FirstOrDefault(x => x.UserTypeId == (int)UserTypeDTO.SystemManager);

            if (dbSystemManager != null)
            {
                var dbHeaderData = dBService.entities.HeaderData.FirstOrDefault(x => x.ManagerId == dbSystemManager.Id);
                if (dbHeaderData != null)
                {
                    HeaderData.Id = dbHeaderData.Id;
                    HeaderData.Title = dbHeaderData.Title;
                    HeaderData.Slogan = dbHeaderData.Slogan;
                    HeaderData.ColorBackroundHeader = dbHeaderData.ColorBackroundHeader;
                    HeaderData.ColorFont = dbHeaderData.ColorFont;
                    HeaderData.ContentLogo = dbHeaderData.LogoContent;
                    HeaderData.FileName = dbHeaderData.FileName;
                    HeaderData.Src = "File/ShowFileDesign/" + dbHeaderData.Id;
                    HeaderData.IsCustomDesign = false;

                }
            }

            return HeaderData;
        }


        public HeaderDataDTO GetHeaderData(int ManagerId, int CurrentUserId)
        {

            var HeaderData = new HeaderDataDTO();
            if (CurrentUserId == null)
            {
                HeaderData = GetHeaderDataDefault();
            }
            else
            {
                var dbUser = dBService.entities.Users.FirstOrDefault(x => x.Id == CurrentUserId);
                if (dbUser != null)
                {
                    if (dbUser.UserTypeId == (int)UserTypeDTO.LendersManager || dbUser.UserTypeId == (int)UserTypeDTO.Lender || dbUser.UserTypeId == (int)UserTypeDTO.UnderLander)
                    {
                        var dbHeaderData = dBService.entities.HeaderData.FirstOrDefault(x => x.ManagerId == ManagerId);
                        if (dbHeaderData != null)
                        {
                            HeaderData.Id = dbHeaderData.Id;
                            HeaderData.Title = dbHeaderData.Title;
                            HeaderData.Slogan = dbHeaderData.Slogan;
                            HeaderData.ColorBackroundHeader = dbHeaderData.ColorBackroundHeader;
                            HeaderData.ColorFont = dbHeaderData.ColorFont;
                            HeaderData.ContentLogo = dbHeaderData.LogoContent;
                            HeaderData.FileName = dbHeaderData.FileName;
                            HeaderData.Src = "File/ShowFileDesign/" + dbHeaderData.Id;
                            HeaderData.IsCustomDesign = true;


                        }
                        else
                        {
                            HeaderData = GetHeaderDataDefault();
                        }
                    }



                    else
                    {
                        HeaderData = GetHeaderDataDefault();
                    }
                }


            }

            return HeaderData;
        }



        public bool AddHeaderData(HeaderDataDTO HeaderData, int CurrentUserId)
        {
            if (dBService.entities.HeaderData.Any(h => h.ManagerId == CurrentUserId))
            {
                return false;
            }
            dBService.entities.HeaderData.Add(new HeaderDatum()
            {
                Title = HeaderData.Title,
                Slogan = HeaderData.Slogan,
                LogoContent = HeaderData.ContentLogo,
                ColorBackroundHeader = HeaderData.ColorBackroundHeader,
                ColorFont = HeaderData.ColorFont,
                ManagerId = CurrentUserId,
                FileName = HeaderData.FileName

            });
            dBService.Save();
            return true;
        }

        public bool UpdateHeaderData(HeaderDataDTO HeaderData, int CurrentUserId)
        {
            if (dBService.entities.HeaderData.Any(h => h.Id == HeaderData.ManagerId))
            {
                return false;
            }
            var HD = dBService.entities.HeaderData.FirstOrDefault(h => h.Id == HeaderData.ManagerId);
            if (HD != null)
            {

                HD.Title = HeaderData.Title;
                HD.Slogan = HeaderData.Slogan;
                HD.LogoContent = HeaderData.ContentLogo;
                HD.ColorBackroundHeader = HeaderData.ColorBackroundHeader;
                HD.ColorFont = HeaderData.ColorFont;
                HD.FileName = HeaderData.FileName;


            };
            dBService.Save();
            return true;
        }

        public HeaderDataDTO GetFile(int id)
        {
            var mdFile = new HeaderDataDTO();
            var dbFile = dBService.entities.HeaderData.FirstOrDefault(x => x.Id == id);
            if (dbFile != null)
            {
                mdFile.ContentLogo = dbFile.LogoContent;
                mdFile.FileName = dbFile.FileName;
            }
            return mdFile;
        }

    }
}


