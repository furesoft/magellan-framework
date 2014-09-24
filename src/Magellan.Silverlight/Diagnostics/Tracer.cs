using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;

namespace Magellan.Diagnostics
{
    internal class IsolatedStorageTracer
    {
        private readonly IsolatedStorageFile _storageFile;
        private readonly IsolatedStorageFileStream _storageFileStream;
        private readonly StreamWriter _streamWriter;

        public IsolatedStorageTracer()
        {
            _storageFile = IsolatedStorageFile.GetUserStoreForApplication();
            _storageFileStream = _storageFile.OpenFile("MagellanTrace.log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            _streamWriter = new StreamWriter(_storageFileStream);
            _streamWriter.AutoFlush = true;
        }

        ~IsolatedStorageTracer()
        {
            _storageFileStream.Close();
        }

        public void Write(String message)
        {
            Debug.WriteLine(message);
            _streamWriter.WriteLine(message);
        }
    }
}