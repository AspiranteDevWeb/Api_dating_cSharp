using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UsersController;

public class MessagesController : BaseApiController
{
    private readonly IUnitOfWork _uok;
    private readonly IMapper _mapper;

    public MessagesController( IUnitOfWork uow, IMapper mapper)
    {
        _uok = uow;
        _mapper = mapper;
    }

    [HttpPost]

    public async Task<ActionResult<MemberDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUsername();

        if (username == createMessageDto.RecipientUsername.ToLower())
            return BadRequest("You cannot send menssages to yourself");

        var sender = await _uok.UserRepository.GetUserByUsernameAsync(username);
        var recipient = await _uok.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        if (recipient == null) return NotFound();

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };
        
        _uok.MessageRepository.AddMessage(message);

        if (await _uok.Complete()) return Ok(_mapper.Map<MessageDto>(message));
        return BadRequest("Failed to send message");

    }

    [HttpGet]

    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername();
        var messages = await _uok.MessageRepository.GetMessagesForUser(messageParams);
        
        Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

        return messages;
    }



    [HttpDelete("{id}")]

    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUsername();

        var message = await _uok.MessageRepository.GetMessage(id);

        if (message.SenderUsername != username && message.RecipientUsername != username) 
            return Unauthorized();

        if (message.SenderUsername == username)
        {
            message.SenderDeleted = true;
        }

        if (message.RecipientUsername == username)
        {
            message.RecipientDeleted = true;
        }

        if (message.SenderDeleted && message.RecipientDeleted)
        {
            _uok.MessageRepository.DeleteMessage(message);
        }

        if (await _uok.Complete())
        {
            return Ok();
        }

        return BadRequest("Problem deleting the message");
    }
    
}