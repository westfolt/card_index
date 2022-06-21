using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            try
            {
                var takenFromDb = await _unitOfWork.AuthorRepository.GetAllWithDetailsAsync();
                var mapped = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get authors", ex);
            }
        }

        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            try
            {
                var takenFromDb = await _unitOfWork.AuthorRepository.GetByIdWithDetailsAsync(id);
                var mapped = _mapper.Map<Author, AuthorDto>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get author with id: {id}", ex);
            }
        }

        public async Task<int> AddAsync(AuthorDto model)
        {
            try
            {
                var mapped = _mapper.Map<AuthorDto, Author>(model);
                await _unitOfWork.AuthorRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add author to db", ex);
            }
        }

        public async Task UpdateAsync(AuthorDto model)
        {
            try
            {
                var mapped = _mapper.Map<AuthorDto, Author>(model);
                _unitOfWork.AuthorRepository.Update(mapped);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot update author with id: {model.Id}", ex);
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            try
            {
                await _unitOfWork.AuthorRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete author with id: {modelId}", ex);
            }
        }

        public async Task<IEnumerable<AuthorDto>> GetAuthorsForPeriodAsync(int startYear, int endYear)
        {
            try
            {
                var takenFromDb = (await _unitOfWork.AuthorRepository.GetAllWithDetailsAsync())
                    .Where(a => a.YearOfBirth >= startYear && a.YearOfBirth <= endYear);
                return _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(takenFromDb);
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get authors for desired date range", ex);
            }
        }
    }
}
