﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessTier.DistributedCache;
using BusinessTier.DTO;
using BusinessTier.Fields;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
namespace FIEP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ICacheStore _cacheStore;
        private readonly IUnitOfWork _unitOfWork;
        private NotificationPublisher _notificationPublisher;
        private readonly IMediator _mediator;
        public EventsController(IUnitOfWork unitOfWork, ICacheStore cacheStore, NotificationPublisher notificationPublisher, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _notificationPublisher = notificationPublisher;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetEvents([FromQuery]GetEventsRequest request)
        {

            var result = await _mediator.Send(request);

            //please remove this in later commit
            new GoogleSheetApiUtils(_unitOfWork).UpdateDataToSheet();

            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{EventId}")]
        public async Task<ActionResult> GetEvent([FromRoute] GetEventByIdRequest request)
        {
            var result = await _mediator.Send(request);
            if(result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpGet("{eventID}/posts")]
        public async Task<ActionResult> GetPostsOfEvent([FromRoute]int eventID,[FromQuery]GetPostsRequest request)
        {
            request.EventId = eventID;
            var result = await _mediator.Send(request);
            if (result.Response == null)
            {
                return BadRequest();
            }
            return Ok(result.Response);
        }

        [HttpPut("{eventID}/notification")]
        public async Task<ActionResult> CreatePushNotification([FromRoute] int eventID, [FromBody] CreateEventNotificationRequest request)
        {
            request.EventId = eventID;
            await _mediator.Send(request);
            return Ok();
        }
        [HttpPost]
        public ActionResult CreateEvent([FromQuery] EventDTO dto)
        {
            var model = new Event()
            {
                EventId = dto.EventId,
                EventName = dto.EventName,
                GroupId = 3,
            };
            _unitOfWork.Repository<Event>().Update(model);
            _unitOfWork.Commit();
            return Ok("testing");
        }
    }
}