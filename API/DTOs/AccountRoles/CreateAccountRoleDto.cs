namespace API.DTOs.AccountRoles
{
    public class CreateAccountRoleDto
    {
        public Guid AccountGuid { get; set; }
        public Guid RoleGuid { get; set; }
    }
}
