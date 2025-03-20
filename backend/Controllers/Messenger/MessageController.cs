using AutoMapper;
using backend.Middleware;
using EduConnect.Data;
using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Extensions;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers.Messenger
{
    [ApiController]
    [Route("Messenger")]
    public class MessageController : MainController
    {
        private readonly DataContext context;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public MessageController(DataContext context, IMessageRepository messageRepository, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("GetUserForChatList")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> GetPersonListChat()
        {
            var caller = new Caller(this.HttpContext);
            var CurrentUserEmail = caller.Email;
            var defaultPhotoUrl = "https://res.cloudinary.com/dsuwjnudy/image/upload/v1735186361/ivbfqfru35jp7m8aeosn.jpg";

            var personDetails = await context.PersonDetails.ToListAsync();
            var personEmails = await context.PersonEmail.ToListAsync();
            var personPhotos = await context.PersonPhoto.ToListAsync();

            var personListChats = new List<PersonListChat>();

            foreach (var person in personDetails)
            {
                var email = personEmails.FirstOrDefault(e => e.PersonId == person.PersonId);
                var photo = personPhotos.FirstOrDefault(p => p.PersonId == person.PersonId);

                if (email != null && email.Email != CurrentUserEmail)
                {
                    var personChat = new PersonListChat
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Email = email.Email,
                        PhotoLink = photo?.Url ?? defaultPhotoUrl
                    };

                    personListChats.Add(personChat);
                }
            }

            var distinctPersonListChats = personListChats
                .GroupBy(p => p.Email)
                .Select(group => group.First())
                .ToList();

            return Ok(distinctPersonListChats);
        }


        [HttpPost]
        [Route("CreateMessageForUser")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {

            if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            {
                return BadRequest(new
                {
                    message = "Message content cannot be empty",
                    timestamp = DateTime.UtcNow
                });
            }

            var caller = new Caller(this.HttpContext);
            var callerEmail = caller.Email?.ToLower();


            if (callerEmail == createMessageDto.RecipientEmail.ToLower())
            {
                return BadRequest(new
                {
                    message = "You cannot message yourself",
                    timestamp = DateTime.UtcNow
                });
            }


            var senderMail = await context.PersonEmail.FirstOrDefaultAsync(x => x.Email == callerEmail);
            var recipientMail = await context.PersonEmail.FirstOrDefaultAsync(x => x.Email == createMessageDto.RecipientEmail.ToLower());


            if (senderMail == null || recipientMail == null)
            {
                return BadRequest(new
                {
                    message = "Sender or recipient email not found, cannot send message.",
                    timestamp = DateTime.UtcNow
                });
            }


            var sender = await context.Person.FirstOrDefaultAsync(x => x.PersonId == senderMail.PersonId);
            var recipient = await context.Person.FirstOrDefaultAsync(x => x.PersonId == recipientMail.PersonId);


            if (sender == null || recipient == null)
            {
                return BadRequest(new
                {
                    message = "Sender or recipient not found",
                    timestamp = DateTime.UtcNow
                });
            }


            var senderPhotoUrl = sender?.PersonPhoto?.FirstOrDefault()?.Url ?? "No User photo";
            var recipientPhotoUrl = recipient?.PersonPhoto?.FirstOrDefault()?.Url ?? "No User photo";


            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderEmail = senderMail.Email,
                RecipientEmail = recipientMail.Email,
                Content = createMessageDto.Content,
                MessageSent = DateTime.UtcNow
            };


            messageRepository.AddMessage(message);
            var saveSuccess = await messageRepository.SaveAllAsync();

            if (saveSuccess)
            {

                return Ok(new
                {
                    message = "Message was successfully created",
                    data = mapper.Map<MessageDto>(message)
                });
            }
            else
            {

                return BadRequest(new
                {
                    message = "Failed to save the message",
                    timestamp = DateTime.UtcNow
                });
            }



        }
        [HttpGet]
        [Route("GetMessageThread/{email}")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string email)
        {
            var caller = new Caller(this.HttpContext);
            var currentEmail = caller.Email;
            return Ok(await messageRepository.GetMessageThread(currentEmail, email));
        }

        [HttpGet("GetLastMessagesForDirectMessaging")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetLastMessagesForDirectMessaging([FromQuery] MessageParamsDirect messageParams)
        {
            var caller = new Caller(this.HttpContext);
            messageParams.Email = caller.Email;

            if (string.IsNullOrWhiteSpace(messageParams.Email))
                return BadRequest("Invalid email for the caller.");

            try
            {
                var messages = await messageRepository.GetLastMessagesForDirectMessaging(messageParams);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetMessagesForUser")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser(
            [FromQuery] MessageParams messageParams)
        {
            var Caller = new Caller(this.HttpContext);
            messageParams.Email = Caller.Email;
            var messages = await messageRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(messages);
            return messages;

        }
    }


}
