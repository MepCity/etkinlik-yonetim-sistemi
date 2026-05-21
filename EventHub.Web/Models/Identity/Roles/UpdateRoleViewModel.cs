namespace EventHub.Web.Models.Identity.Roles

{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
