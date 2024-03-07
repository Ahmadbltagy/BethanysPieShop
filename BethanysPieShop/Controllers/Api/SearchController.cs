using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BethanysPieShop.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IPieRepository _pieRepository;
        public SearchController(
            IPieRepository pieRepository
        )
        {
            this._pieRepository = pieRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var pies = _pieRepository.AllPies;

            return Ok(pies);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            
            if(pie == null) return NotFound($"Pie with id {id} not found");

            return Ok(pie);
        }

        [HttpPost]
        public IActionResult SearchPie([FromBody]string searchQuery)
        {
            IEnumerable<Pie> pies = new List<Pie>();
            if(!string.IsNullOrWhiteSpace(searchQuery))
            {
                pies = _pieRepository.SearchPies(searchQuery);
            }
            
            if(pies == null) return NotFound();

            return Ok(pies);
        }
    }
}