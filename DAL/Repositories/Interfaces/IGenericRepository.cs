using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    // Tüm varlık repository'leri için temel CRUD operasyonlarını tanımlayan genel arayüz
    public interface IGenericRepository<T> where T : class
    {
        // Tablodaki tüm kayıtları asenkron olarak getirir
        Task<IEnumerable<T>> GetAllAsync();

        // Verilen ID'ye sahip kaydı asenkron olarak getirir
        Task<T> GetByIdAsync(int id);

        // Yeni bir kaydı tabloya asenkron olarak ekler
        Task AddAsync(T entity);

        // Mevcut bir kaydı günceller
        void Update(T entity);

        // Verilen ID'ye sahip kaydı tablodan siler
        void Delete(int id);
    }

}
