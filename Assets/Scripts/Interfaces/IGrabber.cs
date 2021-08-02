namespace Crenix
{
    public interface IGrabber
    {
        bool IsOccupied { get; }
        bool IsActive { get; }
        void Grab(IGrabbable grababble);
        void Release();
    } 
}