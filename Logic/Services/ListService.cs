using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logic.Services
{
    public interface IListService
    {
        ListsDTO GetAllLists();
        List<IdName> GetList(IdNameDB item);
        AnsOption AddItem(IdNameDB idName);
        AnsOption DeleteItem(IdNameDB idName);
        bool UpdateItem(IdNameDB idName);
    }
    public class ListService : IListService
    {
        private IDBService dbService;
        public ListService(IDBService dbService)
        {
            this.dbService = dbService;
        }

        public ListsDTO GetAllLists()
        {
            var lists = new ListsDTO();
            lists.UserTypes = dbService.entities.UserTypes.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description
            }).ToList();
            lists.Cities = dbService.entities.Cities.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            lists.Areas = dbService.entities.Areas.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description,
                Type = x.Type
            }).ToList();
            lists.UrgencyDebts = dbService.entities.UrgencyDebts.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description
            }).ToList();
            lists.Statuses = dbService.entities.Statuses.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description
            }).ToList();
            lists.PayOptions = dbService.entities.PayOptions.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description,
                IsActive = x.IsActive
            }).ToList();

            return lists;
        }

        public List<IdName> GetList(IdNameDB item)
        {
            List<IdName> list = new List<IdName>();
            if (item.TableCode == TableCode.UserTypes)
            {
                list = dbService.entities.UserTypes.Select(x => new IdName()
                {
                    Id = x.Id,
                    Name = x.Description
                }).ToList();
            }
            else if (item.TableCode == TableCode.Cities)
            {
                list = dbService.entities.Cities.Select(x => new IdName()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            else if (item.TableCode == TableCode.Status)
            {
                list = dbService.entities.Statuses.Select(x => new IdName()
                {
                    Id = x.Id,
                    Name = x.Description
                }).ToList();
            }
            else if (item.TableCode == TableCode.UrgencyDebt)
            {
                list = dbService.entities.UrgencyDebts.Select(x => new IdName()
                {
                    Id = x.Id,
                    Name = x.Description
                }).ToList();
            }
            else if (item.TableCode == TableCode.Areas)
                list = dbService.entities.Areas.Where(x => x.Type == item.Type).Select(x => new IdName()
                {
                    Id = x.Id,
                    Name = x.Description,
                    Type = x.Type
                }).ToList();
            else if (item.TableCode==TableCode.PayOption)
                list = dbService.entities.PayOptions.Select(x => new IdName()
                {
                    Id=x.Id,
                    Name = x.Description,
                    IsActive = x.IsActive,
                }).ToList();
            else if (item.TableCode == TableCode.Users)
            {
                list = dbService.entities.Users.Where(x => x.UserTypeId == item.Type && x.IsActive).Select(x => new IdName()
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                }).ToList();
            }
            return list;
        }

        public AnsOption AddItem(IdNameDB idName)
        {
            
            switch (idName.TableCode)
            {
                case TableCode.Cities:
                    {
                        return AddCity(idName);
                    }
                case TableCode.Areas:
                    {
                        return AddArea(idName);
                    }
                case TableCode.UrgencyDebt:
                    {
                        return AddUrgencyDebt(idName);
                    }
                case TableCode.Status:
                    {
                        return AddStatus(idName);
                    }
                case TableCode.PayOption:
                    {
                        return AddPayOption(idName);
                    }
                default:
                    break;
            }
            return AnsOption.No;
        }

        public AnsOption DeleteItem(IdNameDB idName)
        {
            switch (idName.TableCode)
            {
           
                case TableCode.Cities:
                    {
                        return DeleteCity(idName.Id);
                    }
                case TableCode.Areas:
                    {
                        return DeleteArea(idName.Id);
                    }
                case TableCode.UrgencyDebt:
                    {
                        return DeleteUrgencyDebt(idName.Id);
                    }
                case TableCode.Status:
                    {
                        return DeleteStatus(idName.Id);
                    }
                case TableCode.PayOption:
                    {
                        return DeletePayOption(idName.Id);
                    }
                default:
                    break;
            }
            return AnsOption.No;
        }

        public bool UpdateItem(IdNameDB idName)
        {
            switch (idName.TableCode)
            {
             
                case TableCode.Cities:
                    {
                        return UpdateCity(idName);
                    }
                case TableCode.Areas:
                    {
                        return UpdateArea(idName);
                    }
                case TableCode.UrgencyDebt:
                    {
                        return UpdateUrgencyDebt(idName);
                    }
                case TableCode.Status:
                    {
                        return UpdateStatus(idName);
                    }
                case TableCode.PayOption:
                    {
                        return UpdatePayOption(idName);
                    }
                default:
                    break;
            }
            return false;

        }

        private bool UpdateCity(IdNameDB idName)
        {
            var dbCity = dbService.entities.Cities.FirstOrDefault(x => x.Id == idName.Id);
            if (dbCity == null) return false;
            dbCity.Name = idName.Name;
            dbService.Save();
            return true;
        }

        private bool UpdateArea(IdName idName)
        {
            var dbArea = dbService.entities.Areas.FirstOrDefault(x => x.Id == idName.Id);
            if (dbArea == null) return false;
            dbArea.Description = idName.Name;
            dbService.Save();
            return true;
        }

        private bool UpdateUrgencyDebt(IdName idName)
        {
            var dBurgencyDebt = dbService.entities.UrgencyDebts.FirstOrDefault(x => x.Id == idName.Id);
            if (dBurgencyDebt != null)
            {
                dBurgencyDebt.Description = idName.Name;
                dbService.Save();
                return true;
            }
            return false;
        }
        private bool UpdateStatus(IdName idName)
        {
            var dBStatus = dbService.entities.Statuses.FirstOrDefault(x => x.Id == idName.Id);
            if (dBStatus != null)
            {
                dBStatus.Description = idName.Name;
                dbService.Save();
                return true;
            }
            return false;
        }
        private bool UpdatePayOption(IdName idName)
        {
            var dBPayOption = dbService.entities.PayOptions.FirstOrDefault(x=>x.Id == idName.Id);
            if(dBPayOption != null)
            {
                dBPayOption.Description = idName.Name;
                dbService.Save();
                return true;
            }
            return false;
        }
        private AnsOption DeleteCity(int id)
        {
            var city = dbService.entities.Cities.FirstOrDefault(x => x.Id == id);
            if (city != null)
            {
                if(dbService.entities.Users.Any(u=>u.CityId == id))
                    return AnsOption.OtherOption;
                dbService.entities.Cities.Remove(dbService.entities.Cities.FirstOrDefault(x => x.Id == id));
                dbService.Save();
                return AnsOption.Yes;
            }
            return AnsOption.No;
        }

        private AnsOption DeleteArea(int id)
        {
            var ar = dbService.entities.Areas.FirstOrDefault(x => x.Id == id);
           
            if (ar != null)
            {
                dbService.entities.Areas.Remove(dbService.entities.Areas.FirstOrDefault(x => x.Id == id));
                dbService.Save();
                return AnsOption.Yes;
            }
            return AnsOption.No;
        }

        private AnsOption DeleteUrgencyDebt(int id)
        {
            if (dbService.entities.UrgencyDebts.Any(x => x.Id == id))
            {
                if (dbService.entities.Tasks.Any(x => x.UrgencyId == id)||dbService.entities.Debts.Any(x => x.UrgencyId == id))
                    return AnsOption.OtherOption;
                dbService.entities.UrgencyDebts.Remove(dbService.entities.UrgencyDebts.FirstOrDefault(x => x.Id == id));
                dbService.Save();
                return AnsOption.Yes;
            }
            return AnsOption.No;
           
        }

        private AnsOption DeleteStatus(int id)
        {
            if (dbService.entities.Statuses.Any(x => x.Id == id))
            {
                if (dbService.entities.Tasks.Any(t => t.StatusId == id)) 
                     return AnsOption.OtherOption;
                dbService.entities.Statuses.Remove(dbService.entities.Statuses.FirstOrDefault(x => x.Id == id));
                dbService.Save();
                return AnsOption.Yes;
            }
            return AnsOption.No;
        }
        
        private AnsOption DeletePayOption(int id)
        {
            var dbPayOpt = dbService.entities.PayOptions.FirstOrDefault(x => x.Id == id);
            if (dbPayOpt != null)
            {
                if (dbPayOpt.Movings != null && dbPayOpt.Movings.Count > 0)
                {
                    dbPayOpt.IsActive = false;
                    dbService.entities.SaveChanges();
                    return AnsOption.Yes;
                }
                else
                {
                    dbService.entities.PayOptions.Remove(dbService.entities.PayOptions.FirstOrDefault(x => x.Id == id));
                    dbService.entities.SaveChanges();
                    return AnsOption.Yes;
                }
            }
            return AnsOption.No;
        }
        private AnsOption AddCity(IdNameDB idName)
        {
            if (dbService.entities.Cities.Any(x => x.Name == idName.Name)) return AnsOption.OtherOption;
            var newItem = new City()
            {
                Name = idName.Name
            };
            dbService.entities.Cities.Add(newItem);
            dbService.Save();
            return AnsOption.Yes;
        }

        private AnsOption AddArea(IdNameDB idName)
        {
            if (dbService.entities.Areas.Any(x => x.Description == idName.Name))
            {
                return AnsOption.OtherOption;
            }
            var newItem = new Area()
            {
                Description = idName.Name,
                Type = (int)idName.Type
            };
            dbService.entities.Areas.Add(newItem);
            dbService.Save();
            return AnsOption.Yes;
        }

        private AnsOption AddUrgencyDebt(IdNameDB idName)
        {
            if (dbService.entities.UrgencyDebts.Any(x => x.Description == idName.Name))
            {
                return AnsOption.OtherOption;
            }
            var newItem = new UrgencyDebt()
            {
                Description = idName.Name
            };
            dbService.entities.UrgencyDebts.Add(newItem);
            dbService.Save();
            return AnsOption.Yes;
        }

        private AnsOption AddStatus(IdNameDB idName)
        {
            if (dbService.entities.Statuses.Any(x => x.Description == idName.Name))
            {
                return AnsOption.OtherOption;
            }
            var newItem = new Status()
            {
                Description = idName.Name
            };
            dbService.entities.Statuses.Add(newItem);
            dbService.Save();
            return AnsOption.Yes;
        }

        private AnsOption AddPayOption(IdNameDB idName)
        {
            if (!dbService.entities.PayOptions.Any(x => x.Description == idName.Name))
            {
                var newPayOpt = new PayOption()
                {
                    Description = idName.Name,
                    IsActive = true
                };
                dbService.entities.PayOptions.Add(newPayOpt);
                dbService.entities.SaveChanges();
                return AnsOption.Yes;
            }
            return AnsOption.No;
        }
        
    }
}
