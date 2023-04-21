using WebApplication8.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;

namespace WebApplication8.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
      
    }
   
}
