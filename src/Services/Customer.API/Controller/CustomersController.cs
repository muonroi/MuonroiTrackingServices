using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customers;

namespace Customer.API.Controller
{
    public static class CustomersController
    {
        public static void MapCustomersApi(this WebApplication app)
        {
            app.MapGet("/api/customer/{username}",
                async ([FromRoute] string username, ICustomerService customerService) =>
                {
                    var result = await customerService.GetCustomerByUsernameAsync(username);
                    return result;
                });

            app.MapPost("/api/customer",
                async ([FromBody] CreateCustomerDto customer, ICustomerService customerService) =>
                {
                    var result = await customerService.CreateCustomerAsync(customer);
                    return result;
                });

            app.MapPut("/api/customer/{username}",
                async ([FromRoute] string username, [FromBody] UpdateCustomerDto customer, ICustomerService customerService) =>
                {
                    var result = await customerService.UpdateCustomerAsync(username, customer);
                    return result;
                });

            app.MapDelete("/api/customer/{username}",
                async ([FromRoute] string username, ICustomerService customerService) =>
                {
                    var result = await customerService.DeleteCustomerAsync(username);
                    return result;
                });
        }
    }
}
