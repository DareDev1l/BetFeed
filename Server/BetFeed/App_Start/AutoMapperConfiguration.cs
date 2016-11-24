using AutoMapper;
using BetFeed.Models;
using BetFeed.ViewModels;

namespace BetFeed.App_Start
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Sport, SportViewModel>()
                    .ForMember(dest => dest.Categories, opt => opt.Ignore());

                cfg.CreateMap<Event, EventViewModel>()
                    .ForMember(dest => dest.MatchCount, opt => opt.Ignore());
                cfg.CreateMap<Event, NewMatchesViewModel>()
                    .ForMember(dest => dest.RequestDate, opt => opt.Ignore());

                cfg.CreateMap<Match, MatchViewModel>();
                cfg.CreateMap<Bet, BetViewModel>();
                cfg.CreateMap<Odd, OddViewModel>();

                cfg.CreateMap<Match, MatchWithBetsViewModel>()
                    .ForMember(dest => dest.First, opt => opt.MapFrom(match => match.Name.Split('-')[0]))
                    .ForMember(dest => dest.Second, opt => opt.MapFrom(match => match.Name.Split('-')[1]));

                cfg.CreateMap<Sport, SportWithNameAndId>()
                    .ForMember(dest => dest.SportId, opt => opt.MapFrom(sport => sport.Id))
                    .ForMember(dest => dest.SportName, opt => opt.MapFrom(sport => sport.Name))
                    .ForMember(dest => dest.EventsCount, opt => opt.MapFrom(sport => sport.Events.Count));

                cfg.CreateMap<Event, EventWithMatchesViewModel>()
                    .ForMember(dest => dest.RequestDate, opt => opt.Ignore());
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}