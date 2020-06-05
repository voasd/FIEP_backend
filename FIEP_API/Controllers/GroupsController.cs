﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DTO;
using BusinessTier.Response;
using DataTier.Models;
using DataTier.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public GroupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public ActionResult GetGroups()
        {
            var groups = _unitOfWork.Repository<GroupInformation>().GetAll().ToList();
            var listOfGroups = new List<GroupDTO>();
            foreach (var item in groups)
            {   GroupDTO groupDTO = new GroupDTO()
                {
                    GroupName = item.GroupName,
                    GroupImageUrl = item.GroupImageUrl,
                };
                listOfGroups.Add(groupDTO);
            }
            return Ok(new ResponseBase<List<GroupDTO>>()
            {
                ErrorCode = 0,
                Data = listOfGroups
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetGroup(int id)
        {
            var group = _unitOfWork.Repository<GroupInformation>().FindFirstByProperty(x => x.GroupId == id);
            return Ok(new ResponseBase<GroupInformation>()
            {
                ErrorCode = 0,
                Data = group
            });
        }
    }
}