
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.DataMapper;

namespace ListMeeting.Core.Repository
{
    public class MeetingRepository : IMeetingRepository<Meeting, MeetingDTO>
    {
        private readonly ConcurrentMeetingsList _dataBase;
        private readonly IDataMapper<Meeting, MeetingDTO> _dataMapper;
        public MeetingRepository(ConcurrentMeetingsList dataBase,
         IDataMapper<Meeting, MeetingDTO> dataMapper   )
        {
            _dataBase = dataBase;
            _dataMapper = dataMapper;
        }
        
        public ServiceResponse AddMeeting(Meeting entity)
        {
            var obj = _dataMapper.CreateDTO(entity);
            //Если новая встреча пересекается по времени с текущими возвращаем ошибку
            if (CheckIntersectionMeetings(entity)) 
            {
                return new ServiceResponse 
                { 
                      Message = StringConst.ErrorAdd + StringConst.ErrorMeetingsIntersection
                    , Success = 409 
                };
            }
            //Если новая встреча планируется в прошлом возвращаем ошибку
            if (obj.DateTimeStartEvent < DateTime.Now) 
            {
                return new ServiceResponse 
                { 
                    Message = StringConst.ErrorAdd + StringConst.ErrorMeetingInPast
                   ,Success = 409 };
            }
                //получаем максимальный номер ID(считаем что ID автоматически не икрементируется)
                var maxID = _dataBase.Count() > 0 ? _dataBase.Max(x => x.Id) : 0;
                obj.Id = maxID + 1;
                _dataBase.Add(obj);
                return new ServiceResponse() { Message= StringConst.AddSuccess};
            
        }

        public (Meeting?, ServiceResponse) FirstOfDefault(Func<MeetingDTO, bool> filter)
        {
            
                var obj = _dataMapper.CreateDomainModel(_dataBase.FirstOrDefault(filter));
                if (obj is null){
                    return (obj, new ServiceResponse 
                    { Message = StringConst.ObjectNotFound
                    , Success = 404});
                }

                return (obj,
                    new ServiceResponse());  
        }


        public List<Meeting> GetAllMeetings(Func<MeetingDTO, bool> dateFilter = null)
        {
            List < Meeting > result = new List<Meeting >();
            if (dateFilter != null)
            {
                var tempResult = _dataBase.Where(dateFilter).ToList();
              foreach (MeetingDTO meeting in tempResult)
                {
                    result.Add(_dataMapper.CreateDomainModel(meeting));
                }

                return result.OrderBy(x => x.DateTimeStartEvent).ToList();
            }
            foreach (MeetingDTO meeting in _dataBase)
                result.Add(_dataMapper.CreateDomainModel(meeting));


            return result.OrderBy(x=>x.DateTimeStartEvent).ToList();
        }



        public ServiceResponse RemoveMeeting(int id)
        {
            var obj = _dataBase.FirstOrDefault(x => x.Id == id);
            if (obj == null){       //если запись отсутствует возварщаем ошибку 

                return new ServiceResponse  
                { 
                    Message = StringConst.ObjectNotFound
                    , Success = 409 
                };  
            }
            _dataBase.Remove(obj);
            return new ServiceResponse() { Message = StringConst.DelSuccess };

        }


        public ServiceResponse UpdateMeeting(Meeting entity)
        {
            //получаем ссылку на объект который необходимо обновить
            var oldMeeting = _dataBase.FirstOrDefault(x => x.Id == entity.Id);
            
            //удаляем временно его из списка
            RemoveMeeting(entity.Id);

            //провемряем пересекается ли измененная запись с оставшимися записями, если да то
            //возращаем старую запись назад и сигнализируем об ошибке
            if (CheckIntersectionMeetings(entity))
            {
                _dataBase.Add(oldMeeting);
             
                return new ServiceResponse { Message = StringConst.ErrorUpdate + StringConst.ErrorMeetingsIntersection, Success = 409 };
            }
            //Если новая встреча планируется в прошлом возвращаем ошибку
            if (entity.DateTimeStartEvent < DateTime.Now)
            {
                _dataBase.Add(oldMeeting);
                return new ServiceResponse { Message = StringConst.ErrorUpdate + StringConst.ErrorMeetingInPast, Success = 409 };
                
            }
            //Если все учловия выполнены добавляем измененую запись
            var obj = _dataMapper.CreateDTO(entity);
            _dataBase.Add(obj);
            return new ServiceResponse() { Message = StringConst.UpdateSuccess };

        }

        /// <summary>
        /// Проверка на пересечение на наличие пересечения дат встреч
        /// </summary>
        bool CheckIntersectionMeetings(Meeting entity)=> _dataBase.Any(x =>
                  !(x.DateTimeStartEvent > entity.DateTimeEndEvent ||
                   x.DateTimeEndEvent < entity.DateTimeStartEvent));


    }
}
