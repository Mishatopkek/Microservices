using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;
    
    public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
    }
    
    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        ShoppingCart? basket = await _repository.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        foreach (ShoppingCartItem item in basket.Items)
        {
            CouponModel coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }

}