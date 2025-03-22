using AutoMapper;
using EduConnect.DTOs.Messenger;
using EduConnect.Entities.Messenger;
using EduConnect.Entities.Person;
using Org.BouncyCastle.Cms;

namespace EduConnect.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
                .ForMember(dest => dest.SenderEmail, opt => opt.MapFrom(src => src.SenderEmail))
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => GetPhotoUrl(src.Sender.PersonPhoto,src.SenderId)))
                .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.RecipientId))
                .ForMember(dest => dest.RecipientEmail, opt => opt.MapFrom(src => src.RecipientEmail))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => GetPhotoUrl(src.Recipient.PersonPhoto,src.RecipientId)))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.DateRead, opt => opt.MapFrom(src => src.DateRead))
                .ForMember(dest => dest.MessageSent, opt => opt.MapFrom(src => src.MessageSent));
        }

        private static string GetPhotoUrl(IEnumerable<PersonPhoto> photos,Guid MessengePersonID)
        {
            var photo= photos?
                .Where(x=> x.PersonId == MessengePersonID)
                .FirstOrDefault()?.Url;
            return photo != null ? photo : "NoPhotoUser";
        }
    }

}
