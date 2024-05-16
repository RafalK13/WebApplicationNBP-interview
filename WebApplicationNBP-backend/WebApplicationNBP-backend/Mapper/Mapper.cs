using AutoMapper;
using WebApplicationNBP_backend.Domain;
using WebApplicationNBP_backend.Domain.DTOs;
using WebApplicationNBP_backend.Resolvers;

namespace WebApplicationNBP_backend.Mapper
{
	public class Mapper : Profile
	{
		public Mapper()
		{
			CreateMap<ArrayOfExchangeRatesTableDto, ArrayOfExchangeRatesTable>()
				.ForMember(dest => dest.ExchangeRatesTables, opt => opt.MapFrom(src => src.ExchangeRatesTable));

			CreateMap<ExchangeRatesTableDto, ExchangeRatesTable>();

			CreateMap<RateDto, Rate>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CurrencyId, opt => opt.MapFrom<CurrencyResolver>())
				.ForMember(dest => dest.MidRate, opt => opt.MapFrom(src => src.Mid))
				.ForMember(dest => dest.Currency, opt => opt.Ignore());
		}
	}
}
