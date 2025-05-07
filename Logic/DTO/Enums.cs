using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DTO
{
    public enum MoovingType
    {
        Revenues = 1,
        Expenses = 2
    }
    public enum TableCode
    {
        UserTypes = 1,
        Cities = 2,
        Areas = 3,
        UrgencyDebt = 4,
        Status = 5,
        PayOption = 6,
        Users = 8
    }


    public enum AnsOption
    {
        Yes = 1,
        No = 2,
        OtherOption = 3
    }


    public enum UserTypeDTO
    {
        SystemManager = 1,
        Lender = 2,
        Regular = 3,
        LendersManager = 4,
        UnderLander = 5,
        Attendance = 6
    }


}

