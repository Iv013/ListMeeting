
using ListMeeting.Models.Models;


namespace ListMeetings.Core.Data
{
    public  class ConcurrentMeetingsList
    {
        //сделаем потокобезопасное обращение к листу
        private readonly ReaderWriterLockSlim _listLock = new ReaderWriterLockSlim();
        public  List<MeetingDTO> meeting = new List<MeetingDTO>() ;

        public void Add(MeetingDTO elem)
        {
            _listLock.EnterWriteLock();
            try
            {
                meeting.Add(elem);
            }
            finally
            {
                _listLock.ExitWriteLock();
            }
        }
        public MeetingDTO FirstOrDefault(Func<MeetingDTO, bool> filter = null)
        {
            _listLock.EnterReadLock();
            try
            {
              return  meeting.FirstOrDefault(filter);
            }
            finally
            {
                _listLock.ExitReadLock();
            }
        }
        public List<MeetingDTO> Where(Func<MeetingDTO, bool> filter)
        {
            _listLock.EnterReadLock();
            try
            {
                return meeting.Where(filter).ToList();
            }
            finally
            {
                _listLock.ExitReadLock();
            }
        }

        public void Remove(MeetingDTO elem)
        {
            _listLock.EnterWriteLock();
            try
            {
                    meeting.Remove(elem);
            }
            finally
            {
                _listLock.ExitWriteLock();
            }
        }

        public int Count()
        {
            _listLock.EnterReadLock();
            try
            {
                return meeting.Count;
            }
            finally
            {
                _listLock.ExitReadLock();
            }
        }



        public IEnumerator<MeetingDTO> GetEnumerator()
        {
            _listLock.EnterReadLock();
            try
            {
                return meeting.GetEnumerator();
            }
            finally
            {
                _listLock.ExitReadLock();
            }
           
        }


        public int Max(Func<MeetingDTO, int> filter)
        {
            _listLock.EnterReadLock();
            try
            {
                return meeting.Select(filter).Max();
            }
            finally
            {
                _listLock.ExitReadLock();
            }
        }
        public bool Any(Func<MeetingDTO, bool> filter)
        {
            _listLock.EnterReadLock();
            try
            {
                return meeting.Any(filter);
            }
            finally
            {
                _listLock.ExitReadLock();
            }
        }

    }
}
