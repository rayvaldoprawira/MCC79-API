﻿using API.DTOs.Rooms;
using API.Services;
using API.Utilities;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}, {nameof(RoleLevel.Manager)}")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _service;

        public RoomController(RoomService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetRoom();

            if (entities == null)
            {
                return NotFound(new ResponseHandler<GetRoomDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetRoomDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var room = _service.GetRoom(guid);
            if (room is null)
            {
                return NotFound(new ResponseHandler<GetRoomDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = room
            });
        }

        [HttpPost]
        public IActionResult Create(CreateRoomDto newRoomDto)
        {
            var createRoom = _service.CreateRoom(newRoomDto);
            if (createRoom is null)
            {
                return BadRequest(new ResponseHandler<GetRoomDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createRoom
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateRoomDto updateRoomDto)
        {
            var update = _service.UpdateRoom(updateRoomDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<UpdateRoomDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new ResponseHandler<UpdateRoomDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandler<UpdateRoomDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteRoom(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandler<GetRoomDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new ResponseHandler<GetRoomDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandler<GetRoomDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            var room = _service.GetRoom(name);
            if (room is null)
            {
                return NotFound(new ResponseHandler<GetRoomDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data By Name Not Found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetRoomDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data By Name Found",
                Data = room
            });
        }

        [HttpGet("get-unused-rooms")]
        public IActionResult GetUnusedRooms()
        {
            var rooms = _service.GetUnusedRoom();
            if (rooms.Count() == 0)
            {
                return NotFound(new ResponseHandler<IEnumerable<UnusedRoomDto>>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "All Rooms In Used"

                });
            }

            return Ok(new ResponseHandler<IEnumerable<UnusedRoomDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Room Unused",
                Data = rooms
            });
        }
    }
}