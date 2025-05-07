using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public UserTypeDTO UserType { get; set; }
        public JWTResponseToken Token { get; set; }
        public bool IsYearlyPay { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? PayDate { get; set; }
        public IdName Lender { get; set; }
        public IdName Manager { get; set; }
        public IdName City { get; set; }
               
        public int? OrganizationId { get; set; }
        public bool? UserKind { get; set; }




    }
    public class UserGlobalDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public UserTypeDTO UserType { get; set; }
        public bool IsYearlyPay { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? PayDate { get; set; }
        public IdName Lender { get; set; }
        public IdName Manager { get; set; }
        public IdName City { get; set; }
        public bool? UserKind { get; set; }

        public int? OrganizationId { get; set; }






    }
    //namespace Logic.DTO
    //{
    //    public class UserDTO
    //    {
    //        public int Id { get; set; }
    //        public string Email { get; set; }
    //        public string FirstName { get; set; }
    //        public string LastName { get; set; }
    //        public string Phone { get; set; }
    //        public string Password { get; set; }
    //        public bool IsActive { get; set; }
    //        public UserTypeDTO UserType { get; set; }
    //        public JWTResponseToken Token { get; set; }
    //        public bool IsYearlyPay { get; set; }
    //        public DateTime RegisterDate { get; set; }
    //        public DateTime? PayDate { get; set; }
    //        public IdName Lender { get; set; }
    //        public IdName Manager { get; set; }
    //        public IdName City { get; set; }
    //        public bool? UserKind { get; set; }
    //    }

    //    public class UserGlobalDTO
    //    {
    //        public int Id { get; set; }
    //        public string Email { get; set; }
    //        public string FirstName { get; set; }
    //        public string LastName { get; set; }
    //        public string Phone { get; set; }
    //        public string Password { get; set; }
    //        public bool IsActive { get; set; }
    //        public UserTypeDTO UserType { get; set; }
    //        public bool IsYearlyPay { get; set; }
    //        public DateTime RegisterDate { get; set; }
    //        public DateTime? PayDate { get; set; }
    //        public IdName Lender { get; set; }
    //        public IdName Manager { get; set; }
    //        public IdName City { get; set; }
    //        public bool? UserKind { get; set; }
    //    }


    public class LenderParams
    {
        public int oldLender { get; set; }
        public UserTypeDTO userType { get; set; }
        public int? newLender { get; set; }
    }

    public class SearchUser
    {

        public IdName UserType { get; set; }

        public IdName Lender { get; set; }

        public IdName GuidersManager { get; set; }

      //  public IdName City { get; set; }

    }
   
}
