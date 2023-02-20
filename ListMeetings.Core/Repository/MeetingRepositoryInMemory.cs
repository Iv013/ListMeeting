using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.DataMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListMeetings.Core.Repository
{
    public class MeetingRepositoryInMemory : IMeetingRepository<Meeting, MeetingDTO>
    {

        private readonly ApplicationDbContext _dataBase;
        private readonly IDataMapper<Meeting, MeetingDTO> _dataMapper;
        internal DbSet<MeetingDTO> dbSet;
        public MeetingRepositoryInMemory(ApplicationDbContext dataBase,
         IDataMapper<Meeting, MeetingDTO> dataMapper)
        {
            _dataBase = dataBase;
            _dataMapper = dataMapper;
            dbSet = _dataBase.Set<MeetingDTO>();

        }





        public ServiceResponse AddMeeting(Meeting entity)
        {
            var obj = _dataMapper.CreateDTO(entity);
        
            if (CheckIntersectionMeetings(entity))
            {
                return new ServiceResponse
                {
                    Message = StringConst.ErrorAdd + StringConst.ErrorMeetingsIntersection
                    ,
                    Success = 409
                };
            }
            //Если новая встреча планируется в прошлом возвращаем ошибку
            if (obj.DateTimeStartMeeting < DateTime.Now)
            {
                return new ServiceResponse
                {
                    Message = StringConst.ErrorAdd + StringConst.ErrorMeetingInPast
                   ,
                    Success = 409
                };
            }


            dbSet.Add(obj);
            _dataBase.SaveChanges();
            return new ServiceResponse() { Message = StringConst.AddSuccess };
        }




        public (Meeting, ServiceResponse) FirstOfDefault(Func<MeetingDTO, bool> filter)
        {
            var obj = _dataMapper.CreateDomainModel(dbSet.FirstOrDefault(filter));
            if (obj is null)
            {
                return (obj, new ServiceResponse
                {
                    Message = StringConst.ObjectNotFound
                ,
                    Success = 404
                });
            }

            return (obj,
                new ServiceResponse());
        }

        public List<Meeting> GetAllMeetings(Func<MeetingDTO, bool> dateFilter = null)
        {
            List<Meeting> result = new List<Meeting>();
            if (dateFilter != null)
            {
                var tempResult = dbSet.Where(dateFilter).ToList();
                foreach (MeetingDTO meeting in tempResult)
                {
                    result.Add(_dataMapper.CreateDomainModel(meeting));
                }

                return result.OrderBy(x => x.DateTimeStartMeeting).ToList(); // результат из нескольких записей всегда сортируем по дате начала
            }
            foreach (MeetingDTO meeting in dbSet)
                result.Add(_dataMapper.CreateDomainModel(meeting));


            return result.OrderBy(x => x.DateTimeStartMeeting).ToList();  // результат из нескольких записей всегда сортируем по дате начала
        }

        public ServiceResponse RemoveMeeting(int IdEntity)
        {
            var obj = dbSet.FirstOrDefault(x => x.Id == IdEntity);
            if (obj == null)
            {       //если запись отсутствует возварщаем ошибку 

                return new ServiceResponse
                {
                    Message = StringConst.ObjectNotFound
                    ,
                    Success = 409
                };
            }
            dbSet.Remove(obj);
            _dataBase.SaveChanges();
            return new ServiceResponse() { Message = StringConst.DelSuccess };
        }

        public ServiceResponse UpdateMeeting(Meeting entity)
        {
             //получаем ссылку на объект который необходимо обновить
            var oldMeeting = dbSet.FirstOrDefault(x => x.Id == entity.Id);

            //удаляем временно его из списка
            RemoveMeeting(entity.Id);

            //провемряем пересекается ли измененная запись с оставшимися записями, если да то
            //возращаем старую запись назад и сигнализируем об ошибке
            if (CheckIntersectionMeetings(entity))
            {
                dbSet.Add(oldMeeting);
                _dataBase.SaveChanges();

                return new ServiceResponse { Message = StringConst.ErrorUpdate + StringConst.ErrorMeetingsIntersection, Success = 409 };
            }
            //Если новая встреча планируется в прошлом возвращаем ошибку
            if (entity.DateTimeStartMeeting < DateTime.Now)
            {
                dbSet.Add(oldMeeting);
                _dataBase.SaveChanges();
                return new ServiceResponse { Message = StringConst.ErrorUpdate + StringConst.ErrorMeetingInPast, Success = 409 };

            }
            //Если все учловия выполнены добавляем измененую запись
            var obj = _dataMapper.CreateDTO(entity);
            _dataBase.Add(obj);
            _dataBase.SaveChanges();
            return new ServiceResponse() { Message = StringConst.UpdateSuccess };
        }



        /// <summary>
        /// Проверка на пересечение на наличие пересечения дат встреч
        /// </summary>
        bool CheckIntersectionMeetings(Meeting entity) => dbSet.Any(x =>
                  !(x.DateTimeStartMeeting > entity.DateTimeEndMeeting ||
                   x.DateTimeEndMeeting < entity.DateTimeStartMeeting));
    }
}
