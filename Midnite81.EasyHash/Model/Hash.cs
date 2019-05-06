namespace Midnite81.EasyHash.Model
{
    public class Hash
    {
        public string HashString { get; set; }

        public byte[] HashBytes { get; set; }

        public Salt Salt { get; set; }
    }
}