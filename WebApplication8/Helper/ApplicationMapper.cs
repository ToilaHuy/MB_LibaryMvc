using WebApplication8.Data;
using AutoMapper;
using WebApplication8.Models;

namespace WebApplication8.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Item, ItemModel>().ReverseMap();
            CreateMap<Borrower, BorrowerModel>().ReverseMap();
            CreateMap<BorrowingHistory, BorrowingHistoryModel>().ReverseMap();
            CreateMap<HistoryDetail, BorrowingHistoryModel>().ReverseMap();
            CreateMap<Borrower, BorrowingHistoryModel>().ReverseMap();
            CreateMap<BorrowingHistory, BorrowingCreate>().ReverseMap();
        }
    }
}
