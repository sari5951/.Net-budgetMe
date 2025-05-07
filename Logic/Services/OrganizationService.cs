using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IOrganizationService
    {
        OrganizationDTO GetOrganizationById(int id);
        OrganizationDTO GetCurrentOrganization(int id);
        bool AddOrganization(OrganizationDTO organization, int userId);

        bool UpdateOrganization(OrganizationDTO organization);
    }
    public class OrganizationService:IOrganizationService
    {
        private IDBService dbService;



        public OrganizationService( IDBService dbService )
        {
            this.dbService = dbService;
        }

        public OrganizationDTO GetOrganizationById(int id )
        {
            var dbOrganization = dbService.entities.Organizations.Where(o=>o.Id == id).Select(o=> new OrganizationDTO
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                Email = o.Email,
                Phone = o.Phone,
                About = o.About
            }).FirstOrDefault();

            return dbOrganization;
        }
        public OrganizationDTO GetCurrentOrganization(int id)
        {
            var user = dbService.entities.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return null;
            }
            if (user?.OrganizationId == null)
            {
                return new OrganizationDTO();
            }

            var dbOrganization = dbService.entities.Organizations.Where(o => o.Id == user.OrganizationId)
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address,
                    Email = o.Email,
                    Phone = o.Phone,
                    About = o.About
                }).FirstOrDefault();
            return dbOrganization;

        }

        public bool AddOrganization(OrganizationDTO organization, int userId)
        {
            bool isExist = dbService.entities.Organizations.Any(o=>o.Name == organization.Name && o.Email == organization.Email);
            if (!isExist)
            {

                var NewOrganization = new Organization
                {

                    Name = organization.Name,
                    Address = organization.Address,
                    Email = organization.Email,
                    Phone = organization.Phone,
                    About = organization.About,
                };

                dbService.entities.Organizations.Add(NewOrganization);
                dbService.entities.SaveChanges();

                var user = dbService.entities.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.OrganizationId = NewOrganization.Id;
                    dbService.entities.SaveChanges();
                    return true;
                }
            }
            return false;
        }

      
       public bool UpdateOrganization(OrganizationDTO organization)
       {
            var dbOrganization = dbService.entities.Organizations.FirstOrDefault(o => o.Id == organization.Id);
            if (dbOrganization != null)
            {
                dbOrganization.Name = organization.Name;
                dbOrganization.Email = organization.Email;
                dbOrganization.Phone = organization.Phone;
                dbOrganization.About = organization.About;
                dbOrganization.Address = organization.Address;
                dbService.entities.SaveChanges();
                return true;
            }
            return false;
            
       }


       

    }
}
