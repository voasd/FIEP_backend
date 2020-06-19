﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.ServiceWorkers;
using DataTier.Models;
using DataTier.UOW;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ICacheStore _cacheStore;
        private IUnitOfWork _unitOfWork;
        private NotificationPublisher _notificationPublisher;
        private readonly IMediator _mediator;
        public GroupsController(IUnitOfWork unitOfWork, ICacheStore cacheStore, NotificationPublisher notificationPublisher, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _notificationPublisher = notificationPublisher;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult> GetGroups([FromQuery]GetGroupsRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{GroupId}")]
        public async Task<ActionResult> GetGroupById([FromRoute]GetGroupByIdRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{groupId:int}/events")]
        public async Task<ActionResult> GetEventsOfGroup([FromRoute]int groupId,[FromQuery] GetEventsRequest request)
        {
            request.GroupId = groupId;
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpPut("{groupId}/notification")]
        public async Task<ActionResult> CreatePushNotification([FromRoute] int groupId, [FromBody] CreateNotificationRequest request)
        {
            DataTier.Models.Notification notification = new DataTier.Models.Notification()
            {
                NotificationID=new Guid(),
                Body = request.Body,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                GroupId= groupId
            };
            _unitOfWork.Repository<DataTier.Models.Notification>().Insert(notification);
            _unitOfWork.Commit();

            _notificationPublisher.Publish(notification.NotificationID.ToString());

            return Ok();
        }
    }
}