using System.ComponentModel.DataAnnotations;
using System.Net;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.BaseResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(BaseResponse<Cart>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BasketByUsername([Required] string username)
        {
            BaseResponse<Cart> result;
            var data = await _basketRepository.GetBasketByUsername(username);
            if (data is null)
            {
                result = new BaseResponse<Cart>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            result = new BaseResponse<Cart>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<Cart>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Basket([FromBody] Cart cart)
        {
            BaseResponse<Cart> result;
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            var data = await _basketRepository.UpdateBasket(cart, options);
            if (data is null)
            {
                result = new BaseResponse<Cart>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            result = new BaseResponse<Cart>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<ActionResult<bool>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Basket([Required] string username)
        {
            BaseResponse<ActionResult<bool>> result;

            var data = await _basketRepository.DeleteBasketFromUsername(username);
            if (data is false)
            {
                result = new BaseResponse<ActionResult<bool>>()
                {
                    Result = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = nameof(StatusCodes.Status400BadRequest)
                };
                return BadRequest(result);
            }
            result = new BaseResponse<ActionResult<bool>>()
            {
                Result = true,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }
    }
}
