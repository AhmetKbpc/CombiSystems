namespace CombiSystems.Web.Models.Role
{
    public class Roles
    {
        public static readonly string Admin = "Admin";
        public static readonly string Technician = "Technician";
        public static readonly string Operator = "Operator";
        public static readonly string User = "User";

        public static List<string> RoleList = new List<string>()
        {
            Admin,Technician,Operator,User
        };

    }
}
