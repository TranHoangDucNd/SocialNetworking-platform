using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net.WebSockets;
using WebDating.Entities.ProfileEntities;
using WebDating.Extensions;

namespace WebDating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("provinces")]
        public IActionResult GetAllProvinces()
        {
            var data = Enum.GetValues<Provice>()
                .Select(it => new
                {
                    key = (int)it,
                    value = it.GetAttribute<DisplayAttribute>().Name ?? Convert.ToString(it)
                });

            return Ok(data);
        }

        [HttpGet]
        [Route("interestes")]
        public IActionResult GetAllInterestes()
        {
            var data = Enum.GetValues<Interest>()
                 .Select(it => new
                 {
                     key = (int)it,
                     value = it.GetAttribute<DisplayAttribute>().Name ?? Convert.ToString(it)
                 });

            return Ok(data);
        }

        [HttpGet]
        [Route("heights")]
        public IActionResult GetAllHeights()
        {
            var data = Enum.GetValues<Height>()
                 .Select(it => new
                 {
                     key = (int)it,
                     value = it.GetAttribute<DisplayAttribute>().Name ?? Convert.ToString(it)
                 });

            return Ok(data);
        }
        [HttpGet]
        [Route("genders")]
        public IActionResult GetAllGenders()
        {
            var data = Enum.GetValues<Gender>()
                 .Select(it => new
                 {
                     key = (int)it,
                     value = it.GetAttribute<DisplayAttribute>().Name ?? Convert.ToString(it)
                 });

            return Ok(data);
        }
    }
}
