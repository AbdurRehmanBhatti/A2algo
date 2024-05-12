using BusinessLayer.PurchaseService;
using Entities.Dto_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace A2algo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost("RecordPurchase")]
        public async Task<ActionResult<int>> RecordPurchaseAsync(PurchaseDto purchaseDto)
        {
            var (statusCode, purchaseId) = await _purchaseService.RecordPurchaseAsync(purchaseDto);

            if (statusCode == HttpStatusCode.Created)
            {
                return Created("", purchaseId);
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                return StatusCode((int)statusCode);
            }
        }

        [HttpGet("GetAllPurchases")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetAllPurchasesAsync()
        {
            var (statusCode, purchases) = await _purchaseService.GetAllPurchasesAsync();

            if (statusCode == HttpStatusCode.OK)
            {
                return Ok(purchases);
            }
            else
            {
                return StatusCode((int)statusCode);
            }
        }
    }
}
