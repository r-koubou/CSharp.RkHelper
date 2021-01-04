using System.IO;
using System.Text;

namespace RkHelper.IO
{
    public sealed class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding()
            : this( Encoding.UTF8 )
        {
        }

        public StringWriterWithEncoding( Encoding encoding )
        {
            Encoding = encoding;
        }

        public override Encoding Encoding { get; }
    }
}