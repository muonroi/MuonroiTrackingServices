namespace Shared.DTOs.Customers
{
    public class CreateCustomerDto : CreateOrUpdateDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}
