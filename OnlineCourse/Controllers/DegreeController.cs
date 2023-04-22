﻿using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private DegreeSvc _degreeSvc = new DegreeSvc();

        [HttpGet("{idUser}")]
        [Authorize(Roles = "Expert")]
        public IActionResult GetDegreesByIdUser(Guid idUser)
        {
            var res = _degreeSvc.GetDegreesByIdUser(idUser);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("{idDegree}")]
        [Authorize(Roles = "Expert")]
        public IActionResult GetDegreesByIdDegree(Guid idUser)
        {
            var res = _degreeSvc.GetDegreeByIdDegree(idUser);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
    }
}
