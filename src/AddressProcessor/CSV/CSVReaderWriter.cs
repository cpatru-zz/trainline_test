using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.

        Assumptions:
        - is this application assuming input is always 2 columns CSV data?
        - interface and all public methods maintained even though not all are used
        - is this a refactoring or accounting for existing bugs? - I assumed not and 'refactored', not redesigned
        - there could have been 2 classes that handle reading/writing wrapped in here, but since the class is small
         it's fine for now.
        - I've added IDisposable, even though that's adding to the public interface this class has, it's 
        still backwards compatible.
    */

    public class CSVReaderWriter : IDisposable
    {
        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        private const String separator = "\t";

        private StreamReader _readerStream;
        private StreamWriter _writerStream;

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _readerStream = File.OpenText(fileName);
                    break;
                case Mode.Write:
                    FileInfo fileInfo = new FileInfo(fileName);
                    _writerStream = fileInfo.CreateText();
                    break;
                default:
                    throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            _writerStream.WriteLine(string.Join(separator, columns));
        }

        public bool Read(string column1, string column2)
        {
            return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            var line = _readerStream.ReadLine();

            var columns = line?.Split(separator.ToCharArray());

            column1 = columns?[0];
            column2 = columns?[1];

            return columns != null && columns.Length != 0;
        }

        public void Close()
        {
            _writerStream?.Close(); _writerStream = null;
            _readerStream?.Close(); _readerStream = null;
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
