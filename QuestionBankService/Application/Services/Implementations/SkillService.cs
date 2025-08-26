using Application.Dtos.SkillDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class SkillService : ISkillService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public SkillService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SkillViewDto>> GettAllSkill()
        {
            var result =  await _unitOfWork.SkillRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SkillViewDto>>(result);
        }
    }
}
