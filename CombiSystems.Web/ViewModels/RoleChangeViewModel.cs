namespace CombiSystems.Web.ViewModels
{
    public class RoleChangeViewModel
    {

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Role { get; set; }
    }
}
