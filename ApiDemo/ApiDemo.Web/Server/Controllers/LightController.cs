using ApiDemo.Web.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightController : ControllerBase
    {
        private readonly LightService _lights;

        public LightController(LightService lights)
        {
            _lights = lights;
        }

        [Authorize]
        [Route("[action]/{color}")]
        [HttpPost]
        public async Task<IActionResult> ChangeColor(string color)
        {
            try
            {
                await _lights.ChangeColor(color);
                return Ok("Color changed to " + color + ".");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{color}")]
        [HttpPost]
        public async Task<IActionResult> Post(string color)
        {
            try
            {
                await _lights.ChangeColor(color);
                return Ok("Color changed to " + color + ".");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("power/{onoff}")]
        [HttpPut]
        public async Task<IActionResult> Power(string onoff)
        {
            try
            {
                if (onoff == "on")
                {
                    await _lights.On();
                    return Ok("Light On.");
                }
                
                else
                {
                    await _lights.Off();
                    return Ok("Light Off.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
