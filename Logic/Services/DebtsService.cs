using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IDebtsService
    {
        List<DebtDTO> GetDebts(int currentUserId, SearchDebtDTO searchDebt);
        void AddDebt(DebtDTO debt, int currentUserId);
        bool UpdateDebt(DebtDTO debt);
        bool DeleteDebt(int debtId);
        List<IdName> GetUrgencyies();
    }
    public class DebtsService : IDebtsService
    {
        IDBService dBService;
        public DebtsService(IDBService dBService)
        {
            this.dBService = dBService;
        }

        public void AddDebt(DebtDTO debt, int currentUserId)
        {
            var newDebt = new Debt()
            {
                Description = debt.Description,
                IsActive = debt.IsActive,
                Payments = debt.Payments,
                UrgencyId = debt.Urgency.Id,
                UserId = currentUserId,
                Sum = debt.Sum,
                AreaId=debt.AreaId.Id
            };
            dBService.entities.Debts.Add(newDebt);
            dBService.Save();
        }

        public bool DeleteDebt(int debtId)
        {
            var debt = dBService.entities.Debts.FirstOrDefault(x => x.Id == debtId);
            if (debt != null)
            {
                dBService.entities.Debts.Remove(debt);
                dBService.Save();
                return true;
            }
            return false;
        }

        public List<DebtDTO> GetDebts(int currentUserId, SearchDebtDTO searchDebt)
        {
            List<DebtDTO> debts = new List<DebtDTO>();

            if (dBService.entities.Debts.Any(x => x.UserId == currentUserId))
            {
                var query = dBService.entities.Debts.Where(x => x.UserId == currentUserId);

                if (searchDebt.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == searchDebt.IsActive.Value);
                }

                if (searchDebt.Urgency.Name != "")
                {
                    query = query.Where(x => x.Urgency.Description.Contains(searchDebt.Urgency.Name));
                }

                if (!string.IsNullOrEmpty(searchDebt.Description))
                {
                    query = query.Where(x => x.Description.Contains(searchDebt.Description));
                }

                if (searchDebt.Payments!= 0)
                {
                    query = query.Where(x => x.Payments == searchDebt.Payments);
                }

                if (searchDebt.Sum != 0)
                {
                    query = query.Where(x => x.Sum == searchDebt.Sum);
                }



                debts = query.Select(x => new DebtDTO()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Payments = x.Payments,
                    Urgency = new IdName
                    {
                        Id = x.Urgency.Id,
                        Name = x.Urgency.Description
                    },
                    UserId = x.UserId,
                    IsActive = x.IsActive,
                    Sum = x.Sum,
                    AreaId = x.Area!=null ? new IdName
                    {
                        Id = x.Area.Id,
                        Name = x.Area.Description
                    } : null

                }).ToList();
            }

            return debts;
        }
        public List<IdName> GetUrgencyies()
        {
            List<IdName> list = new List<IdName>();
            list = dBService.entities.UrgencyDebts.Select(x => new IdName()
            {
                Id = x.Id,
                Name = x.Description,
            }).ToList();
            return list;
        }

        public bool UpdateDebt(DebtDTO debt)
        {
            var dbDebt = dBService.entities.Debts.FirstOrDefault(x => x.Id == debt.Id);
            if (dbDebt != null)
            {
                dbDebt.Description = debt.Description;
                dbDebt.IsActive = debt.IsActive;
                dbDebt.Payments = debt.Payments;
                dbDebt.UrgencyId = debt.Urgency.Id;
                dbDebt.Sum = debt.Sum;
                dBService.Save();
                return true;
            }
            return false;
        }


    }
}
