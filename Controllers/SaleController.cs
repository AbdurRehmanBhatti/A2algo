using BusinessLayer.SaleService;
using Entities.Dto_s;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace A2algo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _salesService;

        public SaleController(ISaleService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost("RecordSale")]
        public async Task<ActionResult<int>> RecordSaleAsync(SaleDto saleDto)
        {
            var (statusCode, saleId) = await _salesService.RecordSaleAsync(saleDto);

            if (statusCode == HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(GetSaleByIdAsync), new { id = saleId }, saleId);
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                return NotFound("Product not found.");
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest("Not enough quantity available.");
            }

            return StatusCode((int)statusCode);
        }

        [HttpGet("GetSaleById/{id}")]
        public async Task<ActionResult<SaleDto>> GetSaleByIdAsync(int id)
        {
            var (statusCode, saleDto) = await _salesService.GetSaleByIdAsync(id);

            if (statusCode == HttpStatusCode.OK)
            {
                return Ok(saleDto);
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
    }
}
