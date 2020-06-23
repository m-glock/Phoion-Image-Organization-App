
namespace DLuOvBamG.Droid
{
    class JavaHelper<T> : Java.Lang.Object
    {
        public T Value { get; private set; }

        public JavaHelper(T obj)
        {
            this.Value = obj;
        }

        
    }
}