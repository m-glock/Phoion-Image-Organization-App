namespace DLuOvBamG.Models
{
    public class PictureDeletedEvent
    {
        private int DeletedPictureId;
        public PictureDeletedEvent(int picutureId)
        {
            DeletedPictureId = picutureId;
        }
        public int GetPictureId()
        {
            return DeletedPictureId;
        }
    }
}
